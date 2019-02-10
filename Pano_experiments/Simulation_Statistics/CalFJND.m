function [result]=CalFJND(imgraw)
[H W Z]=size(imgraw);

img=zeros(H,W); %转化为亮度图
Fscore=zeros(H,W);

xf=[ceil(H/2),ceil(W/2)]; %假设用户看的是屏幕的中心
for i=1:H %求彩色图片对应的亮度图
    for j=1:W
        img(i,j)=0.3*imgraw(i,j,1)+0.6*imgraw(i,j,2)+0.1*imgraw(i,j,3);
    end
end

xx=1:W;
yy=1:H;
for i=1:H
    for j=1:W
        x=[i,j];
        d=sqrt((x(1)-xf(1))^2+(x(2)-xf(2))^2);
        N=840;
        v=0.5;
        e=(atan(d/(N*v))/pi)*180;
        Fscore(i,j)=FJND(img,i,j,v,e);
    end
end

result=Fscore;
% 
% Gap_X=240;
% Gap_Y=240;
% BitRate=zeros(4,6);
% BitRateSum=zeros(4,6);
% 
% %求每一个tile的平均JND值
% for i=1:H
%     for j=1:W
%         BitRate(ceil(i/Gap_X),ceil(j/Gap_Y))=BitRate(ceil(i/Gap_X),ceil(j/Gap_Y))+Fscore(i,j);
%         BitRateSum(ceil(i/Gap_X),ceil(j/Gap_Y))=BitRateSum(ceil(i/Gap_X),ceil(j/Gap_Y))+1;
%     end
% end
% 
% for i=1:4
%     for j=1:6
%          BitRate(i,j)= BitRate(i,j)/BitRateSum(i,j);
%     end
% end


% M=min(BitRate(:));
% 
% %%将JND转换为码率
% for i=1:5
%     for j=1:6
%          BitRate(i,j)= (M/BitRate(i,j))*100; %每一个块能够感受到的码率阈值
%     end
% end
% 
% Downscale=[70,70,70,70,70,70;
%            70,85,85,85,85,70;
%            70,85,100,100,75,70;
%            70,85,85,85,85,70;
%            70,70,70,70,70,70]
% %求我们想比一圈一圈降低的有多大提升
% 
% sum(Downscale(:))
% for i=1:5
%     for j=1:6
%        if Downscale(i,j)>BitRate(i,j)
%           Downscale(i,j)=BitRate(i,j);
%        end
%     end
% end
% 
% sum(Downscale(:))
%            
% AllSameImproment=(3000-sum(BitRate(:)))/3000  %和全传一样的去比
% DownscaleImproment=0                                           %和一圈一圈的去比
% [X,Y]=meshgrid(1:6,1:5);
% shading interp;
% surf(X,Y,BitRate);
% shading interp;