# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt


# 状态码为 0代表正常， 5代表不从0开始， 2代表 快进， 3代表卡顿，4代表回放,1，6代表异常

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        list_name.append(file)

path="C:/Aiqiyi/new_iqiyi_all/1_1s/"
filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list) # 进行排序

for filename in filename_list:
    file=open(path+filename,"r")
    lines=file.readlines()
    i=0
    sum=0
    time=0
    for line in lines:
        temp=line.split(" ")
        rotate=float(temp[2])
        sum=sum+rotate
        time=time+1
    sum=sum/time
    if sum>=300:
        print filename
