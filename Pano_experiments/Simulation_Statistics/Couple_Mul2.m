videoid=0;

framegap=30;
set='set2'
frame=3901:300:13001;
[x y]=size(frame)
parfor i=1:y
    CoupleTest(videoid,frame(i),frame(i)+299,30,set);
end