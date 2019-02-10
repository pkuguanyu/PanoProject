# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt



def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        list_name.append(file)

def findsub(dict,line):
    start2=-1
    end2=-1
    for i in range(0,len(line)-1):
        start=line[i]
        end=line[i+1]
        flag=0
        for key in dict:
            temp=key.split("_")
            start2=int(temp[0])
            end2=int(temp[1])
            if abs(start-start2)<=20 and abs(end-end2)<=20:
                flag=1
                break#代表找到了
        if flag==1:
            dict[str(start2)+"_"+str(end2)]= dict[str(start2)+"_"+str(end2)]+1
        else:
            dict[str(start) + "_" + str(end)]=1





path=""
outputpath=""
filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list) # 进行排序


lines=[]

for filename in filename_list:
    file=open(path+filename,'r')
    line=file.readlines()
    temp=line.split(',')
    templine=[]
    for t in temp:
        templine.append(t)
    lines.append(templine)

dict={}

for line in lines:
    findsub(dict,line)

#对dict进行排序
dict=sorted(dict.items(),key = lambda x:x[1],reverse = True)
outputfile=open(outputpath+"",'w')

for d in dict:
    outputfile.write(d+" "+dict[d]+"\n")