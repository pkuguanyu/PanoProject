function [result]=GetPictureByViewpoint(im,Center) %Center是相对原图的坐标，而不是viewport内的坐标
Center=floor(Center);
[HH,WW,ZZ]=size(im);
H=630;
W=840;
temp=zeros(H,W);
for i=Center(1)-314:Center(1)+315
    for j=Center(2)-419:Center(2)+420
         X=i;
         Y=j;
         if i<=0
            X=HH+i;
         end 
         if j<=0
            Y=WW+j;
         end 
         if i>HH
             X=(i-HH);
         end
         if j>WW
             Y=(j-WW);
         end
         idx=floor(i-(Center(1)-314));
         idy=floor(j-(Center(2)-419));
         if idx+1<=0 ||idx+1>H || idy+1<=0 ||idy+1>W ||X<=0||X>HH||Y<=0||Y>WW
             continue
         end
         temp(idx+1,idy+1)=im(X,Y);
    end
end
result=temp;
end
