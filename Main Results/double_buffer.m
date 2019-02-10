%为VR视点自适应传输设计的双队列buffer的ABR算法
function [PSPNR,starttime,stalltime] = double_buffer(path , throughput , std,video,buffer_B_min)
buffersize_A = 5;
buffersize_B = 30;
n = 0;
start_bitrate = throughput / 2;
subpath = ['1/3/';'1/4/';'1/6/';'1/7/';'1/8/';'2/1/';'2/3/';'2/4/';'2/6/';'2/7/'];


for spath = video:video
    PSPNR_per_frame_A = 1:10000;
    PSPNR_per_frame_B = 1:10000;
    %11 states:
    A_finish = 0;
    B_finish = 0;
    start = 0;
    buffer_A=0;
    buffer_B=0;
    buffer_A_downloadingframe = -29;
    buffer_A_readyframe=-59;
    buffer_B_downloadingframe = -29;
    buffer_B_readyframe=-59;
    last_download = 1;
    playframe = -29;
    downloadtime = 0;
    playtime = 0;
    
    %3 outputs:
    starttime = 0;
    stalltime = 0;
    PSPNR = 0;
    
    rate = start_bitrate;
    for time=0:0.01:300
        state = [time,playframe,playtime,buffer_A_readyframe,buffer_B_readyframe,downloadtime,buffer_A,buffer_B];
        %上一次下载完成
        if downloadtime <= 0
            
            %决定现在该填bufferA还是bufferB
            %flag=0:不下载
            %flag=1:填充bufferA
            %flag=2:填充bufferB
            flag = 0;
            if (buffer_B < buffer_B_min || buffer_B <= buffer_A) && B_finish == 0
                flag = 2;
            else
                if buffer_A < buffersize_A && A_finish == 0
                    flag = 1;
                elseif buffer_B < buffersize_B && B_finish == 0
                    flag = 2;
                end
            end
            
            %在支离破碎的frame中找到下一个有实验数据的frame
            if last_download == 1
                buffer_A_readyframe = buffer_A_downloadingframe;
            else
                buffer_B_readyframe = buffer_B_downloadingframe;
            end

            if flag > 0
                if flag == 1
                    frame = max(buffer_A_readyframe, playframe);
                    last_download = 1;
                else
                    frame = buffer_B_readyframe;
                    last_download = 2;
                end
                
                while 1==1
                    frame = frame + 30;
                    frame0 = num2str(frame);
                    tt = [subpath(spath,:),'1/'];
                    %tt='';
                    name = ['video2realperformance(2)/',path,tt,'XXX_',frame0,'.mat'];
                    name1 = ['video2realperformance(2)/','distance/',tt,'XXX_',frame0,'.mat'];
                    name2 = ['video2realperformance(2)/','VR/',tt,'XXX_',frame0,'.mat'];
                    name3 = ['video2realperformance(2)/','WithoutVR/',tt,'XXX_',frame0,'.mat'];
                    name4 = ['video2realperformance(2)/','Theory/',tt,'XXX_',frame0,'.mat'];
                    name5 = ['video2realperformance(2)/','mono/',tt,'YYY_',frame0,'.mat'];
            
                    if ~exist(name1,'file')==0 && ~exist(name2,'file')==0 && ~exist(name3,'file')==0 && ~exist(name4,'file')==0 && ~exist(name5,'file')==0
                        break;
                    end
                    if frame>6000
                        if flag == 1
                            A_finish = 1;
                        else
                            B_finish = 1;
                        end
                        break;
                    end
                end
            
                if frame<6000
                    if flag == 1
                        buffer_A_downloadingframe = frame;
                    else
                        buffer_B_downloadingframe = frame;
                    end
                    a = cell2mat(struct2cell(load(name)));
                    baselayer_matrix = cell2mat(struct2cell(load(name2)));
                    baselayer = baselayer_matrix(1);
                    name = ['video2realperformance(2)/',path,tt,'YYY_',frame0,'.mat'];
                    if path(1) =='d'
                        a = a .* 10;
                    end
                    if path(1)=='W'
                        a = a + 1100;
                    end
                    b = cell2mat(struct2cell(load(name)));
                    mmin = 1000000000;
                    level = 0;
                    if time < 1
                        throughput_now = throughput;
                    else
                        throughput_now = max(1000,normrnd(throughput,std));
                    end
                    if flag == 1
                        %bufferA:下最合适的
                        if buffer_A>3
                            rate = min(80000,rate * 1.2);
                        end
                        if buffer_A<2
                            rate = rate * 0.7;
                        end
                        a_size = size(a);
                        len = a_size(2);
                        for i=1:len
                            if abs(a(i)-rate) < mmin
                                mmin = abs(a(i)-baselayer-rate);
                                level = i;
                            end
                        end
                        increment = min(100,b(level));
                        downloadtime = (a(level) - baselayer) / throughput_now;
                        PSPNR_per_frame_A(frame) = increment;
                        buffer_A = buffer_A + 1;
                    else
                        %bufferB:下最低的
                        level = 1;
                        increment = min(100,b(level));
                        downloadtime = baselayer / throughput_now;
                        PSPNR_per_frame_B(frame) = increment;
                        buffer_B = buffer_B + 1;
                    end
                end
            end
        end
        
        downloadtime = downloadtime - 0.01;
        
        if playtime < 0.001
            frame = playframe;
            while 1==1
                frame = frame + 30;
                frame0 = num2str(frame);
                tt = [subpath(spath,:),'1/'];
                %tt='';
                name = ['video2realperformance(2)/',path,tt,'XXX_',frame0,'.mat'];
                name1 = ['video2realperformance(2)/','distance/',tt,'XXX_',frame0,'.mat'];
                name2 = ['video2realperformance(2)/','VR/',tt,'XXX_',frame0,'.mat'];
                name3 = ['video2realperformance(2)/','WithoutVR/',tt,'XXX_',frame0,'.mat'];
                name4 = ['video2realperformance(2)/','Theory/',tt,'XXX_',frame0,'.mat'];
                name5 = ['video2realperformance(2)/','mono/',tt,'YYY_',frame0,'.mat'];
            
                if ~exist(name1,'file')==0 && ~exist(name2,'file')==0 && ~exist(name3,'file')==0 && ~exist(name4,'file')==0 && ~exist(name5,'file')==0
                    break;
                end
                if frame>6000
                    break;
                end
            end
            if frame > 6000
                break;
            end
            playframe = frame;
            playtime = 1;
            buffer_A = buffer_A - 1;
            buffer_B = buffer_B - 1;
            if buffer_A < 0
                buffer_A = 0;
            end
        end
        
        if time >= 2 && playframe<=buffer_A_readyframe
            playtime = playtime - 0.01;
            start = 1;
            if PSPNR_per_frame_A(playframe)>0.01
                PSPNR = PSPNR + PSPNR_per_frame_A(playframe);
                n=n+1;
            end
            PSPNR_per_frame_A(playframe) = 0;
            PSPNR_per_frame_B(playframe) = 0;
        elseif playframe<=buffer_B_readyframe
            playtime = playtime - 0.01;
            start = 1;
            if PSPNR_per_frame_B(playframe)>0.01
                PSPNR = PSPNR + PSPNR_per_frame_B(playframe);
                n=n+1;
            end
            PSPNR_per_frame_A(playframe) = 0;
            PSPNR_per_frame_B(playframe) = 0;
        else
            if start == 1
                stalltime = stalltime + 0.01;
            else
                starttime = starttime + 0.01;
            end
        end
        
    end
    PSPNR = PSPNR / n;
    stalltime = stalltime / state(1);
end
end