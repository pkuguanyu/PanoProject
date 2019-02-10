
for num=21:48
    for j=0:8
        figure
        xlim([0 1])
        ylim([0,1])
        xlabel('U')
        ylabel('V')
        title('Viewpoint distribution diagram')
        m=readtable(['C:/Aiqiyi/expdata/Experiment_1/',num2str(num),'/video_',num2str(j),'.csv']);
        quan_list=m(1:end,3:6);
        [x,y]=size(quan_list)
        for i=1:x
              q1=quan_list(i,:);
              q2=table2array(q1);
              tempxyz=[2*q2(1)*q2(3)+2*q2(2)*q2(4);2*q2(2)*q2(3)-2*q2(1)*q2(4);1-2*q2(1)*q2(1)-2*q2(2)*q2(2)];
              r=1.0;
              if tempxyz(3)>0 & tempxyz(1)>0
                 U=atan(tempxyz(3) / tempxyz(1));
              end
              if tempxyz(3)>0 & tempxyz(1)<0
                  U=atan(tempxyz(3) / tempxyz(1)) +pi;
              end
              if tempxyz(3)<0 & tempxyz(1)<0
                  U=atan(tempxyz(3) / tempxyz(1))+pi;
              end
              if tempxyz(3)<0 & tempxyz(1)>0
                  U=atan(tempxyz(3) / tempxyz(1))+pi*2;
              end
              U=U/(2*pi);
              V=acos(tempxyz(2) / r) / pi;
              hold on
              plot(U, V, 'r+')
              hold on
        end
        saveas(gcf,['C:/Aiqiyi/expdata/graph/',num2str(num),'/' ,num2str(j) , '.png'])
    end
end



