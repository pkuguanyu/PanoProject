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
            file = temp[1] + "_" + temp[0] + "_" + temp[2]+"_"+temp[3]
            list_name.append(file)

def getRealFile(file):
            temp = file.split("_")
            file = temp[1] + "_" + temp[0] + "_" + temp[2]+"_"+temp[3]
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

#1.对于同一个视频，观察离开比较高的点的头部 头部运动方向改变情况，计算其平均运动方向改变次数，也观察离开适中的点，观察期平均运动方向改变次数

gap=1
path="D:/2018.3.27/PeakLeaving_"+str(gap)+"s/"
path2="C:/Aiqiyi/new_iqiyi_all/1_"+str(gap)+"s/"
outpupath="D:/2018.3.27/User/Angle/ComparasionLeaveAngle_"+str(gap)+"s/" #记录高离开率的动作信息
outpupath2="D:/2018.3.27/User/Angle/ComparasionStayAngle_"+str(gap)+"s/" #记录正常离开率的动作信息

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
    file = open(path + filename, 'r')
    lines = file.readlines()
    i = 0
    playbackTime = []  # 存储播放时间戳
    leavingRate = []  # 存储用户离开率

    for i in range(0, len(lines)):
        temp = lines[i].split(" ")
        playbackTime.append(float(temp[0]))
        leavingRate.append(float(temp[1]))

    video_id = str((filename.split("_"))[1])
    peoplenumber = float((filename.split("_"))[0])
    video_id = video_id[0:len(video_id) - 4]
    StayUser=[]
    LeaveUser=[]

    locationStart = -1
    locationEnd = -1
    i = 0
    for filename_raw in filename_list_raw:
        if video_id in filename_raw:
            locationStart = i
            break
        i = i + 1

    for i in range(locationStart, len(filename_list_raw)):
        if video_id not in filename_list_raw[i]:
            locationEnd = i
            break
        if i == len(filename_list_raw) - 1:
            locationEnd = len(filename_list_raw)

    print ""
    # 将提前离开的用户和没离开的用户的动作行为进行对比
    for i in range(0,1):

        StayUser = []
        LeaveUser = []
        maxlines=-1

        for j in range(locationStart, locationEnd):
            filename_raw = filename_list_raw[j]
            file2 = open(path2 + getRealFile(filename_raw), 'r')
            lines2 = file2.readlines()
            if len(lines2)>=maxlines:
                maxlines=len(lines2)

        total_pre=[0.0]*maxlines
        for j in range(locationStart, locationEnd):
            filename_raw = filename_list_raw[j]
            file2 = open(path2 + getRealFile(filename_raw), 'r')
            lines2 = file2.readlines()
            for ll in range(0, len(lines2)):
                total_pre[ll] = total_pre[ll] + 1


        end=len(total_pre)-1
        for j in range(1,len(total_pre)):
            if total_pre[j]<=total_pre[0]*0.3:
                end=j
                break



        print end
        print len(total_pre)
        print ""
        for j in range(locationStart, locationEnd):
            filename_raw = filename_list_raw[j]
            file2 = open(path2 + getRealFile(filename_raw), 'r')
            lines2 = file2.readlines()
            if(len(lines2)*gap>end*gap):
                StayUser.append(filename_raw)
            else:
                LeaveUser.append(filename_raw)


        sum_stay=[0.0]*maxlines
        total=[0.0]*maxlines
        for stay in StayUser:
            file2 = open(path2 + getRealFile(stay), 'r')
            line2 = file2.readlines()
            for j in range(0,len(line2)):
                sum_stay[j]=sum_stay[j]+float((line2[j].split(" "))[2])
                total[j]=total[j]+1


        for j in range(0,len(sum_stay)):
            if(abs(total[j]-0)<0.001):
                break
            sum_stay[j]=sum_stay[j]/total[j]

        sum_leave=[0.0]*maxlines
        total = [0.0] * maxlines

        for leave in LeaveUser:
            file2 = open(path2 + getRealFile(leave), 'r')
            line2 = file2.readlines()
            for j in range(0, len(line2)):
                sum_leave[j] = sum_leave[j] + float((line2[j].split(" "))[2])
                total[j] = total[j] + 1



        for j in range(0, len(sum_leave)):
            if (abs(total[j] - 0) < 0.001):
                break
            sum_leave[j] = sum_leave[j] / total[j]

        outputfile1 = open(outpupath + str(peoplenumber) + "_" + video_id + "_Leave.txt", 'w')
        outputfile2 = open(outpupath2 + str(peoplenumber) + "_" + video_id + "_Stay.txt", 'w')

        for l in range(0, len(sum_leave)):
            outputfile1.write(str(sum_leave[l]) + "\n")

        for l in range(0, len(sum_stay)):
            outputfile2.write(str(sum_stay[l]) + "\n")




