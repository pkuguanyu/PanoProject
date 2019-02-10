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







path="D:/2018.4.1/"
filename="PlaytimeAndAngle.txt"
file = open(path+filename)
x = []
y = []

for line in file:
    temp = line.split(" ")
    if float(temp[0])<200:
        x.append(float(temp[0]))
        y.append(float(temp[1]))
x_final=[]
y_final=[]
y_final2=[]
y_final3=[]
for i in range(0,200,1):
    temp_sum=0
    temp_num=0
    for j in range(0,len(x)):
        if x[j]>=i and x[j]<i+1:
               temp_sum=temp_sum+y[j]
               temp_num=temp_num+1
    if temp_num == 0:
        continue
    else:
        if temp_num>10:
            x_final.append(i)
            y_final.append(temp_sum * 1.0 / temp_num)
            y_final3.append(temp_num)


plt.plot(x_final, y_final, 'r*')
plt.title("Correlation between rotation angle and \n average playback time of watching video")
plt.xlabel("Rotation Angle(/degree)")
plt.ylabel("Average playback time of watching video(/s)")
plt.xlim([0,100])
plt.show()
plt.plot(x_final, y_final3, 'r*')
plt.title("Correlation between rotation angle and \n numbers of watching people")
plt.xlabel("Rotation Angle(/)")
plt.ylabel("Numbers of watching people")
plt.xlim([0,100])
plt.show()





