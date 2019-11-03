
/*
* This function is used to map the YUV format frame into the corresponding 2D texture in Unity engine.
* The Copy function in Marshal is used to copy data of IntPtr format to data of array format.
* And the 2D texture is spread evenly on the sphere, where the users finally watch.
*/


void GetTexture_Frame(byte* Y, int YL, byte* U, int UL, byte* V, int VL, int Width, int Height)
{

        Marshal.Copy((IntPtr)Y, Y_raw, 0, Width * Height);
        Marshal.Copy((IntPtr)U, U_raw, 0, Width * Height / 4);
        Marshal.Copy((IntPtr)V, V_raw, 0, Width * Height / 4);
        texY.LoadRawTextureData(Y_raw);
        texY.Apply();

        texU.LoadRawTextureData(U_raw);
        texU.Apply();

        texV.LoadRawTextureData(V_raw);
        texV.Apply();

        yuvm.mainTexture = texY;
        yuvm.SetTexture("_MainTexU", texU);
        yuvm.SetTexture("_MainTexV", texV);

}