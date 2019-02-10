function [result]=CalSJND_FAST_GPU(img)

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
%R=zeros(H,W);
%计算MG和BG



BG=imfilter(img, B, 'corr')./32;
MG1=abs(imfilter(img, G1, 'corr'))./16;
MG2=abs(imfilter(img, G2, 'corr'))./16;
MG3=abs(imfilter(img, G3, 'corr'))./16;
MG4=abs(imfilter(img, G4, 'corr'))./16;
MG=MG1.*((MG1>=MG2).*(MG1>=MG3).*(MG1>=MG4))+MG2.*((MG2>MG1).*(MG2>=MG3).*(MG2>=MG4))+MG3.*((MG3>MG2).*(MG3>MG1).*(MG3>=MG4))+MG4.*((MG4>MG2).*(MG4>MG3).*(MG4>MG1));
%计算f1
alpha=BG.*0.0001+0.115;
beta=lambda-BG.*0.01;
f1=MG.*alpha+beta;
f2=(T0.*(1-(BG./127).^(0.5))+3).*(BG<=127)+(gama.*(BG-127)+3).*(BG>127);
R=max(f1,f2);

result=R;
end