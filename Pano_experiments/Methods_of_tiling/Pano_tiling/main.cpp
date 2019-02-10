#include <sys/stat.h>
#include <cstdio>
#include <cstring>
#include <algorithm>
#include <iostream>
#include <string>
#include<stdio.h>
#include<stdlib.h>
#include<iostream>
#include<string.h>
#include <cmath>
#include<vector>
#define SIZEX 12
#define SIZEY 24
#define stage1 19
#define CUT 29
#define SumUser 48
//#define cutting_threshold 7
using namespace std;
vector<string> rs;

FILE *fp, *fq;
double MSE[5][SIZEX + 1][SIZEY + 1] , rate[5][SIZEX + 1][SIZEY + 1] , eff[SIZEX + 1][SIZEY + 1];
int QP[48][SIZEX + 1][SIZEY + 1] , QP0[48][SIZEX + 1][SIZEY + 1] , realcut;
double JND[SIZEX + 1][SIZEY + 1] , JND0[SIZEX + 1][SIZEY + 1][SumUser + 1];
double avgtiles = 0 , area = 0 , SC[SIZEX + 1][SIZEY + 1][SIZEX + 1][SIZEY + 1] , SC2[SIZEX + 1][SIZEY + 1][SIZEX + 1][SIZEY + 1] , f[SIZEX + 1][SIZEY + 1][SIZEX + 1][SIZEY + 1][CUT + 1] , g[SIZEX + 1][SIZEY + 1][SIZEX + 1][SIZEY + 1][CUT + 1];
int isfixed[SIZEX + 1][SIZEY + 1][SIZEX + 1][SIZEY + 1] , st1dir[SIZEX + 1][SIZEY + 1][SIZEX + 1][SIZEY + 1] , st1pos[SIZEX + 1][SIZEY + 1][SIZEX + 1][SIZEY + 1];
int viewport[SIZEX + 1][SIZEY + 1][SumUser + 1];
int direction[SIZEX + 1][SIZEY + 1][SIZEX + 1][SIZEY + 1][CUT + 1] , position[SIZEX + 1][SIZEY + 1][SIZEX + 1][SIZEY + 1][CUT + 1] , cutp[SIZEX + 1][SIZEY + 1][SIZEX + 1][SIZEY + 1][CUT + 1];
int direction2[SIZEX + 1][SIZEY + 1][SIZEX + 1][SIZEY + 1][CUT + 1] , position2[SIZEX + 1][SIZEY + 1][SIZEX + 1][SIZEY + 1][CUT + 1] , cutp2[SIZEX + 1][SIZEY + 1][SIZEX + 1][SIZEY + 1][CUT + 1];
int sum , tilemap[SIZEX + 1][SIZEY + 1];

double avgarea , avgbias;

struct mylist{
    int x1 , y1 , x2 , y2;
} myl[1000];

double score(int x1 , int y1 , int x2 , int y2)
{
    if (x1 == x2 && y1 == y2) return 0;
    double avg = 0 , bias = 0;
    for (int i = x1; i <= x2; ++i)
        for (int j = y1; j <= y2; ++j)
            avg += JND[i][j];
    avg = avg / (x2 - x1 + 1) / (y2 - y1 + 1);
    for (int i = x1; i <= x2; ++i)
        for (int j = y1; j <= y2; ++j)
            bias += (JND[i][j] - avg) * (JND[i][j] - avg);
    double dx = x2 - x1 + 1;
    double dy = y2 - y1 + 1;
    double k = max(dx , dy);
    //bias = (bias + 0.00001) * sqrt(max(dx / dy , dy / dx));
    return bias;
}

double score2(int x1 , int y1 , int x2 , int y2)
{
    int res = 0 , flag = 0;
    
    for (int i = 1; i <= 48; ++i) {
        flag = 0;
        for (int j = x1; j <= x2; ++j) {
            for (int k = y1; k <= y2; ++k)
                if (viewport[j][k][i] == 1) {
                    flag = 1;
                    break;
                }
            if (flag) break;
        }
        res += flag * (x2 - x1 + 1) * (y2 - y1 + 1);
    }
    return res;
}

