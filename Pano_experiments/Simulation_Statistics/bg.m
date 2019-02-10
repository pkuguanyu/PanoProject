function [result]=bg(img,x,y)
[H,W]=size(img);
x=ceil(x);
y=ceil(y);
B=[1,1,1,1,1;
   1,2,2,2,1;
   1,2,0,2,1;
   1,2,2,2,1;
   1,1,1,1,1];

sum=0;
for i=1:5
    for j=1:5
        if x-3+i<1 || y-3+j<1 ||x-3+i>H||y-3+j>W
           continue
        end
        sum=sum+img(x-3+i,y-3+j)*B(i,j);
    end
end
sum=sum/32;
result=sum;


end


