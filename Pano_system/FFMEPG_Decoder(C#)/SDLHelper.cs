using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UnityPlayer
{
    public unsafe class SDLHelper
    {
        private IntPtr screen;
        private IntPtr sdlrenderer;
        private IntPtr sdltexture;
        SDL.SDL_Rect sdlrect;
        SDL.SDL_Event sdlevent;

        bool isInit = false;
        public SDLHelper()
        {

        }


        public void SDL_MaximizeWindow()
        {

        }

        public int SDL_Init(int width, int height)
        {
            lock (this)
            {
                if (!isInit)
                {
                    // 初始化调用SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO | SDL.SDL_INIT_TIMER)
                    if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO | SDL.SDL_INIT_TIMER) < 0)
                    {
                        Console.WriteLine("Could not initialize SDL - {0}\n", SDL.SDL_GetError());
                        return -1;
                    }
                    isInit = true;
                }
                #region SDL调用
                if (sdltexture != IntPtr.Zero)
                {
                    SDL.SDL_DestroyTexture(sdltexture);
                }
                if (sdlrenderer != IntPtr.Zero)
                {
                    SDL.SDL_DestroyRenderer(sdlrenderer);
                }
                if (screen != IntPtr.Zero)
                {
                    SDL.SDL_DestroyWindow(screen);
                    SDL.SDL_RaiseWindow(screen);
                    SDL.SDL_RestoreWindow(screen);
                }
                //创建显示窗口 
                //screen = SDL.SDL_CreateWindowFrom(intPtr);
                screen = SDL.SDL_CreateWindow("SDL EVENT TEST", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, width, height, SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
                SDL.SDL_ShowWindow(screen);

                SDL.SDL_SetWindowSize(screen, width, height);
                //screen = SDL.SDL_CreateWindow("SDL EVENT TEST", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, width, height, SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
                //screen = SDL.SDL_CreateWindow("SDL EVENT TEST", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, screen_w, screen_h, SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
                if (screen == IntPtr.Zero)
                {
                    Console.WriteLine("Can't creat a window:{0}\n", SDL.SDL_GetError());
                    return -1;
                }

                //创建渲染器
                sdlrenderer = SDL.SDL_CreateRenderer(screen, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
                //创建纹理 
                sdltexture = SDL.SDL_CreateTexture(sdlrenderer, SDL.SDL_PIXELFORMAT_IYUV, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, width, height);
                #endregion

                return 0;
            }
        }


        public int SDL_Display(int width, int height, IntPtr pixels, int pixelsSize,
            int pitch)
        {
            lock (this)
            {
                #region SDL 视频数据渲染播放
                //设置纹理的数据
                sdlrect.x = 0;
                sdlrect.y = 0;
                sdlrect.w = width;
                sdlrect.h = height;
                SDL.SDL_UpdateTexture(sdltexture, ref sdlrect, pixels, pitch);
                //SDL.SDL_UpdateTexture(sdltexture, IntPtr.Zero, pixels, pitch);
                //复制纹理信息到渲染器目标
                SDL.SDL_RenderClear(sdltexture);
                //SDL.SDL_Rect srcRect = sdlrect;
                //SDL.SDL_RenderCopy(sdlrenderer, sdltexture, ref srcRect, ref sdlrect);

                SDL.SDL_RenderCopy(sdlrenderer, sdltexture, IntPtr.Zero, IntPtr.Zero);
                //视频渲染显示
                SDL.SDL_RenderPresent(sdlrenderer);
                return 0;
            }



            #endregion



        }
    }
    public unsafe class SDLAudio
    {
        class aa
        {
            public byte[] pcm;
            public int len;
        }
        int lastIndex = 0;

        private List<aa> data = new List<aa>();

        //private List<byte> data = new List<byte>();
        SDL.SDL_AudioCallback Callback;
        public void PlayAudio(IntPtr pcm, int len)
        {
            lock (this)
            {
                byte[] bts = new byte[len];
                Marshal.Copy(pcm, bts, 0, len);
                data.Add(new aa
                {
                    len = len,
                    pcm = bts
                });
            }

            //SDL.SDL_Delay(10);
        }
        void SDL_AudioCallback(IntPtr userdata, IntPtr stream, int len)
        {
            ////SDL 2.0  
            ////SDL.SDL_RWFromMem(stream, 0, len);
            //if (audio_len == 0)
            //    return;
            //len = (len > audio_len ? audio_len : len);
            if (data.Count == 0)
            {
                for (int i = 0; i < len; i++)
                {
                    ((byte*)stream)[i] = 0;
                }
                return;
            }
            for (int i = 0; i < len; i++)
            {
                if (data[0].len > i)
                {
                    ((byte*)stream)[i] = data[0].pcm[i];
                }
                else
                    ((byte*)stream)[i] = 0;
            }
            data.RemoveAt(0);



        }
        public int SDL_Init()
        {
            Callback = SDL_AudioCallback;
            #region SDL调用
            //// 初始化调用SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO | SDL.SDL_INIT_TIMER)
            //if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO | SDL.SDL_INIT_TIMER) < 0)
            //{
            //    Console.WriteLine("Could not initialize SDL - {0}\n", SDL.SDL_GetError());
            //    return -1;
            //}

            #endregion


            SDL.SDL_AudioSpec wanted_spec = new SDL.SDL_AudioSpec();
            SDL.SDL_AudioSpec got_spec = new SDL.SDL_AudioSpec();
            wanted_spec.freq = 8000;
            wanted_spec.format = SDL.AUDIO_S16;
            wanted_spec.channels = 1;
            wanted_spec.silence = 0;
            wanted_spec.samples = 320;
            wanted_spec.callback = Callback;

            if (SDL.SDL_OpenAudio(ref wanted_spec, out got_spec) < 0)
            {
                Console.WriteLine("can't open audio.");
                return -1;
            }
            //Play  
            SDL.SDL_PauseAudio(0);
            return 0;
        }

    }


}
