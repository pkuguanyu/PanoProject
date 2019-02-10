
cd/d "%~dp0"

setlocal enabledelayedexpansion
set /a videoid=-1

for /f "delims=" %%i in ('dir/b/s *.mp4')do (
set /a videoid+= 1
set name=%%~ni

md !videoid!
move .\!name!.mp4 !videoid!\!name!.mp4
copy .\ReadJson1_1.py !videoid!\ReadJson1_1.py
copy .\ReadJson_3_6.py !videoid!\ReadJson3_6.py
copy .\ReadJson_6_12.py !videoid!\ReadJson6_12.py
copy .\ReadJson_12_24.py !videoid!\ReadJson12_24.py
cd !videoid!
md 1_1
md 3_6
md 6_12
md 12_24

ffmpeg -i .\!name!.mp4 -strict -2 -vf crop=2880:1440:0:0  .\1_1\!name!.mp4

for /L %%i in (0,480,960) do (
     for /L %%j in (0,480,2400) do (
      ffmpeg -i .\!name!.mp4 -vf crop=480:480:%%j:%%i  .\3_6\!name!_%%i_%%j.mp4
)
)

for /L %%i in (0,240,1200) do (
     for /L %%j in (0,240,2640) do (
      ffmpeg -i  .\!name!.mp4 -vf crop=240:240:%%j:%%i  .\6_12\!name!_%%i_%%j.mp4
)
)

for /L %%i in (0,120,1320) do (
           for /L %%j in (0,120,2760) do (
       ffmpeg -i .\!name!.mp4 -vf crop=120:120:%%j:%%i .\12_24\!name!_%%i_%%j.mp4
)
)


@echo off 
for /L %%q in (22,1,42) do (
      x264.exe --qp %%q   --acodec none  -o  .\1_1\!name!_%%q.mp4 .\1_1\!name!.mp4
)

@echo off 

for /L %%q in (22,5,42) do (
    for /L %%i in  (0,480,960)  do (
           for /L %%j in (0,480,2400) do (
      x264.exe --qp %%q   --acodec none  -o  .\3_6\!name!_%%i_%%j_%%q.mp4 .\3_6\!name!_%%i_%%j.mp4
)
)
)
@echo off 

for /L %%q in (22,5,42) do (
    for /L %%i in (0,240,1200) do (
           for /L %%j in (0,240,2640) do (
      x264.exe --qp %%q   --acodec none  -o  .\6_12\!name!_%%i_%%j_%%q.mp4 .\6_12\!name!_%%i_%%j.mp4
)
)
)
@echo off 

for /L %%q in (22,5,42) do (
    for /L %%i in (0,120,1320) do (
           for /L %%j in (0,120,2760) do (
      x264.exe --qp %%q   --acodec none  -o  .\12_24\!name!_%%i_%%j_%%q.mp4 .\12_24\!name!_%%i_%%j.mp4
)
)
)


md J:\json\set3\!videoid!\1_1_json\
md J:\json\set3\!videoid!\3_6_json\
md J:\json\set3\!videoid!\6_12_json\
md J:\json\set3\!videoid!\12_24_json\

for /L %%q in (22,5,42) do (
      ffprobe -v quiet -print_format json -show_format -show_frames .\1_1\!name!_%%q.mp4  >>J:\json\set3\!videoid!\1_1_json\!name!_0_0_%%q.json
)

for /L %%q in (22,5,42) do (
      for /L %%i in (0,480,960) do (
           for /L %%j in (0,480,2400) do (
      ffprobe -v quiet -print_format json -show_format -show_frames .\3_6\!name!_%%i_%%j_%%q.mp4  >>J:\json\set3\!videoid!\3_6_json\!name!_%%i_%%j_%%q.json

)
)
)



@echo off 

for /L %%q in (22,5,42) do (
     for /L %%i in (0,240,1200) do (
        for /L %%j in (0,240,2640) do (
      ffprobe -v quiet -print_format json -show_format -show_frames .\6_12\!name!_%%i_%%j_%%q.mp4  >>J:\json\set3\!videoid!\6_12_json\!name!_%%i_%%j_%%q.json

)
)
)



@echo off 

for /L %%q in (22,5,42) do (
    for /L %%i in (0,120,1320) do (
           for /L %%j in (0,120,2760) do (
      ffprobe -v quiet -print_format json -show_format -show_frames .\12_24\!name!_%%i_%%j_%%q.mp4  >>J:\json\set3\!videoid!\12_24_json\!name!_%%i_%%j_%%q.json

)
)
)

ReadJson1_1.py !videoid! !name!
ReadJson3_6.py !videoid! !name!
ReadJson6_12.py !videoid! !name!
ReadJson12_24.py !videoid! !name!

cd..

)
