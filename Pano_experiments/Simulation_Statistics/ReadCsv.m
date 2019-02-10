%m = csvread('C:/Aiqiyi/expdata/Experiment_1/1/video_0.csv',1,0);
m=readtable('C:/Aiqiyi/expdata/Experiment_1/1/video_1.csv');
quan_list=m(1:end,3:6);
[x,y]=size(quan_list)

for i=1:x
   current_vector=[0;0;1];
   q1=quan_list(i,:);
   q2=table2array(q1);
   q3=[q2(4),q2(1),q2(2),q2(3)];
   q4=quatnormalize(q3);
   R1=quat2dcm(q4);
   tempxyz=R1*current_vector;
   r=1.0;
   if tempxyz(2)>0 & tempxyz(1)>0
      U=atan(tempxyz(2) / tempxyz(1));
   end
   if tempxyz(2)>0 & tempxyz(1)<0
      U=atan(tempxyz(2) / tempxyz(1)) +pi;
   end
   if tempxyz(2)<0 & tempxyz(1)<0
      U=atan(tempxyz(2) / tempxyz(1))+pi;
   end
   if tempxyz(2)<0 & tempxyz(1)>0
      U=atan(tempxyz(2) / tempxyz(1))+pi*2;
   end
   U=U/(2*pi);
   V=asin(tempxyz(3) / r) / pi + 0.5;
   hold on
   plot(U, V, 'r+')
   hold on
end



