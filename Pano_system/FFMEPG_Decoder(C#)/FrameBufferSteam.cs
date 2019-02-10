using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFmpeg.AutoGen;
using UnityEngine;



    public unsafe class FrameBuffer
    {
        public AVFrame av;
        public byte* outbuffer;
        public int outbuffersize;
        public FrameBuffer()
        {
        }
        public FrameBuffer(ref AVFrame a)
        {
            av = a;
        }
    }

