clear all;
path(path,'I:\视频评价调研\Potential improment');
path(path,'I:\视频评价调研\Potential improment\第六版Potential improvement\Tiling');
path(path,'I:\视频评价调研\Potential improment\第六版Potential improvement');

%------------------------------------------------
videoid=1;
userid=1;
framegap=30;
frameStart=271;
frameEnd=271;
mm=12;
nn=24;
cont=1
disk='I:'
for frame=frameStart:framegap:frameEnd
    for userid=1:1
        for cont=1:42
            cont
            Qp=cell2mat(struct2cell(load([disk,'\视频评价调研\Potential improment\第六版Potential improvement\Tiling\Qp_',num2str(mm),'_',num2str(nn),'\',num2str(videoid),'\',num2str(userid),'\',num2str(frame),'\',num2str(cont),'.mat'])));
            fid = fopen([disk,'\视频评价调研\Potential improment\第六版Potential improvement\Tiling\Qp_',num2str(mm),'_',num2str(nn),'\',num2str(videoid),'\',num2str(userid),'\',num2str(frame),'\',num2str(cont),'.txt'],'w');
            for i=1:mm
                for j=1:nn
                    fprintf(fid,'%f ',round(Qp(i,j)));
                end
                fprintf(fid,'\n');
            end
            fclose(fid);
        end
    end
end
