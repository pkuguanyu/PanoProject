
clear all;

XX_Quan=[];
YY_Quan=[];

XX_6_12=[];
YY_6_12=[];

XX_Ada_12_24=[];
YY_Ada_12_24=[];

userid=2;

videoid=0;
disk='I:'



frameData=load('Tiling\data_frame.txt');  %读取待处理的一些帧
[size_x,size_y]=size(frameData);
frame=1621;
X_Quan=cell2mat(struct2cell(load(['I:\视频评价调研\Potential improment\第五版Potential improvement\QuanHigh\',num2str(videoid),'\',num2str(userid),'\XXX_',num2str(frame),'.mat'])));
min_X=min(X_Quan(:));
max_X=max(X_Quan(:));

for cont=1:size_x
    frame=frameData(cont,1);
    disk='I:';
    
    idx=(frame-1)/30;
    
    X_Quan=cell2mat(struct2cell(load(['I:\视频评价调研\Potential improment\第五版Potential improvement\QuanHigh\',num2str(videoid),'\',num2str(userid),'\XXX_',num2str(frame),'.mat'])));
    Y_Quan=cell2mat(struct2cell(load(['I:\视频评价调研\Potential improment\第五版Potential improvement\QuanHigh\',num2str(videoid),'\',num2str(userid),'\YYY_',num2str(frame),'.mat'])));
    X_Quan=mapminmax(X_Quan,min_X,max_X);
    XX_Quan=[XX_Quan,X_Quan];
    YY_Quan=[YY_Quan,Y_Quan];
    
    
    X_6_12=cell2mat(struct2cell(load([disk,'\视频评价调研\Potential improment\第五版Potential improvement\VRJND\',num2str(videoid),'\',num2str(6),'_',num2str(12),'\',num2str(userid),'\','\XXX_',num2str(frame),'.mat'])));
    Y_6_12=cell2mat(struct2cell(load([disk,'\视频评价调研\Potential improment\第五版Potential improvement\VRJND\',num2str(videoid),'\',num2str(6),'_',num2str(12),'\',num2str(userid),'\','\YYY_',num2str(frame),'.mat'])));
    X_6_12=X_6_12./100;
    X_6_12=mapminmax(X_6_12,min_X,max_X);
    XX_6_12=[XX_6_12,X_Quan];
    YY_6_12=[YY_6_12,Y_Quan];
    
    
    X_Ada_12_24=cell2mat(struct2cell(load([disk,'\视频评价调研\Potential improment\第五版Potential improvement\VRJND\Ada\',num2str(videoid),'\',num2str(12),'_',num2str(24),'\',num2str(userid),'\XXX_',num2str(frame),'.mat'])));
    Y_Ada_12_24=cell2mat(struct2cell(load([disk,'\视频评价调研\Potential improment\第五版Potential improvement\VRJND\Ada\',num2str(videoid),'\',num2str(12),'_',num2str(24),'\',num2str(userid),'\YYY_',num2str(frame),'.mat'])));
    X_Ada_12_24=X_Ada_12_24./100;
    X_Ada_12_24=mapminmax(X_Ada_12_24,min_X,max_X);
    XX_Ada_12_24=[XX_Ada_12_24,X_Quan];
    YY_Ada_12_24=[YY_Ada_12_24,Y_Quan];
     
end

Final_X_Quan=[];
Final_Y_Quan=[];

Final_X_6_12=[];
Final_Y_6_12=[];

Final_X_Ada_12_24=[];
Final_Y_Ada_12_24=[];

for i=min_X:10:max_X
    [size_x,size_y]=size(XX_Quan);
    temp_Quan_Sum=0;
    temp_Quan_Count=0;
    for j=1:size_y
        if XX_Quan(j)>=i &&XX_Quan(j)<i+10
            temp_Quan_Sum=temp_Quan_Sum+YY_Quan(j);
            temp_Quan_Count=temp_Quan_Count+1;
        end
    end
    Final_X_Quan=[Final_X_Quan,i];
    Final_Y_Quan=[Final_Y_Quan, temp_Quan_Sum/temp_Quan_Count];
    
    
    
    [size_x,size_y]=size(XX_6_12);
    temp_6_12_Sum=0;
    temp_6_12_Count=0;
    for j=1:size_y
        if XX_6_12(j)>=i &&XX_6_12(j)<i+10
            temp_6_12_Sum=temp_6_12_Sum+YY_6_12(j);
            temp_6_12_Count=temp_6_12_Count+1;
        end
    end
    
    Final_X_6_12=[Final_X_6_12,i];
    Final_Y_6_12=[Final_Y_6_12, temp_6_12_Sum/temp_6_12_Count];
    
    [size_x,size_y]=size(XX_Ada_12_24);  
    temp_Ada_12_24_Sum=0;
    temp_Ada_12_24_Count=0;
    for j=1:size_y
         if XX_Ada_12_24(j)>=i &&XX_Ada_12_24(j)<i+10
            temp_Ada_12_24_Sum=temp_Ada_12_24_Sum+YY_Ada_12_24(j);
            temp_Ada_12_24_Count=temp_Ada_12_24_Count+1;
        end
    end
     Final_X_Ada_12_24=[Final_X_Ada_12_24,i];
     Final_Y_Ada_12_24=[Final_Y_Ada_12_24, temp_Ada_12_24_Sum/temp_Ada_12_24_Count];
end

figure,
hold on
plot(X_Quan,Y_Quan,'bo-','LineWidth',1.2);
hold on

plot(X_6_12,Y_6_12,'bo-','LineWidth',1.2);
hold on

plot(X_Ada_12_24,Y_Ada_12_24,'kp-','LineWidth',1.2);
hold on


xlabel('BandWidth (kbps)');
ylabel('PSPNR')
% xlim([0,max(X_Quan(:))])
% ylim([0,210]);
set(gca,'FontSize',12);
set(gca,'ytick',0:30:210);
set(gca,'yticklabel',{'0','30', '60','90' ,'120' ,'150', '...','Inf',' '});


