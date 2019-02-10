#pragma once



#define __STDC_CONSTANT_MACROS

#ifdef _WIN32
 //Windows
extern "C"
{
#include "libavcodec/avcodec.h"
#include "libavformat/avformat.h"
#include "libswscale/swscale.h"
#include "libavutil/imgutils.h"
#include "SDL2/SDL.h"
};
#else
 //Linux...
#ifdef __cplusplus
extern "C"
{
#endif
#include <libavcodec/avcodec.h>
#include <libavformat/avformat.h>
#include <libswscale/swscale.h>
#include <libavutil/imgutils.h>
#include <SDL2/SDL.h>
#ifdef __cplusplus
};
#endif
#endif

#include <iostream>
#include <vector>
#include <thread>
#include <mutex>
#include <chrono>
#include <condition_variable>
#include "FrameQueue.h"
//Refresh Event
#define SFM_REFRESH_EVENT  (SDL_USEREVENT + 1)

#define SFM_BREAK_EVENT  (SDL_USEREVENT + 2)
#define MAX_TILE 100
using namespace std;



typedef struct  //Point
{
	int x;
	int y;
}Point;


typedef struct T  //Transform
{
	T()
	{

	}
	T (int x1, int y1, int x2, int y2)
	{
		lefttop.x = x1;
		lefttop.y = y1;
		rightdown.x = x2;
		rightdown.y = y2;
	}
	Point lefttop;
	Point rightdown;
}Tile;

struct Produce_data
{
	Produce_data(){}
	Produce_data(AVFrame * pDstFrame, int target_height, int target_width, int gap_h, int gap_w)
	{
		this->pDstFrame = pDstFrame;
		this->target_height = target_height;
		this->target_width = target_width;
		this->gap_h = gap_h;
		this->gap_w = gap_w;
	}
	AVFrame * pDstFrame;
	int target_height;
	int target_width;
	int gap_h;
	int gap_w;
};

struct Decoder_data
{
	Decoder_data(){}
	Decoder_data(int tilenumber,char*filepath)
	{
		this->tilenumber = tilenumber;
		this->filepath = filepath;
	}
	int tilenumber;
	char *filepath;
};


int changeFrame = 71;
int thread_exit = 0;
int thread_pause = 0;
bool isTrue[MAX_TILE];
int graph[MAX_TILE];
std::mutex mtx;
std::condition_variable cv[MAX_TILE];
vector<thread> v;
AVFrame	*Cur[MAX_TILE]; //先做两个tile的
int Tl[MAX_TILE];
int Wait[MAX_TILE];
Tile tile[MAX_TILE];
FrameQueue *fq;
int width = 640;
int height = 272;

void  Decoder_Real(Decoder_data *d);
void Update_Tile(AVFrame * pDstFrame, AVFrame **Cur, Tile **tile, int length, int width, int height);

int sfp_refresh_thread(void *opaque) {
	thread_exit = 0;
	thread_pause = 0;

	while (!thread_exit) {
		if (!thread_pause) {
			SDL_Event event;
			event.type = SFM_REFRESH_EVENT;
			SDL_PushEvent(&event);
		}
		SDL_Delay(33);
	}
	thread_exit = 0;
	thread_pause = 0;
	//Break
	SDL_Event event;
	event.type = SFM_BREAK_EVENT;
	SDL_PushEvent(&event);

	return 0;
}
void CheckTile(Tile **tile, int length, int row,int *temp,int &temp_length,int *tileIndex) //用来确定当前行有多少个tile
{
	int i;
	int k = 0;
	int width = 0;
	for (i = 0; i < length; i++)
	{
		if (tile[i]->lefttop.y <= row &&  row <tile[i]->rightdown.y)
		{
			
			tileIndex[k] = row - tile[i]->lefttop.y;
			temp[k++] = i; //这些tile在当前行中		
		}

	}
	temp_length = k;
}

