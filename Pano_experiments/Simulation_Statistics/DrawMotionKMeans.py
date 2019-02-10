# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt

def getkey(x):
    return x[0]
path="E:/2018.4.1/correlationResult/"
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
cdfData={}

for d in data:
    if d[0] not in cdfData:
        cdfData[d[0]]=1
    else:
        cdfData[d[0]]=cdfData[d[0]]+1
cal=[0.0]
for c in cdfData:
    cal.append(cdfData[c])
    print c
x=[]
y=[]
for i in range(0,len(cal)):
    x.append(i)
    temp=0
    for j in range(0,len(cal)):
        if j<=i:
            temp=temp+cal[j]
    y.append(temp*1.0/sum(cal))
plt.xlabel("The number of users' region of interest per video")
plt.ylabel("Fractions of videos(%)")
plt.title("The cdf graph of users' region of interest")
plt.plot(x,y,'r*-',markevery=0.2)


plt.show()



