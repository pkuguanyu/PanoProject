# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt

def getkey(x):
    return x[0]
path="D:/2018.4.1/correlationResult/"
filename="type_2.txt"
file=open(path+filename,"r")

lines=file.readlines()
data=[]
for line in lines:
    temp=line.split(" ")
    numbersofmotionarea=int(temp[0])
    averageangle=float(temp[1])
    data.append([numbersofmotionarea,averageangle])

data.sort(key=getkey)
x=[]
y=[]
for d in data:
    x.append(d[0])
    y.append(d[1])
xx=[]
yy=[]
for i in range(0,70):
    temp=0
    num=0
    for j in range(1,len(x)):
        if x[j]==i:
            temp=temp+y[j]
            num=num+1
    if num==0:
        continue
    temp=temp/num
    xx.append(i)
    yy.append(temp)


plt.title("Correlation between number of users' region of interest \n and average viewpoint standard variation ")
plt.xlabel("The number of users' region of interest per video")
plt.ylabel("The average viewpoint standard variation \n of each region of interest(degree)")
plt.plot(xx,yy,'ro')
plt.show()

