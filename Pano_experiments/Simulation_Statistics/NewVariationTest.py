# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        file_path = os.path.join(path, file)
        temp=file.split("_")
        file=temp[1]+"_"+temp[0]+"_"+temp[2]+"_"+temp[3]
        list_name.append(file)

def getRealFile(file):
    temp = file.split("_")
    file = temp[1] + "_" + temp[0] + "_" + temp[2]+"_"+temp[3]
    return file



path="C:/Aiqiyi/new_iqiyi_all/1_1s/"
outputpath="C:/Aiqiyi/Final/dataresult/1_1sfinal_result/"
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


         # 这里将数据累计出来求方差和平均值，有几个片段，
         arr2 = []
         if (len(lines)==0 or float((lines[0].split(" "))[0])>1):
             continue

         for line in lines:
            temp=float((line.split(" "))[2])
            time=float((line.split(" "))[0])
            time = int(time)
            if time>=maxlines:
                maxlines=time
            arr2.append([time,temp])
         arr.append(arr2) #将数据加进来，不对齐的然后处理即可

         if(i==len(filename_list)):

                 # 这里代表进行了更换，因此还需要进一步处理
                 # 首先进行收尾处理，统计上一个的
                 sum_average = [0.0] * (maxlines + 1)
                 total_access = [0.0] * (maxlines + 1)
                 variation = [0.0] * (maxlines + 1)
                 # 求每个片段的平均值


                 #还不知道每一个块的数量
                 for m in range(0,len(arr)): # m代表
                     lines=len(arr[m])
                     for n in range(0,lines):
                         time=arr[m][n][0]
                         value=arr[m][n][1]
                         total_access[int(time)]=total_access[int(time)]+1
                         sum_average[int(time)] = sum_average[int(time)] + value

                 for k in range(0, len(sum_average)):
                     if total_access[k] == 0:
                         sum_average[k] = -1
                         continue
                     sum_average[k] = sum_average[k] / total_access[k]

                 # 求每个片段的方差
                 for j in range(0, len(arr)):
                     for k in range(0, len(arr[j])):
                         time = arr[j][k][0]
                         value = arr[j][k][1]
                         if time < len(variation):
                             variation[time] = variation[time] + (value - sum_average[time]) * (
                                     value - sum_average[time])
                         else:
                             variation.append((value - sum_average[time]) * (value - sum_average[time]))

                 for k in range(0, len(variation)):
                     if total_access[k] == 0:
                         variation[k] = -1
                         continue
                     variation[k] = variation[k] / total_access[k]

                 # 输出均值
                 outputfile = open(outputpath + "/" + initial_videoId +"_"+ str(i-start)+ "_average.txt", "w")
                 std = sys.stdout
                 sys.stdout = outputfile
                 for average in sum_average:
                     print average

                 outputfile = open(outputpath + "/" + initial_videoId + "_" + str(i - start) + "_sum.txt", "w")
                 sys.stdout = outputfile
                 for ta in total_access:
                     print ta

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

             sum_average = [0.0] * (maxlines+1)
             total_access = [0.0] * (maxlines+1)
             variation = [0.0] * (maxlines+1)
             # 求每个片段的平均值

             # 还不知道每一个块的数量
             for m in range(0, len(arr)):  # m代表
                 lines = len(arr[m])
                 for n in range(0, lines):
                     time = arr[m][n][0]
                     value = arr[m][n][1]
                     total_access[int(time)] = total_access[int(time)] + 1
                     sum_average[int(time)] = sum_average[int(time)] + value


             for k in range(0, len(sum_average)):
                 if total_access[k]==0:
                     sum_average[k]=-1
                     continue
                 sum_average[k] = sum_average[k] / total_access[k]

             #求每个片段的方差
             for j in range(0, len(arr)):
                 for k in range(0, len(arr[j])):
                     time = arr[j][k][0]
                     value = arr[j][k][1]
                     if time< len(variation):
                          variation[time] =  variation[time]+(value-sum_average[time])*(value-sum_average[time])
                     else:
                          variation.append((value-sum_average[time])*(value-sum_average[time]))

             for k in range(0, len(variation)):
                 if total_access[k]==0:
                     variation[k]=-1
                     continue
                 variation[k]=variation[k]/total_access[k]


             #输出均值
             outputfile=open(outputpath+"/"+initial_videoId+"_"+str(i-start) +"_average.txt","w")
             std=sys.stdout
             sys.stdout=outputfile
             for average in sum_average:
                 print average

            #输出人数
             outputfile=open(outputpath+"/"+initial_videoId+"_"+str(i-start) +"_sum.txt","w")
             sys.stdout = outputfile
             for ta in total_access:
                 print ta


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

             maxlines=-1
             arr2 = []
             for line in lines:
                 temp = float((line.split(" "))[2])
                 time = float(((line.split(" "))[0]))
                 time=int(time)
                 if time >= maxlines:
                     maxlines = time
                 arr2.append([time,temp])
             arr.append(arr2)  # 将数据加进来，不对齐的然后处理即可
             continue
