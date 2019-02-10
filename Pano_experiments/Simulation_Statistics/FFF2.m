function [result]=FFF2(X,Y)
fitTypeMseToBitrate=fittype('a.*x.^b+c ','independent','x','coefficients',{'a','b','c'});
opt=fitoptions(fitTypeMseToBitrate);
opt.StartPoint=[100,-1,-20];
result=fit(X,Y,fitTypeMseToBitrate,opt);
end