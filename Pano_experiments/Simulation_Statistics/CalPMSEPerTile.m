function [result]=CalPMSEPerTile(Center,img_raw,img_target,R) %这个是算的具体某个图像的，不是全图,所以图像，R矩阵都要给部分的
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
    temp_PSPNR= temp_PSPNR;
    result=temp_PSPNR;
end