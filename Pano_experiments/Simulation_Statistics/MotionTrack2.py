# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np

# 追踪轨迹，这个程序用来，设置初始点为0,0,1，将每一个点的位置计算输出，并将其合并在一个文件中，文件命名格式为


# 数据输出格式，timestamp,x,y,z,状态码
# 状态码为 0代表正常， 5代表不从0开始， 2代表 快进， 3代表卡顿，4代表回放,1，6代表异常


def listdir(path, list_name):  # 传入存储的list
    for file in os.listdir(path):
        file_path = os.path.join(path, file)
        temp=file.split("_")
        if len(temp)==5:
            list_name.append(file)



def getCurrentPoint(filename,current_point,current_vector,outputfile): #洗数据在这里面处理
    file = open(filename, 'r')
    lines = file.readlines()
    i=0
    forwardTimeStamp=0
    flag2=0

    for line in lines: #代表整个数据
        temp1 = line.split(']')
        timestamp = 0.0
        motionmatrix = ''
        tempmotion = []
        flag2=0
        for temp2 in temp1: #代表每一个矩阵
            i = i + 1
            motionmatrix = ''
            tempmotion = []
            temp3=temp2.split("[")
            if len(temp3)==2:
                flag2=0
                #洗数据部分----------------------------------------------------------------------------------------------------
                if (len(temp3[0])>=9):
                    flag2=6
                    if flag2 == 6:
                        file.close()
                        return -1
                try:
                    timestamp = float(temp3[0]) / 1000
                except:
                    return -1

                if(i==1):
                    forwardTimeStamp=timestamp
                    tf=filename.split("_")
                    if(timestamp>=3 and tf[4]=="0"): #去掉初始时间不为0的
                        flag2=5
                else:
                    if(forwardTimeStamp>timestamp+0.1):
                        flag2=4
                    if  abs(forwardTimeStamp-timestamp)<=0.05:
                        flag2=3
                    if timestamp-forwardTimeStamp>2:
                        flag2=2

                # 洗数据部分----------------------------------------------------------------------------------------------------
                tempmotion=temp3[1].split(',')
                t = 0
                for num in tempmotion:
                    if(len(num)<9):
                         number = float(num)
                    else:
                         number = num
                    t = t + 1
                    if t % 4 == 0 and t!=16:
                        motionmatrix = motionmatrix + str(number) + ";"
                    else:
                         if t!=16:
                              motionmatrix = motionmatrix + str(number) + ","
                         else:
                              motionmatrix = motionmatrix + str(number)

                forwardTimeStamp=timestamp
            else:
                flag2=1



            if flag2!=1:

               motionmatrix_real=np.mat(motionmatrix).T
               current_vector=np.mat("0;0;1;0")
               current_vector = motionmatrix_real * current_vector
               len2=np.sqrt(current_vector[0,0]*current_vector[0,0]+current_vector[1,0]*current_vector[1,0]+current_vector[2,0]*current_vector[2,0])
               current_vector=current_vector/len2
               outputfile.write(str(float(timestamp))+","+str(current_vector[0, 0]) + "," + str(current_vector[1, 0]) + "," + str(current_vector[2, 0])+","+str(flag2)+"\n")

    file.close()
    return 1


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



path='D:/Aiqiyi/iqiyi3'
outputpath='C:/Aiqiyi/iqiyi_xyz_raw_final'



filename_list = []
listdir(path,filename_list)
filename_list=sorted(filename_list,strcmp) # 进行排序

point_Initial = np.mat('0;0;0;1')
point_List=[]

vector_Initial = np.mat('0;0;1;0')

current_filename=""
current_point=point_Initial
current_vector=vector_Initial
i=0

outputfilename=""
current_videoid = ""
current_deviceid = ""
flag=0# 代表该次会话是否需要处理 0代表需要处理 1代表不需要处理
test = 0
t=0
xxxx=0
for filename in filename_list:
    xxxx=xxxx+1
    print str(xxxx) + " " + str(t) + " " + str(len(filename_list))
    print filename
    # 用来获取当前待处理的videoId
    split = filename.split('_')
    videoid = split[1]
    deviceid =split[2]
    timestamp = split[3]
    if os.path.exists(outputpath + "/VR_" + videoid + "_" + deviceid + "_" + timestamp +"_" ".txt"):
        continue

    i=i+1
    test = 0

    if i==1:
        current_filename=filename
        # 用来获取当前待处理的videoId
        current_split=current_filename.split('_')
        current_videoid=current_split[1]
        current_deviceid=current_split[2]
        current_timestamp=current_split[3]
        outputfile=open(outputpath + "/VR_" + current_videoid + "_" + current_deviceid + "_" + current_timestamp +"_" ".txt","w")
        current_point=point_Initial
        current_vector=vector_Initial
        test=getCurrentPoint(path+"/"+filename,current_point,current_vector,outputfile)

        if(test==-1):
            t=t+1
            #os.remove(outputpath + "/VR_" + current_videoid + "_" + current_deviceid + "_" + current_timestamp +"_" ".txt")
            flag=1
        continue

    temp_split=filename.split('_')
    tempvideo_id=temp_split[1] # 每一次循环中的videoID
    tempdevice_id=temp_split[2]
    temp_timestamp=temp_split[3]

    if current_videoid==tempvideo_id and current_deviceid==tempdevice_id and current_timestamp==temp_timestamp:  #代表遇到相同文件
        if flag==0:
            test=getCurrentPoint(path+"/"+filename,current_point,current_vector,outputfile)
            if(test==-1):
                t = t + 1
                flag=1
            continue
    else: # 代表遇到不同文件了
        point_Initial=np.mat('0;0;0;1')
        vector_Initial = np.mat('0;0;1;0')
        current_point=point_Initial
        current_vector = vector_Initial
        current_filename=filename # 更换当前处理文件

        # 用来获取当前待处理的videoId
        current_split = current_filename.split('_')
        current_videoid = current_split[1]
        current_deviceid = current_split[2]
        current_timestamp = current_split[3]

        flag=0
        outputfile = open(outputpath + "/VR_" + current_videoid + "_" + current_deviceid + "_" + current_timestamp +"_" ".txt", 'w')
        test=getCurrentPoint(path+"/"+filename,current_point,current_vector,outputfile)

        if(test==-1):
            t = t + 1
            #os.remove(outputpath + "/VR_" + current_videoid + "_" + current_deviceid + "_" + current_timestamp +"_" ".txt")
            flag = 1





