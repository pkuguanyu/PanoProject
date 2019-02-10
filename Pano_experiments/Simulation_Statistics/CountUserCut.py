# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import numpy as np
import copy
import matplotlib.pyplot as plt

def getKey(x):
    return int(x[0])

# 对于每一个用户所观看的会话进行分类统计
userId="E035AB13-AC18-47F4-925E-1200CE34C28D"
file=open("C:/Aiqiyi/Final/dataresult/Cut_User/cut_user_"+str(userId)+"_. txt",'r')
lines=file.readlines()
lenCut=[]
for line in lines:
    temp=line.split(",")
    length=len(temp)-1
    length=length-2+1
    lenCut.append([length,temp[len(temp)-1]])
lenCut.sort(key=getKey,reverse=True)
count=[0.0]*11
for i in range(1,11):
    for lC in lenCut:
        if i==lC[0]:
            count[i]=count[i]+1
x=[]
label=[]
for i in range(1,len(count)):
    if count[i]!=0:
        x.append(count[i])
        label.append(str(i))

plt.title("The pie chart of user's ROI number (Viewer level, \nuserID: "+str(userId)+",\n views: "+str(len(lenCut))+" )")
plt.pie(x=x,labels=label,shadow=True, autopct='%3.1f %%', pctdistance = 0.9)
plt.legend(loc='upper left',bbox_to_anchor=(-0.15, 1))
plt.grid()
plt.show()