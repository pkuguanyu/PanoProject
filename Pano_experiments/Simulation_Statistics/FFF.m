function [result]=FFF(X,Y)
fitTypeMseToBitrate=fittype('(a*x+b)/(x+c)','independent','x','coefficients',{'a','b','c'});
opt=fitoptions(fitTypeMseToBitrate);
opt.StartPoint=[20,200,8];
result=fit(X,Y,fitTypeMseToBitrate,opt);
end