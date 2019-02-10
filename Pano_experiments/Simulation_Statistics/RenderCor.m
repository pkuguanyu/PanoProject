

%本程序用来生成对应帧对应得视点坐标
for videoId=0:0
    for j=1:1
        fid = fopen(['F:\RenderCor\set1\',num2str(j),'_',num2str(videoId),'.txt'],'a');
        m=readtable(['C:/Aiqiyi/expdata/Experiment_1/',num2str(j),'/video_',num2str(videoId),'.csv']);
        quan_list=m(1:end,3:6);
        time_list=m(1:end,2);
        [x,y]=size(quan_list);
        data=[];
        data_3=[];
        for i=1:x
            q1=quan_list(i,:); %table类型的四维数坐标数据
            time2=time_list(i,:);%table类型的时间数据
            q2=table2array(q1); %matrix类型的四维数坐标数据
            time1=table2array(time2);%matrix类型的时间数据
            frame=floor(str2double(time1{1})/0.033);        
            tempxyz=[2*str2double(q2{1})*str2double(q2{3})+2*str2double(q2{2})*str2double(q2{4});2*str2double(q2{2})*str2double(q2{3})-2*str2double(q2{1})*str2double(q2{4});1-2*str2double(q2{1})*str2double(q2{1})-2*str2double(q2{2})*str2double(q2{2})];
%             frame
%             tempxyz
%             if abs(tempxyz(1))>=1
%                 if tempxyz(1)>0
%                     tempxyz(1)=floor(tempxyz(1));
%                 else
%                     tempxyz(1)=ceil(tempxyz(1));
%                 end
%             end
%             
%             if abs(tempxyz(2))>=1
%                 if tempxyz(2)>0
%                     tempxyz(2)=floor(tempxyz(2));
%                 else
%                     tempxyz(2)=ceil(tempxyz(2));
%                 end
%             end
%             
%             if abs(tempxyz(3))>=1
%                 if tempxyz(3)>0
%                     tempxyz(3)=floor(tempxyz(3));
%                 else
%                     tempxyz(3)=ceil(tempxyz(3));
%                 end
%             end
            A=[0,1,0];
            B=tempxyz;
            angle_Y=acos(dot(A,B)/(norm(A)*norm(B)))*180.0/pi;
            A=[1,0,0];
            B=tempxyz;
            if B(3)>0
                angle_X=acos(dot(A,B)/(norm(A)*norm(B)))*180.0/pi;
            else
                angle_X=360-acos(dot(A,B)/(norm(A)*norm(B)))*180.0/pi;
            end
            U=angle_X/360.0;
            V=angle_Y/180.0;
            data=[data;frame,U,V];
            data_3=[data_3;frame,tempxyz'];
            %fprintf(fid,'%d,%f,%f\r\n ',frame,U,V);
        end
        fclose(fid);
    end
end
