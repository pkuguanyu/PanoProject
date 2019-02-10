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
	T(int x1, int y1, int x2, int y2)
	{
		lefttop.x = x1;
		lefttop.y = y1;
		rightdown.x = x2;
		rightdown.y = y2;
	}
	Point lefttop;
	Point rightdown;
}Tile;


int thread_exit = 0;
int thread_pause = 0;
bool isTrue[MAX_TILE];
int graph[MAX_TILE];
std::mutex mtx;
std::condition_variable cv[MAX_TILE];
vector<thread> v;
AVFrame	*Cur[MAX_TILE]; //先做两个tile的
AVPacket *packetCur[MAX_TILE];
int Tl[MAX_TILE];
int Wait[MAX_TILE];
Tile tile[MAX_TILE];
int width = 480;
int height = 480;
FrameQueue *fq;




int sfp_refresh_thread(void *opaque) {
	thread_exit = 0;
	thread_pause = 0;

	while (!thread_exit) {
		if (!thread_pause) {
			SDL_Event event;
			event.type = SFM_REFRESH_EVENT;
			SDL_PushEvent(&event);
		}
		SDL_Delay(40);
	}
	thread_exit = 0;
	thread_pause = 0;
	//Break
	SDL_Event event;
	event.type = SFM_BREAK_EVENT;
	SDL_PushEvent(&event);

	return 0;
}
void CheckTile(Tile *tile, int length, int row, Tile *temp, int &temp_length, int *tileIndex) //用来确定当前行有多少个tile
{
	int i;
	int k = 0;
	int width = 0;
	for (i = 0; i < length; i++)
	{
		if (tile[i].lefttop.x >= row && tile[i].rightdown.x <= row)
		{

			tileIndex[k] = row - tile[i].lefttop.x;
			temp[k++] = tile[i];
		}

	}
	temp_length = k;
}

//当前线程获取tile数量，以及具体tile的位置
void ProduceFrame(int target_height, int target_width, int gap_h, int gap_w)   //不停的往队列里添加帧，到时间就自动切换
{
	AVFrame * pDstFrame;
	//初始创建一批decoder
	Tile *tile;
	int tileNumber;
	int T = 0;  //每隔30帧重新启动decoder
	while (true)  //不停的向前进行decoder
	{
		//如果当前已经解完了就换个文件读，重新给decoder赋予新的文件名
		T = T + 1;
		if (T % 30 == 1)
		{
			//每隔30帧自动的去更换一个文件去读
			v.clear();
			for (int i = 0; i < tileNumber; i++)
			{
				isTrue[i] = false;
				v.emplace_back(CreateDecoder, i);
			}
			//重新启动一堆解码器
		}
		//告诉所有decoder可以进行解码了
		for (int i = 0; i < tileNumber; i++)
		{
			unique_lock<mutex> loc(mtx);
			isTrue[i] = true;  //当所有Istrue都是false 的时候代表所有块都已经渲染完了
			cv[i].notify_all();
		}

		int pass = 1;
		while (true)
		{
			pass = 1;
			for (int i = 0; i < tileNumber; i++)
			{
				if (isTrue[i] == true) pass = 0;
			}
			if (pass == 1)
				break;

			this_thread::sleep_for(chrono::microseconds(1));
		}

		//渲染完了就进行拼接,update函数其实是拼接函数	
		Update_Tile(pDstFrame, Cur, tile, tileNumber, width*gap_w, height*gap_h);
		//完成拼接放入队列
		while (true)
		{
			if (fq->Push(pDstFrame) == 1)  //代表成功放入队列了
			{
				break;
			}
			this_thread::sleep_for(chrono::microseconds(100));

		}
	}
}


AVFrame* GetFrame()
{
	AVFrame *temp = fq->Pop();
	if (temp == NULL)  //代表没有获得画面
	{
		return  NULL;	//返回空暂停先别放
	}
	else
	{
		return temp;
	}

}




