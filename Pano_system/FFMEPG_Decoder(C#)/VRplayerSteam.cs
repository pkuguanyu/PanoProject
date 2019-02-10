using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFmpeg.AutoGen;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Text;

public unsafe class VRplayer : MonoBehaviour
{

    // Use this for initialization
    private Material yuvm;
    private int gap_h = 1;
    private int gap_w = 1;
    private int width = 2880;
    private int height = 1440;
    Texture2D t = null;
    Texture2D texY = null;
    Texture2D texU = null;
    Texture2D texV = null;
    byte[] Y_raw;
    byte[] U_raw;
    byte[] V_raw;
    private int show_flag = 5;  
    void Start()
    {
        Thread videoPlayer = new Thread(() => RunVideo(gap_h, gap_w, width, height));
        videoPlayer.Start();  //启动video渲染
        videoPlayer.IsBackground = true;
        yuvm = GameObject.FindGameObjectWithTag("BaseLayer").GetComponent<MeshRenderer>().material;
        fq = new FrameQueue();
        texY = new Texture2D(width * gap_w, height * gap_h, TextureFormat.Alpha8, false);
        texU = new Texture2D(width * gap_w / 2, height * gap_h / 2, TextureFormat.Alpha8, false);
        texV = new Texture2D(width * gap_w / 2, height * gap_h / 2, TextureFormat.Alpha8, false);
        Y_raw = new byte[width * gap_w * height * gap_h];
        U_raw = new byte[width * gap_w * height * gap_h / 4];
        V_raw = new byte[width * gap_w * height * gap_h / 4];
        currentPath = Application.dataPath;
        StartCoroutine(Refresh());
    }


    AVFrame pFrameYUV;
    FrameBuffer fb = null;

    void sfp()
    {
        GetFrame(out fb);
        if (fb == null)
        {
            Debug.Log("尚未获取到帧");
        }
        else
        {
            frameNow++;
            pFrameYUV = fb.av;
            if (pFrameYUV.data[0] != null)
            {
                GetTexture_Frame(pFrameYUV.data[0], pFrameYUV.linesize[0], pFrameYUV.data[1], pFrameYUV.linesize[1], pFrameYUV.data[2], pFrameYUV.linesize[2], gap_w * width, gap_h * height);
            }
           // Debug.Log("渲染第" + frameNow.ToString() + "帧");
            fq.Pop();
        }

    }
    IEnumerator Refresh()
    {
        while (true)
        {
            sfp();
            yield return new WaitForSecondsRealtime(0.033f);
            // Debug.Log("尚味获取到帧");
        }
    }

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
    private AVFrame*[] cur = new AVFrame*[100];
    /// <summary>
    ///  切换Qp帧
    /// </summary>
    public int changeFrame = 29;
    /// <summary>
    ///  线程同步变量
    /// </summary>
    private int frameNow = 0;
    private bool[] isTrue = new bool[100];
    private AutoResetEvent[] Ar = new AutoResetEvent[100];
    private Mutex m = new Mutex();
    private int thread_exit = 0;
    private int thread_pause = 0;
    private string currentPath;
    /// <summary>
    ///  线程同步变量
    /// </summary>
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
    private Semaphore[] semaphore = new Semaphore[100];
    /// <summary>
    ///  帧队列，相当于buffer
    /// </summary>
    private FrameQueue fq;
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
    private static readonly object tLock = new object();


    /// <summary>
    /// 视频H264转YUV并使用SDL进行播放
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="sdlVideo"></param>
    /// <returns></returns>
    /// 

    public unsafe int Decoder(string fileName, int tilenumber)  //这个是用来渲染小画面的
    {

        int error, frame_count = 0;
        int got_picture, ret;
        SwsContext* pSwsCtx = null;
        AVFormatContext* ofmt_ctx = null;
        IntPtr convertedFrameBufferPtr = IntPtr.Zero;
        show_flag = 1;
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
                Debug.Log(fileName);
                Debug.Log("打开失败");
                show_flag = 1;
                return 0;
            }

            // 获取流的通道
            for (int i = 0; i < ofmt_ctx->nb_streams; i++)
            {
                if (ofmt_ctx->streams[i]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                {
                    videoindex = i;
                    Debug.Log("video.............." + videoindex);
                }
            }

            if (videoindex == -1)
            {
                Debug.Log("Couldn't find a video stream.（没有找到视频流）");
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
                    Debug.Log("没有找到编码器");
                    show_flag = 2;
                    return -1;
                }

                //打开编码器
                if (ffmpeg.avcodec_open2(pCodecCtx, pCodec, null) < 0)
                {
                    Debug.Log("编码器无法打开");
                    show_flag = 3;
                    return -1;
                }

                Debug.Log("Find a  video stream.channel=" + videoindex);

                //输出视频信息
                var format = ofmt_ctx->iformat->name->ToString();
                var len = (ofmt_ctx->duration) / 1000000;
                var width = pCodecCtx->width;
                var height = pCodecCtx->height;
                pCodecCtx->thread_count = 8;
                /*
                Debug.Log("video format：" + format);
                Debug.Log("video length：" + len);
                Debug.Log("video width&height：width=" + width + " height=" + height);
                Debug.Log("video codec name：" + pCodec->name->ToString());
                */
                show_flag = 0;
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
                            Debug.Log("视频解码错误");
                            return -1;
                        }

                        // 读取解码后的帧数据
                        if (got_picture > 0)
                        {
                            frame_count++;
                            Debug.Log("视频帧数:第 " + frame_count + " 帧");
                            //AVFrame转为像素格式YUV420，宽高
                            ffmpeg.sws_scale(sws_ctx, pFrame->data, pFrame->linesize, 0, pCodecCtx->height, pFrameYUV->data, pFrameYUV->linesize);
                            //SDL播放YUV数据
                            if (frame_count >= changeFrame)
                            {
                                semaphore[tilenumber].Release();
                                ffmpeg.av_free_packet(packet);
                                ffmpeg.avformat_close_input(&ofmt_ctx);
                                ffmpeg.av_frame_free(&pFrameYUV);
                                ffmpeg.av_frame_free(&pFrame);
                                //ffmpeg.avcodec_close(pCodecCtx);*/
                                ffmpeg.av_free(out_buffer);
                                Debug.Log("当前视频解码完毕" + tilenumber.ToString());
                                return 0;
                            }
                            flag = 0;
                            cur[tilenumber] = pFrameYUV;
                            isTrue[tilenumber] = false; // 代表当前帧渲染完毕


                        }
                        else
                        {
                            flag = 1;
                        }
                        ffmpeg.av_free_packet(packet);
                    }
                    else
                    {
                        flag = 1;
                    }
                    //释放资源

                    //添加等待

                }

            }

        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            Thread.ResetAbort();
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

    int[] temp = new int[100];
    int[] tileIndex = new int[100];
    int[] tilewidhIndex = new int[100];
    void Update_Tile(ref AVFrame pDstFrame, AVFrame*[] Cur, Tile[] tile, int length, int width, int height) //width 和 height是整个画面的
    {

        int temp_length, i, j;
        int temp_width = 0;

        
        for (i = 0; i < height; i++)   //这个是每一行
        {
            temp_width = 0;
            CheckTile(tile, length, i, ref temp, out temp_length, ref tileIndex);
            for (j = 0; j < temp_length; j++)
            {

                temp_width = tile[temp[j]].lefttop.x;
               // memcpy(pDstFrame.data[0] + temp_width + i * pDstFrame.linesize[0], (Cur[temp[j]])->data[0] + tileIndex[j] * Cur[temp[j]]->linesize[0], (uint)Cur[temp[j]]->linesize[0]);
                 CopyMemory((IntPtr)(pDstFrame.data[0] + temp_width + i * pDstFrame.linesize[0]), (IntPtr)(Cur[temp[j]]->data[0] + tileIndex[j] * Cur[temp[j]]->linesize[0]), (uint)Cur[temp[j]]->linesize[0]);
                //Debug.Log(pDstFrame.linesize[0]);
            }
        }
        
        for (i = 0; i < height / 2; i++)
        {
            temp_width = 0;
            CheckTile_UV(tile, length, i, ref temp, out temp_length, ref tileIndex);
            for (j = 0; j < temp_length; j++)
            {
                temp_width = tile[temp[j]].lefttop.x/2;
                CopyMemory((IntPtr)(pDstFrame.data[1] + temp_width + i * pDstFrame.linesize[1]), (IntPtr)(Cur[temp[j]]->data[1] + tileIndex[j] * Cur[temp[j]]->linesize[1]), (uint)Cur[temp[j]]->linesize[1]);
            }

            temp_width = 0;
            for (j = 0; j < temp_length; j++)
            {
                temp_width = tile[temp[j]].lefttop.x /2;
                CopyMemory((IntPtr)(pDstFrame.data[2] + temp_width + i * pDstFrame.linesize[2]), (IntPtr)(Cur[temp[j]]->data[2] + tileIndex[j] * Cur[temp[j]]->linesize[2]), (uint)Cur[temp[j]]->linesize[2]);
            }

        }
    }
    void GetFrame(out FrameBuffer fb)
    {
        fq.GetTop(out fb);
    }


    private Tile[] tile = new Tile[72]; //暂时使用方格
    private int[] tileIdx = new int[72];
    private int[] qp = new int[72];
    //当前线程获取tile数量，以及具体tile的位置
    public unsafe void ProduceFrame(ref Produce_data p)   //不停的往队列里添加帧，到时间就自动切换
    {
        //初始创建一批decoder

        int height = p.target_height;
        int width = p.target_width;
        int gap_h = p.gap_h;
        int gap_w = p.gap_w;

        int tileNumber=1;
        //暂时这么写
        int i, j;

        Debug.Log("启动帧生成！");

        int T = 0;  //每隔30帧重新启动decoder
        AVFrame[] pDstFrameBuffer = new AVFrame[3];
        byte*[] distBuffer = new byte*[3];
        AVFrame* ptempDstFrame;
        int target_width = width, target_height = height;
        int nDstSize = ffmpeg.avpicture_get_size(AVPixelFormat.AV_PIX_FMT_YUV420P, target_width * gap_w, target_height * gap_h);
        byte[] buf;

        show_flag = 1;
        for (i = 0; i < 3; i++)
        {
            buf = new byte[nDstSize];
            //fixed (byte* dsttempbuf = buf)
            {
                IntPtr dsttempbuf = Marshal.AllocHGlobal(buf.Length);
                Marshal.Copy(buf, 0, dsttempbuf, buf.Length);
                ptempDstFrame = ffmpeg.av_frame_alloc();
                ffmpeg.avpicture_fill((AVPicture*)ptempDstFrame, (byte*)dsttempbuf, AVPixelFormat.AV_PIX_FMT_YUV420P, target_width * gap_w, target_height * gap_h);
                ptempDstFrame->width = target_width * gap_w;
                ptempDstFrame->height = target_height * gap_h;
                ptempDstFrame->format = (int)AVPixelFormat.AV_PIX_FMT_YUV420P;
                memset(ptempDstFrame->data[0], 0, (uint)(target_height * target_width * gap_w * gap_h));
                memset(ptempDstFrame->data[1], 0x80, (uint)(target_height * target_width * gap_w * gap_h / 4));
                memset(ptempDstFrame->data[2], 0x80, (uint)(target_height * target_width * gap_w * gap_h / 4));
                pDstFrameBuffer[i] = *ptempDstFrame;
                distBuffer[i] = (byte*)dsttempbuf;
            }
        }

        FrameBuffer fb;
        AVFrame tempFrame;
        int pts = 0;
        while (true)  //不停的向前进行decode,每一次循环产生一帧
        {

            //如果当前已经解完了就换个文件读，重新给decoder赋予新的文件名
            T = T + 1;
            Debug.Log("拼接第" + T.ToString() + "帧");
            if (T % (changeFrame - 1) == 1)
            {
                //每隔30帧自动的去更换一个文件去读
                //v.clear();
                //这里要获取新的tilenumber和tile方式
                //0号tile是背景tile，其他1-X号是中间的
                //segment命名方式为 秒数_tile_Qp
                T = 1;
                int videoid = 6;
                int offset = 70;
                string path = currentPath + "/StreamingAssets/"+videoid+""+"_trace/VRJND/"+""+(int)(pts+ offset)+""+".txt";
                String []lines=File.ReadAllLines(path, Encoding.Default);
                tileNumber = lines.Length + 1;
                for (i = 0; i < tileNumber; i++)
                {
                    semaphore[i] = new Semaphore(0, 1);
                }
                for (i = 0; i < tileNumber;i++)
                {
                 
                    if (i == 0)
                    {
                        tile[i] = new Tile(0, 0, 2879, 1439);
                        tileIdx[i] = 0;
                        qp[i] = 42;
                    }
                    else
                    {
                        string[] temp = lines[i-1].Split(' ');
                        tile[i] = new Tile(int.Parse(temp[2]), int.Parse(temp[1]), int.Parse(temp[4])+120-1, int.Parse(temp[3])+120-1);
                        tileIdx[i] = int.Parse(temp[0]);
                        qp[i] = ((int)((int.Parse(temp[5])-22)/5))*5+22;
                    }
                   
                }
                for (i = 0; i < tileNumber; i++)
                {
                    isTrue[i] = false;
                    string filepath;  
                    filepath = currentPath + "/StreamingAssets/" + videoid+""+ "_seg/" + (int)(pts + offset)+""+"/"+tileIdx[i]+""+"_"+qp[i]+""+".mp4";
                    string temp1 = filepath;// i.ToString()+".mp4";
                    int tn = i;
                    Thread t = new Thread(() => Decoder(temp1, tn));
                    t.Start();
                    t.IsBackground = true;
                }
                pts = pts + 1;
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

                Thread.Sleep(10);
            }

            //渲染完了就进行拼接,update函数其实是拼接函数	
            Update_Tile(ref tempFrame, cur, tile, tileNumber, width * gap_w, height * gap_h);
            fb = new FrameBuffer();
            fb.av = tempFrame;
            //完成拼接放入队列
            while (true)
            {
                if (fq.Push(ref fb) == 1)  //代表成功放入队列了
                {
                    break;
                }
                Thread.Sleep(10);
            }
            //Debug.Log("拼接第" + T.ToString() + "帧");

        }
    }
 
    void CheckTile(Tile[] tile, int length, int row, ref int[] temp, out int temp_length, ref int[] tileIndex) //用来确定当前行有多少个tile
    {
        int i;
        int k = 0;
        for (i = 0; i < length; i++)
        {
            if (tile[i].lefttop.y <= row && row <= tile[i].rightdown.y)
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

    public unsafe int RunVideo(int gap_h, int gap_w, int width, int height)  //这个是用来渲染整个大画面的
    {
        Produce_data p = new Produce_data();

        p.gap_h = gap_h;
        p.gap_w = gap_w;
        p.target_height = height;
        p.target_width = width;
        IsRun = true;
        exit_thread = false;
        pause_thread = false;
        Thread produce = new Thread(() => ProduceFrame(ref p));
        produce.Start();
        produce.IsBackground = true;
        return 0;
    }
    unsafe public void* memcpy(void* dst, void* src, uint count/*,ref int []temp,ref Tile []tile,ref int []tileIndex,int k1,int k2,int temp_width*/)
    {
        System.Diagnostics.Debug.Assert(dst != null);
        System.Diagnostics.Debug.Assert(src != null);
        void* ret = dst;

       // Debug.Log(((int)dst));
        //Debug.Log(((int)src));
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

    unsafe public void* memmove(void* dst, void* src, int count)
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



    void GetTexture_Frame(byte* Y, int YL, byte* U, int UL, byte* V, int VL, int Width, int Height)
    {

        Marshal.Copy((IntPtr)Y, Y_raw, 0, Width * Height);
        Marshal.Copy((IntPtr)U, U_raw, 0, Width * Height / 4);
        Marshal.Copy((IntPtr)V, V_raw, 0, Width * Height / 4);
        texY.LoadRawTextureData(Y_raw);
        texY.Apply();

        texU.LoadRawTextureData(U_raw);
        texU.Apply();

        texV.LoadRawTextureData(V_raw);
        texV.Apply();

        yuvm.mainTexture = texY;
        yuvm.SetTexture("_MainTexU", texU);
        yuvm.SetTexture("_MainTexV", texV);

    }





}
