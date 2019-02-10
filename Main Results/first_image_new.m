function first_image_new()
clear all;
bandwidth = 8000;
ana = video_ana()
fix(clock) 
turns = 1;
videos = 10;
number_of_delta = 8;
%stdx = [500,1000,2000,3000,4000];
stdx = [2000];
sz = size(stdx);
min_buffer_B = [20,15,10,8,6,4,2,0];
%min_buffer_B = [0,0,0,0,0,0,0,0,0];
delta = [-0.2,-0.15,-0.1,-0.05,0,0.1,0.2,0.25,0.275];

x0 = zeros(sz(2)*turns*videos,2);
y0 = zeros(sz(2)*turns*videos,2);
z0 = zeros(sz(2)*turns*videos,2);
x = zeros(number_of_delta,2);
y = zeros(number_of_delta,2);
z = zeros(number_of_delta,2);

n = 0;
for dt = 1:number_of_delta
    m = 0;
    for i=1:sz(2)
        for j=1:turns
            for k=1:10
                m = m + 1;
                [x0(m,2),tmp,x0(m,1)] = double_buffer('distance/',bandwidth * ana(1,k) / ana(1,1),stdx(1,i) * sqrt(ana(1,k) / ana(1,1)),k,min_buffer_B(dt));
                [y0(m,2),tmp,y0(m,1)] = double_buffer('VR/',bandwidth * ana(1,k) / ana(1,1),stdx(1,i) * sqrt(ana(1,k) / ana(1,1)),k,min_buffer_B(dt));
                [z0(m,2),tmp,z0(m,1)] = double_buffer('mono/',bandwidth * ana(1,k) / ana(1,1),stdx(1,i) * sqrt(ana(1,k) / ana(1,1)),k,min_buffer_B(dt));
            end
        end
    end
    x(dt,1) = mean(x0(:,1));
    x(dt,2) = mean(x0(:,2));
    y(dt,1) = mean(y0(:,1));
    y(dt,2) = mean(y0(:,2));
    z(dt,1) = mean(z0(:,1));
    z(dt,2) = mean(z0(:,2));
end
        
x(:,1) = x(:,1) .* 100;
y(:,1) = y(:,1) .* 100;
z(:,1) = z(:,1) .* 100;
x
y
z
plot(z(:,1),z(:,2),x(:,1),x(:,2),y(:,1),y(:,2),'LineWidth',2.5);

%[p1,p2,p3,p4] = ConfidenceRegion('b+',x,0.25)
%hold on
%[p5,p6,p7,p8] = ConfidenceRegion('r+',y,0.25)
xlabel( 'Stalling Ratio (%)' , 'FontSize',20);
ylabel( 'User-Perceived Quality (PSPNR)' , 'FontSize',20);
%legend([p1,p5],'Viewpoint driven','Pano','Location','NorthEast');
legend('Monolithic','Viewpoint driven','Pano','Location','NorthEast');
output = [x,y,z];
save('1st_image_output_8000.mat','output');
hgexport(gcf,'first_image_8000.eps');
fix(clock) 
