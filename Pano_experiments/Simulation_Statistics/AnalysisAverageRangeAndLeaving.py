# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt


def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
            list_name.append(file)

def listdir2(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        temp = file.split("_")
        file = temp[1] + "_" + temp[0] + "_" + temp[2]
        list_name.append(file)

def getRealFile(file):
        temp = file.split("_")
        file = temp[1] + "_" + temp[0] + "_" + temp[2]
        return file


# 数据输出格式，timestamp,x,y,z,状态码
# 状态码为 0代表正常， 5代表不从0开始， 2代表 快进， 3代表卡顿，4代表回放,1，6代表异常
def readFile(filename,vary):
    file=open(filename,"r")
    lines=file.readlines()
    result=[]

    for i in range(0,len(vary)):
        result.append([0]*8)  #result 和vary对应的值一一对应，vary 有两行，代表result有两行

    for line in lines:
        temp=line.split(",")
        if(len(temp[0])>9):
            continue
        timeStamp=float(temp[0])
        if (len(temp)>4):
            status=float(temp[4])
        else:
            return result
        for i in range(0, len(vary)):
            if timeStamp>=vary[i][0] and timeStamp<=vary[i][1]:
                result[i][int(status)]=result[i][int(status)]+1

    for i in range(0,len(vary)):
        for j in range(0,8):
            if result[i][j]>0:
                result[i][j]=1
    return result

#1.对于同一个视频，观察离开比较高的点的头部 头部运动方向改变情况，计算其平均运动方向改变次数，也观察离开适中的点，观察期平均运动范围

gap=5
path="D:/2018.3.27/PeakLeaving_"+str(gap)+"s/"
path2="C:/Aiqiyi/new_iqiyi_final/3_"+str(gap)+"s/"
outpupath="D:/2018.3.27/Range/ComparasionHigh_"+str(gap)+"s/" #记录高离开率的动作信息
outpupath2="D:/2018.3.27/Range/ComparasionNormal_"+str(gap)+"s/" #记录正常离开率的动作信息

filename_list = [] #存储离开率
filename_list_raw=[] #存储运动方向改变
listdir(path,filename_list)
listdir2(path2,filename_list_raw)
filename_list=sorted(filename_list) # 进行排序
filename_list_raw=sorted(filename_list_raw) #进行排序
initial_videoId=""
initial_userId=""


threshold=0.1
for filename in filename_list:  #这个是统计过离开率的视频，要找原来的初始的视频
    file=open(path+filename,'r')
    lines=file.readlines()
    i=0
    end=i
    for i in range(0,len(lines)):
        temp=lines[i].split(" ")
        leavingRate=float(temp[1])
        if leavingRate<threshold:
            end=i
            break


    if end==0:
        continue
    #同样一个循环处理一个视频
    video_id=str((filename.split("_"))[1])
    peoplenumber=float((filename.split("_"))[0])
    video_id = video_id[0:len(video_id) - 4]
    playbackTime=[]  #存储播放时间戳
    leavingRate=[]  #存储用户离开率

    for i in range(0,len(lines)):
        temp = lines[i].split(" ")
        playbackTime.append(float(temp[0]))
        leavingRate.append(float(temp[1]))


    # 计算离开率较高的几个点 对应的头部运动方向改变次数
    sum_high_normal = [-1] * len(playbackTime)  # -1 代表没有意义的数据

    #序号对应离开率排名


    locationStart=-1
    locationEnd=-1
    i=0
    for filename_raw in filename_list_raw:
        if video_id in filename_raw:
            locationStart=i
            break
        i=i+1

    for i in range(locationStart,len(filename_list_raw)):
        if video_id not in filename_list_raw[i] :
            locationEnd=i
            break
        if i==len(filename_list_raw)-1:
            locationEnd=len(filename_list_raw)



    # 每一次循环计算一个时间戳对应的头部运动次数
    end2=0
    if end+10>len(lines):
        end2=len(lines)
    else:
        end2=end+10
    for i in range(0,end2):
        startTime = playbackTime[i] - gap
        endTime = playbackTime[i] + gap


        # 这个循环找该时间戳对应所有文件的平均值
        t = 0  # t代表计算的时间片段数
        for j in range(locationStart,locationEnd):
                filename_raw=filename_list_raw[j]
                file2 = open(path2 + getRealFile(filename_raw), 'r')
                lines2 = file2.readlines()
            #这一部分计算
                timestampStart=0
                timestampEnd=timestampStart+gap
                for line in lines2:
                    templine=line
                    data=float(templine)
                    if abs(startTime-timestampStart)<=1 or abs(endTime-timestampEnd)<=1:
                        if sum_high_normal[i]==-1:
                            sum_high_normal[i]=0
                        t = t + 1 #每统计到一个时间戳 对应的数据加1
                        sum_high_normal[i]= sum_high_normal[i] + data
                    timestampStart=timestampStart+gap
                    timestampEndt=timestampStart+gap
        print t
                        # i对应i号时间戳的平均运动方向改变次数
        if sum_high_normal[i] != -1:
                sum_high_normal[i] = sum_high_normal[i]*1.0 / (gap * t)



    outputfile1=open(outpupath+str(peoplenumber)+"_"+video_id+"_high.txt",'w')
    outputfile2=open(outpupath2+str(peoplenumber)+"_"+video_id+"_normal.txt",'w')


    for i in range(0,end):
        outputfile1.write(str(playbackTime[i])+" "+str(leavingRate[i])+" "+str(sum_high_normal[i])+"\n")


    for i in range(end,end2):
        outputfile2.write(str(playbackTime[i])+" "+str(leavingRate[i])+" "+str(sum_high_normal[i])+"\n")





