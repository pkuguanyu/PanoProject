using FFmpeg.AutoGen;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using UnityPlayer;
using SDL2;
using System.Diagnostics;

namespace UnityPlayer
{   
    public unsafe class VRpalyer
    {

        public const SDL.SDL_EventType SFM_REFRESH_EVENT = (SDL.SDL_EventType.SDL_USEREVENT + 1);

        public const SDL.SDL_EventType SFM_BREAK_EVENT = (SDL.SDL_EventType.SDL_USEREVENT + 2);
        /// <summary>
        /// 指示当前解码是否在运行
        /// </summary>
        public bool IsRun { get; protected set; }
        /// <summary>
        /// 视频线程
        /// </summary>
        private Thread threadVideo;
        /// <summary>
        /// 音频线程
        /// </summary>
        private Thread threadAudio;
        /// <summary>
        /// 退出控制
        /// </summary>
        private bool exit_thread = false;
        /// <summary>
        /// 暂停控制
        /// </summary>
        private bool pause_thread = false;
        /// <summary>
        ///  视频输出流videoindex
        /// </summary>
        private int videoindex = -1;
        /// <summary>
        ///  音频输出流audioindex
        /// </summary>
        private int audioindex = -1;
        /// <summary>
        ///  当前渲染帧
        /// </summary>
        private AVFrame*[] cur=new AVFrame*[100];
        /// <summary>
        ///  切换Qp帧
        /// </summary>
        public int changeFrame=9001;
        /// <summary>
        ///  线程同步变量
        /// </summary>
        public bool[] isTrue=new bool[100];

        public Semaphore []semaphore = new Semaphore[100];

        public int []Resource = new int[100];
        public Mutex m = new Mutex();
        public int thread_exit = 0;
        public int thread_pause = 0;
        /// <summary>
        ///  线程同步变量
        /// </summary>
        public AutoResetEvent []CV;
        /// <summary>
        ///  tile的边界坐标结构体
        /// </summary>
        public struct Point
        {
            public int x;
            public int y;
        }
        /// <summary>
        ///  tile的坐标结构体
        /// </summary>
        public struct Tile
        {
            public Point lefttop;
            public Point rightdown;
            public Tile(int x1, int y1, int x2, int y2)
            {
                lefttop = new Point();
                rightdown = new Point();
                lefttop.x = x1;
                lefttop.y = y1;
                rightdown.x = x2;
                rightdown.y = y2;
            }
          
        }
        /// <summary>
        ///  帧队列，相当于buffer
        /// </summary>
        public FrameQueue fq;
        /// <summary>
        ///  线程变量传递结构体
        public struct Produce_data
        {
            public AVFrame* pDstFrame;
            public int target_height;
            public int target_width;
            public int gap_h;
            public int gap_w;

            Produce_data(AVFrame* pDstFram, int target_heigh, int target_widt, int gap_hh, int gap_ww)
            {
                pDstFrame = pDstFram;
                target_height = target_heigh;
                target_width = target_widt;
                gap_h = gap_hh;
                gap_w = gap_ww;
            }
            
        };
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
        public bool isFinished = false;
        public static int thread_count = 0;
     
        static void Main()
        {
            
            VRpalyer mp = new VRpalyer();
            SDLHelper s = new SDLHelper();

            int gap_h = 3;
            int gap_w = 3;
            int width = 240;
            int height = 240;
            s.SDL_Init(width * gap_w, height * gap_h);
            Thread videoPlayer = new Thread(() => mp.RunVideo(gap_h , gap_w, width, height, s));
            //videoPlayer.IsBackground = true;
            videoPlayer.Start();
            
            //mp.RunVideo(480,480, s);

        }

        void sfp_refresh_thread()
        {
            thread_exit = 0;
            thread_pause = 0;
            SDL.SDL_Event even = new SDL.SDL_Event();
            while (thread_exit==0)
            {
                if (thread_pause==0)
                {
                     even.type = SFM_REFRESH_EVENT;
                     SDL.SDL_PushEvent(ref even);
                }
                SDL.SDL_Delay(1);
            }
                thread_exit = 0;
	            thread_pause = 0;
            //Break
                 even.type = SFM_BREAK_EVENT;
                 SDL.SDL_PushEvent(ref even);


        }





