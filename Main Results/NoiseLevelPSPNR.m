
clear all;
close all;
base=[58,54,47,42];
Noise_Level=[5,20,40,80];
PSPNR_FLARE=base;
PSPNR_PANO=[72,69,63,53];
Var1=[2.0,1.8,1.5,1.4];
Var2=[1.7,1.5,1.3,1.2];
h1=axes;
plot(Noise_Level,PSPNR_PANO,...
    'MarkerFaceColor',[0.274509817361832 0.560784339904785 0.823529422283173],...
    'MarkerSize',10,...
    'Marker','o',...
    'LineWidth',5,...
    'Color',[0.27 0.55 0.82]);
hold on

plot(Noise_Level,PSPNR_FLARE,...
    'Color',[255.0 128.0 64.0]./255,...
    'MarkerFaceColor',[255.0 128.0 64.0]./255,...
    'MarkerSize',10,...
    'Marker','o',...
    'LineStyle','--',...
    'LineWidth',5)


for i=1:4
    errorbar(Noise_Level(i),PSPNR_FLARE(i),Var1(i),'Color',[0 0 0]./255,'LineWidth',2)  
    hold on
    errorbar(Noise_Level(i),PSPNR_PANO(i),Var2(i),'Color',[0 0 0]./255,'LineWidth',2)  
end


set(gcf,'color',[1 1 1])
set(gca,'FontSize',25)
set(gca,'box','off') 
set(gca, 'LineWidth',1.5)
xlim([0,80]);
ylim([40,75]);
xlabel('Noise level (deg)')
ylabel('PSPNR')

legend('Location','northeast')
legend({'Pano','Viewport-driven'})
h2=legend;
set(h2,'box','off')



Position=h1.Position;
annotation('arrow',[Position(1) Position(1)],[Position(2) Position(2)+Position(4)+0.05],'linewidth',1.5);
annotation('arrow',[Position(1) Position(1)+Position(3)+0.05],[Position(2) Position(2)],'linewidth',1.5);

