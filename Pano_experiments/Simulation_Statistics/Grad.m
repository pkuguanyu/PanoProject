function [result]=Grad(img,G,x,y)
[H,W]=size(img);  
sum=0;
for i=1:5
    for j=1:5
       if x-3+i<1 || y-3+j<1 ||x-3+i>H||y-3+j>W
           continue
       end
       sum=sum+img(x-3+i,y-3+j)*G(i,j);
    end
end
sum=sum/16;
result=sum;


end