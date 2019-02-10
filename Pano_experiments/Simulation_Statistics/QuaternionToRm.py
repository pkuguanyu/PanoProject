import numpy as np
import quaternion


def QuanternionToRm(w,x,y,z):

   Rm=[[1-2*y*y-2*z*z,2*(x*y-z*w),2*(x*z+y*w)],[2*(x*y+z*w),1-2*x*x-2*z*z,2*(y*z-x*w)],[2*(x*z-y*w),2*(y*z+x*w),1-2*x*x-2*y*y]]
   return Rm


q1=np.quaternion(-0.31,-0.015,0.948,-0.063)
print quaternion.as_rotation_matrix(q1)