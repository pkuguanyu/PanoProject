#include "FrameQueue.h"






int  FrameQueue::Push(FrameBuffer * temp)
{
	lock_frame.lock();
//	printf("Frameµÿ÷∑£∫%d\n", temp);
	if (data_frame.size() < 3)
	{
		data_frame.push(temp);
		lock_frame.unlock();
		return 1;
	}
	else
	{
		lock_frame.unlock();
		return 0;
	}
	
}

FrameBuffer * FrameQueue::Pop()
{
	lock_frame.lock();
	FrameBuffer * top;
	if (data_frame.size() > 0)
	{

		top = data_frame.front();
		data_frame.pop();
	}
	else
		top = NULL;
	lock_frame.unlock();
	return top;
}

FrameBuffer * FrameQueue::GetTop()
{
	lock_frame.lock();
	FrameBuffer * top;
	if (data_frame.size() > 0)
	{
		top = data_frame.front();
	}
	else
		top = NULL;
	lock_frame.unlock();
	return top;
}


