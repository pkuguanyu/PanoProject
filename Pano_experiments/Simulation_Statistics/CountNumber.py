# -*- coding: utf-8 -*-
import os
import  sys
# 统计打点一次和打点多次的

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        file_path = os.path.join(path, file)
        list_name.append(file)


def strcmp(str1,str2):

    temp1=str1.split("_")
    temp2=str2.split("_")
    str1video=temp1[1]
    str2video=temp2[1]
    str1device=temp1[2]
    str2device=temp2[2]
    str1timestamp=temp1[3]
    str2timestamp=temp2[3]
    id1=temp1[4]
    id2=temp2[4]
    s1=str1video+str1device+str1timestamp
    s2=str2video+str2device+str2timestamp
    if(s1>s2):
        return 1
    else:
        if(s1<s2):
            return -1
        else:
            if int(id1)>int(id2):
                return 1
            else:
                return -1



path='F:/AiqiyiDataAnalysis/data/58.56.65.65_8080output'

outputpath='F:/AiqiyiDataAnalysis/data/AnalysisResult'
filename_list=[]

listdir(path,filename_list)
filename_list=sorted(filename_list,strcmp)
sum=0 #统计正常数据
sum1=0
for file in filename_list:

    sum=sum+1
print sum