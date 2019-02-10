
clear all;
for i=22:5:42
    imgraw=imread(['F:\视频评价调研\Potential improment\frame\frame',num2str(i),'.bmp']);
    [H W Z]=size(imgraw);
    imgraw=imgraw(ceil(H/2)-479:ceil(H/2)+480,ceil(W/2)-719:ceil(W/2)+720,:);
    [obj_height,obj_width,Z]=size(imgraw);
    x=0:(obj_width/6):obj_width;
    y=0:(obj_height/4):obj_height;
    M = meshgrid(x,y); %产生网格
    N = meshgrid(y,x);  %产生网格
    figure
    imshow(imgraw)
    hold on
    plot(x,N); %画出水平横线
    plot(M,y); %画出垂直竖线
    imwrite(imgraw,['F:\视频评价调研\Potential improment\frame\960frame',num2str(i),'.bmp'])
end