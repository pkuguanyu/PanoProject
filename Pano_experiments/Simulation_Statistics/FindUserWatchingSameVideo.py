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

path1="C:/Aiqiyi/Final/dataresult/PerUserWatching/"
outputpath="C:/Aiqiyi/Final/dataresult/SameVideoName/"
usernameFilename="PerUserWatchingTimes.txt"
usernameFile=open(path1+usernameFilename,"r")
userNameLines=usernameFile.readlines()
rank=[2,4,5]
userName=[]
for i in range(0,len(rank)):
    line=userNameLines[rank[i]]
    temp=line.split(",")
    userName.append(temp[1][0:len(temp[1])-1])

print userName

path2="C:/Aiqiyi/new_iqiyi_all/iqiyi_xyz_raw_final/"
filename_list = []
listdir(path2,filename_list)
filename_list=sorted(filename_list) # 按照userID排序# 进行排序
samevideoName={}

i=0
for filename in filename_list:
     i=i+1
     temp=filename.split("_")
     videoId=temp[2]
     userId=temp[1]
     if userId in userName:
         if videoId+"_"+userId  not in samevideoName:
               samevideoName[videoId+"_"+userId ]=1
         else:
               samevideoName[videoId+"_"+userId ]=samevideoName[videoId+"_"+userId ]+1

samevideoName=sorted(samevideoName.items(), lambda x, y: cmp(x[1], y[1]), reverse=True)
tempvN=[]
for svN in samevideoName:
    tempvN.append(svN[0])

tempvN.sort()
finalsamevideoName={}
for t in tempvN:
    temp=t.split("_")
    videoId=temp[0]
    if videoId  not in finalsamevideoName:
           finalsamevideoName[videoId]=1
    else:
           finalsamevideoName[videoId]=finalsamevideoName[videoId]+1

finalsamevideoName=sorted(finalsamevideoName.items(), lambda x, y: cmp(x[1], y[1]), reverse=True)

outputsamevideoName=[]
for fsvN in finalsamevideoName:
    if fsvN[1]==len(rank):
        outputsamevideoName.append(fsvN[0])

outputfile=open(outputpath+"sameVideoName.txt",'w')

for ovN in outputsamevideoName:
    outputfile.write(ovN+"\n")







