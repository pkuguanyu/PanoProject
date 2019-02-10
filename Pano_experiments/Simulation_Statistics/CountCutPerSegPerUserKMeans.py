# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import numpy as np
import copy
import matplotlib.pyplot as plt
from sklearn.cluster import KMeans
from sklearn.externals import joblib
from sklearn import cluster

def angle(x,y):
    Lx=np.sqrt(x[0]*x[0]+x[1]*x[1]+x[2]*x[2])
    Ly=np.sqrt(y[0]*y[0]+y[1]*y[1]+y[2]*y[2])
    cos_angle=(x[0]*y[0]+x[1]*y[1]+x[2]*y[2])/(Lx*Ly)
    angle=np.arccos(cos_angle)
    angle=angle*360/2/np.pi
    return angle

def variationPoint(point):
    point_mean=[0.0]*3
    for p in point:
        point_mean[0]=point_mean[0]+p[0]
        point_mean[1] = point_mean[1] + p[1]
        point_mean[2] = point_mean[2] + p[2]

    point_mean[0] = point_mean[0] / len(point)
    point_mean[1] = point_mean[1] / len(point)
    point_mean[2] = point_mean[2] / len(point)
    ll=np.sqrt(point_mean[0]*point_mean[0]+point_mean[1]*point_mean[1]+point_mean[2]*point_mean[2])
    point_mean[0]=point_mean[0]/ll
    point_mean[1] = point_mean[1] / ll
    point_mean[2] = point_mean[2] / ll
    distance=0
    for p in point:
        #temp_distance= np.sqrt((point_mean[0]-p[0])*(point_mean[0]-p[0])+(point_mean[1]-p[1])*(point_mean[1]-p[1])+(point_mean[2]-p[2])*(point_mean[2]-p[2]))
        temp_distance=angle(point_mean,p)
        distance=temp_distance+distance
    distance=distance
    return distance,len(point)

def cal(data):
    dataToBeVp=[]
    sum=0
    len2=0
    for d in data:
        dataToBeVp=d
        tempsum, templen = variationPoint(dataToBeVp)
        sum = tempsum + sum
        len2 = templen + len2
        start = start + 1
        end = end + 1
    return sum*1.0/len2


def getKey(x):
    return x[0]

def check(videoId,lenCut):
    for lC in lenCut:
        fn=lC[2][0:len(lC[2]) - 1]
        if videoId in fn:
            return 0
    return 1


def Kmeans(data):
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
        if currentVariation < forwardVariation:
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
    return lable_pred


def judgeLabel(label,data):
     maxL=max(label)
     tempdata=[]
     result=[]
     for i in range(0,maxL+1):
         tempdata = []
         k=0
         for l in label:
             if l==i:
                 tempdata.append(data[k])
             k=k+1
         result.append(tempdata)
     return result

path1="C:/Aiqiyi/Final/dataresult/PerUserWatching/"
usernameFilename="PerUserWatchingTimes.txt"
usernameFile=open(path1+usernameFilename,"r")
userNameLines=usernameFile.readlines()
rank=[2,4,5]
userName=[]
for i in range(0,len(rank)):
    line=userNameLines[rank[i]]
    temp=line.split(",")
    userName.append(temp[1][0:len(temp[1])-1])

#找出相同的视频名称
sameVideoList=[]
sameVideoFile=open("C:/Aiqiyi/Final/dataresult/SameVideoName/sameVideoName.txt",'r')
svFLines=sameVideoFile.readlines()
for svF in  svFLines:
    sameVideoList.append(svF[0:len(svF)-1])

#写入Kmeans



#画图
for un in userName:
    userId = un
    path = "C:/Aiqiyi/new_iqiyi_all/iqiyi_xyz_raw_final/"
    file = open("C:/Aiqiyi/Final/dataresult/Cut_User_KMeans/cut_user_" + str(userId) + "_KMeans_.txt", 'r')
    lines = file.readlines()
    lenCut = []
    for line in lines:
        temp = line.split(",")
        length = len(temp) - 1
        length = length - 2 + 1
        t = temp[len(temp) - 1].split("_")
        videoId = t[1]
        if videoId in sameVideoList and check(videoId,lenCut):
            lenCut.append([length, [temp[0:len(temp) - 1]], temp[len(temp) - 1]])
        # 长度 ,序列，文件名

    lenCut.sort(key=getKey, reverse=True)

    for lC in lenCut:
        print lC[2][0:len(lC[2]) - 1]

    for seg in range(1, 2):
        fileNameOf1Seg = []
        fileseg = []
        for lC in lenCut:
            fileNameOf1Seg.append(lC[2][0:len(lC[2]) - 1])
            fileseg.append(lC[1][0])
        if fileNameOf1Seg == []:
            continue
        vP = []
        i = 0
        for filename in fileNameOf1Seg:
            file = open(path + filename, "r")
            temp = filename.split("_")
            lines = file.readlines()
            data = []
            for line in lines:
                temp = line.split(",")
                x = float(temp[1])
                y = float(temp[2])
                z = float(temp[3])
                time = float(temp[0])
                data.append([x, y, z, time])
            result=judgeLabel(fileseg[i],data)
            if len(data)>5:
                averageAngle = cal(result, data)
            else:
                i=i+1
                continue
            i = i + 1
            if averageAngle == -1:
                continue
            vP.append([averageAngle, filename])

        vP.sort(key=getKey)
        maxV = 30
        x = [0.0]
        y = [0.0]
        for i in range(0, 100, 1):
            temp = 0
            for v in vP:
                if v[0] <= maxV * i * 1.0 / 100:
                    temp = temp + 1
            if temp == 0:
                continue
            x.append(maxV * i * 1.0 / 100)
            y.append(temp * 100)

        for i in range(0, len(y)):
            y[i] = y[i] * 1.0 / len(vP)
        plt.plot(x, y)
        plt.xlim([-1, 30])
        # plt.xlabel("Average viewpoint discreteness of "+str(seg)+" ROI session(/degree)")
        plt.xlabel("Average viewpoint discreteness of video sessions(/degree)")
        plt.ylabel("The fractions of total session (%)")
        # plt.title("The cdf graph of average viewpoint discreteness of "+str(seg)+" ROI session \n (Viewer level, userID: "+str(userId)+",\n views: "+str(len(vP))+" )")
        plt.title("The cdf graph of average viewpoint discreteness of video sessions \n (Viewer level, userID: " + str(
            userId) + ",\n views: " + str(len(lenCut)) + " )")
        plt.tight_layout()
        plt.show()

