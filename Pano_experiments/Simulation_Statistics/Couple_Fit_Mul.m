
framegap=30;
frameStart=1;
frameEnd=13001;
frame=301:300:13001;
[x y]=size(frame)
sett={'set1','set2'}
for videoid=7:8
    for ss=1:2
        set=sett{ss};
        videoid 
        ss
        parfor i=1:y
            CoupleWithDeCouple(videoid,set,frame(i),frame(i)+299,30);
        end
    end
end