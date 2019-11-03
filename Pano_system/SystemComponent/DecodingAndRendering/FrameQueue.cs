/* This function is used to store the frame which is merged and decoded by the decoder temporaily.
* For specifically, this is not a buffer in disk. It is the rendering buffer in RAM to guarantee the playback smoooth.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFmpeg.AutoGen;

using System.Threading;
using UnityEngine;

public class FrameQueue 
    {

        public FrameQueue()
        {
            data_frame = new Queue<FrameBuffer>();
            m = new Mutex();
        }
        ~FrameQueue() { }
        public int Push(ref FrameBuffer f)
        {
            m.WaitOne();
            if (data_frame.Count < 3)
            {
                data_frame.Enqueue(f);
                m.ReleaseMutex();
                return 1;
            }
            else
            {
                m.ReleaseMutex();
                return 0;
            }


        }
        public void Pop()
        {
            m.WaitOne();

            if (data_frame.Count > 0)
            {
                data_frame.Dequeue();
                m.ReleaseMutex();
            }
            else
            {
                m.ReleaseMutex();
            }

        }

        public int GetTop(out FrameBuffer f)
        {
            m.WaitOne();
            if (data_frame.Count > 0)
            {
                f = data_frame.Peek();
                m.ReleaseMutex();
                return 1;
            }
            else
            {
                f = null;
                m.ReleaseMutex();
                return 0;
            }

        }

        private Queue<FrameBuffer> data_frame;
        private Mutex m;
    }

