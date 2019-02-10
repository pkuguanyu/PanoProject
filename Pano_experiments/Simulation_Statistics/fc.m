function [result]=fc(img,e)
a=0.106;
CT0=1.0/64;
e2=2.3;
f=e2*log(1.0/CT0)/((e+e2)*a);
result=f;
end