# -*- coding: utf-8 -*-
import os
import sys
from numpy import *
import numpy as np
import matplotlib.pyplot as plt
import copy

def angle(x,y):
    Lx=np.sqrt(x[0]*x[0]+x[1]*x[1]+x[2]*x[2])
    Ly=np.sqrt(y[0]*y[0]+y[1]*y[1]+y[2]*y[2])
    cos_angle=(x[0]*y[0]+x[1]*y[1]+x[2]*y[2])/(Lx*Ly)
    angle=np.arccos(cos_angle)
    angle=angle*360/2/np.pi
    return angle


print angle([0,0,1],[0,0.5,0.865])