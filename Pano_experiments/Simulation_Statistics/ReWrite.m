videoid=0;
framegap=30;
path(path,'I:\视频评价调研\Potential improment')
for userid=1:48
RotationSpeed=GetRotationSpeed(videoid,userid,framegap); %每一行代表第X个10帧内的旋转速度
[x,y]=size(RotationSpeed);
mkdir('RotationSpeed');
fid=fopen(['RotationSpeed\',num2str(userid),'.txt'],'w');
for i=1:x
    fprintf(fid,'%f\r\n',RotationSpeed(i));
end
fclose(fid);
end