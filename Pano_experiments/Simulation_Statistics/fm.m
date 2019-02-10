function [result]=fm(img,v,e)

result=min(fc(img,e),fd(img,v));
end