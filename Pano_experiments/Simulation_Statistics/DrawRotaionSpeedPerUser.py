# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt


def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        list_name.append(file)


def getKey(x):
    return x[0]

def RotationAngle(userID,videoID,timestamp):
    path2="C:/Aiqiyi/new_iqiyi_all/1_1s/"
    file=open(path2+userID+"_"+videoID+"_"+timestamp+"_1s.txt","r")
    sum=0
    lines=file.readlines()
    time = float((lines[0].split(" "))[0])
    if len(lines)<5:
        return -1
    for line in lines:
        temp=line.split(" ")
        sum=sum+float(temp[2])
    file.close()
    return sum/len(lines)

userId="E035AB13-AC18-47F4-925E-1200CE34C28D"
filename_list = []
file=open("C:/Aiqiyi/Final/dataresult/Cut_User/cut_user_"+str(userId)+"_. txt",'r')
lines=file.readlines()
for line in lines:
    temp=(line.split(","))[-1]
    temp=temp[0:len(temp)-1]
    filename_list.append((temp))
print filename_list
data=[]

for filename in filename_list:
    temp=filename.split("_")
    uId=temp[2]
    if uId  != userId:
        continue
    data.append(RotationAngle(temp[2],temp[1],temp[3]))

data.sort()
print data
maxD=60

x=[0.0]
y=[0.0]
for i in range(0,100,1):
    tempsum=0
    for d in data:
        if d<=maxD*i*1.0/100:
            tempsum=tempsum+1
    if tempsum==0:
        continue
    x.append(maxD*i*1.0/100)
    y.append(tempsum*100*1.0/len(data))

plt.xlabel("The average rotation speed of video session (degree/s)")
plt.ylabel("The fractions of total sessions (%)")
plt.title("The cdf graph of the average rotation speed of video session \n(Viewer level, userID: "+str(userId)+",\n views: "+str(len(data))+" )")
plt.plot(x,y)
plt.tight_layout()
plt.show()


