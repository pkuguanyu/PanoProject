function expimprovement()
clear all;
mark = '';
[x1,y1,z1] = solidimprovement('distance/',mark);
[x2,y2,z2] = solidimprovement('WithoutVR/',mark);
[x3,y3,z3] = solidimprovement('VR/',mark);
[x4,y4,z4] = solidimprovement('Theory/',mark);
x1 = x1 ./ 1;
x2 = x2 ./ 10;
x3 = x3 ./ 10;
x4 = x4 ./ 10 ./ 1.1;
dx = x1(1) - x2(1);
dy = y1(1) - y2(1);
x5 = x3 + dx;
y5 = y3 + dy;
z5 = z3;
x2 = x2 + dx;
y2 = y2 + dy;

errorbar(x1(1:10),y1(1:10),z1(1:10),'LineWidth',2.5); 
hold on
errorbar(x2(1:10),y2(1:10),z2(1:10),'LineWidth',2.5);
 hold on
 errorbar(x5(1:10),y5(1:10),z5(1:10),'LineWidth',2.5); 
hold on
errorbar(x3(1:10),y3(1:10),z3(1:10),'LineWidth',2.5);
 hold on
errorbar(x4(1:10),y4(1:10),z4(1:10),'LineWidth',2.5);
%plot(x1(1:10),y1(1:10),x2(1:10),y2(1:10),x4(1:10),y4(1:10),'LineWidth',2.5);
xlabel( 'Bandwidth (kb/s)' , 'FontSize',20);
%xlim([350,1800]);
ylabel( 'PSPNR (dB)' , 'FontSize',20); 
set(gca,'FontSize',15);
legend({'Viewpoint driven','PQVRS','PQVRS+','PQVRS++','Theory'},'Location','SouthEast');
%legend({'Viewpoint driven','Perceived-Quality driven','Perceived-Quality driven (3 new factors)'},'Location','SouthEast');
grid on;
hgexport(gcf,['practical_improvement',mark,'.eps']);
output = [x1(1:10);y1(1:10);x2(1:10);y2(1:10);x5(1:10);y5(1:10);x3(1:10);y3(1:10);x4(1:10);y4(1:10)];
save('component_wise_improvement.mat','output');