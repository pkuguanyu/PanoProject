/* Update_Tile function is used to merge the YUV format frame of each tile into a complete frame.
*  We use the "copy by line" method to merge all the tile. First, we allocate a meomory space of a complete black frame.
*  The real data of the complete frame is copied by line from each tile.
*  The CheckTile function is used to calculate which tile belongs to the corresponding line.
*  MemoryCopy function is optimized by compiler instruction which can merge tile within 1ms.
*/


void Update_Tile(ref AVFrame pDstFrame, AVFrame*[] Cur, Tile[] tile, int length, int width, int height) //width 和 height是整个画面的
{

        int temp_length, i, j;
        int temp_width = 0;
    
        for (i = 0; i < height; i++)   //这个是每一行
        {
            temp_width = 0;
            CheckTile(tile, length, i, ref temp, out temp_length, ref tileIndex);
            for (j = 0; j < temp_length; j++)
            {

                temp_width = tile[temp[j]].lefttop.x;
                CopyMemory((IntPtr)(pDstFrame.data[0] + temp_width + i * pDstFrame.linesize[0]), (IntPtr)(Cur[temp[j]]->data[0] + tileIndex[j] * Cur[temp[j]]->linesize[0]), (uint)Cur[temp[j]]->linesize[0]);
            }
        }
        
        for (i = 0; i < height / 2; i++)
        {
            temp_width = 0;
            CheckTile_UV(tile, length, i, ref temp, out temp_length, ref tileIndex);
            for (j = 0; j < temp_length; j++)
            {
                temp_width = tile[temp[j]].lefttop.x/2;
                CopyMemory((IntPtr)(pDstFrame.data[1] + temp_width + i * pDstFrame.linesize[1]), (IntPtr)(Cur[temp[j]]->data[1] + tileIndex[j] * Cur[temp[j]]->linesize[1]), (uint)Cur[temp[j]]->linesize[1]);
            }

            temp_width = 0;
            for (j = 0; j < temp_length; j++)
            {
                temp_width = tile[temp[j]].lefttop.x /2;
                CopyMemory((IntPtr)(pDstFrame.data[2] + temp_width + i * pDstFrame.linesize[2]), (IntPtr)(Cur[temp[j]]->data[2] + tileIndex[j] * Cur[temp[j]]->linesize[2]), (uint)Cur[temp[j]]->linesize[2]);
            }

        }
}

void CheckTile(Tile[] tile, int length, int row, ref int[] temp, out int temp_length, ref int[] tileIndex) //用来确定当前行有多少个tile
{
        int i;
        int k = 0;
        for (i = 0; i < length; i++)
        {
            if (tile[i].lefttop.y <= row && row <= tile[i].rightdown.y)
            {

                tileIndex[k] = row - tile[i].lefttop.y;
                temp[k++] = i; //这些tile在当前行中		
            }

        }
        temp_length = k;
}

void CheckTile_UV(Tile[] tile, int length, int row, ref int[] temp, out int temp_length, ref int[] tileIndex) //用来确定当前行有多少个tile
{
        int i;
        int k = 0;
        for (i = 0; i < length; i++)
        {
            if (tile[i].lefttop.y / 2 <= row && row < tile[i].rightdown.y / 2)
            {

                tileIndex[k] = (row - tile[i].lefttop.y / 2);
                temp[k++] = i; //这些tile在当前行中		
            }

        }
        temp_length = k;
}