function [result]=r2g(img)
  [H,W,Z]=size(img);
  temp=zeros(H,W);
  for i=1:H
      for j=1:W
          temp(i,j)=0.299*img(i,j,1)+0.587*img(i,j,2)+0.114*img(i,j,3);
      end
  end
  result=temp;
end