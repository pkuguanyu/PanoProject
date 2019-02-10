function [result]=calVal(CenterX,CenterY,data)

  [x y z]=size(data)
  sum=0
  for i=1:x
    for j=1:y
       A=[data(i,j,1),data(i,j,2)]
       B=[CenterX,CenterY]   
       sum=(acos(dot(A,B)/(norm(A)*norm(B))))*180/pi+sum;
    end
  end
  sum=sum/x
  result=sum
end