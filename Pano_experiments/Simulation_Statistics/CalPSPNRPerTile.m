function [result]=CalPSPNRPerTile(Center,img_raw,img_target,R,Fresult_all) %这个给的是整张图片，图，R矩阵都要给全部只要是处理部分的JND给对就可以，外面是无限大可以无视
     D_JND=abs(img_target-img_raw);
    [H,W]=size(img_raw);
    temp_PSPNR=0;
    for j=1:H
        for k=1:W
            if D_JND(j,k)-R(j,k)>=0
                temp_PSPNR=temp_PSPNR+(D_JND(j,k)-R(j,k))^2;
            end
        end
    end   
    temp_PSPNR= sqrt(temp_PSPNR/(H*W));
    PSPNR=20*log10(255.0/temp_PSPNR);
    result=PSPNR;
end