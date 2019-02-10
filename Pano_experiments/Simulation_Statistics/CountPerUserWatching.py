# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        file_path = os.path.join(path, file)
        temp=file.split("_")
        file=temp[0]+"_"+temp[2]+"_"+temp[1]+"_"+temp[3]
        list_name.append(file)

def getRealFile(file):
    temp = file.split("_")
    file = temp[0] + "_" + temp[2] + "_" + temp[1]+"_"+temp[3]
    return file

def getKey(x):
    return x[0]


path="C:/Aiqiyi/new_iqiyi_all/iqiyi_xyz_raw_final/"
outputpath="C:/Aiqiyi/Final/dataresult/PerUserWatching/"
filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list) # 进行排序

initial_videoId=""
initial_userId=""
i=0
start=i

data=[]

for filename in filename_list:
     i=i+1
     temp=filename.split("_")
     videoId=temp[2]
     userId=temp[1]
     print filename
     if i==1:
         initial_userId=userId
         start=i

     if userId==initial_userId : #这一部分统计同一个视频，不同用户的数据
         if(i==len(filename_list)):
                 data.append([abs(start-i),initial_userId])
                 initial_userId = userId
                 start=i
     else:
             data.append([abs(start - i), initial_userId])
             start=i
             initial_userId=userId
             continue

outputfile=open(outputpath+"PerUserWatchingTimes.txt",'w')

data.sort(key=getKey,reverse=True)
for d in data:
    outputfile.write(str(d[0])+","+d[1]+"\n")
