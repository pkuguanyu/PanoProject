# PanoProject
The simulation source code is under the folder Pano_experiments\Simulation_Statistics. And now we supply methods of push button  running or component running.
The system source code is under the folder Pano_system\SystemComponent. And we only supply each component core source code of the Pano system.
The following shows the requirements of the system and simulation comparision and the two running method of the simluation source code.

## 1 Requirements
.Net Framework 4.6.1 or Above.  
Unity 2018.2 Pro Edition or Above. 
Windows 10 Professional Edition 1709.    
Any C++ IDE (Visual Studio 2015 is recommended).  
Matlab (R2015a or higher) with Parallel Computing Toolbox and Image Processing Toolbox.  
FFMPEG (add the binary folder, e.g. ¡°C:\Program Files\ffmpeg-20190625-dd662bb-win64-static\bin¡±, to system PATH.).  


## 2 Running
Due to limitaion of file size, the whole source code is stored in the BaiduNetdisk. First, download the whole simluation source code from the link under the folder Pano_experiments\Simulation_Statistics. Second, enter the
directory of the source code and follow the running commands below.
## (1) Componenet Running
### 1)  Prepare the programs and data
All the Matlab programs are in root folder "/". The data used are as follows: the videos in "/videos", the pre-calculated depth-of-field data in "/DepthMap", the viewpoint trajectory in "/traj" and "/viewpoint".
### 2)  Split the videos into one-second chunks
Run "/cutChunk.m" to split the videos to one-second chunks in "/videos/setID/videoID/chunkID.mp4".
### 3)  Calculate quality-bitrate efficiency
Run "/getAllTileValueness.m" to calculate the efficiency score of each tile. The efficiency score is stored in "/ratio/setID/videoID/userID/frameID_Value_SMSE.txt".
### 4)  Group similar tiles
Run the C++ program "/main.cpp" to group similar tiles. Before running, modify the relative path at line 254/358/380 as appropriate. The result stored in "/tiling1004" consists of the index, the start and end row, the start column and end column of each grouped tile.
### 5)  Run the simulation
Run "/main.m" to simulate grid tiling baseline and Pano cli
## (2) Automatic Running
### Run the batch.bat