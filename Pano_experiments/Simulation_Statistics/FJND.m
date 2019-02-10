function [result]=FJND(img,x,y,v,e)
result=F(img,x,y,v,e)*SJND(img,x,y);
end