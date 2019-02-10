# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt

def calcMean(x,y):
    sum_x = sum(x)
    sum_y = sum(y)
    n = len(x)
    x_mean = float(sum_x+0.0)/n
    y_mean = float(sum_y+0.0)/n
    return x_mean,y_mean

#计算Pearson系数
def calcPearson(x,y):
    x_mean,y_mean = calcMean(x,y)	#计算x,y向量平均值
    n = len(x)
    sumTop = 0.0
    sumBottom = 0.0
    x_pow = 0.0
    y_pow = 0.0
    for i in range(n):
        sumTop += (x[i]-x_mean)*(y[i]-y_mean)
    for i in range(n):
        x_pow += math.pow(x[i]-x_mean,2)
    for i in range(n):
        y_pow += math.pow(y[i]-y_mean,2)
    sumBottom = math.sqrt(x_pow*y_pow)
    p = sumTop/sumBottom
    return p

#六维度 分别指：用户观看时间（百分比），
# 用户卡顿（百分比），用户快进（百分比），用户回放（百分比），头部旋转角度，头部旋转范围么？
# 时间是0，卡顿是1，快进是2，回放是3，旋转角度是4，旋转范围是5 videosession
#散点图 一个会话代表一个点

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        list_name.append(file)

#现在要统计每一个视频会话对应的观看时间（百分比），卡顿，快进，回放次数


def getKey(x):
    return float(x[0])


# 时间是0，卡顿是1，快进是2，回放是3，旋转角度是4，旋转范围是5 videosession
TimeAndAngle=[]
id1=1
id2=0
file=open("D:/2018.4.1/CorrelationResult2_percent/correlation_"+str(id1)+"_"+str(id2)+"_percent.txt","r")
lines=file.readlines()
for line in lines:
    temp=line.split(" ")
    Time=float(temp[0])
    Angle=float(temp[1])
    TimeAndAngle.append([Time,Angle])

TimeAndAngle.sort(key=getKey)
for tA in TimeAndAngle:
    print tA

x=[]
y=[]
for i in range(0,10000,1):
    if i >= 100:
        break
    x.append(i*1.0/10000)
    sum2=0
    times=0

    for tA in TimeAndAngle:
        if tA[0]>=i*1.0/10000 and tA[0]<(i+1)*1.0/10000:
                times = times + 1
                sum2=sum2+tA[1]
    if times==0:
        y.append(sum2)
        continue
    sum2=sum2*1.0/times
    y.append(sum2)


plt.plot(x,y,'ro')

plt.show()




