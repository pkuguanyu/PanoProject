function [result]=CalParonomicAngle(C,V)

if(V(1,2)<=839)
    if(C(1,2)>=2880-839) %需要镜像一波
        C(1,2)=0-(2880-C(1,2));
        E=[1,0];
        L=V-C;
        if(L(1,1)<0)
            result=2*pi-acos(dot(L,E)/(norm(L)*norm(E)));
            return;
        else
            result=acos(dot(L,E)/(norm(L)*norm(E)));
            return;
        end
    else
        E=[1,0];
        L=V-C;
        if(L(1,1)<0)
            result=2*pi-acos(dot(L,E)/(norm(L)*norm(E)));
            return;
        else
            result=acos(dot(L,E)/(norm(L)*norm(E)));
            return;
        end
    end
end


if(V(1,2)>=2880-839)
    if(C(1,2)<=839) %需要镜像一波
        C(1,2)=2880+C(1,2);
        E=[1,0];
        L=V-C;
        if(L(1,1)<0)
            result=2*pi-acos(dot(L,E)/(norm(L)*norm(E)));
            return;
        else
            result=acos(dot(L,E)/(norm(L)*norm(E)));
            return;
        end
    else
        E=[1,0];
        L=V-C;
        if(L(1,1)<0)
            result=2*pi-acos(dot(L,E)/(norm(L)*norm(E)));
            return;
        else
            result=acos(dot(L,E)/(norm(L)*norm(E)));
            return;
        end
    end
end

E=[1,0];
L=V-C;
if(L(1,1)<0)
    result=2*pi-acos(dot(L,E)/(norm(L)*norm(E)));
    return;
else
    result=acos(dot(L,E)/(norm(L)*norm(E)));
    return;
end

end