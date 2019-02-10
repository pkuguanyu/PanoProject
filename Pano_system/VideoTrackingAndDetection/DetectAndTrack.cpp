
#include <fstream>
#include <sstream>
#include <iostream>
#include <io.h>
#include <direct.h>
#define PATH_DELIMITER '\\'
#include <opencv2/dnn.hpp>
#include <opencv2/imgproc.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/core/utility.hpp>
#include <opencv2/tracking.hpp>
#include <opencv2/videoio.hpp>
#include <string>


using namespace cv;
using namespace dnn;
using namespace std;

// Initialize the parameters
float confThreshold = 0.5; // Confidence threshold
float nmsThreshold = 0.4;  // Non-maximum suppression threshold
int inpWidth = 608;  // Width of network's input image
int inpHeight = 608; // Height of network's input image
vector<string> classes;
string videoName1[] ={"1-1_video.mp4"};
string videoName2[] ={"1-1_video.mp4"};


// Remove the bounding boxes with low confidence using non-maxima suppression
vector<Rect> postprocess(Mat& frame, const vector<Mat>& out);

// Draw the predicted bounding box
void drawPred(int classId, float conf, int left, int top, int right, int bottom, Mat& frame);

// Get the names of the output layers
vector<String> getOutputsNames(const Net& net);


bool createDirectory(const string folder);



