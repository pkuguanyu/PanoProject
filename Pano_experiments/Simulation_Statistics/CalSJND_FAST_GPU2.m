function [result]=CalSJND_FAST_GPU2(img)
B=[1,1,1,1,1;
   1,2,2,2,1;
   1,2,0,2,1;
   1,2,2,2,1;
   1,1,1,1,1];
G1=[0,0,0,0,0;      %垂直方向亮度差
    1,3,8,3,1;
    0,0,0,0,0;
    -1,-3,-8,-1,-1;
    0,0,0,0,0];

G2=[0,0,1,0,0;      %主对角线亮度差
    0,8,3,0,0;
    1,3,0,-3,-1;
    0,0,-3,-8,0;
    0,0,-1,0,0];

G3=[0,0,1,0,0;      %副对角线亮度差
    0,0,3,8,0;
    -1,-3,0,3,1;
    0,-8,-3,0,0;
    0,0,-1,0,0];

G4=[0,1,0,-1,0;     %水平方向亮度差
    0,3,0,-3,0;
    0,8,0,-8,0;
    0,3,0,-3,0;
    0,1,0,-1,0];

lambda=0.5;
T0=17;
gama=3/128;

%进入GPU

GPU_lambda=gpuArray(lambda);
GPU_T0=gpuArray(T0);
GPU_gama=gpuArray(gama);
GPU_G1=gpuArray(G1);
GPU_G2=gpuArray(G2);
GPU_G3=gpuArray(G3);
GPU_G4=gpuArray(G4);
GPU_img=gpuArray(img);
GPU_B=gpuArray(B);
GPU_BG=imfilter(GPU_img, GPU_B, 'corr')./32;
GPU_MG1=abs(imfilter(GPU_img, GPU_G1, 'corr'))./16;
GPU_MG2=abs(imfilter(GPU_img, GPU_G2, 'corr'))./16;
GPU_MG3=abs(imfilter(GPU_img, GPU_G3, 'corr'))./16;
GPU_MG4=abs(imfilter(GPU_img, GPU_G4, 'corr'))./16;

GPU_MG=GPU_MG1.*((GPU_MG1>=GPU_MG2).*(GPU_MG1>=GPU_MG3).*(GPU_MG1>=GPU_MG4))+GPU_MG2.*((GPU_MG2>GPU_MG1).*(GPU_MG2>=GPU_MG3).*(GPU_MG2>=GPU_MG4))+GPU_MG3.*((GPU_MG3>GPU_MG2).*(GPU_MG3>GPU_MG1).*(GPU_MG3>=GPU_MG4))+GPU_MG4.*((GPU_MG4>GPU_MG2).*(GPU_MG4>GPU_MG3).*(GPU_MG4>GPU_MG1));
%计算f1
GPU_alpha=GPU_BG.*0.0001+0.115;
GPU_beta=GPU_lambda-GPU_BG.*0.01;
GPU_f1=GPU_MG.*GPU_alpha+GPU_beta;
GPU_f2=(GPU_T0.*(1-(GPU_BG./127).^(0.5))+3).*(GPU_BG<=127)+(GPU_gama.*(GPU_BG-127)+3).*(GPU_BG>127);

GPU_R=max(GPU_f1,GPU_f2);
result=gather(GPU_R);

end