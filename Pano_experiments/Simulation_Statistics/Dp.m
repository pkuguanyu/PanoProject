function [result,Qp_result2]=Dp(N,Band,BandWidth,Value)

% Band=10;
% N=2;
%把N个tile放进Band大小
Min_PMSE=zeros(N,Band);
Path=zeros(N,Band);
Path_Qp=zeros(N,Band);


for i=1:N
    for j=1:Band
        flag=0;%代表当前位置是不是有能放进去的，默认是没有
        tempMax=-1;
        tempQp=0;
        for Qp=21:-1:1    
            if(j>=round(BandWidth(i,Qp)))
                data=0;
                if i-1<=0 || j-round(BandWidth(i,Qp))<=0
                    data=0;
                else
                    data=Min_PMSE(i-1,j-round(BandWidth(i,Qp)));
                end
                if tempMax<data+ Value(i,Qp)
                    tempMax=data+ Value(i,Qp);
                    tempQp=Qp+21;
                end
                flag=1;
            end
        end
        if flag==0
            if i-1<=0
                data=0;
            else
                data=Min_PMSE(i-1,j);
            end
            Min_PMSE(i,j)=data;
        else
            x=tempMax;
            if i-1<=0
                data=0;
            else
                data=Min_PMSE(i-1,j);
            end
            y=data;
            Min_PMSE(i,j)=max(x,y);
            if x>y
                Path(i,j)=1;%想办法获取到Qp
                Path_Qp(i,j)=tempQp;
            end
        end
    end
end


i=N;
j=Band;
Qp_result=[];
while(i>0&&j>0)
    if (Path(i,j)>0)
        Qp_result=[Qp_result;i,Path_Qp(i,j)];
        j=j-BandWidth(i,Path_Qp(i,j)-21);
    end
    i=i-1;
end

result=Min_PMSE(N,Band);
tempQp_result=cell(1,1);
tempQp_result{1,1}=Qp_result;
Qp_result2=tempQp_result{1,1};