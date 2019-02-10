function [result]=FFF3(X,Y)
fitTypeMseToBitrate=fittype('a.*x.^b+c ','independent','x','coefficients',{'a','b','c'});
opt=fitoptions(fitTypeMseToBitrate);
opt.StartPoint=[100000,-2,-1];
opt.Lower =[-Inf -Inf -Inf];
opt.Upper =[Inf Inf Inf];
result=fit(X,Y,fitTypeMseToBitrate,opt);
end