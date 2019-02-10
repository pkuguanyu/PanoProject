# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import numpy as np
import quaternion
import csv
import math
import matplotlib.pyplot as plt

def Rx(yaw):
    cosYaw=math.cos(yaw/180*math.pi)
    sinYaw=math.sin(yaw/180*math.pi)
    str1="1,0,0;"+"0,"+str(cosYaw)+","+str(-sinYaw)+";"+"0,"+str(sinYaw)+","+str(cosYaw)
    return np.matrix(str1)
def Ry(pitch):
    cosPitch = math.cos(pitch / 180 * math.pi)
    sinPitch = math.sin(pitch / 180 * math.pi)
    str1=str(cosPitch)+",0,"+str(sinPitch)+";"+"0,1,0;"+str(-sinPitch)+",0,"+str(cosPitch)
    return np.matrix(str1)
def Rz(raw):
    str1="1,0,0;0,1,0;0,0,1"
    return np.matrix(str1)

for i in range(1,2):
    for j in range(1,10):
       file=open("C:/Aiqiyi/expdata/ypr/ypr/video"+str(i)+"."+str(j)+".txt")
       lines=file.readlines()
       ypr=[]
       for line in lines:
           temp=line.split(" ")
           yprtemp=[float(temp[1]),float(temp[2]),float(temp[3])]
           ypr.append(yprtemp)

       xyz=[]
       current_vector=np.matrix("0;0;1")
       for y in ypr:
            yaw=y[0]
            pitch=y[1]
            raw=y[2]
            tempxyz=Rx(yaw)*Ry(pitch)*Rz(raw)*current_vector
            xyz.append(tempxyz)

       k = 0
       for cor in xyz:
           k = k + 1
           print str(k) + " " + str(len(xyz))
           r = 1.0
           if cor[1]  > 0 and cor[0]> 0:
               U = math.atan(cor[1] / cor[0]);
           if cor[1] > 0 and cor[0] < 0:
               U = math.atan(cor[1] / cor[0]) + math.pi;
           if cor[1] < 0 and cor[0]< 0:
               U = math.atan(cor[1] / cor[0]) + math.pi;
           if cor[1]< 0 and  cor[0] > 0:
               U = math.atan(cor[1] / cor[0]) + math.pi * 2;
           U = U / (2 * pi);
           V = math.asin(cor[2] / r) / math.pi + 0.5
           plt.plot(U, V, 'r+')

       plt.savefig("C:/Aiqiyi/expdata/yprgraph/" + str(j) + ".png")
       plt.show()
