function [x_out,y_out,std] = solidimprovement(path,mark)
%mark = '741';
n = 0;
a = zeros(100000,42);
from = 1:100000;
b = zeros(100000,42);
m = [1:100000];
sz = size(a)
start = 0;
endd = 0;
subpath = ['set1/3/1/';'set1/4/1/';'set1/6/1/';'set1/7/1/';'set1/8/1/';'set2/1/1/';'set2/3/1/';'set2/4/1/';'set2/5/1/';'set2/6/1/';'set2/7/1/'];
toppath = ['topbandwidth/set1/3.mat';'topbandwidth/set1/4.mat';'topbandwidth/set1/6.mat';'topbandwidth/set1/7.mat';'topbandwidth/set1/8.mat';'topbandwidth/set2/1.mat';'topbandwidth/set2/3.mat';'topbandwidth/set2/4.mat';'topbandwidth/set2/5.mat';'topbandwidth/set2/6.mat';'topbandwidth/set2/7.mat'];
top = 1:100000;
spath = 0;
weighted_n = 0;
%start = 10000000;
%endd = 0;
for set=4:4
    for video=6:6
        %if set == 2 && video > 3
        %    continue;
        %end
        for i=1:100000
            top(i)=-1;
        end
        set0 = num2str(set);
        video0 = num2str(video);
        name = ['topbandwidth',mark,'/set',set0,'/',video0,'.mat'];
        if exist(name,'file')==0
            continue;
        end
        spath = spath + 1;
        toplist = cell2mat(struct2cell(load(name)));
        for i=1:(size(toplist))(1)
        %for i=4:4
            if path(1) == 'd'
                top(toplist(i,1)) = 1000000000;
            end
            if path(1) == 'T'
                top(toplist(i,1)) = toplist(i,2);
            end
            if path(1) == 'V'
                top(toplist(i,1)) = toplist(i,3);
            end
            if path(1) == 'W'
                top(toplist(i,1)) = toplist(i,4);
            end
        end
        for frame=1:30:1801
            %list = [301,331,361,631,1021,1351,1441,1741];
            %flag = 0;
            %for i = 1:8
            %    if frame == list(i)
            %        flag = 1;
            %    end
            %end
            %if flag == 1
            %    continue;
            %end
            if top(frame)<0
                continue;
            end
            frame0 = num2str(frame);
            tt = ['',set0,'/',video0,'/1/'];
            name = ['video2realperformance(2)',mark,'/',path,tt,'XXX_',frame0,'.mat'];
            name1 = ['video2realperformance(2)',mark,'/','distance/',tt,'XXX_',frame0,'.mat'];
            name2 = ['video2realperformance(2)',mark,'/','VR/',tt,'XXX_',frame0,'.mat'];
            name3 = ['video2realperformance(2)',mark,'/','WithoutVR/',tt,'XXX_',frame0,'.mat'];
            name4 = ['video2realperformance(2)',mark,'/','Theory/',tt,'XXX_',frame0,'.mat'];
            if ~exist(name1,'file')==0 && ~exist(name2,'file')==0 && ~exist(name3,'file')==0 && ~exist(name4,'file')==0
                n = n + 1
                from(n) = spath;
                tmp = cell2mat(struct2cell(load(name)));
                sz = size(tmp);
                m(n)=sz(2);
                %tmpout = m(n)
                a(n,1:m(n)) = tmp;
                name = ['video2realperformance(2)',mark,'/',path,tt,'YYY_',frame0,'.mat'];
                b(n,1:m(n)) = cell2mat(struct2cell(load(name)));
                for i=1:m(n)
                    b(n,i) = min(b(n,i),100);
                end
                mmax = max(a(n,:));
                
                for i=1:m(n)
                    if i>1 && a(n,i) > top(frame) - 1
                        m(n) = i;
                        %tmpout = m(n)
                        break;
                    end
                end
                %m(n)
                start = start + log(a(n,1));
                endd = endd + log(a(n,m(n)));
                %start = start + a(n,1) * m(n);
                %endd = endd + a(n,m(n)) * m(n);
                %weighted_n = weighted_n + m(n);
                %start = min(start , a(n,1));
                %endd = max(endd , a(n,m(n)));
            end
        end
    end
end

start = exp(start / n)
endd = exp(endd / n)
%start = start / weighted_n;
%endd = endd / weighted_n;

x = zeros(100,100);
y = zeros(100,100);
z = zeros(100,100);
for i=1:21
    for j=1:spath
        x(j,i) = start + (endd - start) / 10 * (i - 1);
    end
end

for i=1:n
    rate = (a(i,m(i)) - a(i,1)) / (endd - start);
    startnow = a(i,1);
    for j=1:m(i)
        a(i,j) = (a(i,j) - startnow) / rate + start;
        
        for k=1:20
            if a(i,j) >= x(1,k) && a(i,j) < x(1,k+1) + 1
                y(from(i),k) = y(from(i),k) + b(i,j);
                z(from(i),k) = z(from(i),k) + 1;
                break;
            end
        end
    end
end
spath
for i=1:20
    for j=1:spath
        if z(j,i) > 0
            y(j,i) = y(j,i) / z(j,i);
        end
    end
end
for i=2:20
    for j=1:spath
        %y(j,i) = max(y(j,i),y(j,i-1));
    end
end

x_out = 1:20;
y_out = 1:20;
std = 1:20;

for i=1:20
    x_out(i) = x(1,i);
    tmp = 0;
    tmpy = 0;
    for j=1:spath
        %if z(j,i)>0
            tmp = tmp + y(j,i);
            tmpy = tmpy + 1;
        %end
    end
    y_out(i) = tmp / spath;
    tmp = 0;
    for j=1:spath
        %if z(j,i)>0
            tmp = tmp + (y(j,i) - y_out(i)) * (y(j,i) - y_out(i));
        %end
    end
    tmp = sqrt(tmp) / spath;
    std(i) = tmp;
end