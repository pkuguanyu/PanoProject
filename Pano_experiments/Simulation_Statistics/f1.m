function [result]=f1(img,x,y)
[H,W]=size(img);
x=ceil(x);
y=ceil(y);
lambda=0.5;
alpha=bg(img,x,y)*0.0001+0.115;
beta=lambda-bg(img,x,y)*0.01;
result=mg(img,x,y)*alpha+beta;
end