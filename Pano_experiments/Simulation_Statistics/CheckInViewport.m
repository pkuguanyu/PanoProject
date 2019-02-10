function [result]=CheckInViewport(Center,x,y,gap_Height,gap_Width) %判断视点在Center  ,x,ytile是否完全咋tile内
H=1440;
W=2880;
img=zeros(H,W);

for j=Center(1)-314:Center(1)+315
    for k=Center(2)-419:Center(2)+420
        X=j;
        Y=k;
        if j<=0
            X=H+j;
        end
        if k<=0
            Y=W+k;
        end
        if j>H
            X=(j-H);
        end
        if k>W
            Y=(k-W);
        end
        img(X,Y)=1;
    end
end
temp=find(img((x-1)*gap_Height+1:x*gap_Height,(y-1)*gap_Width+1:y*gap_Width)==0);
[row col]=size(temp);
if  row==0
    result=1;
else
    result=0;
end

end