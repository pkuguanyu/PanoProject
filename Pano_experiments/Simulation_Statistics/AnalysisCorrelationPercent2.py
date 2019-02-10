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




path="D:/2018.4.1/correlationResult_percent/"
outputpath="D:/2018.4.1/CorrelationResult2_percent/"
filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list) # 进行排序
# 时间是0，卡顿是1，快进是2，回放是3，旋转角度是4，旋转范围是5 videosession

corr=[]
for j in range(0,5):
    cor=[]
    for i in range(0,5):
        temp =[]
        cor.append(temp)
    corr.append(cor)

corr2=[]
for j in range(0,5):
    cor2=[]
    for i in range(0,5):
        temp =[]
        cor2.append(temp)
    corr2.append(cor2)


name=[]

for filename in filename_list:
    file=open(path+filename)
    for line in file:
        temp=line.split(" ")
        for i in range(0,5):
            for j in range(0,5):
                corr[i][j].append(float(temp[i]))
                corr2[i][j].append(float(temp[j]))
        name.append(temp[6])



for i in range(0, 5):
    for j in range(0, 5):
        print i
        print j
        arr=corr[i][j]
        arr2=corr2[i][j]
        outputfile=open(outputpath+"correlation_"+str(i)+"_"+str(j)+"_percent.txt","w")
        for  k in range(0,len(arr)):
            outputfile.write(str(corr[i][j][k])+" "+str(corr2[i][j][k])+" "+name[k])
        outputfile.close()

