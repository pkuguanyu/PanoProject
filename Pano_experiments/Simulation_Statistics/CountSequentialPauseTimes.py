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

path="C:/Aiqiyi/new_iqiyi_all/iqiyi_xyz_raw_final/"
filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list) # 进行排序

count=[0.0]*100
kk=0
for filename in filename_list:

    kk=kk+1
    file=open(path+filename,"r")
    lines=file.readlines()
    forwardstatus=-1
    countsequential=0
    i=0
    for line in lines:
        temp=line.split(",")
        status=int(temp[4])
        if i==0:
            forwardstatus=status
            if status==3:
                countsequential=countsequential+1
        else:
           if status == 3:
                countsequential = countsequential + 1
                if forwardstatus==1:
                    countsequential=countsequential+1
           else:
               if countsequential==2:
                   print filename
                   print line
               count[countsequential]=count[countsequential]+1
               countsequential=0
           forwardstatus=status
        i=i+1

print count
