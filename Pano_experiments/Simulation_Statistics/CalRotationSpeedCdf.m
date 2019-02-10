
 
 %
 for j=0:8 %代表视频号
     figure
     CdfRotationSpeed=[];
     %每个视频绘制一张图片
    for num=1:48   %代表用户
        m=readtable(['C:/Aiqiyi/expdata/Experiment_2/',num2str(num),'/video_',num2str(j),'.csv']);
        quan_list=table2array(m(1:end,3:6));
        data_list=[quan_list];
        [x,y]=size(data_list);
        TXYZ=[];
        %获取时间和对应坐标
        for i=1:x
              q2=data_list(i,:);
              tempTimexyz=[2*q2(1)*q2(3)+2*q2(2)*q2(4),2*q2(2)*q2(3)-2*q2(1)*q2(4),1-2*q2(1)*q2(1)-2*q2(2)*q2(2)];
              TXYZ=[TXYZ;tempTimexyz];
        end
        %初始化数据存储矩阵
        dataToBeHandled=[];
        %每隔1秒钟处理一次,求出本次观看的所有视频切片的
        for i=1:x
            if mod(i,91)~=0
                 dataToBeHandled=[dataToBeHandled;TXYZ(i,1:3)];
            else
                 RotationAngle=CalRotationAnlge(dataToBeHandled);
                 dataToBeHandled=[];
                 CdfRotationSpeed=[CdfRotationSpeed,RotationAngle];
            end
        end    
    end
    CdfRotationSpeed=sort(CdfRotationSpeed);
    MaxSpeed=max(CdfRotationSpeed);
    X=[];
    Y=[];
    for speed =0:200
        X=[X,speed];
        Y=[Y,sum(CdfRotationSpeed<=speed)*1.0/sum(CdfRotationSpeed>=0)];
    end
    plot(X,Y,'r-')
    title(['CDF of rotation speed per video (videoid:',num2str(j),')'])
    xlabel('Rotation speed(degree/s)')
    ylabel('Fractions of video clips(%)')
    saveas(gcf,['C:/Aiqiyi/expdata/cdfgraph/',num2str(j),'.png'])
 end


 
 