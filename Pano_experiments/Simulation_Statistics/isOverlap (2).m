function [result]= isOverlap(x1,y1,width1,height1,x2,y2,width2,height2)
rc1=struct;
rc2=struct;
rc1.x=x1;rc1.y=y1;rc1.width=width1;rc1.height=height1;
rc2.x=x2;rc2.y=y2;rc2.width=width2;rc2.height=height2;
if(rc1.x + rc1.width  > rc2.x && rc2.x + rc2.width  > rc1.x && rc1.y + rc1.height > rc2.y && rc2.y + rc2.height > rc1.y)
    result=1;
else
    result=0;
end
    
