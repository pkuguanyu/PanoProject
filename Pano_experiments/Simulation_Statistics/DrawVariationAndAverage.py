# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        file_path = os.path.join(path, file)
        temp=file.split("_")
        if(temp[2]=="variation.txt"):
              continue

        file=temp[1]+"_"+temp[0]+"_"+temp[2]
        list_name.append(file)

def getRealFile(file):
    temp = file.split("_")
    file = temp[1] + "_" + temp[0] + "_" + temp[2]
    return file


def comp(a,b):
    tempa=a.split("_")
    numbera=int(tempa[0])
    tempb = b.split("_")
    numberb = int(tempb[0])
    if numbera>=numberb:
        return 1
    else:
        return -1

path="C:/Aiqiyi/1_1s_result/"
outputpath="C:/Aiqiyi/Graph_1_1s/"
filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list,comp,reverse=True) # 进行排序

i=0

for i in range(0,50):
    filename=filename_list[i]
    inputfile=open(path+getRealFile(filename),"r")
    lines=inputfile.readlines()
    x=[]
    y=[]
    j=0
    for line in lines:
        ave=float(line)
        x.append([j])
        j=j+1
        y.append(ave)
    print filename
    plt.title("The variation grapth of rotation anlge between video sections"+str(i))
    plt.xlabel("The order number of video sectionings")
    plt.ylabel("Rotation Angle")
    plt.plot(x,y)
    plt.savefig(outputpath+filename+".png")
    plt.show()
