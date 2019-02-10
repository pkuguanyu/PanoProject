function [result]=GetRotationSpeed(set,videoId,userId,framegap)%一次性获取帧间隔为framegap的对应videoid userid的旋转速度
 speed=[];
 for j=videoId:videoId %代表视频号
     %每个视频绘制一张图片
    for num=userId:userId  %代表用户
        m=readtable(['F:/expdata/Experiment_',num2str(set),'/',num2str(num),'/video_',num2str(j),'.csv']);
        Map=cell(48,1); %存储每一帧每个用户的X,Y,Z坐标
        quan_list=table2array(m(1:end,3:6));
        time_list=str2double(table2array(m(1:end,2)));
        data_list=str2double(quan_list);
        [x,y]=size(data_list);
        TXYZ=[];
        for i=1:48
            Map{i}=containers.Map();
        end
        %获取时间和对应坐标 time_list和TXYZ
        for i=1:x 
              q2=data_list(i,:);
              tempTimexyz=[2*q2(1)*q2(3)+2*q2(2)*q2(4),2*q2(2)*q2(3)-2*q2(1)*q2(4),1-2*q2(1)*q2(1)-2*q2(2)*q2(2)];
              TXYZ=[TXYZ;tempTimexyz];
        end
        %初始化数据存储矩阵
     
        for i=1:x %将数据转化为每一帧的坐标，存在在map中
           frame=floor(time_list(i,1)/0.033);
           Map{userId}(num2str(frame))=[TXYZ(i,1),TXYZ(i,2),TXYZ(i,3)];
        end
        
        for i=1:framegap:floor(time_list(x,1)/0.033) %以framegap为间隔
            Start=i;
            End=i+framegap;
            dataToBeHandled=[];
            for k=Start:End
                if Map{num}.isKey(num2str(k)) 
                    dataToBeHandled=[dataToBeHandled;Map{num}(num2str(k))];
                end
            end
            Rotationspeed=abs(CalRotationAngle(dataToBeHandled)*1.0/(framegap*1.0/30));
            speed=[speed;Rotationspeed]; 
        end
        %将Map数据以framegap加入到dataToBeHandled中，计算对应的旋转速度
    end

 end
    result=speed;

end

 
 