function theta = CalParaAngle_G(C,V)
x1=C(1);
y1=C(2);
x2=V(1);
y2=V(2);
dx = x2-x1;
dy = y2-y1;
if dx<-1440
    dx = dx+2880;
end
if dx>1440
    dx=dx-2880;
end
theta = atan(dy/dx);
if dx<0 
    theta = theta + 3.1415926;
end
if theta<0
    theta = theta+3.1415926 * 2;
end