void CheckTile_UV(Tile **tile, int length, int row, int *temp, int &temp_length, int *tileIndex) //用来确定当前行有多少个tile
{
	int i;
	int k = 0;
	int width = 0;
	for (i = 0; i < length; i++)
	{
		if (tile[i]->lefttop.y/2 <= row && row < tile[i]->rightdown.y/2)
		{

			tileIndex[k] = (row - tile[i]->lefttop.y/2);
			temp[k++] = i; //这些tile在当前行中		
		}

	}
	temp_length = k;
}
//当前线程获取tile数量，以及具体tile的位置
void ProduceFrame(Produce_data *p)   //不停的往队列里添加帧，到时间就自动切换
{
	//初始创建一批decoder
	
	int height = p->target_height;
	int width = p->target_width;
	int gap_h = p->gap_h;
	int gap_w = p->gap_w;

	int tileNumber=gap_w*gap_h;
	int i, j;
	Tile * tile[72];
	for (i = 0; i < gap_h; i++)
	{
		for (j = 0; j < gap_w; j++)
		{
			tile[i*gap_w + j] = new Tile(j*width, i*height, j*width + width, i*height + height);
		}
	}
	//tile[0] = new Tile(0, 0, 640, 272);
	//tile[1] = new Tile(0, 272, 640,544);
	
	int T = 0;  //每隔30帧重新启动decoder
	AVFrame *pDstFrameBuffer[3];
	uint8_t *distBuffer[3];
	FrameBuffer *temp[3];
	for (i = 0; i < 3; i++)
	{
		temp[i] = new FrameBuffer();
		AVFrame *pDstFrame;
		int target_width = width, target_height = height;
		int nDstSize = avpicture_get_size(AV_PIX_FMT_YUV420P, target_width*gap_w, target_height*gap_h);
		uint8_t *dstbuf;
		pDstFrame = av_frame_alloc();
		dstbuf = new uint8_t[nDstSize];
		avpicture_fill((AVPicture*)pDstFrame, dstbuf, AV_PIX_FMT_YUV420P, target_width*gap_w, target_height*gap_h);
		pDstFrame->width = target_width * gap_w;
		pDstFrame->height = target_height * gap_h;
		pDstFrame->format = AV_PIX_FMT_YUV420P;
		memset(pDstFrame->data[0], 0, target_height*target_width*gap_w*gap_h);
		memset(pDstFrame->data[1], 0x80, target_height*target_width*gap_w*gap_h / 4);
		memset(pDstFrame->data[2], 0x80, target_height*target_width*gap_w*gap_h / 4);
		pDstFrameBuffer[i] = pDstFrame;
		distBuffer[i] = dstbuf;
		
	}
	AVFrame *pDstFrame;
	
	uint8_t *dstbuf;
	while(true)  //不停的向前进行decode,每一次循环产生一帧
	{ 
	
		//如果当前已经解完了就换个文件读，重新给decoder赋予新的文件名
		T = T + 1;
		printf("Produce:%d\n", T);
		if (T % (changeFrame-1)== 1)
		{
			//每隔30帧自动的去更换一个文件去读
			//v.clear();
			//这里要获取新的tilenumber和tile方式
			//
			//this_thread::sleep_for(chrono::milliseconds(500));
			T = 1;
			for (i = 0; i < tileNumber; i++)
			{
				isTrue[i] = false;
				char temp1[5];
				char temp2[5]=".mkv";
				//char temp3[5];
				//itoa(int(T / 30), temp3, 10);
				itoa(i%2, temp1, 10);
				strcat(temp1, temp2);
				Decoder_data *d = new Decoder_data(i, temp1);
				v.emplace_back(Decoder_Real,d);
			
			}
			 //重新启动一堆解码器
		}
		pDstFrame = pDstFrameBuffer[(T-1) % 3];
		dstbuf = distBuffer[(T - 1) % 3];
		//告诉所有decoder可以进行解码了
		for (i = 0; i < tileNumber; i++) 
		{
			unique_lock<mutex> loc(mtx);
			isTrue[i] = true;  //当所有Istrue都是false 的时候代表所有块都已经渲染完了
			cv[i].notify_all();
		}

		int pass = 1;
		while (true)
		{
			pass = 1;
			for (i = 0; i < tileNumber; i++)
			{
				if (isTrue[i] == true) pass = 0;
			}
			if (pass == 1)
				break;

		this_thread::sleep_for(chrono::microseconds(100));
		}

		//渲染完了就进行拼接,update函数其实是拼接函数	
		Update_Tile(pDstFrame, Cur, tile, tileNumber, width*gap_w, height*gap_h);
		//完成拼接放入队列
		while (true)
		{
		
			temp[(T - 1) % 3]->frame = pDstFrame;
			temp[(T - 1) % 3]->buffer = dstbuf;
			if (fq->Push(temp[(T - 1) % 3]) == 1)  //代表成功放入队列了
			{
				break;
			}
			this_thread::sleep_for(chrono::microseconds(100));
			
		}
		
	}
}


FrameBuffer* GetFrame()
{
	FrameBuffer *temp= fq->GetTop();
	if (temp ==NULL)  //代表没有获得画面
	{
		return  NULL;	//返回空暂停先别放
	}
	else
	{
			return temp;
	}
}

