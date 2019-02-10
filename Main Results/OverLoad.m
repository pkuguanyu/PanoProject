clear all;
close all;





ratio1=[0.79,0.28,0.2,0.04]; %½âÂë äÖÈ¾ ÍøÂç PSPNR
ratio2=[0.9,0.34,0.2,0.01]; %½âÂë äÖÈ¾ ÍøÂç PSPNR
h=figure,
set(h,'position',[100,100,450,600])
h1=axes;
bar1=bar([30*ratio2; 30*ratio1] ,'stack','barwidth',0.6,'edgecolor',[1,1,1]);

set(bar1(4),'DisplayName','Decoding',...
     'FaceColor',[0 0.447058826684952 0.74117648601532],...
    'EdgeColor','none');
set(bar1(3),'DisplayName','Rendering',...
    'DisplayName','Downloading','FaceColor',[1 0.843137264251709 0],...
    'EdgeColor','none');
set(bar1(2),'FaceColor',[0.466666668653488 0.674509823322296 0.18823529779911],...
    'EdgeColor','none');
set(bar1(1),'DisplayName','PSPNR computation',...
    'FaceColor',[0.87058824300766 0.490196079015732 0],...
    'EdgeColor','none');

xticks(0:2)
set(gca,'XTicklabel',{'Pano','Baseline'});
xlim([0.5,2.5])
ylim([0,60]);
ylabel( 'CPU usage (%)');
set(gca,'FontSize',30);
set(gca,'box','off');
set(gca,'linewidth',1.5);
set(gcf,'color',[1 1 1])
legend('Decoding','Rendering','Downloading','Quality adaptation','Location','NorthEast');

h2=legend;
set(h2,'box','off')
Position=h1.Position;
eff=0
annotation('arrow',[Position(1) Position(1)],[Position(2)+eff Position(2)+Position(4)+eff+0.05],'linewidth',3);
annotation('arrow',[Position(1) Position(1)+Position(3)+0.05],[Position(2)+eff Position(2)+eff],'linewidth',3);
%hgexport(gcf,'CPUworkload.eps');



clear all;
ratio1=[0.8,0.7,0.075]; %loading player ¼ÓÔØµÚÒ»¸ötrunk  fetching mpd
ratio2=[0.75,0.35,0.21];  %loading player ¼ÓÔØµÚÒ»¸ötrunk  fetching mpd
h=figure,
set(h,'position',[100,100,450,600])
h1=axes;
bar1=bar([ratio1; ratio2] ,'stack','barwidth',0.6,'edgecolor',[1,1,1]);

% set(bar1(3),'DisplayName','Encoding',...
%     'FaceColor',[0.87058824300766 0.490196079015732 0]);
% set(bar1(2),'DisplayName','PSPNR fitting',...
%     'FaceColor',[0.466666668653488 0.674509823322296 0.18823529779911]);
% set(bar1(1),'DisplayName','Tracking object',...
%     'FaceColor',[0 0.447058826684952 0.74117648601532]);

set(bar1(3),'DisplayName','Loading the player',...
     'FaceColor',[0 0.447058826684952 0.74117648601532],...
    'EdgeColor','none');
set(bar1(2),'DisplayName','Loading the 1st chunk',...
    'FaceColor',[0.466666668653488 0.674509823322296 0.18823529779911],...
    'EdgeColor','none');
set(bar1(1),'DisplayName','Fetching MPD file',...
    'FaceColor',[0.87058824300766 0.490196079015732 0],...
    'EdgeColor','none');

xticks(0:2)
set(gca,'XTicklabel',{'Pano','Baseline'});
xlim([0.5,2.5])
ylim([0,2]);
ylabel( 'Time (s)');
set(gca,'FontSize',30);
set(gca,'box','off');
set(gca,'linewidth',1.5);
set(gcf,'color',[1 1 1])
legend({'Loading the player','Loading the 1st chunk','Fetching MPD file'},'Fontsize',22,'NorthEast');
h2=legend;
set(h2,'box','off')
Position=h1.Position;
eff=0
annotation('arrow',[Position(1) Position(1)],[Position(2)+eff Position(2)+Position(4)+eff+0.05],'linewidth',3);
annotation('arrow',[Position(1) Position(1)+Position(3)+0.05],[Position(2)+eff Position(2)+eff],'linewidth',3);
%hgexport(gcf,'StartUpTime.eps');

clear all;
ratio1=[13.7,2.04]; %±àÂë£¬PSPNR¼ÆËã,¸ú×Ù
ratio2=[12.2,0];  %±àÂë£¬PSPNR¼ÆËã,¸ú×Ù
h=figure,
set(h,'position',[100,100,450,600])
h1=axes;
bar1=bar([ratio2; ratio1] ,'stack','barwidth',0.6,'edgecolor',[1,1,1]);

set(bar1(2),'DisplayName','PSPNR fitting',...
    'FaceColor',[0.466666668653488 0.674509823322296 0.18823529779911],...
    'EdgeColor','none');
set(bar1(1),'DisplayName','Tracking object',...
    'FaceColor',[0.87058824300766 0.490196079015732 0],...
    'EdgeColor','none');

xticks(0:2)
set(gca,'XTicklabel',{'Pano','Baseline'});
xlim([0.5,2.5])
ylim([0,20]);
ylabel( 'Time (min)');
set(gca,'FontSize',30);
set(gca,'box','off');
set(gca,'linewidth',1.5);
set(gcf,'color',[1 1 1])
legend('Encoding','PSPNR fitting','Tracking objects','NorthEast');
h2=legend;
set(h2,'box','off')
Position=h1.Position;
eff=0
annotation('arrow',[Position(1) Position(1)],[Position(2)+eff Position(2)+Position(4)+eff+0.05],'linewidth',3);
annotation('arrow',[Position(1) Position(1)+Position(3)+0.05],[Position(2)+eff Position(2)+eff],'linewidth',3);