void dp(int x1 , int y1 , int x2 , int y2 , int k)
{
    if (k == 0) {
        f[x1][y1][x2][y2][k] = SC[x1][y1][x2][y2];
        return;
    }
    double tmp;
    f[x1][y1][x2][y2][k] = 1000000;
    for (int i = x1 + 1; i <= x2; ++i)
        for (int k1 = 0; k1 < k; ++k1) {
            tmp = f[x1][y1][i - 1][y2][k1] + f[i][y1][x2][y2][k - 1 - k1];
            if (tmp < f[x1][y1][x2][y2][k]) {
                f[x1][y1][x2][y2][k] = tmp;
                direction[x1][y1][x2][y2][k] = 0;
                position[x1][y1][x2][y2][k] = i;
                cutp[x1][y1][x2][y2][k] = k1;
            }
        }
    for (int i = y1 + 1; i <= y2; ++i)
        for (int k1 = 0; k1 < k; ++k1) {
            tmp = f[x1][y1][x2][i - 1][k1] + f[x1][i][x2][y2][k - 1 - k1];
            if (tmp < f[x1][y1][x2][y2][k]) {
                f[x1][y1][x2][y2][k] = tmp;
                direction[x1][y1][x2][y2][k] = 1;
                position[x1][y1][x2][y2][k] = i;
                cutp[x1][y1][x2][y2][k] = k1;
            }
        }
}

void dp2(int x1 , int y1 , int x2 , int y2 , int k)
{
    if (k == 0) {
        if (isfixed[x1][y1][x2][y2]) g[x1][y1][x2][y2][k] = 1000000; else g[x1][y1][x2][y2][k] = SC2[x1][y1][x2][y2];
        return;
    }
    double tmp;
    g[x1][y1][x2][y2][k] = 1000000;
    for (int i = x1 + 1; i <= x2; ++i) {
        if (isfixed[x1][y1][x2][y2]) {
            if (st1dir[x1][y1][x2][y2] != 0) continue;
            if (st1pos[x1][y1][x2][y2] != i) continue;
        }
        for (int k1 = 0; k1 < k; ++k1) {
            tmp = g[x1][y1][i - 1][y2][k1] + g[i][y1][x2][y2][k - 1 - k1];
            if (tmp < g[x1][y1][x2][y2][k]) {
                g[x1][y1][x2][y2][k] = tmp;
                direction2[x1][y1][x2][y2][k] = 0;
                position2[x1][y1][x2][y2][k] = i;
                cutp2[x1][y1][x2][y2][k] = k1;
            }
        }
    }
    for (int i = y1 + 1; i <= y2; ++i) {
        if (isfixed[x1][y1][x2][y2]) {
            if (st1dir[x1][y1][x2][y2] != 1) continue;
            if (st1pos[x1][y1][x2][y2] != i) continue;
        }
        for (int k1 = 0; k1 < k; ++k1) {
            tmp = g[x1][y1][x2][i - 1][k1] + g[x1][i][x2][y2][k - 1 - k1];
            if (tmp < g[x1][y1][x2][y2][k]) {
                g[x1][y1][x2][y2][k] = tmp;
                direction2[x1][y1][x2][y2][k] = 1;
                position2[x1][y1][x2][y2][k] = i;
                cutp2[x1][y1][x2][y2][k] = k1;
            }
        }
    }
}

void maketile()
{
    for (int i = 0; i < SIZEX; ++i)
        for (int j = 0; j < SIZEY; ++j)
            for (int i1 = i; i1 < SIZEX; ++i1)
                for (int j1 = j; j1 < SIZEY; ++j1) {
                    SC2[i][j][i1][j1] = score(i , j , i1 , j1);
                    SC[i][j][i1][j1] = score2(i , j , i1 , j1);
                }
    for (int dx = 0; dx < SIZEX; ++dx)
        for (int dy = 0; dy < SIZEY; ++dy)
            for (int i = 0; i + dx < SIZEX; ++i)
                for (int j = 0; j + dy < SIZEY; ++j)
                    for (int k = 0; k <= stage1; ++k)
                        dp(i , j , i + dx , j + dy , k);
}

void tiling(int x1 , int y1 , int x2 , int y2 , int k)
{
    if (k == 0) {
        /*myl[++sum].x1 = x1 + 1;
        myl[sum].y1 = y1 + 1;
        myl[sum].x2 = x2 + 1;
        myl[sum].y2 = y2 + 1;
        for (int i = x1; i <= x2; ++i)
            for (int j = y1; j <= y2; ++j)
                tilemap[i][j] = sum;*/
        return;
    }
    isfixed[x1][y1][x2][y2] = 1;
    st1dir[x1][y1][x2][y2] = direction[x1][y1][x2][y2][k];
    st1pos[x1][y1][x2][y2] = position[x1][y1][x2][y2][k];
    if (direction[x1][y1][x2][y2][k] == 0) {
        tiling(x1 , y1 , position[x1][y1][x2][y2][k] - 1 , y2 , cutp[x1][y1][x2][y2][k]);
        tiling(position[x1][y1][x2][y2][k] , y1 , x2 , y2 , k - 1 - cutp[x1][y1][x2][y2][k]);
    } else {
        tiling(x1 , y1 , x2 , position[x1][y1][x2][y2][k] - 1 , cutp[x1][y1][x2][y2][k]);
        tiling(x1 , position[x1][y1][x2][y2][k] , x2 , y2 , k - 1 - cutp[x1][y1][x2][y2][k]);
    }
}

