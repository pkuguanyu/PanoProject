#pragma once
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
class FrameBuffer
{

public:
	AVFrame *frame;
	uint8_t *buffer;
	FrameBuffer();
	~FrameBuffer();
};

