# -*- coding: utf-8 -*-
from sklearn.cluster import KMeans
from sklearn.externals import joblib
from sklearn import cluster
import os
import sys
from numpy import *
import numpy as np
import copy
import numpy as np

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        list_name.append(file)

def RotationAngle(userID,videoID,timestamp,threshold):
    path2="C:/Aiqiyi/new_iqiyi_all/1_1s/"
    file=open(path2+userID+"_"+videoID+"_"+timestamp+"_1s.txt","r")
    sum=0
    total=0
    lines=file.readlines()
    time = float((lines[0].split(" "))[0])
    if(len(lines)<=5 or time>1):
        return -1
    for line in lines:
        temp=line.split(" ")
        sum=sum+float(temp[2])
    if sum<=20:
        return -1
    file.close()
    return 1

# 生成10*3的矩阵
data = [[0,0,0],[0.1,0.1,0.1],[2,2,2],[2.1,2.1,2.1],[3,3,3],[3.1,3.1,3.1]]

#path = "C:/Aiqiyi/new_iqiyi_all/iqiyi_xyz_raw_final/"
path = "C:/Aiqiyi/test/"
outputpath="C:/Aiqiyi/Final/dataresult/Cut_KMeans/"
filename_list=[]
listdir(path, filename_list)
filename_list=sorted(filename_list) # 进行排序


kk = 0

for filename in filename_list:
    kk = kk + 1
    outputfile = open(outputpath + "cut.txt", 'a')
    print str(kk)+" "+str(len(filename_list))
    file = open(path + filename, "r")
    temp = filename.split("_")
    videoid = temp[1]
    deviceid = temp[2]
    lines = file.readlines()
    if len(lines) < 100 and RotationAngle(deviceid, videoid, temp[3], 30) == -1:
        file.close()
        continue
    data = []
    maxTime=len(lines)*0.2
    jj=0
    for line in lines:
        temp = line.split(",")
        x = float(temp[1])
        y = float(temp[2])
        z = float(temp[3])
        time=jj*1.0/len(lines)
        jj=jj+1
        data.append([x, y, z,time])
    forwardinertia = 0
    forwardinertia2 = 0
    forwardVariation = 0
    currentVariation = 0
    bestCluster = 1
    for clusters in range(1, 6):
        # 聚类为4类
        estimator = KMeans(n_clusters=clusters)
        # fit_predict表示拟合+预测，也可以分开写
        res = estimator.fit_predict(data)
        # 预测类别标签结果
        lable_pred = estimator.labels_
        # 各个类别的聚类中心值
        centroids = estimator.cluster_centers_
        # 聚类中心均值向量的总和
        inertia = estimator.inertia_
        if clusters == 1:
            forwardinertia = inertia
            continue
        if clusters == 2:
            currentVariation = float(abs(forwardinertia - inertia)) / forwardinertia
            if currentVariation < 0.1:
                bestCluster = clusters - 1
                break
            forwardinertia2 = forwardinertia
            forwardinertia = inertia
            continue
        forwardVariation = float(abs(forwardinertia - forwardinertia2)) / forwardinertia2
        currentVariation = float(abs(forwardinertia - inertia)) / forwardinertia
        if currentVariation < forwardVariation*0.5:
            bestCluster = clusters - 1
            break
        forwardinertia2 = forwardinertia
        forwardinertia = inertia
    estimator = KMeans(n_clusters=bestCluster)
    # fit_predict表示拟合+预测，也可以分开写
    res = estimator.fit_predict(data)
    # 预测类别标签结果
    lable_pred = estimator.labels_
    # 各个类别的聚类中心值
    centroids = estimator.cluster_centers_
    # 聚类中心均值向量的总和
    inertia = estimator.inertia_
    for l in lable_pred:
        outputfile.write(str(l)+",")
    outputfile.write(filename+"\n")