void maketile2()
{
    for (int dx = 0; dx < SIZEX; ++dx)
        for (int dy = 0; dy < SIZEY; ++dy)
            for (int i = 0; i + dx < SIZEX; ++i)
                for (int j = 0; j + dy < SIZEY; ++j)
                    for (int k = 0; k <= CUT; ++k)
                        dp2(i , j , i + dx , j + dy , k);
}

void tiling2(int x1 , int y1 , int x2 , int y2 , int k)
{
    if (k == 0) {
        myl[++sum].x1 = x1 + 1;
        myl[sum].y1 = y1 + 1;
        myl[sum].x2 = x2 + 1;
        myl[sum].y2 = y2 + 1;
        for (int i = x1; i <= x2; ++i)
            for (int j = y1; j <= y2; ++j)
                tilemap[i][j] = sum;
        return;
    }
    

    if (direction2[x1][y1][x2][y2][k] == 0) {
        tiling2(x1 , y1 , position2[x1][y1][x2][y2][k] - 1 , y2 , cutp2[x1][y1][x2][y2][k]);
        tiling2(position2[x1][y1][x2][y2][k] , y1 , x2 , y2 , k - 1 - cutp2[x1][y1][x2][y2][k]);
    } else {
        tiling2(x1 , y1 , x2 , position2[x1][y1][x2][y2][k] - 1 , cutp2[x1][y1][x2][y2][k]);
        tiling2(x1 , position2[x1][y1][x2][y2][k] , x2 , y2 , k - 1 - cutp2[x1][y1][x2][y2][k]);
    }
}

void print()
{
    //double area = 0;
    for (int i = 1; i <= CUT + 1; ++i) {
        fprintf(fq , "%d %d %d %d %d %d\n" , i , myl[i].x1 , myl[i].x2 , myl[i].y1 , myl[i].y2 , 42);
        avgbias += SC2[myl[i].x1-1][myl[i].y1-1][myl[i].x2-1][myl[i].y2-1];
        avgarea += SC[myl[i].x1-1][myl[i].y1-1][myl[i].x2-1][myl[i].y2-1];
    }
    //printf("%.2lf\n" , area);
    /*for (int i = 0; i < SIZEX; ++i) {
        for (int j = 0; j < SIZEY; ++j)
            fprintf(fq , "%d " , tilemap[i][j]);
        fprintf(fq , "\n");
    }*/
}

