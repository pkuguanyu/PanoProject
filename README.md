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
## (1) Componenet Running
Due to limitaion of file size, the whole source code is stored in the BaiduNetdisk.

### 1)  Prepare the programs and data
All the Matlab programs are in root folder ¡°/¡±. The videos are in the folder ¡°/videos¡±, the trajectory data is in two format is in ¡°/traj¡± and ¡°viewpoint¡±, the pre-calculated tile depth data is in ¡°/DepthMap¡±. 
### 2)  cut chunks
Run ¡°/cutChunk.m¡± to cut the videos to one-second chunks in ¡°/videos¡±.
### 3)  calculate tile valueness
Set the variable calcTileVal to 1 in ¡°/main.m¡±, then run ¡°/main.m¡± to calculate the valueness of each tile in each chunk. The result is stored in ¡°/ratio¡±.
### 4)  generate tiling scheme
If you¡¯re using Visual Studio 2015, open the project file ¡°/tilingDP/tilingDP.sln¡± and run it, otherwise you should build a new project in your IDE and copy ¡°/tilingDP/main.cpp¡± into it, modify the data path at line 254/358/380 as appropriate. The tiling scheme is stored in ¡°/tiling1004¡±.
### 5)  run the simulation
Set the variable calcTileVal to 0 in ¡°/main.m¡±, then run ¡°/main.m¡±. The results about PSPNR and bitrate consumption per user per chunk are stored in ¡°/baselineResult¡± and ¡°/PanoResult¡± for further analyzation. ¡°/main.m¡± visualizes a comparison between baseline and Pano in Matlab window.

## (2) Push Button Running
### Just run the batch.bat