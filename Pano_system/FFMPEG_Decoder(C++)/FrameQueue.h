#pragma once
#include <vector>
#include <thread>
#include <mutex>
#include <chrono>
#include <condition_variable>
#include <queue>
#include "libavcodec/avcodec.h"
#include "libavformat/avformat.h"
#include "libswscale/swscale.h"
#include "libavutil/imgutils.h"
#include "SDL2/SDL.h"
#include "FrameBuffer.h"

using namespace std;
class FrameQueue
{
public:
	FrameQueue() :lock_frame(),data_frame() {}
	~FrameQueue() {}
	int Push(FrameBuffer * temp);
	FrameBuffer * Pop();
	FrameBuffer * GetTop();




private:
	mutex lock_frame;
	queue<FrameBuffer *> data_frame;
};