void read_write()
{
    int flag;
    fp = NULL;//输入
    fq = NULL;//输出
    avgarea = avgbias = 0;
    char filename_body[10];
    for (int set=1; set<=2; ++set)
    for (int video = 0; video <= 8; ++video) {
        //if (video == 5) continue;
        for(int i = 1; i <= 12001; i += 30) {
            flag = 0;
            for (int user = 1; user <= 48; ++user) {
                //char filename1[50] = "set5/";
                char filename1[100] = "ratio/";
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"%d",set);
                strcat(filename1,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"/");
                strcat(filename1,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"%d",video);
                strcat(filename1,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"/");
                strcat(filename1,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"%d",user);
                strcat(filename1,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"/");
                strcat(filename1,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"%d",i);
                strcat(filename1,filename_body);
                char filename_tail1[50] = "_VRJND_Value_PMSE.txt";
                strcat(filename1,filename_tail1);
                if((fp=fopen(filename1, "r")) == NULL)
                {
                    cout<<"open filename fail..."<<endl;
                    continue;
                }
                flag = 1;
                for (int j = 0; j < SIZEX; ++j) {
                    for (int k = 0; k < SIZEY; ++k) {
                        fscanf(fp,"%lf" , &JND0[j][k][user]);
                        if (isnan(JND0[j][k][user])) JND0[j][k][user] = 200000;
                        //if (JND0[j][k][user] < 0.01) JND0[j][k][user] = 0.01;
                        ++JND0[j][k][user];
                        if (JND0[j][k][user] > 200000) JND0[j][k][user] = 200000;
                        JND0[j][k][user] = log10(JND0[j][k][user]);
                        //if (JND[j][k] < 0.0001) JND[j][k] = -10;
                    }
                }
                fclose(fp);
                
                char filename2[100] = "viewport/";
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"%d",set);
                strcat(filename2,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"/");
                strcat(filename2,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"%d",video);
                strcat(filename2,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"/");
                strcat(filename2,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"%d",user);
                strcat(filename2,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"/realviewport_");
                strcat(filename2,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"%d",i);
                strcat(filename2,filename_body);
                char filename_tail2[50] = ".txt";
                strcat(filename2,filename_tail2);
                if((fp=fopen(filename2, "r")) == NULL)
                {
                    cout<<"open filename fail..."<<endl;
                    cout << filename2 << endl;
                    continue;
                }
                flag = 1;
                for (int j = 0; j < SIZEX; ++j)
                    for (int k = 0; k < SIZEY; ++k)
                        fscanf(fp,"%d" , &viewport[j][k][user]);
                fclose(fp);
            }
            if (flag == 0) continue;
            
            for (int j = 0; j < SIZEX; ++j)
                for (int k = 0; k < SIZEY; ++k) {
                    //sort(JND0[j][k] + 1 , JND0[j][k] + SumUser + 1);
                    //JND[j][k] = JND0[j][k][SumUser >> 1];
                    JND[j][k] = 0;
                    for (int user = 1; user <= SumUser; ++user)
                        JND[j][k] += JND0[j][k][user];
                    JND[j][k] /= SumUser;
                }
            
            sum = 0;
            memset(isfixed , 0 , sizeof(isfixed));
            maketile();
            /*for (int j = 1; j <= stage1; ++j)
                printf("%.2lf %.2lf\n" , f[0][0][SIZEX - 1][SIZEY - 1][j] , SC[0][0][SIZEX - 1][SIZEY - 1]);*/
            memset(tilemap , 0 , sizeof(tilemap));
            tiling(0 , 0 , SIZEX - 1 , SIZEY - 1 , stage1);
            maketile2();
            tiling2(0 , 0 , SIZEX - 1 , SIZEY - 1 , CUT);
            
            
            char dir[100] = "tiling0121/";
            char dir_name[100];
            memset(dir_name, 0, sizeof(dir_name));
            sprintf(dir_name,"%d",set);
            strcat(dir,dir_name);
            memset(dir_name, 0, sizeof(dir_name));
            sprintf(dir_name,"/");
            strcat(dir,dir_name);
            memset(dir_name, 0, sizeof(dir_name));
            sprintf(dir_name,"%d",video);
            strcat(dir,dir_name);
            memset(dir_name, 0, sizeof(dir_name));
            sprintf(dir_name,"/");
            strcat(dir,dir_name);
            memset(dir_name, 0, sizeof(dir_name));
            sprintf(dir_name,"%d",i);
            strcat(dir,dir_name);
            
            int status = mkdir(dir, S_IRWXU | S_IRWXG | S_IROTH | S_IXOTH);
            printf("%d\n" , i);
            
            for (int k = 1; k <= 42; ++k) {
                char outputfile[100] = "tiling0121/";
                char outputfile_body[100];
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"%d",set);
                strcat(outputfile,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"/");
                strcat(outputfile,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"%d",video);
                strcat(outputfile,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"/");
                strcat(outputfile,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"%d",i);
                strcat(outputfile,filename_body);
                memset(filename_body, 0, sizeof(filename_body));
                sprintf(filename_body,"/");
                strcat(outputfile,filename_body);
                memset(outputfile_body, 0, sizeof(outputfile_body));
                sprintf(outputfile_body,"%d",k);
                strcat(outputfile, outputfile_body);
                char outputfile_tail[100] = ".txt";
                strcat(outputfile,outputfile_tail);
                
                if((fq=fopen(outputfile, "w")) == NULL)
                {
                    cout<<"open outputfile fail..."<<endl;
                }
                print();
                
                /*for (int j = 0; j < SIZEX; ++j) {
                 for (int k = 0; k < SIZEY; ++k)
                 fprintf(fq , "%.2lf " , JND[j][k]);
                 fprintf(fq , "\n");
                 }*/
                
                fclose(fq);
            }
        }
    }
    printf("%.2lf %.2lf\n" , avgbias , avgarea);
}

int main()
{
    read_write();
}
