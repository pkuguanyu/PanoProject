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




path="D:/2018.4.1/CorrelationResult2_percent/"
outputpath="D:/2018.4.1/"
filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list) # 进行排序
# 时间是0，卡顿是1，快进是2，回放是3，旋转角度是4，旋转范围是5 videosession



key=["time","pause","fastwind","replay","Rotaion Angle"]

for filename in filename_list:
    file = open(path+filename)
    x = []
    y = []
    keys1 = (filename.split("_"))[1]
    keys2 = (filename.split("_"))[2]
    print keys1
    print keys2
    for line in file:
        temp = line.split(" ")
        x.append(float(temp[0]))
        y.append(float(temp[1]))
    if int(keys1)==4:
        plt.xlim([0,200])
    if int(keys2)==4:
        plt.ylim([0,200])
    plt.xlabel(key[int(keys1)])
    plt.ylabel(key[int(keys2)])
    plt.plot(x,y,'ro')
    plt.savefig(outputpath+key[int(keys1)]+"_"+key[int(keys2)]+".png")

    plt.show()