//每一帧调用一次 ，用于合并tile
void Update_Tile(AVFrame * pDstFrame, AVFrame **Cur, Tile *tile, int length, int width, int height) //width 和 height是整个画面的
{
	int nYIndex = 0;
	Tile temp[MAX_TILE];
	int tileIndex[MAX_TILE];
	int temp_length, i, j;
	memset(tileIndex, 0, sizeof(tileIndex));
	for (i = 0; i < height; i++)   //这个是每一行
	{
		int temp_width = 0;
		CheckTile(tile, length, i, temp, temp_length, tileIndex);
		for (j = 0; j < temp_length; j++)
		{
			memcpy(pDstFrame->data[0] + temp_width + i * pDstFrame->linesize[0], (Cur[j])->data[0] + tileIndex[j] * Cur[j]->linesize[0], Cur[j]->linesize[0]);
			temp_width = temp_width + Cur[j]->linesize[0];
		}
	}

	for (i = 0; i < height / 2; i++)
	{
		int temp_width = 0;
		CheckTile(tile, length, i, temp, temp_length, tileIndex);
		for (j = 0; j < temp_length; j++)
		{
			memcpy(pDstFrame->data[1] + temp_width + i * pDstFrame->linesize[1], (Cur[j])->data[1] + tileIndex[j] * Cur[j]->linesize[1], Cur[j]->linesize[1]);
			temp_width = temp_width + Cur[j]->linesize[1];
		}

		temp_width = 0;
		for (j = 0; j < temp_length; j++)
		{
			memcpy(pDstFrame->data[2] + temp_width + i * pDstFrame->linesize[2], (Cur[j])->data[2] + tileIndex[j] * Cur[j]->linesize[2], Cur[j]->linesize[2]);
			temp_width = temp_width + Cur[j]->linesize[2];
		}

	}
}
void  CreateDecoder(int tilenumber)
{

	int T = 0;
	int tag = 1;
	AVFormatContext	*pFormatCtx;
	int				i, videoindex;
	AVCodecContext	*pCodecCtx;
	AVCodec			*pCodec;
	AVFrame	*pFrame, *pFrameYUV;
	unsigned char *out_buffer;
	AVPacket *packet;
	int ret, got_picture;
	struct SwsContext *img_convert_ctx;

	//char filepath[]="bigbuckbunny_480x272.h265";
	char *filepath;
	if (tilenumber == 0)
	{
		char *temp = "0.mp4";
		filepath = temp;
		//tag = 0;
	}

	if (tilenumber == 1)
	{
		char *temp = "1.mp4";
		filepath = temp;
	}

	if (tilenumber == 2)
	{
		char *temp = "0.mp4";
		filepath = temp;
		tag = 0;
		//tilenumber = 0;
	}

	if (tilenumber == 3)
	{
		char *temp = "1.mp4";
		filepath = temp;
		tag = 0;
		tilenumber = 0;
	}
	//av_frame_free(&Cur[tilenumber]);

	av_register_all();
	avformat_network_init();
	pFormatCtx = avformat_alloc_context();

	if (avformat_open_input(&pFormatCtx, filepath, NULL, NULL) != 0) {
		printf("Couldn't open input stream.\n");
		return;
	}
	if (avformat_find_stream_info(pFormatCtx, NULL) < 0) {
		printf("Couldn't find stream information.\n");
		return;
	}
	videoindex = -1;
	for (i = 0; i < pFormatCtx->nb_streams; i++)
		if (pFormatCtx->streams[i]->codec->codec_type == AVMEDIA_TYPE_VIDEO) {
			videoindex = i;
			break;
		}
	if (videoindex == -1) {
		printf("Didn't find a video stream.\n");
		return;
	}
	pCodecCtx = pFormatCtx->streams[videoindex]->codec;
	pCodecCtx->thread_count = 8;
	pCodec = avcodec_find_decoder(pCodecCtx->codec_id);
	if (pCodec == NULL) {
		printf("Codec not found.\n");
		return;
	}
	if (avcodec_open2(pCodecCtx, pCodec, NULL) < 0) {
		printf("Could not open codec.\n");
		return;
	}
	pFrame = av_frame_alloc();
	pFrameYUV = av_frame_alloc();

	out_buffer = (unsigned char *)av_malloc(av_image_get_buffer_size(AV_PIX_FMT_YUV420P, pCodecCtx->width, pCodecCtx->height, 1));
	av_image_fill_arrays(pFrameYUV->data, pFrameYUV->linesize, out_buffer,
		AV_PIX_FMT_YUV420P, pCodecCtx->width, pCodecCtx->height, 1);

	//Output Info-----------------------------
	printf("---------------- File Information ---------------\n");
	av_dump_format(pFormatCtx, 0, filepath, 0);
	printf("-------------------------------------------------\n");

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
			return;
		}
		if (got_picture) {
			//sws_scale(img_convert_ctx, (const uint8_t* const*)pFrame->data, pFrame->linesize, 0, pCodecCtx->height,
				//pFrameYUV->data, pFrameYUV->linesize);
			sws_scale(img_convert_ctx, (const unsigned char* const*)pFrame->data, pFrame->linesize, 0, pCodecCtx->height, pFrameYUV->data, pFrameYUV->linesize);
			T = T + 1;
			Tl[tilenumber] = T;
			printf("%d\n", T);
			if (T >= 999)
			{
				if (tilenumber == 0 && tag == 1)
				{
					isTrue[tilenumber] = true;
					flag = 1;
					av_free_packet(packet);
					av_frame_free(&pFrameYUV);
					av_frame_free(&pFrame);
					avcodec_close(pCodecCtx);
					avformat_close_input(&pFormatCtx);
					v.emplace_back(CreateDecoder, 3);
					return;
				}

			}
			flag = 0;
			Cur[tilenumber] = pFrameYUV;
			packetCur[tilenumber] = packet;
			isTrue[tilenumber] = false;
		}
		else {
			av_free_packet(packet);
			flag = 1;
		}
	}
	return;

}   //重载函数 ，测试用



