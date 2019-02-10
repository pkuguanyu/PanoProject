# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        file_path = os.path.join(path, file)
        list_name.append(file)

path="C:/Aiqiyi/CalculateCdf/data/"
outputpath="C:/Aiqiyi/CalculateCdf/"
filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list) # 进行排序

threshold=60
threshold2=10
timegap=1

#计算一个视频 每个切片的CDF

MaxClips=-1
MaxAngles=180
for filename in filename_list:
     inputfile=open(path+filename,"r")
     lines=inputfile.readlines()
     if (len(lines) == 1 or len(lines) == 0):
         continue
     Clips=len(lines)
     if(MaxClips<Clips):
         MaxClips=Clips

print MaxClips
x=[]
for i in range(0,MaxAngles):
    x.append(i)

for i in range(0,MaxClips) :#每一个代表一个切片,每一个循环画一个切片
        y=[0.0]*MaxAngles
        sum=0
        for filename in filename_list:
            inputfile = open(path + filename, "r")
            lines = inputfile.readlines()
            if(len(lines)==1 or len(lines)==0):
                continue
            if(len(lines)>i):
                A=float(lines[i])
                sum=sum+1
            else:
                continue
            for angle in range(0, MaxAngles):  # 每一个切片的小于一个角度
                if(A<angle):
                    y[angle]=y[angle]+1
        for j in range(0,len(y)):
             y[j]=y[j]*1.0/sum
        if(i %10 !=0):
            continue
        plt.title("The rotaion angles of Cdf in each Clips of each videos")
        plt.xlabel("Angle of Rotation")
        plt.ylabel("Percent")
        plt.plot(x,y)
        plt.savefig(outputpath+"Clip"+str(i)+".png")
        plt.show()





