# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt

#六维度 分别指：用户观看时间（百分比），
# 用户卡顿（百分比），用户快进（百分比），用户回放（百分比），头部旋转角度，头部旋转范围么？
# 时间是0，卡顿是1，快进是2，回放是3，旋转角度是4，旋转范围是5 videosession
#散点图 一个会话代表一个点

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        list_name.append(file)

#现在要统计每一个视频会话对应的观看时间（百分比），卡顿，快进，回放次数

def RotationAngle(userID,videoID,timestamp,threshold):
    path2="C:/Aiqiyi/new_iqiyi_all/1_1s/"
    file=open(path2+userID+"_"+videoID+"_"+timestamp+"_1s.txt","r")
    sum=0
    total=0

    lines=file.readlines()
    time = float((lines[0].split(" "))[0])
    if(len(lines)<=5 or time>1):
        return -1
    num=0
    for line in lines:
        temp=line.split(" ")
        sum=sum+float(temp[2])
        num=num+1
        if float(temp[2])>=threshold:
            total=total+1
    if sum<=20:
        return -1
    file.close()
    return total


#path="D:/2018.4.1/test/"
path="C:/Aiqiyi/new_iqiyi_all/iqiyi_xyz_raw_final/"

filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list) # 进行排序

initial_videoId=""
initial_userId=""
i=0
start=i
arr = [] #用来存储数据
name=[] #用来存储文件名
maxTimes=-1


outputpath="D:/2018.4.1/correlationResult/"
threshold=0

for threshold in range(5,105,10):
    outputfile = open(outputpath + "videosession_highmotion_"+str(threshold)+"_.txt", "w")
    videosession_highmotion = []
    i=0
    initial_videoId = ""
    initial_userId = ""
    i = 0
    start = i
    arr = []  # 用来存储数据
    name = []  # 用来存储文件名
    maxTimes = -1
    for filename in filename_list:
        i = i + 1
        print str(i) + " " + str(len(filename_list))
        # print filename
        temp = filename.split("_")
        videoId = temp[1]
        userId = temp[2]
        timestamp = temp[3]
        if i == 1:
            initial_videoId = videoId
            start = i
        if videoId == initial_videoId:  # 这一部分统计同一个视频，不同用户的数据
            inputfile = open(path + filename, "r")
            lines = inputfile.readlines()

            # 这里将数据累计出来求方差和平均值，有几个片段，
            arr2 = []
            if (len(lines) == 0):
                continue

            for line in lines:
                arr2.append(line)
                time = float((line.split(","))[0])
                behavior = int((line.split(","))[4])
                if time < len(lines) * 0.15 or time > len(lines) * 0.3:
                    time = len(lines) * 0.2
                if time >= maxTimes:
                    maxTimes = time

            arr.append(arr2)  # 将数据加进来
            name.append(filename)

            if (i == len(filename_list)):

                # 时间是0，卡顿是1，快进是2，回放是3，旋转角度是4，旋转范围是5 videosession
                # 状态码为 0代表正常， 5代表不从0开始， 2代表 快进， 3代表卡顿，4代表回放,1，6代表异常
                # maxlines代表最大的观看时间
                for m in range(0, len(arr)):  # m代表
                    lines = len(arr[m])
                    tempmaxTimes = -1
                    sum_highmotion_time = 0
                    sum_time = 0
                    for n in range(0, lines):
                        temp = arr[m][n]
                        time = float((temp.split(","))[0])
                        if tempmaxTimes < time:
                            tempmaxTimes = time
                    if tempmaxTimes < lines * 0.15 or tempmaxTimes > lines * 0.3:
                        tempmaxTimes = lines * 0.2
                    if tempmaxTimes >= maxTimes:
                        tempmaxTimes = maxTimes
                    current_filename = name[m]
                    vID = current_filename.split("_")[1]
                    uID = current_filename.split("_")[2]
                    tS = current_filename.split("_")[3]
                    rA = RotationAngle(uID, vID, tS, threshold)
                    if (rA != -1 and maxTimes >= 5):
                        sum_highmotion_time = sum_highmotion_time + rA * 1.0
                        sum_time = sum_time + tempmaxTimes * 1.0
                        videosession_highmotion.append([sum_highmotion_time, sum_time])

                initial_videoId = videoId
                arr = []
                name = []
                start = i


        else:

            # 这里代表进行了更换，因此还需要进一步处理
            # 首先进行收尾处理，统计上一个的

            # 时间是0，卡顿是1，快进是2，回放是3，旋转角度是4，旋转范围是5 videosession
            # 状态码为 0代表正常， 5代表不从0开始， 2代表 快进， 3代表卡顿，4代表回放,1，6代表异常

            # maxlines代表最大的观看时间
            for m in range(0, len(arr)):  # m代表
                lines = len(arr[m])
                tempmaxTimes = -1
                sum_highmotion_time = 0
                sum_time = 0
                for n in range(0, lines):
                    temp = arr[m][n]
                    time = float((temp.split(","))[0])
                    if tempmaxTimes < time:
                        tempmaxTimes = time
                if tempmaxTimes < lines * 0.15 or tempmaxTimes > lines * 0.3:
                    tempmaxTimes = lines * 0.2
                if tempmaxTimes >= maxTimes:
                    tempmaxTimes = maxTimes
                current_filename = name[m]
                vID = current_filename.split("_")[1]
                uID = current_filename.split("_")[2]
                tS = current_filename.split("_")[3]
                rA = RotationAngle(uID, vID, tS, threshold)
                if (rA != -1 and maxTimes >= 5):
                    sum_highmotion_time = sum_highmotion_time + rA * 1.0
                    sum_time = sum_time + tempmaxTimes * 1.0
                    videosession_highmotion.append([sum_highmotion_time, sum_time])

            arr = []  # 清空数据
            name = []
            start = i
            # 接着将当前数据读进去
            initial_videoId = videoId
            inputfile = open(path + filename, "r")
            lines = inputfile.readlines()
            # 这里将数据累计出来求方差和平均值，有几个片段，动态开辟几个数组

            maxTimes = -1
            arr2 = []
            for line in lines:
                arr2.append(line)
                time = float((line.split(","))[0])
                behavior = int((line.split(","))[4])
                if time < len(lines) * 0.15 or time > len(lines) * 0.3:
                    time = len(lines) * 0.2
                if time >= maxTimes:
                    maxTimes = time
            arr.append(arr2)  # 将数据加进来
            name.append(filename)
            continue

    for i in range(0, len(videosession_highmotion)):
       outputfile.write(str(videosession_highmotion[i][0]) + " " + str(videosession_highmotion[i][1]) + "\n")
