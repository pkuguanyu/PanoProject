

%  timegap=5  %代表每一次统计的时间区间为1秒
%  option=2 %2代表UV坐标 3代表XYZ坐标
%  %统计每个视频的用户视点方差
% realviewPoint=[]
% videoTotalTime=-1;
%  for j=8:8  %代表视频ID
% 
%     quan_list=[]; %存储四元数
%     time_list=[]; %存储四元数 对应播放时间
%     for num=1:48 %每一个用户
%         m=readtable(['C:/Aiqiyi/expdata/Experiment_1/',num2str(num),'/video_',num2str(j),'.csv']);
%         quan_list=m(1:end,3:6);
%         time_list=table2array(m(1:end,2));
%         tempVideoTotalTime=max(time_list(:));
%         %求视频长度
%         if tempVideoTotalTime>videoTotalTime
%              videoTotalTime=tempVideoTotalTime;
%         end
%     end
%     videoTotalTime=ceil(videoTotalTime); %计算视频长度
%     viewPoint=zeros(48,ceil(videoTotalTime*1.0/timegap)+1,option);  %存储每个时间间隔内的平均中心视点
%  
%     %对用户视点做统计
%     for num=1:48 %每一个用户
%         num
%         m=readtable(['C:/Aiqiyi/expdata/Experiment_1/',num2str(num),'/video_',num2str(j),'.csv']);
%         quan_list=m(1:end,3:6);
%         time_list=table2array(m(1:end,2));
%         [x,y]=size(quan_list);
%         tempviewPoint=zeros(ceil(videoTotalTime*1.0/timegap)+1,option);
%         lenviewPoint=zeros(ceil(videoTotalTime*1.0/timegap)+1,1);     
%         %求该用户的timegap间隔的视点中心
%         
%         for i=1:x %每一行数据
%               q1=quan_list(i,:);
%               q2=table2array(q1); %每一个代表一行的数据
%               time=ceil(time_list(i,1)/timegap)+1;
%               
%               tempxyz=[2*q2(1)*q2(3)+2*q2(2)*q2(4);2*q2(2)*q2(3)-2*q2(1)*q2(4);1-2*q2(1)*q2(1)-2*q2(2)*q2(2)];
%               
%               if abs(tempxyz(1))>=1
%                   if tempxyz(1)>0
%                       tempxyz(1)=floor(tempxyz(1));
%                   else
%                       tempxyz(1)=ceil(tempxyz(1));
%                   end
%               end
%               
%               if abs(tempxyz(2))>=1
%                   if tempxyz(2)>0
%                       tempxyz(2)=floor(tempxyz(2));
%                   else
%                       tempxyz(2)=ceil(tempxyz(2));
%                   end
%               end
%               
%               if abs(tempxyz(3))>=1
%                   if tempxyz(3)>0
%                       tempxyz(3)=floor(tempxyz(3));
%                   else
%                       tempxyz(3)=ceil(tempxyz(3));
%                   end
%               end
%                
%               r=1.0;
%               if tempxyz(3)>0 & tempxyz(1)>0
%                  U=atan(tempxyz(3) / tempxyz(1));
%               end
%               if tempxyz(3)>0 & tempxyz(1)<0
%                   U=atan(tempxyz(3) / tempxyz(1)) +pi;
%               end
%               if tempxyz(3)<0 & tempxyz(1)<0
%                   U=atan(tempxyz(3) / tempxyz(1))+pi;
%               end
%               if tempxyz(3)<0 & tempxyz(1)>0
%                   U=atan(tempxyz(3) / tempxyz(1))+pi*2;
%               end
%             
%               U=U/(2*pi);
%               V=acos(tempxyz(2) / r) / pi;
%               
%               lenviewPoint(time,1)=lenviewPoint(time,1)+1;
%               tempviewPoint(time,1)=tempviewPoint(time,1)+U;
%               tempviewPoint(time,2)=tempviewPoint(time,2)+V;
%         end
%         
%         for i=1:ceil(videoTotalTime*1.0/timegap)+1
%             if lenviewPoint(i,1)==0
%                 continue
%             end
%             tempviewPoint(i,1)= tempviewPoint(i,1)*1.0/lenviewPoint(i,1);
%             tempviewPoint(i,2)= tempviewPoint(i,2)*1.0/lenviewPoint(i,1);
%             viewPoint(num,i,1)= tempviewPoint(i,1);
%             viewPoint(num,i,2)= tempviewPoint(i,2);
%         end
%     end
%     
%     savePath =['C:\Aiqiyi\expdata\clusterData\',num2str(j),'.mat'];
%     save(savePath,'viewPoint'); 
% end 


j=1
realviewPoint=cell2mat(struct2cell(load(['C:\Aiqiyi\expdata\clusterData\',num2str(j),'.mat'])))
%     调用Kmeans函数
%     X N*P的数据矩阵
%     Idx N*1的向量,存储的是每个点的聚类标号
%     Ctrs K*P的矩阵,存储的是K个聚类质心位置
%     SumD 1*K的和向量,存储的是类间所有点与该类质心点距离之和
%     D N*K的矩阵，存储的是每个点与所有质心的距离;
    

 [xxx yyy zzz]=size(realviewPoint)
  %统计一个指标，每一个5秒，异于其他用户的视点位置的次数
  AbnormalTimes=zeros(48,1);
for j=1:yyy
    X=realviewPoint(:,j,:);
    X=reshape(X,48,2)
    AverageDistance=[];
    flag=0;
    bestClusters=1;
    %确定最佳聚类数量
    for Types=1:48
        if flag==1
            break
        end
        opts = statset('Display','final');
        [Idx,Ctrs,SumD,D] = kmeans(X,Types,'Options' ,opts,'Replicates',100);
        for i=1:Types
            SumD(i,1)=SumD(i,1)/sum(Idx==i);
        end
        AverageDistance=[AverageDistance,mean(SumD)];
        if Types>=3 
            Diff=abs(diff(AverageDistance));
             %求差分导数
             for i=1:Types-1
                 Diff(i)=Diff(i)/AverageDistance(1,i);
             end

       %求最佳聚簇数
             for i=1:Types-2
                  if(i==1 & Diff(i)<0.2)
                     flag=1;
                     bestClusters=1;
                     break
                  end
                  if Diff(i+1)<Diff(i)*0.7 
                     flag=1;
                     bestClusters=i+1;
                     break
                  end
            end
       
         end
    end
    
    opts = statset('Display','final');
    [Idx,Ctrs,SumD,D] = kmeans(X,bestClusters,'Options' ,opts,'Replicates',100);
%     figure
%     hold on
    MaxNum=-1;
    MaxTag=1;
    
    for c =1:bestClusters
        if sum(Idx==c)>=MaxNum
            MaxNum=sum(Idx==c);
            MaxTag=c;
        end
    end
    
    for c=1:48
        if Idx(c,1)~=MaxTag
             AbnormalTimes(c,1)=AbnormalTimes(c,1)+1;
        end
    end
    
%     for c =1:bestClusters
%         plot(X(Idx==c,1),X(Idx==c,2),'o','color',[0,c*1.0/bestClusters,0],'MarkerSize',6)
%         hold on
%     end
%     saveas(gcf,['C:\Aiqiyi\expdata\clusterGraph\',num2str(j),'_timegap.png'])
end
['最奇葩用户:']
find(AbnormalTimes==max(AbnormalTimes))
max(AbnormalTimes)
