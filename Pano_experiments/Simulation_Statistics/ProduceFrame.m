function ProduceFrame(videoid,set)
try
    temp=dir(['J:\video\',set,'\',num2str(videoid),'\*.mp4']);
    name=temp.name;
    name=name(1:end-4);
    obj = VideoReader(['J:\video\',set,'\',num2str(videoid),'\1_1\',name,'.mp4']);%输入视频位置
    t=obj.NumberOfFrames;
    mkdir(['I:\视频评价调研\Potential improment\frame\',set,'\',num2str(videoid),'\original']);
    for j=1:30:t
        j
        if(exist(['I:\视频评价调研\Potential improment\frame\',set,'\',num2str(videoid),'\original\',num2str(j),'.png'],'file')==0)     
            frame = read (obj,j);%读取第几帧
            imwrite(frame,['I:\视频评价调研\Potential improment\frame\',set,'\',num2str(videoid),'\original\',num2str(j),'.png']);
        end
    end
    
    %五档Qp图
    for qp=22:1:42
        qp
        obj = VideoReader(['J:\video\',set,'\',num2str(videoid),'\1_1\',name,'_',num2str(qp),'.mp4']);%输入视频位置
        t=obj.NumberOfFrames;
        mkdir(['I:\视频评价调研\Potential improment\frame\',set,'\',num2str(videoid),'\',num2str(qp)]);
        for j=1:30:t
            j
            if(exist(['I:\视频评价调研\Potential improment\frame\',set,'\',num2str(videoid),'\',num2str(qp),'\',num2str(j),'.png'],'file')==0)
                frame = read (obj,j);%读取第几帧
                imwrite(frame,['I:\视频评价调研\Potential improment\frame\',set,'\',num2str(videoid),'\',num2str(qp),'\',num2str(j),'.png']);
            end
        end
    end
catch
    '出错了'
end

