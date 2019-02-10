function [result]=Wf(img,v,e)
result=1+(1-fm(img,v,e)*1.0/fm(img,v,0));
end