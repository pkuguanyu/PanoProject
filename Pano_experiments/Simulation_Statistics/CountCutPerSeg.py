# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import numpy as np
import copy
import matplotlib.pyplot as plt
def angle(x,y):
    Lx=np.sqrt(x[0]*x[0]+x[1]*x[1]+x[2]*x[2])
    Ly=np.sqrt(y[0]*y[0]+y[1]*y[1]+y[2]*y[2])
    cos_angle=(x[0]*y[0]+x[1]*y[1]+x[2]*y[2])/(Lx*Ly)
    angle=np.arccos(cos_angle)
    angle=angle*360/2/np.pi
    return angle

def variationPoint(point):
    point_mean=[0.0]*3
    for p in point:
        point_mean[0]=point_mean[0]+p[0]
        point_mean[1] = point_mean[1] + p[1]
        point_mean[2] = point_mean[2] + p[2]

    point_mean[0] = point_mean[0] / len(point)
    point_mean[1] = point_mean[1] / len(point)
    point_mean[2] = point_mean[2] / len(point)
    ll=np.sqrt(point_mean[0]*point_mean[0]+point_mean[1]*point_mean[1]+point_mean[2]*point_mean[2])
    point_mean[0]=point_mean[0]/ll
    point_mean[1] = point_mean[1] / ll
    point_mean[2] = point_mean[2] / ll
    distance=0
    for p in point:
        #temp_distance= np.sqrt((point_mean[0]-p[0])*(point_mean[0]-p[0])+(point_mean[1]-p[1])*(point_mean[1]-p[1])+(point_mean[2]-p[2])*(point_mean[2]-p[2]))
        temp_distance=angle(point_mean,p)
        distance=temp_distance+distance
    distance=distance
    return distance,len(point)

def cal(fileseg,data):
    start=0
    end=1
    dataToBeVp=[]
    forward=0
    i=0
    sum=0
    len2=0
    for d in data:
        if i==0:
            forward=d[3]
        else:
            if d[3]<forward:
                return -1
        forward=d[3]
        i=i+1

        if d[3]>=float(fileseg[start]) and d[3]<float(fileseg[end]):
              dataToBeVp.append(d[0:3])
              continue
        else:
            if len(dataToBeVp)!=0:
                 tempsum,templen=variationPoint(dataToBeVp)
            else:
                continue
            dataToBeVp=[]
            sum=tempsum+sum
            len2=templen+len2
            start=start+1
            end=end+1
    return sum*1.0/len2


def getKey(x):
    return x[0]
videoId="569229100"
view=1201

path="C:/Aiqiyi/test/"
file=open("C:/Aiqiyi/cut.txt",'r')
lines=file.readlines()
lenCut=[]
for line in lines:
    temp=line.split(",")
    length=len(temp)-1
    length=length-2+1
    lenCut.append([length,[temp[0:len(temp)-1]],temp[len(temp)-1]])
    #长度 ,序列，文件名
lenCut.sort(key=getKey,reverse=True)


for seg in range(1,3):
    fileNameOf1Seg = []
    fileseg=[]
    for lC in lenCut:
        if seg == lC[0]:
            fileNameOf1Seg.append(lC[2][0:len(lC[2]) - 1])
            fileseg.append(lC[1][0])
    vP = []
    i=0
    for filename in fileNameOf1Seg:
        file = open(path + filename, "r")
        temp = filename.split("_")
        lines = file.readlines()
        data = []
        for line in lines:
            temp = line.split(",")
            x = float(temp[1])
            y = float(temp[2])
            z = float(temp[3])
            time = float(temp[0])
            data.append([x, y, z, time])
        averageAngle=cal(fileseg[i],data)
        i = i + 1
        if averageAngle==-1:
            continue
        vP.append([averageAngle, filename])


    vP.sort(key=getKey)
    if seg==6:
        print vP
    maxV = int(vP[-1][0])
    x = []
    y = [0.0] * maxV
    for i in range(0, maxV):
        for v in vP:
            if v[0] <= i:
                y[i] = y[i] + 1
        x.append(i)
    for i in range(0, len(y)):
        y[i] = y[i]*100 / len(vP)
    plt.plot(x, y)

    plt.xlabel("Average viewpoint discreteness of "+str(seg)+" ROI session (/degree)")
    plt.ylabel("Fractions of total session (%)")
    plt.title("The cdf graph of average viewpoint discreteness of "+str(seg)+" ROI session \n (view level,videoID: "+str(videoId)+", views: "+str(view)+" )")
    plt.tight_layout()
    plt.show()

