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



path="C:/Aiqiyi/DirectionResult3/"
filename_list = []
listdir(path,filename_list)
k=0
x=[]


for i in range(0,15):
    x.append(i)
y=[0]*15

sum=0.0
for filename in filename_list:
    file = open(path + filename, "r")
    lines = file.readlines()
    sum = sum + len(lines)
    print sum
for filename in filename_list:
    print filename
    k=k+1
    file=open(path+filename,"r")
    lines=file.readlines()

    if len(lines)==0:
        continue
    for i in range(0,15):
        tempsum=0
        for line in lines:
            temp=line.split(" ")
            changeTimes=int(temp[2])
            if(changeTimes<=i-1):
                tempsum=tempsum+1
        y[i]=y[i]+tempsum*1.0/sum

    if k%1000==0:
        plt.plot(x, y)
        plt.title("The Cdf Of Direction Changes during each user's watching")
        plt.xlabel("Times Of Changing")
        plt.ylabel("Percent")
        plt.savefig("C:/Aiqiyi/Graph3/" + str(k) + ".png")
        plt.show()