    /// <summary>
    /// 视频H264转YUV并使用SDL进行播放
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="sdlVideo"></param>
    /// <returns></returns>
    /// 
        public unsafe int Decoder(string fileName,int tilenumber)  //这个是用来渲染小画面的
        {

            int error, frame_count = 0;
            int got_picture, ret;
            SwsContext* pSwsCtx = null;
            AVFormatContext* ofmt_ctx = null;
            IntPtr convertedFrameBufferPtr = IntPtr.Zero;
            try
            {
                // 注册编解码器
                ffmpeg.avcodec_register_all();
                // 获取文件信息上下文初始化
                ofmt_ctx = ffmpeg.avformat_alloc_context();

               
                // 打开媒体文件
                error = ffmpeg.avformat_open_input(&ofmt_ctx, fileName, null, null);
                if (error != 0)
                {
                    //  throw new ApplicationException(ffmpeg.FFmpegBinariesHelper.GetErrorMessage(error));
                    Console.WriteLine(fileName);
                    Console.WriteLine("打开失败");
                    return 0;
                }

                // 获取流的通道
                for (int i = 0; i < ofmt_ctx->nb_streams; i++)
                {
                    if (ofmt_ctx->streams[i]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                    {
                        videoindex = i;
                        Console.WriteLine("video.............." + videoindex);
                    }
                }

                if (videoindex == -1)
                {
                    Console.WriteLine("Couldn't find a video stream.（没有找到视频流）");
                    return -1;
                }

                // 视频流处理
                if (videoindex > -1)
                {
                    //获取视频流中的编解码上下文
                    AVCodecContext* pCodecCtx = ofmt_ctx->streams[videoindex]->codec;

                    //根据编解码上下文中的编码id查找对应的解码
                    AVCodec* pCodec = ffmpeg.avcodec_find_decoder(pCodecCtx->codec_id);
                    if (pCodec == null)
                    {
                        Console.WriteLine("没有找到编码器");
                        return -1;
                    }

                    //打开编码器
                    if (ffmpeg.avcodec_open2(pCodecCtx, pCodec, null) < 0)
                    {
                        Console.WriteLine("编码器无法打开");
                        return -1;
                    }

                    Console.WriteLine("Find a  video stream.channel=" + videoindex);

                    //输出视频信息
                    var format = ofmt_ctx->iformat->name->ToString();
                    var len = (ofmt_ctx->duration) / 1000000;
                    var width = pCodecCtx->width;
                    var height = pCodecCtx->height;
                    pCodecCtx->thread_count = 1;
                    Console.WriteLine("video format：" + format);
                    Console.WriteLine("video length：" + len);
                    Console.WriteLine("video width&height：width=" + width + " height=" + height);
                    Console.WriteLine("video codec name：" + pCodec->name->ToString());
                    
                    //准备读取
                    //AVPacket用于存储一帧一帧的压缩数据（H264）
                    //缓冲区，开辟空间
                    AVPacket* packet = (AVPacket*)ffmpeg.av_malloc((ulong)sizeof(AVPacket));

                    //AVFrame用于存储解码后的像素数据(YUV)
                    //内存分配
                    AVFrame* pFrame = ffmpeg.av_frame_alloc();
                    //YUV420
                    AVFrame* pFrameYUV = ffmpeg.av_frame_alloc();
                    //只有指定了AVFrame的像素格式、画面大小才能真正分配内存
                    //缓冲区分配内存
                    int out_buffer_size = ffmpeg.avpicture_get_size(AVPixelFormat.AV_PIX_FMT_YUV420P, pCodecCtx->width, pCodecCtx->height);
                    byte* out_buffer = (byte*)ffmpeg.av_malloc((ulong)out_buffer_size);
                    //初始化缓冲区
                    ffmpeg.avpicture_fill((AVPicture*)pFrameYUV, out_buffer, AVPixelFormat.AV_PIX_FMT_YUV420P, pCodecCtx->width, pCodecCtx->height);

                    //用于转码（缩放）的参数，转之前的宽高，转之后的宽高，格式等
                    SwsContext* sws_ctx = ffmpeg.sws_getContext(pCodecCtx->width, pCodecCtx->height, AVPixelFormat.AV_PIX_FMT_YUV420P /*pCodecCtx->pix_fmt*/, pCodecCtx->width, pCodecCtx->height, AVPixelFormat.AV_PIX_FMT_YUV420P, ffmpeg.SWS_BICUBIC, null, null, null);
                    int flag = 0;
                    while (ffmpeg.av_read_frame(ofmt_ctx, packet) >= 0)
                    {
                        if (flag == 0)
                        {
                            semaphore[tilenumber].WaitOne();
                        }
                      
                        //只要视频压缩数据（根据流的索引位置判断）
                        if (packet->stream_index == videoindex)
                        {
                            //解码一帧视频压缩数据，得到视频像素数据
                            ret = ffmpeg.avcodec_decode_video2(pCodecCtx, pFrame, &got_picture, packet);
                            if (ret < 0)
                            {
                                Console.WriteLine("视频解码错误");
                                return -1;
                            }
                            
                            // 读取解码后的帧数据
                            if (got_picture > 0)
                            { 
                                frame_count++;
                                //Console.WriteLine("视频帧数:第 " + frame_count + " 帧");
                                //AVFrame转为像素格式YUV420，宽高
                                ffmpeg.sws_scale(sws_ctx, pFrame->data, pFrame->linesize, 0, pCodecCtx->height, pFrameYUV->data, pFrameYUV->linesize);
                                //SDL播放YUV数据
                                if(frame_count>=changeFrame)
                                {
                                    ffmpeg.av_free_packet(packet);
                                    ffmpeg.avformat_close_input(&ofmt_ctx);
                                    ffmpeg.av_frame_free(&pFrameYUV);
                                    ffmpeg.av_frame_free(&pFrame);
                                    semaphore[tilenumber].Release();
                                    //ffmpeg.avcodec_close(pCodecCtx);
                                    ffmpeg.av_free(out_buffer);
                                    Console.WriteLine("当前视频解码完毕"+tilenumber.ToString());
                                    return 0;
                                }
                                flag = 0;
                                var data = out_buffer;
                                cur[tilenumber] = pFrameYUV;
                                isTrue[tilenumber] = false; // 代表当前帧渲染完毕

                               // sdlVideo.SDL_Display(pCodecCtx->width, pCodecCtx->height, (IntPtr)data, out_buffer_size, pFrameYUV->linesize[0]);
                            }
                            else
                            {
                                flag = 1;
                            }
                        }
                        else
                        {
                            flag = 1;
                        }
                        //释放资源
                        ffmpeg.av_free_packet(packet);
                        //添加等待
                   
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (&ofmt_ctx != null)
                {
                    ffmpeg.avformat_close_input(&ofmt_ctx);//关闭流文件 
                }

            }
            IsRun = false;
            return 0;
           

        }


        void Update_Tile(AVFrame* pDstFrame, ref AVFrame*[] Cur, ref Tile[] tile, int length, int width, int height) //width 和 height是整个画面的
        {
            int []temp=new int[100];
            int []tileIndex=new int[100];
            int temp_length, i, j;
            for (i = 0; i < height; i++)   //这个是每一行
            {
                int temp_width = 0;
                CheckTile(tile, length, i, ref temp,out temp_length, ref tileIndex);
                for (j = 0; j < temp_length; j++)
                {
                    // memcpy(pDstFrame->data[0] + temp_width + i * pDstFrame->linesize[0], (Cur[temp[j]])->data[0] + tileIndex[j] * Cur[temp[j]]->linesize[0], (uint)Cur[temp[j]]->linesize[0]);
                    CopyMemory((IntPtr)(pDstFrame->data[0] + temp_width + i * pDstFrame->linesize[0]), (IntPtr)(Cur[temp[j]]->data[0] + tileIndex[j] * Cur[temp[j]]->linesize[0]), (uint)Cur[temp[j]]->linesize[0]);                 
                    temp_width = temp_width + Cur[temp[j]]->linesize[0];
                    //Console.WriteLine(pDstFrame->linesize[0]);
                }
            }
            /*
            memset(tileIndex, 0, sizeof(tileIndex));
            for (i = 0; i < height; i++)   //这个是每一行
            {
                int temp_width = 0;
                CheckTile(tile, length, i, temp, temp_length, tileIndex);
                for (j = 0; j < temp_length; j++)
                {
                    memcpy(pDstFrame->data[0] + temp_width + i * pDstFrame->linesize[0], (Cur[temp[j]])->data[0] + tileIndex[j] * Cur[temp[j]]->linesize[0], Cur[temp[j]]->linesize[0]);
                    temp_width = temp_width + Cur[temp[j]]->linesize[0];
                }
            }*/
            
            for (i = 0; i < height / 2; i++)
            {
                int temp_width = 0;
                CheckTile_UV(tile, length, i, ref temp, out temp_length, ref tileIndex);
                for (j = 0; j < temp_length; j++)
                {
                    CopyMemory((IntPtr)(pDstFrame->data[1] + temp_width + i * pDstFrame->linesize[1]), (IntPtr)(Cur[temp[j]]->data[1] + tileIndex[j] * Cur[temp[j]]->linesize[1]), (uint)Cur[temp[j]]->linesize[1]);
                    temp_width = temp_width + Cur[temp[j]]->linesize[1];
                }

                temp_width = 0;
                for (j = 0; j < temp_length; j++)
                {
                    CopyMemory((IntPtr)(pDstFrame->data[2] + temp_width + i * pDstFrame->linesize[2]), (IntPtr)(Cur[temp[j]]->data[2] + tileIndex[j] * Cur[temp[j]]->linesize[2]), (uint)Cur[temp[j]]->linesize[2]);
                    temp_width = temp_width + Cur[temp[j]]->linesize[2];
                }

            }
        }
        void GetFrame(out FrameBuffer fb)
        {
       
            fq.GetTop(out fb);
        }
        //当前线程获取tile数量，以及具体tile的位置
        public unsafe void ProduceFrame(ref Produce_data p)   //不停的往队列里添加帧，到时间就自动切换
        {
            //初始创建一批decoder

            int height = p.target_height;
            int width = p.target_width;
            int gap_h = p.gap_h;
            int gap_w = p.gap_w;

            int tileNumber = gap_w * gap_h;
            tileNumber = tileNumber + 1;
               //暂时这么写
            int i, j;
            Tile []tile=new Tile[72]; //暂时使用方格
            for (i = 0; i < gap_h; i++)
            {
                for (j = 0; j < gap_w; j++)
                {
                    tile[i * gap_w + j+1 ] = new Tile(j * width, i * height, j * width + width, i * height + height);
                }
            }
             tile[0] = new Tile(0, 0, 2880, 1440);
            //tile[0] = new Tile(0, 0, 480, 480);
            //tile[1] = new Tile(0, 480, 480,960);


            int T = 0;  //每隔30帧重新启动decoder
            AVFrame* []pDstFrameBuffer=new AVFrame*[3];
            byte* []distBuffer=new byte*[3];
            
            for (i = 0; i < 3; i++)
            {
                AVFrame* ptempDstFrame;
                int target_width = width, target_height = height;
                int nDstSize = ffmpeg.avpicture_get_size(AVPixelFormat.AV_PIX_FMT_YUV420P, target_width * gap_w, target_height * gap_h);
                byte[] buf = new byte[nDstSize];
                fixed(byte* dsttempbuf = buf)
                {
                    ptempDstFrame = ffmpeg.av_frame_alloc();
                    ffmpeg.avpicture_fill((AVPicture*)ptempDstFrame, dsttempbuf, AVPixelFormat.AV_PIX_FMT_YUV420P, target_width * gap_w, target_height * gap_h);
                    ptempDstFrame->width = target_width * gap_w;
                    ptempDstFrame->height = target_height * gap_h;
                    ptempDstFrame->format = (int)AVPixelFormat.AV_PIX_FMT_YUV420P;
                    memset(ptempDstFrame->data[0], 0, (uint)(target_height * target_width * gap_w * gap_h));
                    memset(ptempDstFrame->data[1], 0x80, (uint)(target_height * target_width * gap_w * gap_h / 4));
                    memset(ptempDstFrame->data[2], 0x80, (uint)(target_height * target_width * gap_w * gap_h / 4));
                    pDstFrameBuffer[i] = ptempDstFrame;
                    distBuffer[i] = dsttempbuf;
                }
            }

            for (i = 0; i < tileNumber; i++)
            {
                semaphore[i] = new Semaphore(0, 1);
            }
            while (true)  //不停的向前进行decode,每一次循环产生一帧
            {
                FrameBuffer fb;
                AVFrame* tempFrame;
                //如果当前已经解完了就换个文件读，重新给decoder赋予新的文件名
                T = T + 1;
                if (T % (changeFrame - 1) == 1)
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
                        string temp1;
                        if (i==0)
                        {
                             temp1 = "base.mp4";// i.ToString()+".mp4";
                            //temp1 = i.ToString() + ".mp4";
                            //temp1 = "0.mp4";
                        }
                        else
                        {
                                  // temp1 = i.ToString() + ".mp4";
                            temp1 = "0.mp4";
                        }
                  
                     
                        int tn = i;
                        Thread t = new Thread(()=>Decoder(temp1, tn));
                        t.Start();
                    }
                    
                    //重新启动一堆解码器
                }
                tempFrame = pDstFrameBuffer[(T - 1) % 3];
                //告诉所有decoder可以进行解码了
                
                for (i = 0; i < tileNumber; i++)
                {
                    isTrue[i] = true;  //当所有Istrue都是false 的时候代表所有块都已经渲染完了
                    semaphore[i].Release();
                }
             
                  //第一帧让他渲染，渲染完之后他进入等待，等我放入队列后，再让他继续走！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
                  //
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

                    Thread.Sleep(1);
                }
                
                //渲染完了就进行拼接,update函数其实是拼接函数	

                Update_Tile(tempFrame,ref cur,ref tile, tileNumber, width * gap_w, height * gap_h);
                fb = new FrameBuffer();
                fb.av = *tempFrame;
                fb.outbuffer = distBuffer[(T - 1) % 3];
                //完成拼接放入队列
                while (true)
                {    
                    if (fq.Push(ref fb)==1)  //代表成功放入队列了
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }

                //Console.WriteLine("拼接第" + T.ToString() + "帧");

            }
        }
        void CheckTile(Tile[] tile, int length, int row, ref int[] temp, out int temp_length, ref int[] tileIndex) //用来确定当前行有多少个tile
        {
            int i;
            int k = 0;
            for (i = 0; i < length; i++)
            {
                if (tile[i].lefttop.y <= row && row < tile[i].rightdown.y)
                {

                    tileIndex[k] = row - tile[i].lefttop.y;
                    temp[k++] = i; //这些tile在当前行中		
                }

            }
            temp_length = k;
        }
        void CheckTile_UV(Tile[] tile, int length, int row, ref int[] temp, out int temp_length, ref int[] tileIndex) //用来确定当前行有多少个tile
        {
            int i;
            int k = 0;
            int width = 0;
            for (i = 0; i < length; i++)
            {
                if (tile[i].lefttop.y / 2 <= row && row < tile[i].rightdown.y / 2)
                {

                    tileIndex[k] = (row - tile[i].lefttop.y / 2);
                    temp[k++] = i; //这些tile在当前行中		
                }

            }
            temp_length = k;
        }

