@echo off
setlocal enabledelayedexpansion
set /a frame=2101
set /a pts=70
set /a videoid=6
for /l %%a in (1,1,21) do (
	md 6_seg
	cd 6_seg
	md !pts!
  	cd ..
	for /f "tokens=1,2,3,4,5 delims= " %%i in (!videoid!/!frame!/1.txt) do (
		set /a lefty= %%j*120 
		set /a lefty-=120
                set /a leftx= %%l*120
		set /a leftx-=120
		set /a width= %%m-%%l+1
		set /a width*= 120
		set /a height= %%k-%%j+1
		set /a height*= 120
                echo !lefty!
		echo !width!
        	echo %%i %%j %%k %%l %%m
		ffmpeg -i .\!videoid!_slice\!pts!.mp4 -vf crop=!width!:!height!:!leftx!:!lefty! .\!videoid!_seg\!pts!\%%i.mp4		
)
		set /a pts+=1
		set /a frame+=30
		echo !frame!
)




@echo off
setlocal enabledelayedexpansion
set /a frame=2101
set /a pts=70
for /l %%a in (1,1,21) do (
	md 6_seg
	cd 6_seg
	md !pts!
  	cd ..
	ffmpeg -i .\!videoid!_slice\!pts!.mp4 -vf crop=2880:1440:0:0 .\!videoid!_seg\!pts!\0.mp4		
	set /a pts+=1
)




for /L %%q in (22,5,42) do (
 for /L %%j in (70,1,90) do (
    for /L %%i in (0,1,72) do (
      	x264.exe --qp %%q   --acodec none  -o  .\!videoid!_seg\%%j\%%i_%%q.mp4  .\!videoid!_seg\%%j\%%i.mp4
)
)
)