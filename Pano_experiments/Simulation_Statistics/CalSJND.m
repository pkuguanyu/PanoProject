 function [result]=CalSJND(imgraw)
[H W Z]=size(imgraw);

img=zeros(H,W); %转化为亮度图
Fscore=zeros(H,W);

for i=1:H %求彩色图片对应的亮度图
    for j=1:W
        img(i,j)=0.3*imgraw(i,j,1)+0.6*imgraw(i,j,2)+0.1*imgraw(i,j,3);
    end
end

for i=1:H
    for j=1:W
        Fscore(i,j)=SJND(img,i,j);
    end
end

result=Fscore;
