 /*
* This function is used to decode each small tile and output the corresponding YUV format frame.
* Input: Filename of the corresponding file name of the small tile and the corresponding index number of this tile.
*/

public unsafe void Decoder(string fileName, int tilenumber)
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
                int skipped_frame = 0; //用来解决末尾解码不完全的问题

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

                        Debug.Log("当前跳过帧数：" + skipped_frame.ToString());
                        // 读取解码后的帧数据
                        if (got_picture > 0)
                        {
                            frame_count++;
                            Debug.Log("视频帧数:第 " + frame_count + " 帧");
                            //AVFrame转为像素格式YUV420，宽高
                            ffmpeg.sws_scale(sws_ctx, pFrame->data, pFrame->linesize, 0, pCodecCtx->height, pFrameYUV->data, pFrameYUV->linesize);                 
                            flag = 0;
                            cur[tilenumber] = pFrameYUV;
                            isTrue[tilenumber] = false; // 代表当前帧渲染完毕
                        }
                        else
                        {
                           skipped_frame++;
                           flag = 1;
                        }
                        ffmpeg.av_free_packet(packet);
                    }
                    else
                    {
                        flag = 1;
                    }
  

                }
                
                flag = 0;
                for(int i = skipped_frame; i>0; i--)
                {
                    frame_count++;
                    if (flag == 0)
                    {
                        semaphore[tilenumber].WaitOne();
                    }
                    ret = ffmpeg.avcodec_decode_video2(pCodecCtx, pFrame, &got_picture, packet);
                    if (got_picture > 0)
                    {
                        
                        Debug.Log("视频帧数:第 " + frame_count + " 帧");
                        //AVFrame转为像素格式YUV420，宽高
                        ffmpeg.sws_scale(sws_ctx, pFrame->data, pFrame->linesize, 0, pCodecCtx->height, pFrameYUV->data, pFrameYUV->linesize);            
                        flag = 0;
                        cur[tilenumber] = pFrameYUV;
                        isTrue[tilenumber] = false; // 代表当前帧渲染完毕
                    }
                    else
                    {
                        flag = 1;
                    }
                }
                Thread.Sleep(10);
                if (frame_count >= changeFrame) //退出播放操作
                {
                    semaphore[tilenumber].Release();
                    ffmpeg.av_free_packet(packet);
                    ffmpeg.avformat_close_input(&ofmt_ctx);
                    ffmpeg.av_frame_free(&pFrameYUV);
                    ffmpeg.av_frame_free(&pFrame);
                    ffmpeg.avcodec_close(pCodecCtx);
                    ffmpeg.av_free(out_buffer);
                    Debug.Log("当前视频解码完毕" + tilenumber.ToString());
                    return 0;
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