//每一帧调用一次 ，用于合并tile
void Update_Tile(AVFrame * pDstFrame,AVFrame **Cur, Tile **tile,int length,int width,int height) //width 和 height是整个画面的
{
	int nYIndex = 0;
	int temp[MAX_TILE];
	int tileIndex[MAX_TILE];
	int temp_length,i,j;
	memset(tileIndex, 0, sizeof(tileIndex));
	for (i = 0; i < height; i++)   //这个是每一行
	{
		int temp_width = 0;
		CheckTile(tile, length, i, temp, temp_length,tileIndex);
		for (j = 0; j < temp_length; j++)
		{
			memcpy(pDstFrame->data[0] + temp_width+ i * pDstFrame->linesize[0], (Cur[temp[j]])->data[0] + tileIndex[j] * Cur[temp[j]]->linesize[0], Cur[temp[j]]->linesize[0]);
			temp_width = temp_width + Cur[temp[j]]->linesize[0];
		}
	}
	
	for (i = 0; i < height/2 ; i++)
	{
		int temp_width = 0;
		CheckTile_UV(tile, length, i, temp, temp_length, tileIndex);

		for (j = 0; j < temp_length; j++)
		{
			memcpy(pDstFrame->data[1] + temp_width + i * pDstFrame->linesize[1], (Cur[temp[j]])->data[1] + tileIndex[j] * Cur[temp[j]]->linesize[1], Cur[temp[j]]->linesize[1]);
			temp_width = temp_width + Cur[temp[j]]->linesize[1];
		}

		temp_width = 0;
		for (j = 0; j < temp_length; j++)
		{
			memcpy(pDstFrame->data[2] + temp_width + i * pDstFrame->linesize[2], (Cur[temp[j]])->data[2] + tileIndex[j] * Cur[temp[j]]->linesize[2], Cur[temp[j]]->linesize[2]);
			temp_width = temp_width + Cur[temp[j]]->linesize[2];
		}
		
	}
}


