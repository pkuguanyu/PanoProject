# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        file_path = os.path.join(path, file)
        temp=file.split("_")
        file=temp[1]+"_"+temp[0]+"_"+temp[2]
        list_name.append(file)

def getRealFile(file):
    temp = file.split("_")
    file = temp[1] + "_" + temp[0] + "_" + temp[2]
    return file



path="C:/Aiqiyi/new_iqiyi/1_1s/"
outputpath="C:/Aiqiyi/1_1s_result/"
filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list) # 进行排序

initial_videoId=""
initial_userId=""
i=0
start=i
arr = [] #用来存储数据

maxlines=-1
for filename in filename_list:
     i=i+1
     temp=filename.split("_")
     videoId=temp[0]
     userId=temp[1]

     if i==1:
         initial_videoId=videoId
         start=i

     if videoId==initial_videoId : #这一部分统计同一个视频，不同用户的数据
         print 1111
         print filename
         inputfile=open(path+getRealFile(filename),"r")
         lines=inputfile.readlines()
         #记录最大的片段数量
         if(len(lines)>=maxlines):
             maxlines=len(lines)

         # 这里将数据累计出来求方差和平均值，有几个片段，
         arr2 = []
         if (len(lines)==1 or len(lines)==0):
             continue

         for line in lines:
            temp=float(line)
            arr2.append(temp)
         arr.append(arr2) #将数据加进来，不对齐的然后处理即可

         if(i==len(filename_list)):

                 # 这里代表进行了更换，因此还需要进一步处理
                 # 首先进行收尾处理，统计上一个的
                 sum_average = [0.0] * maxlines
                 total_access = [0.0] * maxlines
                 variation = [0.0] * maxlines
                 # 求每个片段的平均值


                 #还不知道每一个块的数量
                 for m in range(0,len(arr)): # m代表
                     lines=len(arr[m])
                     for n in range(0,lines):
                         total_access[n]=total_access[n]+1
                         sum_average[n] = sum_average[n] + arr[m][n]

                 for k in range(0, len(sum_average)):
                     sum_average[k] = sum_average[k] / total_access[k]

                 # 求每个片段的方差
                 for j in range(0, len(arr)):
                     for k in range(0, len(arr[j])):
                         if k < len(variation):
                             variation[k] = variation[k] + (arr[j][k] - sum_average[k]) * (arr[j][k] - sum_average[k])
                         else:
                             variation.append((arr[j][k] - sum_average[k]) * (arr[j][k] - sum_average[k]))

                 for k in range(0, len(variation)):
                     variation[k] = variation[k] / total_access[k]

                 # 输出均值
                 outputfile = open(outputpath + "/" + initial_videoId +"_"+ str(i-start)+ "_average.txt", "w")
                 std = sys.stdout
                 sys.stdout = outputfile
                 for average in sum_average:
                     print average

                 # 输出方差
                 outputfile = open(outputpath + "/" + initial_videoId+"_"+ str(i-start) + "_variation.txt", "w")
                 sys.stdout = outputfile
                 for v in variation:
                     print v
                 sys.stdout = std

                 initial_videoId = videoId
                 arr=[]
                 start=i


     else:
             print 222
             print filename
             #这里代表进行了更换，因此还需要进一步处理
             #首先进行收尾处理，统计上一个的

             sum_average = [0.0] * maxlines
             total_access = [0.0] * maxlines
             variation = [0.0] * maxlines
             # 求每个片段的平均值

             # 还不知道每一个块的数量
             for m in range(0, len(arr)):  # m代表
                 lines = len(arr[m])
                 for n in range(0, lines):
                     total_access[n] = total_access[n] + 1
                     sum_average[n] = sum_average[n] + arr[m][n]


             for k in range(0, len(sum_average)):
                 sum_average[k] = sum_average[k] / total_access[k]

             #求每个片段的方差
             for j in range(0, len(arr)):
                 for k in range(0, len(arr[j])):
                     if k< len(variation):
                          variation[k] =  variation[k]+(arr[j][k]-sum_average[k])*(arr[j][k]-sum_average[k])
                     else:
                          variation.append((arr[j][k]-sum_average[k])*(arr[j][k]-sum_average[k]))

             for k in range(0, len(variation)):
                 variation[k]=variation[k]/total_access[k]


             #输出均值
             outputfile=open(outputpath+"/"+initial_videoId+"_"+str(i-start) +"_average.txt","w")
             std=sys.stdout
             sys.stdout=outputfile
             for average in sum_average:
                 print average

             #输出方差
             outputfile = open(outputpath + "/" + initial_videoId +"_"+str(i-start) + "_variation.txt", "w")
             sys.stdout = outputfile
             for v  in  variation:
                 print v
             sys.stdout=std

             arr = [] #清空数据
             start=i
             #接着将当前数据读进去
             initial_videoId=videoId
             inputfile = open(path + getRealFile(filename), "r")
             lines = inputfile.readlines()
             # 这里将数据累计出来求方差和平均值，有几个片段，动态开辟几个数组

             maxlines=len(lines)
             arr2 = []
             for line in lines:
                 temp = float(line)
                 arr2.append(temp)
             arr.append(arr2)  # 将数据加进来，不对齐的然后处理即可
             continue
