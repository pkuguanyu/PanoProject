function PlotFigure18()
clear all;
close all;

name = 'component_wise_improvement.mat';
a = cell2mat(struct2cell(load(name)));
x1=a(1,:);
y1=a(2,:);
x2=a(3,:);
y2=a(4,:);
x3=a(5,:);
y3=a(6,:);
x4=a(7,:);
y4=a(8,:);
x5=a(9,:);
y5=a(10,:);

h1=axes;

plot(x1,y1,...
    'Color',[255.0 128.0 64.0]./255,...
    'LineWidth',3);
hold on;

plot(x2,y2,...
    'Color',[0.274509817361832 0.560784339904785 0.823529422283173],...
    'LineStyle',':',...
    'LineWidth',3);
hold on;

plot(x3,y3,...
    'Color',[0.274509817361832 0.560784339904785 0.823529422283173],...
    'LineStyle','--',...
    'LineWidth',3);
hold on;

plot(x4,y4,...
    'Color',[0.274509817361832 0.560784339904785 0.823529422283173],...
    'LineWidth',3);
hold on;

plot(x5,y5,...
    'Color','k',...
    'LineWidth',3);
hold on;

plot([650,650],[45,80],...
    'Color',[0.4 0.4 0.4],...
    'LineWidth',3);
hold on;

set(gcf,'color',[1 1 1])
set(gca,'FontSize',18)
set(gca,'box','off') 
set(gca, 'LineWidth',2)
%xlim([0,100]);
%ylim([42,75]);
xlabel('Bandwidth Consumption (kb/s)')
ylabel('PSPNR')


legend('Location','best')
legend({'Basic Viewport-driven','Pano (traditional PSPNR)','Pano (PSPNR w/ 360JND))','Pano (full)','Theoretical optimal'})
h2=legend;
set(h2,'box','off')



Position=h1.Position;
annotation('arrow',[Position(1) Position(1)],[Position(2) Position(2)+Position(4)+0.05],'linewidth',2);
annotation('arrow',[Position(1) Position(1)+Position(3)+0.05],[Position(2) Position(2)],'linewidth',2);

hgexport(gcf,'component_wise_improvement.eps');