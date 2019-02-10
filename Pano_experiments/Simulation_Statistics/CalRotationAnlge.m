function [Angle]=CalRotationAnlge(Data)
   [x y]=size(Data);
   sum=0;
   for i=1:x-1
     A=Data(i,:);
     B=Data(i+1,:);
     tempsum=(acos(dot(A,B)/(norm(A)*norm(B))))*180/pi;
     sum=tempsum+sum;
   end
   Angle=sum;
end