int main(int argc, char** argv)
{
    CommandLineParser parser(argc, argv, keys);
    parser.about("Use this script to run object detection using YOLO3 in OpenCV.");
    if (parser.has("help"))
    {
        parser.printMessage();
        return 0;
    }
    // Load names of classes
    string classesFile = "coco.names";
    ifstream ifs(classesFile.c_str());
    string line;
    while (getline(ifs, line)) classes.push_back(line);

    // Give the configuration and weight files for the model
    String modelConfiguration = "yolov3.cfg";
    String modelWeights = "yolov3.weights";

    // Load the network
    Net net = readNetFromDarknet(modelConfiguration, modelWeights);
    net.setPreferableBackend(DNN_BACKEND_OPENCV);
    //net.setPreferableTarget(DNN_TARGET_CPU);
    net.setPreferableTarget(DNN_TARGET_OPENCL);

    // Open a video file or an image file or a camera stream.
    string str, outputFile;
    VideoCapture cap;
    VideoWriter video;
    Mat frame, blob;

    try {

        outputFile = "yolo_out_cpp.avi";
        if (parser.has("image"))
        {
            // Open the image file
            str = parser.get<String>("image");
            ifstream ifile(str);
            if (!ifile) throw("error");
            cap.open(str);
            str.replace(str.end()-4, str.end(), "_yolo_out_cpp.jpg");
            outputFile = str;
        }
        else if (parser.has("video"))
        {
            // Open the video file
            str = parser.get<String>("video");
            ifstream ifile(str);
            if (!ifile) throw("error");
            cap.open(str);
            str.replace(str.end()-4, str.end(), "_yolo_out_cpp.avi");
            outputFile = str;
        }
        // Open the webcaom
        else cap.open(parser.get<int>("device"));

    }
    catch(...) {
        cout << "Could not open the input image/video stream" << endl;
        return 0;
    }

    // Get the video writer initialized to save the output video
    if (!parser.has("image")) {
        video.open(outputFile, VideoWriter::fourcc('M','J','P','G'), 28, Size(cap.get(CAP_PROP_FRAME_WIDTH), cap.get(CAP_PROP_FRAME_HEIGHT)));
    }

    // Create a window
    static const string kWinName = "Deep learning object detection in OpenCV";
    namedWindow(kWinName, WINDOW_NORMAL);



    int detectionTime=0;
    int TrackingTime=0;
    Ptr<MultiTracker> Multracker = MultiTracker::create();
    //创建多目标跟踪器
    vector<Rect2d> MulRoi;
    //创建多个ROI
    int MultiTracking;
    //待跟踪物体的数量
    int RoiNumber=0;
    // Process frames.
    vector<Rect> box;
    //存储detection到的物体
    ofstream out(str.substr(0,str.size()-4) + ".txt");

    while (waitKey(1) < 0)
    {
        // get frame from the video
        cap >> frame;

        //每30帧进行一次detection
        if (detectionTime%30==0)
        {
            // Stop the program if reached end of video
            if (frame.empty()) {
                cout << "Done processing !!!" << endl;
                cout << "Output file is stored as " << outputFile << endl;
                waitKey(3000);
                break;
            }
            // Create a 4D blob from a frame.
            blobFromImage(frame, blob, 1/255.0, cvSize(inpWidth, inpHeight), Scalar(0,0,0), true, false);

            //Sets the input to the network
            net.setInput(blob);

            // Runs the forward pass to get output of the output layers
            vector<Mat> outs;
            net.forward(outs, getOutputsNames(net));

            // Remove the bounding boxes with low confidence
            box = postprocess(frame, outs);

            // Put efficiency information. The function getPerfProfile returns the overall time for inference(t) and the timings for each of the layers(in layersTimes)
            vector<double> layersTimes;
            double freq = getTickFrequency() / 1000;
            double t = net.getPerfProfile(layersTimes) / freq;
            string label = format("Inference time for a frame : %.2f ms", t);
            putText(frame, label, Point(0, 15), FONT_HERSHEY_SIMPLEX, 0.5, Scalar(0, 0, 255));

            // Write the frame with the detection boxes
           /*
            Mat detectedFrame;
            frame.convertTo(detectedFrame, CV_8U);
            if (parser.has("image")) imwrite(outputFile, detectedFrame);
            else video.write(detectedFrame);
            imshow(kWinName, frame);
           */


            RoiNumber=0;
            Multracker = MultiTracker::create();

            for(int i=0;i<box.size();i++)
            {
                Rect2d roi = Rect2d(box[i]);
                //创建ROI感兴趣区域
                if(roi.width==0 || roi.height==0)
                    continue;
                Ptr<Tracker> tracker = TrackerCSRT::create();
                //建立CSRT跟踪器
                tracker->init(frame,roi);
                //初始化tracker
                Multracker->add(tracker,frame,roi);
                //将单目标物体跟踪器加入多目标物体跟踪器
                MulRoi.push_back(roi);
                //将roi加入vector
                RoiNumber++;
            }
            MultiTracking=RoiNumber;
            out<<"SplitLine"<<endl;
            out<<RoiNumber<<endl;


        }
         //每帧都进行tracking
        detectionTime++;


        // update the tracking result
        Multracker->update(frame,MulRoi);

        for(int i=0;i<MultiTracking;i++)
        {
            rectangle( frame, MulRoi[i], Scalar( 255, 0, 0 ), 2, 1 );
        }
        // show image with the tracked object

        Mat detectedFrame;
        frame.convertTo(detectedFrame, CV_8U);
        if (parser.has("image")) imwrite(outputFile, detectedFrame);
        else video.write(detectedFrame);
        imshow(kWinName, frame);


        if (out.is_open())
        {
            for(int i=0;i<MultiTracking;i++)
            {
                double Center_X=(MulRoi[i].x+MulRoi[i].x+MulRoi[i].width)/2;
                double Center_Y=(MulRoi[i].y+MulRoi[i].y+MulRoi[i].height)/2;
                double Time=detectionTime*1.0/30.0;
                out<<Time<<" "<<Center_X<<" "<<Center_Y<<endl;
            }
        }

        //quit on ESC button
        if(waitKey(1)==27)break;
    }

    cap.release();
    out.close();
    if (!parser.has("image")) video.release();

    return 0;
}


