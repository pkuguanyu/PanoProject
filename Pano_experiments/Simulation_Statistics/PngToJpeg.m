

disk='I:';
sett={'set1','set2','set3','set4','set5'};

for s=1:2   
    set=sett{s};
    for videoid=0:8
        mkdir(['H:\framPng\',set,'\',num2str(videoid),'\']);
        for j=1:30:1801
            j
           try
                img=imread(['I:\视频评价调研\Potential improment\frame\',set,'\',num2str(videoid),'\original\',num2str(j),'.png']);
                img=imresize(img,[720 1440]);
                imwrite(img,['H:\framPng\',set,'\',num2str(videoid),'\',num2str(j),'.jpg']);
            catch
                continue
            end
        end
    end
end


