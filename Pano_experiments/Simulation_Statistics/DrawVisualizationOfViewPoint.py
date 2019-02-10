# coding=utf-8
import numpy as np
import matplotlib.pyplot as plt
import mpl_toolkits.mplot3d
import os
import sys
from numpy import *
import copy

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        list_name.append(file)


def averagePoint(point):
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

    return point_mean

u = np.linspace(0, 2 * np.pi, 100)
v = np.linspace(0, np.pi, 100)
radius=1
center=[0,0,0]
x = radius * np.outer(np.cos(u), np.sin(v)) + center[0]
y = radius * np.outer(np.sin(u), np.sin(v)) + center[1]
z = radius * np.outer(np.ones(np.size(u)), np.cos(v)) + center[2]
z2 = 0 * np.outer(np.ones(np.size(u)), np.cos(v)) + center[2]
#三维图形
ax=plt.subplot(111, projection='3d')
ax.view_init(elev=17, azim=7)
ax.plot_surface(x,y,z,rstride=2, cstride=1, cmap=plt.cm.Blues_r,alpha=0.5)
ax.plot_surface(x*1.1,y*1.1,z2,rstride=3, cstride=3, cmap='gray_r',alpha=0.3)
ax.plot_surface(x*1.1,z2,y*1.1,rstride=3, cstride=3, cmap='gray_r',alpha=0.3)
#设置坐标轴标签
ax.set_xlabel('X')
ax.set_ylabel('Y')
ax.set_zlabel('Z')

path="C:/Aiqiyi/new_iqiyi_all/iqiyi_xyz_raw_final/"
filename_list = []
listdir(path,filename_list)
videoId="601143700"
videoSessionCoord=[]
for i in range(0,300):
    temp=[0.0,0.0,0.0,0.0]
    videoSessionCoord.append(temp)
drawSeconds=10
xx=0


for filename in filename_list:
    file=open(path+filename,"r")
    temp=filename.split("_")
    if temp[1]!=videoId:
        continue
    print xx
    xx=xx+1
    lines=file.readlines()
    tempsum=0
    for line in lines:
        temp=line.split(",")
        time=float(temp[0])

        if int(time)==drawSeconds:
            videoSessionCoord[int(time)][0]=videoSessionCoord[int(time)][0]+float(temp[1])
            videoSessionCoord[int(time)][1] = videoSessionCoord[int(time)][1] + float(temp[2])
            videoSessionCoord[int(time)][2] = videoSessionCoord[int(time)][2] + float(temp[3])
            tempsum=tempsum+1
    if tempsum==0:
        continue
    videoSessionCoord[drawSeconds][0] = videoSessionCoord[drawSeconds][0] /tempsum
    videoSessionCoord[drawSeconds][1] = videoSessionCoord[drawSeconds][1] /tempsum
    videoSessionCoord[drawSeconds][2] = videoSessionCoord[drawSeconds][2] /tempsum
    X =  videoSessionCoord[drawSeconds][0]
    Y =  videoSessionCoord[drawSeconds][1]
    Z =  videoSessionCoord[drawSeconds][2]
    ax.plot([0, X], [0, Y], [0, Z])
    #清空数组
    videoSessionCoord = []
    for i in range(0, 300):
        temp = [0.0, 0.0, 0.0, 0.0]
        videoSessionCoord.append(temp)
plt.show()
