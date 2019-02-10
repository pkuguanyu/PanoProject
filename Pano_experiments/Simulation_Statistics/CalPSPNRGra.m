function [result]=CalPSPNRGra(Center,img_raw,img_target,R,mm,nn,T) %这个给的是整张图片，图，R矩阵都要给全部只要是处理部分的JND给对就可以，外面是无限大可以无视
D_JND=abs(img_target-img_raw);
temp_PSPNR=0;
temp_count=0;
Gap_Height=round(1440/mm);
Gap_Width=round(2880/nn);
for i=1:mm
    for j=1:nn    
        for k=(i-1)*Gap_Height+1:i*Gap_Height
            for l=(j-1)*Gap_Width+1:j*Gap_Width
                if T(k,l)==1
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