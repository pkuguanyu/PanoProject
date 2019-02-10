# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np

def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        file_path = os.path.join(path, file)
        list_name.append(file)

path="C:/Aiqiyi/new_iqiyi_final/iqiyi_xyz/"
outputpath="C:/Aiqiyi/DirectionResult5/"
filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list) # 进行排序

threshold=60
threshold2=10
timegap=5




for filename in filename_list:
     inputfile=open(path+filename,"r")
     lines=inputfile.readlines()
     if (len(lines) == 1 or len(lines) == 0):
         continue

     vector1=[]
     vector2=[]
     i=0
     timegap=5.0
     timegap1=0.0
     timegap2 = timegap1+timegap
     timestamp1=0.0
     changeTimes = 0
     outputfile = open(outputpath+filename,"w")
     forward=0 #记录前一个
     print filename

     while(i<len(lines)):

         temp=lines[i].split(",")
         timestamp=float(temp[0])


         if(i==0):
                 forward=timestamp
                 timegap1=timestamp
                 timegap2=timegap1+timegap
         x=float(temp[1])
         y=float(temp[2])
         z=float(temp[3])

         if (forward>timestamp+0.01):
             break

         if(timestamp<timegap2 and timestamp>timegap1) or abs(timestamp-timegap2)<=0.1 or abs(timestamp-timegap1)<=0.1:
             if (i == 0):
                 array1 = np.array([x,y,z])
                 i=i+1
                 continue
             if (i == 1):
                 array2 = np.array([x,y,z])
                 i=i+1
                 continue
             array3 = np.array([x,y,z])
             vector1 = np.cross(array1, array2)
             vector2 = np.cross(array2, array3)
             Lx=np.sqrt(vector1.dot(vector1))
             Ly=np.sqrt(vector2.dot(vector2))
             angle = vector1.dot(vector2)/(Lx*Ly)
             angle=np.arccos(angle)
             angle=angle*360/2/np.pi

             angle2 = array2.dot(array3) / ((np.sqrt(array2.dot(array2))) * (np.sqrt(array3.dot(array3))))
             angle2=np.arccos(angle2)
             angle2=angle2*360/2/np.pi

             if (angle >= threshold and angle2>=threshold2):
                 changeTimes=changeTimes+1

             array1 = array2
             array2 = array3
             forward=timestamp
             i = i + 1


         else:
             tstd=sys.stdout
             sys.stdout=outputfile
             print str(timegap1)+" "+str(timegap2)+" "+str(changeTimes)
             sys.stdout=tstd
             timegap1=timegap1+timegap
             timegap2=timegap2+timegap
             changeTimes = 0






