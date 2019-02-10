
frame=301:300:13001;
[x y]=size(frame)
sett={'set1','set2'}
for videoid=1:8
    for ss=1:1
        set=sett{ss};
        videoid 
        ss
        parfor i=1:y
            CoupleTest(videoid,frame(i),frame(i)+299,30,set);
        end
    end
end