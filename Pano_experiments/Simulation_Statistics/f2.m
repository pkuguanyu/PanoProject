function [result]=f2(b)
T0=17;
gama=3/128;
if b<=127
result=T0*(1-(b/127)^(0.5))+3;
else
result=gama*(b-127)+3;    
end

end