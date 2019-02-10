function [result]=F(img,x,y,v,e)
temp=-(log2(bg(img,x,y)+1)-7)^(2)/(2*0.8*0.8);
n=0.5+(1.0/(sqrt(2*pi)*0.8))*exp(temp);
result=Wf(img,v,e)^(n);
end