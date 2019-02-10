function [result]=CalPSPNR(Center,img_raw,img_target,R,Fresult_all) %这个给的是整张图片，图，R矩阵都要给全部只要是处理部分的JND给对就可以，外面是无限大可以无视
    D_JND=abs(img_target-img_raw);
    [H,W]=size(img_raw);
    temp_PSPNR=0;
    mm=6;
    nn=12;
    temp_count=0;
    Gap_Height=round(1440/mm);
    Gap_Width=round(2880/nn);
    for i=1:mm
        for j=1:nn
            temp=Fresult_all((i-1)*Gap_Height+1:i*Gap_Height,(j-1)*Gap_Width+1:j*Gap_Width);
            row=sum(sum(temp<9));  %判断一下当前是不是完全在viewport内部的
            if round(row)>round(Gap_Height*Gap_Width)*0.7
                for k=(i-1)*Gap_Height+1:i*Gap_Height
                    for l=(j-1)*Gap_Width+1:j*Gap_Width
                        if D_JND(k,l)-R(k,l)>=0
                            temp_PSPNR=temp_PSPNR+(D_JND(k,l)-R(k,l))^2;
                        end
                        temp_count=temp_count+1;
                    end
                end

            end
        end
    end
    
    
    
    temp_PSPNR= sqrt(temp_PSPNR/(temp_count));
    PSPNR=20*log10(255.0/temp_PSPNR);
    result=PSPNR;
end