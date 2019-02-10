# -*-conding:utf-8 -*-
import json
import sys
import os

videoid=sys.argv[1]
name=sys.argv[2]
duration=1
row=12
col=24
qp=22
se='set3'

for i in range(0,row):
    print i
    for j in range(0,col):
        print j
        for qp in range(22,47,5):
            path = 'J:/json/'+se+'/'+str(videoid)+'/12_24_json/'
            filepath = path + name+'_'+ str(i*120)+ '_'+str(j*120)+ '_'+ str(qp)+ '.json'
            outputpath = 'J:/txt/'+se+'/'+str(videoid)+'/12_24/'
            isExists=os.path.exists(outputpath)
            if not isExists: 
                os.makedirs(outputpath)
            f =open(filepath)
            test = json.load(f)
            startFrame = 0
            endFrame = duration
            sumSize = 0
            bitrate = []
            for t in test['frames']:
                if t['media_type'] == 'video':
                    pts = float(t['pkt_pts_time'])
                    size = float(t['pkt_size'])/ 1024

                    if pts <= endFrame and pts >= startFrame:
                        sumSize = sumSize + size
                    else:
                        sumSize = sumSize / 1
                        bitrate.append(sumSize)
                        sumSize = 0
                        startFrame = startFrame + duration
                        endFrame = endFrame + duration

            for idx in range(0, len(bitrate)):
                fobj = open(outputpath + 'duration=' + str(duration) + '_idx='+str(idx)+ '_qp='+str(qp)+ '.txt','a')
                fobj.write(str(bitrate[idx]) + '\r\n')
            fobj.close()

