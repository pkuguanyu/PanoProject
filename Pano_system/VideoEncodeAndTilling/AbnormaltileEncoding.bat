@echo off
setlocal enabledelayedexpansion
set /a frame=2101
set /a pts=70
set /a videoid=6
for /L %%q in (22,5,42) do (
 for /L %%j in (70,1,90) do (
    for /L %%i in (0,1,72) do (
      	x264.exe --qp %%q   --acodec none  -o  .\!videoid!_seg\%%j\%%i_%%q.mp4  .\!videoid!_seg\%%j\%%i.mp4
)
)
)