void  Decoder_Real(Decoder_data *d)  //实际函数
{
	int tilenumber=d->tilenumber;
	char *filepath=d->filepath;
	int T = 0;
	AVFormatContext	*pFormatCtx;
	int				i, videoindex;
	AVCodecContext	*pCodecCtx;
	AVCodec			*pCodec;
	AVFrame	*pFrame, *pFrameYUV;
	unsigned char *out_buffer;
	AVPacket *packet;
	int ret, got_picture;
	struct SwsContext *img_convert_ctx;


	
	av_register_all();
	avformat_network_init();
	pFormatCtx = avformat_alloc_context();

	/*
	if (avformat_open_input(&pFormatCtx, filepath, NULL, NULL) < 0) {
		printf("Couldn't open input stream.\n");
		return;
	}*/
	int tr;
	for(tr=0;tr<5;tr++)
	{ 
		if (avformat_open_input(&pFormatCtx, filepath, NULL, NULL) >= 0) {
			break;
		}
	}
	if (tr == 5)
	{
		printf("无法读入流播放失败");
	}

	if (avformat_find_stream_info(pFormatCtx, NULL) < 0) {
		printf("Couldn't find stream information.\n");
		return ;
	}
	videoindex = -1;
	for (i = 0; i < pFormatCtx->nb_streams; i++)
		if (pFormatCtx->streams[i]->codec->codec_type == AVMEDIA_TYPE_VIDEO) {
			videoindex = i;
			break;
		}
	if (videoindex == -1) {
		printf("Didn't find a video stream.\n");
		return ;
	}
	pCodecCtx = pFormatCtx->streams[videoindex]->codec;
	pCodecCtx->thread_count = 8;
	pCodec = avcodec_find_decoder(pCodecCtx->codec_id);
	if (pCodec == NULL) {
		printf("Codec not found.\n");
		return ;
	}
	if (avcodec_open2(pCodecCtx, pCodec, NULL) < 0) {
		printf("Could not open codec.\n");
		return ;
	}
	pFrame = av_frame_alloc();
	pFrameYUV = av_frame_alloc();

	out_buffer = (unsigned char *)av_malloc(av_image_get_buffer_size(AV_PIX_FMT_YUV420P, pCodecCtx->width, pCodecCtx->height, 1));
	av_image_fill_arrays(pFrameYUV->data, pFrameYUV->linesize, out_buffer,
		AV_PIX_FMT_YUV420P, pCodecCtx->width, pCodecCtx->height, 1);

	//Output Info----------------------------
	/*-
	printf("---------------- File Information ---------------\n");
	av_dump_format(pFormatCtx, 0, filepath, 0);
	printf("-------------------------------------------------\n");
	*/
	img_convert_ctx = sws_getContext(pCodecCtx->width, pCodecCtx->height, pCodecCtx->pix_fmt,
		width, height, AV_PIX_FMT_YUV420P, SWS_BICUBIC, NULL, NULL, NULL);


	packet = (AVPacket *)av_malloc(sizeof(AVPacket));

	unique_lock<mutex> loc(mtx);
	int flag = 0;
	int  target_height = pCodecCtx->height;
	int target_width = pCodecCtx->width;

	while (1)
	{
		if (flag == 0)
		{
			while (!isTrue[tilenumber])
				cv[tilenumber].wait(loc);
		}

		while (1) {
			av_read_frame(pFormatCtx, packet);
			if (packet->stream_index == videoindex)
				break;
		}

		ret = avcodec_decode_video2(pCodecCtx, pFrame, &got_picture, packet);

		if (ret < 0) {
			printf("Decode Error.\n");
			return  ;
		}
		if (got_picture) {	
			sws_scale(img_convert_ctx, (const unsigned char* const*)pFrame->data, pFrame->linesize, 0, pCodecCtx->height, pFrameYUV->data, pFrameYUV->linesize);
			T = T + 1;
			Tl[tilenumber] = T;
			//printf("Decoder:%d\n", T);
			if (T >=changeFrame)
			{
					av_free_packet(packet);
					av_frame_free(&pFrameYUV);
					av_frame_free(&pFrame);
					avcodec_close(pCodecCtx);
					avformat_close_input(&pFormatCtx);	
					av_free(out_buffer);
					return;  //释放当前decoder
			}
			flag = 0;
			Cur[tilenumber] = pFrameYUV;
			isTrue[tilenumber] = false;
		}
		else {
			av_free_packet(packet);
			flag = 1;
		}
	}
	return  ;

}
#undef main
int main(int argc, char* argv[])
{

	//------------SDL----------------
	int screen_w, screen_h;
	SDL_Window *screen;
	SDL_Renderer* sdlRenderer;
	SDL_Texture* sdlTexture;
	SDL_Rect sdlRect;
	SDL_Thread *video_tid;
	SDL_Event event;
	fq = new FrameQueue();


	int gap_w = 3, gap_h = 2;

	if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_AUDIO | SDL_INIT_TIMER)) {
		printf("Could not initialize SDL - %s\n", SDL_GetError());
		return -1;
	}


	int tileNumber = gap_w * gap_h;

	screen_w = width * gap_w;
	screen_h = height * gap_h;
	screen = SDL_CreateWindow("Simplest ffmpeg player's Window", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED,
		screen_w, screen_h, SDL_WINDOW_OPENGL);

	if (!screen) {
		printf("SDL: could not create window - exiting:%s\n", SDL_GetError());
		return -1;
	}
	sdlRenderer = SDL_CreateRenderer(screen, -1, 0);
	//IYUV: Y + U + V  (3 planes)
	//YV12: Y + V + U  (3 planes)
	sdlTexture = SDL_CreateTexture(sdlRenderer, SDL_PIXELFORMAT_IYUV, SDL_TEXTUREACCESS_STREAMING, width*gap_w, height*gap_h);

	sdlRect.x = 0;
	sdlRect.y = 0;
	sdlRect.w = screen_w;
	sdlRect.h = screen_h;


	video_tid = SDL_CreateThread(sfp_refresh_thread, NULL, NULL);
	//------------SDL End------------

	AVFrame *pDstFrame = av_frame_alloc();
	int target_width = width, target_height = height;

	
	Produce_data *p = new Produce_data(pDstFrame, target_height, target_width, gap_h, gap_w);
	thread produce(ProduceFrame,p);
	FrameBuffer *curFrame;
	int T = 0;
	for (;;) {
		//Wait
		SDL_WaitEvent(&event);
		if (event.type == SFM_REFRESH_EVENT) {
			while (true)
			{
				curFrame = GetFrame();
				if (curFrame == NULL)
				{
					this_thread::sleep_for(chrono::microseconds(100));
				}
				else
				{
					break;
				}
			}
			T = T + 1;
			//printf("Render:%d\n", T);
			//y_size=pCodecCtx->width*pCodecCtx->height;

			//SDL---------------------------
			SDL_UpdateTexture(sdlTexture, NULL, curFrame->frame->data[0], curFrame->frame->linesize[0]);
			SDL_RenderClear(sdlRenderer);
			//SDL_RenderCopy( sdlRenderer, sdlTexture, &sdlRect, &sdlRect );
			SDL_RenderCopy(sdlRenderer, sdlTexture, NULL, NULL);
			SDL_RenderPresent(sdlRenderer);
			fq->Pop();
			/*
			for (int i = 0; i < tileNumber; i++)
			{
				printf("%d ", Tl[i]);
			}
			printf("\n");*/
			//SDL End-----------------------
		}
		else if (event.type == SDL_KEYDOWN) {
			printf("拖动！！！！！！！！！！！！！！！！！！！！！\n");
			//Pause
			if (event.key.keysym.sym == SDLK_SPACE)
				thread_pause = !thread_pause;
		}
		else if (event.type == SDL_QUIT) {
			thread_exit = 1;
		}
		else if (event.type == SFM_BREAK_EVENT) {
			break;
		}

	}



	SDL_Quit();

	return 0;
}

