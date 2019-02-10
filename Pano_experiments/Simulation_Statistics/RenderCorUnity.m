function RenderCorUnity(disk,set,videoid,userid,frameStart,frameEnd,framegap)
%------------------------------------------------
ss=1;
if set=='set1'
    ss=1;
elseif set=='set2'
    ss=2;
elseif set=='set3'
    ss=3;
elseif set=='set4'
    ss=4;
elseif set=='set5'
    ss=5;
end

for frame=frameStart:framegap:frameEnd
    viewPoint=ceil(cell2mat(struct2cell(load(['I:\视频评价调研\Potential improment\viewpoint_pd\',num2str(ss),'\',num2str(videoid),'\',num2str(frame),'.mat']))));
    mkdir(['H:\UnityCor\',set,'\',num2str(videoid)])
    fid=fopen(['H:\\UnityCor\',set,'\',num2str(videoid),'\',num2str(userid),'.txt'],'a');
    fprintf(fid,'%d %f %f\r\n',frame,viewPoint(userid,1),viewPoint(userid,2));
end
fclose(fid);