void  CreateDecoder_Real(int tilenumber, char *filepath)  //实际函数
{

	int T = 0;
	int tag = 1;
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

	if (avformat_open_input(&pFormatCtx, filepath, NULL, NULL) != 0) {
		printf("Couldn't open input stream.\n");
		return;
	}
	if (avformat_find_stream_info(pFormatCtx, NULL) < 0) {
		printf("Couldn't find stream information.\n");
		return;
	}
	videoindex = -1;
	for (i = 0; i < pFormatCtx->nb_streams; i++)
		if (pFormatCtx->streams[i]->codec->codec_type == AVMEDIA_TYPE_VIDEO) {
			videoindex = i;
			break;
		}
	if (videoindex == -1) {
		printf("Didn't find a video stream.\n");
		return;
	}
	pCodecCtx = pFormatCtx->streams[videoindex]->codec;
	pCodecCtx->thread_count = 8;
	pCodec = avcodec_find_decoder(pCodecCtx->codec_id);
	if (pCodec == NULL) {
		printf("Codec not found.\n");
		return;
	}
	if (avcodec_open2(pCodecCtx, pCodec, NULL) < 0) {
		printf("Could not open codec.\n");
		return;
	}
	pFrame = av_frame_alloc();
	pFrameYUV = av_frame_alloc();

	out_buffer = (unsigned char *)av_malloc(av_image_get_buffer_size(AV_PIX_FMT_YUV420P, pCodecCtx->width, pCodecCtx->height, 1));
	av_image_fill_arrays(pFrameYUV->data, pFrameYUV->linesize, out_buffer,
		AV_PIX_FMT_YUV420P, pCodecCtx->width, pCodecCtx->height, 1);

	//Output Info-----------------------------
	printf("---------------- File Information ---------------\n");
	av_dump_format(pFormatCtx, 0, filepath, 0);
	printf("-------------------------------------------------\n");

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
			return;
		}
		if (got_picture) {
			sws_scale(img_convert_ctx, (const unsigned char* const*)pFrame->data, pFrame->linesize, 0, pCodecCtx->height, pFrameYUV->data, pFrameYUV->linesize);
			T = T + 1;
			Tl[tilenumber] = T;
			printf("%d\n", T);
			if (T >= 31)
			{
				av_free_packet(packet);
				av_frame_free(&pFrameYUV);
				av_frame_free(&pFrame);
				avcodec_close(pCodecCtx);
				avformat_close_input(&pFormatCtx);
				return;  //释放当前decoder
			}
			flag = 0;
			Cur[tilenumber] = pFrameYUV;
			packetCur[tilenumber] = packet;
			isTrue[tilenumber] = false;
		}
		else {
			av_free_packet(packet);
			flag = 1;
		}
	}
	return;

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
	FILE *fp_yuv = fopen("output.yuv", "wb+");



	if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_AUDIO | SDL_INIT_TIMER)) {
		printf("Could not initialize SDL - %s\n", SDL_GetError());
		return -1;
	}

	int gap_w = 1, gap_h = 2;
	int tileNumber = gap_w * gap_h;


	for (int i = 0; i < tileNumber; i++)
	{
		isTrue[i] = false;
		v.emplace_back(CreateDecoder, i);
	}

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
	//Event Loop

	AVFrame *pDstFrame = av_frame_alloc();
	int target_width = width, target_height = height;

	int nDstSize = avpicture_get_size(AV_PIX_FMT_YUV420P, target_width*gap_w, target_height*gap_h);
	uint8_t *dstbuf = new uint8_t[nDstSize];
	avpicture_fill((AVPicture*)pDstFrame, dstbuf, AV_PIX_FMT_YUV420P, target_width*gap_w, target_height*gap_h);

	pDstFrame->width = target_width * gap_w;
	pDstFrame->height = target_height * gap_h;
	pDstFrame->format = AV_PIX_FMT_YUV420P;
	memset(pDstFrame->data[0], 0, target_height*target_width*gap_w*gap_h);
	memset(pDstFrame->data[1], 0x80, target_height*target_width*gap_w*gap_h / 4);
	memset(pDstFrame->data[2], 0x80, target_height*target_width*gap_w*gap_h / 4);


	for (;;) {
		//Wait

		SDL_WaitEvent(&event);
		if (event.type == SFM_REFRESH_EVENT) {

			for (int i = 0; i < tileNumber; i++)
			{
				unique_lock<mutex> loc(mtx);
				isTrue[i] = true;
				cv[i].notify_all();
			}

			int pass = 1;
			while (true)
			{
				pass = 1;
				for (int i = 0; i < tileNumber; i++)
				{
					if (isTrue[i] == true) pass = 0;
				}
				if (pass == 1)
					break;

				this_thread::sleep_for(chrono::microseconds(1));
			}

			int nYIndex = 0;
			int nUVIndex = 0;


			for (int i = 0; i < target_height*gap_h; i++)
			{
				if (i%target_height == 0)
				{
					nYIndex = 0;
				}
				for (int j = 0; j < gap_w; j++)
				{
					memcpy(pDstFrame->data[0] + j * target_width + i * target_width * gap_w, (Cur[j])->data[0] + nYIndex * target_width, target_width);
					//memcpy(pDstFrame->data[0] + target_width + i*target_width * 2, pFrameYUV->data[0] + nYIndex*target_width, target_width);

				}
				nYIndex++;

			}

			for (int i = 0; i < target_height *gap_h / 2; i++)
			{
				if (i % (target_height / 2) == 0)
				{
					nUVIndex = 0;
				}
				//for (int j = 0; j < gap_w; j++)
				//{

				memcpy(pDstFrame->data[1] + i * pDstFrame->linesize[1], (Cur[0])->data[1] + Cur[0]->linesize[1] * nUVIndex, Cur[0]->linesize[1]);
				memcpy(pDstFrame->data[2] + i * pDstFrame->linesize[2], (Cur[0])->data[2] + Cur[0]->linesize[2] * nUVIndex, Cur[0]->linesize[2]);

				memcpy(pDstFrame->data[1] + Cur[0]->linesize[1] + i * pDstFrame->linesize[1], (Cur[1])->data[1] + Cur[1]->linesize[1] * nUVIndex, Cur[1]->linesize[1]);
				memcpy(pDstFrame->data[2] + Cur[0]->linesize[2] + i * pDstFrame->linesize[2], (Cur[1])->data[2] + Cur[1]->linesize[2] * nUVIndex, Cur[1]->linesize[2]);

				// memcpy(pDstFrame->data[1] + Cur[0]->linesize[1] + Cur[1]->linesize[1] + i * pDstFrame->linesize[1], (Cur[2])->data[1] + Cur[2]->linesize[1] * nUVIndex, Cur[2]->linesize[1]);
				// memcpy(pDstFrame->data[2] + Cur[0]->linesize[2] + Cur[1]->linesize[2] +i * pDstFrame->linesize[2], (Cur[2])->data[2] + Cur[2]->linesize[2] * nUVIndex, Cur[2]->linesize[2]);



			//}
			//U
				nUVIndex++;
			}

			//y_size=pCodecCtx->width*pCodecCtx->height;
			//SDL---------------------------
			SDL_UpdateTexture(sdlTexture, NULL, pDstFrame->data[0], pDstFrame->linesize[0]);
			SDL_RenderClear(sdlRenderer);
			//SDL_RenderCopy( sdlRenderer, sdlTexture, &sdlRect, &sdlRect );
			SDL_RenderCopy(sdlRenderer, sdlTexture, NULL, NULL);
			SDL_RenderPresent(sdlRenderer);



			/*
			for (int i = 0; i < tileNumber; i++)
			{
				printf("%d ", Tl[i]);
			}
			printf("\n");*/
			//SDL End-----------------------
		}
		else if (event.type == SDL_KEYDOWN) {
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

