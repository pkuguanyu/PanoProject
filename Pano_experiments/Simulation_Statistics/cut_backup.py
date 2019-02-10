# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import numpy as np
import matplotlib.pyplot as plt
import copy



def listdir(path, list_name):  # ����洢��list
    for file in os.listdir(path):
        list_name.append(file)

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
    return distance


def findBestPostion(data,cp):
    min2=9999999999
    minlocation=-1
    # 能切的位置
    for i in range(1,len(data)):
        tempcp = copy.copy(cp)
        tempresult=0
        if i not in tempcp:
           tempcp.append(i)
        else:
            continue
        tempcp.append(len(data))
        tempcp.sort()
        for j in range(0,len(tempcp)-1):
            tempresult = tempresult +variationPoint(data[tempcp[j]:tempcp[j+1]])
        if tempresult<min2:
            min2=tempresult
            minlocation=i
    return minlocation,min2



path = "C:/Aiqiyi/new_iqiyi_all/iqiyi_xyz_raw_final/"
#path = "C:/Aiqiyi/test/"
outputpath="E:/2018.4.1/correlationResult/"

filename_list =["VR_569229100_357755071703969_1506003388286_.txt"]
#listdir(path, filename_list)
# filename_list=sorted(filename_list) # ��������


kk = 0

for filename in filename_list:
    kk = kk + 1
    file = open(path + filename, "r")
    temp = filename.split("_")
    videoid = temp[1]
    deviceid = temp[2]
    lines = file.readlines()
    if len(lines) < 100 and RotationAngle(deviceid, videoid, temp[3], 30) == -1:
        file.close()
        continue
    data = []
    for line in lines:
        temp = line.split(",")
        x = float(temp[1])
        y = float(temp[2])
        z = float(temp[3])
        time=float(temp[0])
        data.append([x, y, z,time])
    cutPosition = [0]
    forwardmin2 = variationPoint(data)* (len(cutPosition))
    min2 = 0
    i = 0
    
    while (len(cutPosition) < len(data) - 1):
        location, min2 = findBestPostion(data, cutPosition)
        min2 = min2 * (len(cutPosition)+1)*0.8
        if min2 > forwardmin2:
            cutPosition.sort()
            print cutPosition
            break
        cutPosition.append(location)
        forwardmin2 = min2

    file.close()






