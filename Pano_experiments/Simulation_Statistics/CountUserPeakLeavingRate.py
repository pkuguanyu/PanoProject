# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        file_path = os.path.join(path, file)
        temp=file.split("_")
        file=temp[1]+"_"+temp[0]+"_"+temp[2]
        list_name.append(file)

def getRealFile(file):
    temp = file.split("_")
    file = temp[1] + "_" + temp[0] + "_" + temp[2]
    return file

def getKey(x):
    return float(x[0])

def func():
    total_number = [0.0] * maxlines  # 代表每一个时刻的观看人数
    y = []
    y.append(0)

    for m in range(0, len(arr)):  # m代表
        lines = len(arr[m])
        for n in range(0, lines):
            total_number[n] = total_number[n] + 1

    if (abs(start - i) > 100):
        for m in range(1, len(total_number)):
            leaving_rate = (total_number[m - 1] - total_number[m]) * 1.0 / total_number[m - 1]
            y.append(leaving_rate)
        peak = []
        for m in range(0, len(y)):
            temp = [y[m], m]
            peak.append(temp)

        peak.sort(key=getKey, reverse=True)
        outputfile = open(outputpath + str(abs(start - i)) + "_" + videoId + ".txt", "w")

        for m in range(0, len(peak)):
            outputfile.write(str(peak[m][1] * gap) + " " + str(peak[m][0]) + "\n")

gap=1

path="C:/Aiqiyi/new_iqiyi_final/1_"+str(gap)+"s/"
outputpath="D:/2018.3.27/PeakLeaving_"+str(gap)+"s/"

outputpath="D:/2018.3.27/testo/"
outputpathGraph=""
filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list) # 进行排序

initial_videoId=""
initial_userId=""
i=0
start=i
arr = [] #用来存储数据

maxlines=-1
for filename in filename_list:
     i=i+1
     temp=filename.split("_")
     videoId=temp[0]
     userId=temp[1]

     if i==1:
         initial_videoId=videoId
         start=i

     if videoId==initial_videoId : #这一部分统计同一个视频，不同用户的数据
         print 1111
         print filename
         inputfile=open(path+getRealFile(filename),"r")
         lines=inputfile.readlines()
         #记录最大的片段数量
         if(len(lines)>=maxlines):
             maxlines=len(lines)

         # 这里将数据累计出来求方差和平均值，有几个片段，
         arr2 = []
         if (len(lines)==1 or len(lines)==0):
             continue

         for line in lines:
            temp=float(line)
            arr2.append(temp)
         arr.append(arr2) #将数据加进来，不对齐的然后处理即可

         if(i==len(filename_list)):

             total_number = [0.0] * maxlines  # 代表每一个时刻的观看人数
             y = []
             x = []
             y.append(0)
             x.append(0)
             for m in range(0, len(arr)):  # m代表
                 lines = len(arr[m])
                 for n in range(0, lines):
                     total_number[n] = total_number[n] + 1

             if (abs(start - i) > 100):
                 for m in range(1, len(total_number)):
                     leaving_rate = (total_number[m - 1] - total_number[m]) * 1.0 / total_number[m - 1]
                     print leaving_rate
                     y.append(leaving_rate)
                     x.append(m*gap)

                 plt.plot(x,total_number)
                 plt.show()
                 peak = []

                 plt.title("Variation of user's leaving rate in each video(video id:)"+str(videoId))
                 plt.xlabel("Playbacktime")
                 plt.ylabel("Leaving rate")
                 label="Leaving rate"
                 plt.legend("upper right")
                 plt.plot(x,y,label=label)
                 plt.show()

                 for m in range(0, len(y)):
                     temp = [y[m], m]
                     peak.append(temp)

                 peak.sort(key=getKey, reverse=True)
                 outputfile = open(outputpath + str(abs(start - i)) + "_" + initial_videoId+ ".txt", "w")
                 for m in range(0, len(peak)):
                     outputfile.write(str(peak[m][1] * gap) + " " + str(peak[m][0]) + "\n")

             initial_videoId = videoId
             arr=[]
             start=i


     else:
             print 222
             print filename
             #这里代表进行了更换，因此还需要进一步处理
             #首先进行收尾处理，统计上一个的
             total_number = [0.0] * maxlines  # 代表每一个时刻的观看人数
             y = []
             x = []
             y.append(0)
             x.append(0)
             for m in range(0, len(arr)):  # m代表
                 lines = len(arr[m])
                 for n in range(0, lines):
                     total_number[n] = total_number[n] + 1

             if (abs(start - i) > 100):
                 for m in range(1, len(total_number)):
                     leaving_rate = (total_number[m - 1] - total_number[m]) * 1.0 / total_number[m - 1]
                     print leaving_rate
                     y.append(leaving_rate)
                     x.append(m*gap)

                 peak = []
                 for m in range(0, len(y)):
                     temp = [y[m], m]
                     peak.append(temp)

                 plt.title("Variation of user's leaving rate in each video(video id:)"+str(videoId))
                 plt.xlabel("Playbacktime")
                 plt.ylabel("Leaving rate")
                 label="Leaving rate"
                 plt.legend("upper right")
                 plt.plot(x,y,label=label)
                 plt.show()

                 peak.sort(key=getKey, reverse=True)
                 outputfile = open(outputpath + str(abs(start - i)) + "_" + initial_videoId + ".txt", "w")

                 for m in range(0, len(peak)):
                     outputfile.write(str(peak[m][1] * gap) + " " + str(peak[m][0]) + "\n")

             arr = [] #清空数据
             start=i
             #接着将当前数据读进去
             initial_videoId=videoId
             inputfile = open(path + getRealFile(filename), "r")
             lines = inputfile.readlines()
             # 这里将数据累计出来求方差和平均值，有几个片段，动态开辟几个数组

             maxlines=len(lines)
             arr2 = []
             for line in lines:
                 temp = float(line)
                 arr2.append(temp)
             arr.append(arr2)  # 将数据加进来，不对齐的然后处理即可
             continue
