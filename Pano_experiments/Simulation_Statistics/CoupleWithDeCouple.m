function CoupleWithDeCouple(videoid,set,frameStart,frameEnd,framegap)
%本函数通过在编码的时候不考虑视点因素，而在用户实际选择的时候使用系数进行码率选择
%对于一个240*240的视频块，我们认为失真量超过64的块是不能进行码率降低的
%1、现在需要做的拟合，横轴是超过JND的像素点的数量，纵轴是码率
%2、对于视频中的每个tile分别计算，有如下步骤
%3、计算该tile当超过JND的像素点数量恰巧为64时对应的码率，即横轴是64的对应的纵轴，即为我们应该调整的码率
path(path,'I:\视频评价调研\Potential improment');
path(path,'I:\视频评价调研\Potential improment\第六版Potential improvement');
path(path,'I:\视频评价调研\Potential improment\第六版Potential improvement\Tiling');
%------------------------------------------------
% videoid=0;
% userid=1;
% framegap=30;
% frameStart=781;
% frameEnd=781;
mm=12;
nn=24;
% set='set2'

if set=='set1'
    ss=1;
else
    ss=2;
end
cont=1;%计数用，记录当前是第几个X帧
%------------------------------------------------

%分别计算各个QP计算1501帧的累计带宽
%分别计算各个QP计算1501帧的累计PSPNR
%RotationSpeed=GetRotationSpeed(videoid,1,userid,framegap); %每一行代表第X个10帧内的旋转速度

for frame=frameStart:framegap:frameEnd  %每一秒需要做288*20次拟合
    try
        fit_result=zeros(12,24,20,6);
        fit_result_B=zeros(12,24,1);
        for x=1:mm
            for y=1:nn
                [x y]
                flag=0;
                XXXX=struct2cell(load(['I:\视频评价调研\Pot ential improment\第六版Potential improvement\PMSE\R\cd\',set,'\',num2str(videoid),'\',num2str(frame),'_',num2str(x),'_',num2str(y),'\X.mat']));
                YYYY=struct2cell(load(['I:\视频评价调研\Potential improment\第六版Potential improvement\PMSE\R\cd\',set,'\',num2str(videoid),'\',num2str(frame),'_',num2str(x),'_',num2str(y),'\Y.mat']));
                AAAA=struct2cell(load(['I:\视频评价调研\Potential improment\第六版Potential improvement\PMSE\R\cd\',set,'\',num2str(videoid),'\',num2str(frame),'_',num2str(x),'_',num2str(y),'\A.mat']));
                XXXX=XXXX{1};
                YYYY=YYYY{1};
                AAAA=AAAA{1};
                for qp=42:-1:22
                    %首先按FJND值对平均值进行拟合
                    tempX=XXXX{qp};
                    tempY=YYYY{qp};
                    X=[];
                    Y=[];
                    step=5;
                    for i=1:step-1
                        X=[X,mean(tempX(i:step:end))];
                        Y=[Y,(max(tempY(i:step:end))+min(tempY(i:step:end)))/2];
                    end
                    result_sin=cell(step-1,1);
                    %首先要确定振幅A
                    A_reason=zeros(step-1,1);
                    for i=1:step-1
                        A_reason(i)=(max(tempY(i:step:end))-min(tempY(i:step:end)))/2;  %代表振幅
                    end
                    fitTypeMseToBitrate_A=fittype('a.*x.^b','independent','x','coefficients',{'a','b'});
                    opt_A=fitoptions(fitTypeMseToBitrate_A);
                    opt_A.StartPoint=[max(A_reason(:)),-3];
                    result_A=fit(X',A_reason, fitTypeMseToBitrate_A,opt_A);
                    
                    %然后要确定偏移量B
                    for i=1:1
                        tempA=AAAA{qp};
                        fitTypeMseToBitrate_sin=fittype('a.*sin( x+b) ','independent','x','coefficients',{'a','b'});
                        opt_sin=fitoptions(fitTypeMseToBitrate_sin);
                        opt_sin.StartPoint=[A_reason(i),1];
                        AA=tempA(i:step:end)*2*pi/360;
                        YY=tempY(i:step:end)-(max(tempY(i:step:end))+min(tempY(i:step:end)))/2;
                        t=fit(AA',YY',fitTypeMseToBitrate_sin,opt_sin);
                        B=t.b;
                    end
                    
                    %计算上下偏移量C
                    C_reason=zeros(step-1,1);
                    for i=1:step-1
                        C_reason(i)=(max(tempY(i:step:end))+min(tempY(i:step:end)))/2;  %代表振幅
                    end
                    fitTypeMseToBitrate_C=fittype('a.*x.^b ','independent','x','coefficients',{'a','b'});
                    opt_C=fitoptions(fitTypeMseToBitrate_C);
                    opt_C.StartPoint=[max(C_reason(:)),-3];
                    result_C=fit(X',C_reason, fitTypeMseToBitrate_C,opt_C);
                    fit_result(x,y,qp,1)= single(result_A.a);
                    fit_result(x,y,qp,2)= single(result_A.b);
                    fit_result(x,y,qp,3)= single(result_C.a);
                    fit_result(x,y,qp,4)= single(result_C.b);
                    
                    if(flag==0)
                        flag=1;
                        fit_result_B(x,y,1)=B;
                    end
                    
                    %凡是小于0的都认为是0即可
%                     for i=1:step-1
%                         i
%                         tempA=AAAA{qp};
%                         fitTypeMseToBitrate_sin=fittype('a.*sin( x+b)','independent','x','coefficients',{'a','b'});
%                         opt_sin=fitoptions(fitTypeMseToBitrate_sin);
%                         opt_sin.StartPoint=[A_reason(i),1];
%                         AA=tempA(i:step:end)*2*pi/360;
%                         YY=tempY(i:step:end)-(max(tempY(i:step:end))+min(tempY(i:step:end)))/2;
%                         t=fit(AA',YY',fitTypeMseToBitrate_sin,opt_sin);
%                         result_sin{i}=t;
%                         %实际系统拟合，首先根据FJND计算出对应的上下平移值
%                         %再根据FJND计算出幅值
%                         %server提前求出该tile的相位
%                         %相位就是result_sin{i}.b，周期固定为2pi，幅值需要另外拟合
%                         
%                         y2=@(x,a,b) a*sin(x+b);
%                         figure,
%                         plot(AA,y2(AA,result_A(tempX(i)),B)+result_C(tempX(i)),'k*-');
%                         hold on
%                         plot(AA,result_sin{i}(AA)+(max(tempY(i:step:end))+min(tempY(i:step:end)))/2,'go-');
%                         hold on
%                         plot(AA,YY+(max(tempY(i:step:end))+min(tempY(i:step:end)))/2,'ro-');
%                         legend('我的','拟合','真实')
%                     end
                    
                end
                
            end
        end
        mkdir(['I:\视频评价调研\Potential improment\Fit_B\',num2str(ss),'\',num2str(videoid)]);
        mkdir(['I:\视频评价调研\Potential improment\Fit_AC\',num2str(ss),'\',num2str(videoid)])
        save(['I:\视频评价调研\Potential improment\Fit_B\',num2str(ss),'\',num2str(videoid),'\',num2str(frame),'.mat'],'fit_result_B');
        save(['I:\视频评价调研\Potential improment\Fit_AC\',num2str(ss),'\',num2str(videoid),'\',num2str(frame),'.mat'],'fit_result');
     catch
         [frame]
         continue
     end
end










