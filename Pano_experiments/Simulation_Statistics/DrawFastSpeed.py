# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import  numpy as np
import matplotlib.pyplot as plt

outputpath="D:/2018.4.1/correlationResult/"
for m in range(5,105,10):
    outputfile = open(outputpath + "videosession_highmotion_" + str(m) + "_.txt", "r")
    lines = outputfile.readlines()
    part = []
    for line in lines:
        temp = line.split(" ")
        temppart = float(temp[0]) / float(temp[1])
        part.append(temppart)
    part.sort()
    x = []
    y = []
    for i in range(1, 100, 10):
        x.append(i)
        sum = 0
        for p in part:
            if p < i * 1.0 / 100:
                sum = sum + 1
        y.append(sum * 1.0 / len(part))
    plt.plot(x, y)
    plt.title("The cdf graph of high-speed rotation clips")
    plt.xlabel("High-speed("+str(m)+ " degree per second) rotation clips in a single video(%)")
    plt.ylabel("Fraction of videos(%)")
    plt.savefig(outputpath+ "videosession_highmotion_" + str(m) + "_.png")
    plt.show()