
%本函数通过在编码的时候不考虑视点因素，而在用户实际选择的时候使用系数进行码率选择


%对于一个240*240的视频块，我们认为失真量超过64的块是不能进行码率降低的
%1、现在需要做的拟合，横轴是超过JND的像素点的数量，纵轴是码率
%2、对于视频中的每个tile分别计算，有如下步骤
%3、计算该tile当超过JND的像素点数量恰巧为64时对应的码率，即横轴是64的对应的纵轴，即为我们应该调整的码率

clear all;
path(path,'I:\视频评价调研\Potential improment\');
%------------------------------------------------
videoid=0;
userid=1;
framegap=30;
frameStart=121;
frameEnd=391;
mm=12;
nn=24;
cont=1;%计数用，记录当前是第几个X帧
disk='I:'
%------------------------------------------------

%分别计算各个QP计算1501帧的累计带宽
%分别计算各个QP计算1501帧的累计PSPNR
Luminance=[];
%RotationSpeed=GetRotationSpeed(videoid,userid,framegap); %每一行代表第X个10帧内的旋转速度

RotationSpeed=cell(48,1);
for userid=2:5
      RotationSpeed{userid}=GetRotationSpeed(videoid,userid,framegap); %每一行代表第X个10帧内的旋转速度
end

for frame=frameStart:framegap:frameEnd
 
    idx=floor(frame/30); % 该帧对应的秒数
    for userid=2:5
        viewPoint=cell2mat(struct2cell(load([disk,'\视频评价调研\Potential improment\frame\',num2str(videoid),'_Viewpoint\',num2str(frame),'.mat'])));       
        XXX=[];
        YYY=[];
        disk='I:';
        frame
        H=1440;
        W=2880;
        Gap_Height=round(1440/mm);
        Gap_Width=round(2880/nn);
        R= cell2mat(struct2cell(load([disk,'\视频评价调研\Potential improment\JND_Matrix\',num2str(videoid),'\',num2str(frame),'_R.mat'])));
        Center=floor([viewPoint(userid,2),viewPoint(userid,1)]);
        Fresult_all=ones(H,W)*10;
        xf=Center;
        for j=Center(1)-314:Center(1)+315
            for k=Center(2)-419:Center(2)+420
                x=[j,k];
                d=sqrt((x(1)-xf(1))^2+(x(2)-xf(2))^2);
                N=840;
                v=0.43;
                e=(atan(d/(N*v))/pi)*180;
                Fresult_temp=0.06*e+1;
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
                Fresult_all(X,Y)= Fresult_temp;
            end
        end
        
        F_all=zeros(mm,nn);
        for i=1:mm
            for j=1:nn
                F_all(i,j)=mean(mean(Fresult_all((i-1)*Gap_Height+1:i*Gap_Height,(j-1)*Gap_Width+1:j*Gap_Width)));
            end
        end
        
        
        if exist([disk,'\视频评价调研\Potential improment\depth\',num2str(videoid),'\',num2str(frame),'.txt'],'file')
            depth=load([disk,'\视频评价调研\Potential improment\depth\',num2str(videoid),'\',num2str(frame),'.txt']);
        else
            depth=double(1.0);
        end
        LuminanceJND=1;
        
        
        
        DepthJND_Matrix=zeros(H,W);
        [sizex,sizey]=size(depth);
        for i=1:mm
            for j=1:nn
                if  sizex==1
                    DepthJND_Matrix((i-1)*Gap_Height+1:i*Gap_Height,(j-1)*Gap_Width+1:j*Gap_Width)=1;
                else
                    DepthJND_Matrix((i-1)*Gap_Height+1:i*Gap_Height,(j-1)*Gap_Width+1:j*Gap_Width)=DepthToJND(depth(ceil(i/2),ceil(j/2)));
                end
            end
        end
        
        %计算旋转速度JND系数
        SpeedJND=SpeedToJND(RotationSpeed{userid}(idx+1));
        R=R.*DepthJND_Matrix.*Fresult_all.*SpeedJND*1;
        
        check=zeros(mm,nn);
        for i=1:mm   
            for j=1:nn       
                temp=Fresult_all((i-1)*Gap_Height+1:i*Gap_Height,(j-1)*Gap_Width+1:j*Gap_Width);
                row=sum(sum(temp<9));  %判断一下当前是不是完全在viewport内部的
                if round(row)>0
                    check(i,j)=1;
                end
            end
        end
        
        
        R_all=zeros(mm,nn);
        for i=1:mm
            for j=1:nn
                R_all(i,j)=mean(mean(R((i-1)*Gap_Height+1:i*Gap_Height,(j-1)*Gap_Width+1:j*Gap_Width)));
            end
        end
        
        mkdir(['R_txt\',num2str(videoid),'\',num2str(userid)]);
        fid = fopen(['R_txt\',num2str(videoid),'\',num2str(userid),'\',num2str(frame),'_',num2str(mm),'_',num2str(nn),'_R.txt'],'w');
        
        for i=1:mm
            for j=1:nn
                fprintf(fid,'%f ',R_all(i,j));
            end
            fprintf(fid,'\n');
        end
        fprintf(fid,'\n\n');
        %-----------------------------------------------------------------------------------------MSE版本
        %     MSE_All_static=zeros(mm,nn,42);  %用于拟合的MSE矩阵
        %
        %
        %
        %
        %     for i=1:mm
        %         i
        %         for j=1:nn
        %             j
        %             for  qp=22:5:42
        %                 qp
        %                 MSE_All_static(i,j,qp)=CalPMSEPerTile(Center,img_raw((i-1)*Gap_Height+1:i*Gap_Height,(j-1)*Gap_Width+1:j*Gap_Width),imgQp{qp}((i-1)*Gap_Height+1:i*Gap_Height,(j-1)*Gap_Width+1:j*Gap_Width),R((i-1)*Gap_Height+1:i*Gap_Height,(j-1)*Gap_Width+1:j*Gap_Width));
        %             end
        %
        %         end
        %     end
        %
        %     mkdir(['R_txt\',num2str(mm),'_',num2str(nn)])
        %     fid = fopen(['R_txt\',num2str(mm),'_',num2str(nn),'\',num2str(frame),'_MSE.txt'],'w');
        %     fid2 = fopen(['R_txt\',num2str(mm),'_',num2str(nn),'\',num2str(frame),'_Bitrate.txt'],'w');
        %
        %     for qp=22:5:42
        %         for i=1:mm
        %             for j=1:nn
        %                 fprintf(fid,'%f ',MSE_All_static(i,j,qp));
        %             end
        %             fprintf(fid,'\n');
        %         end
        %         fprintf(fid,'\n\n');
        %     end
        
        %     for qp=22:5:42
        %         for i=1:mm
        %             for j=1:nn
        %                 if qp==22
        %                     fprintf(fid2,'%f ', Bitrate_22(i,j));
        %                 end
        %                 if qp==27
        %                     fprintf(fid2,'%f ', Bitrate_27(i,j));
        %                 end
        %                 if qp==32
        %                     fprintf(fid2,'%f ', Bitrate_32(i,j));
        %                 end
        %                 if qp==37
        %                     fprintf(fid2,'%f ', Bitrate_37(i,j));
        %                 end
        %                 if qp==42
        %                     fprintf(fid2,'%f ', Bitrate_42(i,j));
        %                 end
        %             end
        %             fprintf(fid2,'\n');
        %         end
        %         fprintf(fid2,'\n');
        %     end
        %
        fclose(fid);
        %fclose(fid2);
        cont=cont+1;
    end
end
























