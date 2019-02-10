# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt



def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        list_name.append(file)





path="C:/Aiqiyi/new_iqiyi_all/1_1s/"
filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list) # 进行排序

count_Normal=0
count_Abnormal=0
t=0
for filename in filename_list:
    t=t+1
    print str(t)+" "+str(len(filename_list))
    file=open(path+filename,"r")
    lines=file.readlines()
    time=float((lines[0].split(" "))[0])
    if time>1:
        count_Abnormal=count_Abnormal+1
    else:
        count_Normal=count_Normal+1


print count_Normal
print count_Abnormal