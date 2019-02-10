# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt

# 画一个柱状图

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
            list_name.append(file)

def listdir2(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
         list_name.append(file)



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


gap=3
path="D:/2018.3.27/PeakLeaving_"+str(gap)+"s/"
path2="D:/2018.3.23/iqyi_xyz_raw/"
outpupath="D:/2018.3.27/PeakingLeavingReason_"+str(gap)+"s/"

filename_list = []
filename_list_raw=[]
listdir(path,filename_list)
listdir2(path2,filename_list_raw)
filename_list=sorted(filename_list) # 进行排序
filename_list_raw=sorted(filename_list_raw) #进行排序
initial_videoId=""
initial_userId=""
i=0
start=i
arr = [] #用来存储数据

maxlines=-1
xxxxx=0
for filename in filename_list:  #这个是统计过离开率的视频，要找原来的初始的视频
     xxxxx=xxxxx+1
     print str(xxxxx)+" "+str(len(filename_list))
     inpufile=open(path+filename,"r")
     lines=inpufile.readlines()
     t=filename.split("_")
     video_id=str(t[1])
     end=10
     temp=[] #待处理的视频对应离开的 PlaybackTime
     temp2=[]# 对应离开率
     for i in range(0,end):
         if i<len(lines):
             temp.append((lines[i].split(" "))[0])
             temp2.append((lines[i].split(" "))[1])
     vary=[]
     for i in range(0,len(temp)):
         startTime=float(temp[i])-gap
         endTime=float(temp[i])+gap
         vary.append([startTime,endTime])





     location=-1
     i=0
     video_id=video_id[0:len(video_id)-4]
     for i in range(0,len(filename_list_raw)):
         if video_id in filename_list_raw[i]:
             location=i
             break

     #找到原始视频对应的开始位置，现在目标是找对应时间戳的异常情况
     m=0
     #每一次只处理一个视频

     result=[]
     for i in range(0, len(vary)):
         result.append([0]*8)  # result 和vary对应的值一一对应，vary 有两行，代表result有两行




     current_deviceid=""
     current_videoid=""
     current_timestamp=""
     # 该循环只处理一个视频
     # m/l/k
     while(1):

         m=m+1
         if m==1:
             current_filename=filename_list_raw[location]
             current_split = current_filename.split('_')
             current_videoid = current_split[1]
             current_deviceid = current_split[2]
             current_timestamp = current_split[3]
             tempresult=readFile(path2+current_filename,vary)

             for l in range(0,len(tempresult)):
                 for k in range(0,8):
                     result[l][k]=tempresult[l][k]+result[l][k]

             location=location+1
             if location>=len(filename_list_raw):
                 break
             continue

         temp_filename=filename_list_raw[location]
         temp_split = temp_filename.split('_')
         tempvideo_id = temp_split[1]  # 每一次循环中的videoID
         tempdevice_id = temp_split[2]



         if tempvideo_id!=current_videoid or location>=len(filename_list_raw):
             break
         # l、k
         else:
            tempresult = readFile(path2 + temp_filename, vary)

            for l in range(0, len(tempresult)):
                for k in range(0, 8):
                    result[l][k] = tempresult[l][k] + result[l][k]

            location = location + 1
            if location >= len(filename_list_raw):
                break #



     outputfile=open(outpupath+filename,"w")
     for i in range(0,len(temp)):
         outputfile.write(str(temp[i])+" "+str(temp2[i]))
         for j in range(0,8):
             outputfile.write(" "+str(result[i][j]))
         outputfile.write("\n")








