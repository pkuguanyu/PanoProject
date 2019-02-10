# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        list_name.append(file)





path="C:/Aiqiyi/test/"
filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list) # 进行排序


i=0


maxlines=-1
maxtime_list=[]
for filename in filename_list:
     i=i+1
     file=open(path+filename,'r')
     lines=file.readlines()
     maxtime=-1
     for line in lines:
         temp=line.split(",")
         time=float(temp[0])
         if time>=maxtime:
             maxtime=time
     print maxtime
     maxtime_list.append(maxtime)

maxtime_list.sort()
x=[]
y=[]
print max(maxtime_list)
for i in range(0,int(max(maxtime_list))):
    sum=0
    for j in range(0,len(maxtime_list)):
        if maxtime_list[j]>i and maxtime_list[j]<i+1:
            sum=sum+1
    x.append(i)
    y.append(sum)
plt.plot(x,y,'ro')
plt.show()