# PanoProject

## 0 Abstract
This project is the code for Pano Project (corresponding to the SIGCOMM paper "Pano: Optimizing 360 Video Streaming with a Better Understanding of Quality Perception").

Link of SIGCOMM 2019: http://conferences.sigcomm.org/sigcomm/2019/

Link of our paper: https://dlnext.acm.org/doi/abs/10.1145/3341302.3342063

This project has two part:

(1) The simulation source code is for readers to regenerate the main experimental results in the paper. We present the source code of both Pano and the baseline here, so readers can make a comparison easily.

(2) The system source code is for readers to understand how we implemented a "user percevied quality driven" 360 video streaming. We present all modules in our system here. But we acknowledge that it still has not reached the level for commercial use.

## 1 Location
The simulation source code is under the folder Pano_experiments\Simulation_Statistics. And now we supply methods of push button  running or component running.
The system source code is under the folder Pano_system\SystemComponent. And we only supply each component core source code of the Pano system.
The following shows the requirements of the system and simulation comparision and the two running method of the simluation source code.

## 2 Requirements
.Net Framework 4.6.1 or Above.  
Unity 2018.2 Pro Edition or Above.   
Windows 10 Professional Edition 1709.    
Any C++ IDE (Visual Studio 2015 is recommended).  
Matlab (R2015a or higher) with Parallel Computing Toolbox and Image Processing Toolbox.  
FFMPEG (add the binary folder, e.g. ¡°C:\Program Files\ffmpeg-20190625-dd662bb-win64-static\bin¡±, to system PATH.).  


## 3 Running
Due to limitaion of file size, the whole source code is stored in the BaiduNetdisk. First, download the whole simluation source code from the link under the folder Pano_experiments\Simulation_Statistics. Second, enter the
directory of the source code and follow the running commands below.

### 1)  Prepare the programs and data
All the Matlab programs are in root folder "/". The data used are as follows: the videos in "/videos", the pre-calculated depth-of-field data in "/DepthMap", the viewpoint trajectory in "/traj" and "/viewpoint".
### 2)  Split the videos to one-second chunks
Run "/cutChunk.m" to split the videos to one-second chunks in "/videos/setID/videoID/chunkID.mp4".
### 3)  Calculate quality-bitrate efficiency
Run "/getAllTileValueness.m" to calculate the efficiency score of each tile. The efficiency score is stored in "/ratio/setID/videoID/userID/frameID_Value_SMSE.txt".
### 4)  group similar tiles
Run the C++ program "/main.cpp" to group similar tiles. Before running, modify the relative path at line 254/358/380 as appropriate. The result stored in "/tiling1004" consists of the index, the start and end row, the start column and end column of each grouped tile.
### 5)  Run the simulation
Run "/main.m" to simulate grid tiling baseline and Pano client-side mechanism. The PSPNR and bandwidth consumption per user per chunk are stored in "/baselineResult" and "/PanoResult". A PSPNR-bandwidth graph like Fig 18 is drawed based on the simulation result.
To observe Pano tiling and allocation result, you should put a breakpoint at "/Pano.m" line 188. When pausing, paste and run the following commands in console:
'''
viewedTilesSize = [];
viewedTileMSE = [];
for i=1:nTiles
if viewed(i)
    viewedTileMSE = [viewedTileMSE,meanMSEreal(user,i,QP(iViewed)-22+1)];
    viewedTilesSize = [viewedTilesSize, tileSize(i,QP(iViewed)-22+1)];
end
end
disp(['set ',num2str(set),' vid ',num2str(vid),' sec ',num2str(sec),' user ',num2str(user),' qp ',num2str(qp)]);
disp('transmitted tile:(start row, end row, start column, end column)');
disp(tiling(viewed));
disp('QP allocation:');
disp(QP);
disp('bitrate of each tile: (kbit/s)');
disp(viewedTilesSize);
disp('PMSE * number of pixels:');
disp(viewedTileMSE);
'''

## (2) Automatic Running
### Run the batch.bat

## 4 Contact us if questions
If you have any questions with this project, or you have some questions about the paper, feel free to contact us:

Yu Guan: shanxigy@pku.edu.cn

Chengyuan Zheng: 1801213770@pku.edu.cn

If you have any suggestion for this code, please also feel free to contact us. We will take our time to update the codes.