        public unsafe int RunVideo(int gap_h,int gap_w,int width,int height, SDLHelper sdlVideo)  //这个是用来渲染整个大画面的
        {
            int frame_count = 0;
            Produce_data p = new Produce_data();
            fq = new FrameQueue();
            p.gap_h = gap_h;
            p.gap_w = gap_w;
            p.target_height = height;
            p.target_width = width;
            IsRun = true;
            exit_thread = false;
            pause_thread = false;
            threadVideo = Thread.CurrentThread;
            SDL.SDL_Event even=new SDL.SDL_Event();
            Thread refresh = new Thread(() => sfp_refresh_thread());
            Thread produce = new Thread(() => ProduceFrame(ref p));
            refresh.Start();
            produce.Start();
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            try
            {

                for (; ; )
                {
                    //Wait
                    SDL.SDL_WaitEvent(out even);
                    if (even.type == SFM_REFRESH_EVENT)
                    {
                            FrameBuffer fb;
                            // 退出线程
                            if (exit_thread)
                            {
                                break;
                            }
                            // 暂停解析
                            if (pause_thread)
                            {
                                while (pause_thread)
                                {
                                    Thread.Sleep(1);
                                }
                            }

                            while (true)
                            {

                                GetFrame(out fb);
                                if (fb == null)
                                {
                                    Thread.Sleep(1);
                                }
                                else
                                {
                                    fq.Pop();
                                    break;
                                }
                                // Console.WriteLine("尚味获取到帧");
                            }
                            frame_count++;
                        //SDL播放YUV数据
                        //var data = fb.outbuffer;

                      
                        int out_buffer_size = fb.outbuffersize;
                        AVFrame pFrameYUV = fb.av;
                        sdlVideo.SDL_Display(width*gap_w, height*gap_h, (IntPtr)(pFrameYUV.data[0]), out_buffer_size, pFrameYUV.linesize[0]);
                          
                        
                       //  Console.WriteLine("渲染第" + frame_count.ToString() + "帧");
                        if(frame_count==1)
                        {
                            stopwatch.Start();
                        }
                        if (frame_count == 1000)
                        { 
                                stopwatch.Stop();
                                TimeSpan timespan = stopwatch.Elapsed;
                                Console.WriteLine(timespan.TotalSeconds.ToString());
                                Thread.Sleep(100000);
                        }


                    }
                    else if (even.type == SDL.SDL_EventType.SDL_KEYDOWN)
                    {
                      
                        //Pause
                        if (even.key.keysym.sym == SDL.SDL_Keycode.SDLK_SPACE)
                            thread_pause = 1-thread_pause;
                    }
                    else if (even.type == SDL.SDL_EventType.SDL_QUIT)
                    {
                        thread_exit = 1;
                    }
                    else if (even.type == SFM_BREAK_EVENT)
                    {
                        break;
                    }

                 }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
              

            }
            IsRun = false;
            return 0;
        }