// Remove the bounding boxes with low confidence using non-maxima suppression
vector<Rect>  postprocess(Mat& frame, const vector<Mat>& outs)
{
    vector<int> classIds;
    vector<float> confidences;
    vector<Rect> boxes;

    for (size_t i = 0; i < outs.size(); ++i)
    {
        // Scan through all the bounding boxes output from the network and keep only the
        // ones with high confidence scores. Assign the box's class label as the class
        // with the highest score for the box.
        float* data = (float*)outs[i].data;
        for (int j = 0; j < outs[i].rows; ++j, data += outs[i].cols)
        {
            Mat scores = outs[i].row(j).colRange(5, outs[i].cols);
            Point classIdPoint;
            double confidence;
            // Get the value and location of the maximum score
            minMaxLoc(scores, 0, &confidence, 0, &classIdPoint);
            if (confidence > confThreshold)
            {
                int centerX = (int)(data[0] * frame.cols);
                int centerY = (int)(data[1] * frame.rows);
                int width = (int)(data[2] * frame.cols);
                int height = (int)(data[3] * frame.rows);
                int left = centerX - width / 2;
                int top = centerY - height / 2;

                classIds.push_back(classIdPoint.x);
                confidences.push_back((float)confidence);
                boxes.push_back(Rect(left, top, width, height));
            }
        }
    }

    // Perform non maximum suppression to eliminate redundant overlapping boxes with
    // lower confidences
    vector<int> indices;
    NMSBoxes(boxes, confidences, confThreshold, nmsThreshold, indices);
    for (size_t i = 0; i < indices.size(); ++i)
    {
        int idx = indices[i];
        Rect box = boxes[idx];
        drawPred(classIds[idx], confidences[idx], box.x, box.y,
                 box.x + box.width, box.y + box.height, frame);
    }
    return boxes;
}

// Draw the predicted bounding box
void drawPred(int classId, float conf, int left, int top, int right, int bottom, Mat& frame)
{
    //Draw a rectangle displaying the bounding box
    rectangle(frame, Point(left, top), Point(right, bottom), Scalar(255, 178, 50), 3);

    //Get the label for the class name and its confidence
    string label = format("%.2f", conf);
    if (!classes.empty())
    {
        CV_Assert(classId < (int)classes.size());
        label = classes[classId] + ":" + label;
    }

    //Display the label at the top of the bounding box
    int baseLine;
    Size labelSize = getTextSize(label, FONT_HERSHEY_SIMPLEX, 0.5, 1, &baseLine);
    top = max(top, labelSize.height);
    rectangle(frame, Point(left, top - round(1.5*labelSize.height)), Point(left + round(1.5*labelSize.width), top + baseLine), Scalar(255, 255, 255), FILLED);
    putText(frame, label, Point(left, top), FONT_HERSHEY_SIMPLEX, 0.75, Scalar(0,0,0),1);
}

// Get the names of the output layers
vector<String> getOutputsNames(const Net& net)
{
    static vector<String> names;
    if (names.empty())
    {
        //Get the indices of the output layers, i.e. the layers with unconnected outputs
        vector<int> outLayers = net.getUnconnectedOutLayers();

        //get the names of all the layers in the network
        vector<String> layersNames = net.getLayerNames();

        // Get the names of the output layers in names
        names.resize(outLayers.size());
        for (size_t i = 0; i < outLayers.size(); ++i)
        names[i] = layersNames[outLayers[i] - 1];
    }
    return names;

}



bool createDirectory(const string folder)
{

	string folder_builder;
	string sub;
	sub.reserve(folder.size());

	for (auto it = folder.begin(); it != folder.end(); ++it)
	{

		const char c = *it;
		sub.push_back(c);
		if (c == PATH_DELIMITER || it == folder.end() - 1)
		{
			folder_builder.append(sub);
			if (0 != _access(folder_builder.c_str(), 0))
			{
				// this folder not exist
				if (0 != _mkdir(folder_builder.c_str()))
				{
					// create failed
					return false;
				}
			}
			sub.clear();
		}
	}
	return true;

}