        /// <summary>
        /// 音频AAC转PCM并使用SDL进行播放
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sdlAudio"></param>
        /// <returns></returns>
        


        /// <summary>
        /// 开启线程
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sdlVideo"></param>
        public void Start(int width,int height, SDLHelper sdlVideo)
        {
            // 视频线程
            threadVideo = new Thread(() =>
            {
                try
                {
                    RunVideo(1,1,width,height, sdlVideo);
                }
                catch (Exception ex)
                {
                    //SQ.Base.ErrorLog.WriteLog4Ex("JT1078CodecForMp4.Run Video", ex);
                    Console.WriteLine(ex.ToString());
                    return;
                }
            });
            threadVideo.IsBackground = true;
            threadVideo.Start();

            /*
            // 音频线程
            threadAudio = new Thread(() =>
            {
                try
                {
                    RunAudio(fileName, sdlAudio);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }
            });
            threadAudio.IsBackground = true;
            threadAudio.Start();
            */
        }

        /// <summary>
        /// 暂停继续
        /// </summary>
        public void GoOn()
        {
            pause_thread = false;

        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            pause_thread = true;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            exit_thread = true;
        }

        // 标准仿C memcpy函数
        unsafe public void* memcpy(void* dst, void* src, uint count)
        {
            System.Diagnostics.Debug.Assert(dst != null);
            System.Diagnostics.Debug.Assert(src != null);

            void* ret = dst;

            /*
            * copy from lower addresses to higher addresses
            */
            while (count-- > 0)
            {
                *(char*)dst = *(char*)src;
                dst = (char*)dst + 1;
                src = (char*)src + 1;
            }

            return (ret);
        }

        // 标准仿C memmove函数
            unsafe public void* memmove(void* dst, void* src, uint count)
            {
                System.Diagnostics.Debug.Assert(dst != null);
                System.Diagnostics.Debug.Assert(src != null);

                void* ret = dst;

                if (dst <= src || (char*)dst >= ((char*)src + count))
                {
                    while (count-- > 0)
                    {
                        *(char*)dst = *(char*)src;
                        dst = (char*)dst + 1;
                        src = (char*)src + 1;
                    }
                }
                else
                {
                    dst = (char*)dst + count - 1;
                    src = (char*)src + count - 1;
                    while (count-- > 0)
                    {
                        *(char*)dst = *(char*)src;
                        dst = (char*)dst - 1;
                        src = (char*)src - 1;
                    }
                }

                return (ret);
            }

        // 标准仿C memset函数
        unsafe public void* memset(void* s, int c, uint n)
        {
            byte* p = (byte*)s;

            while (n > 0)
            {
                *p++ = (byte)c;
                --n;
            }

            return s;
        }






    }
}
