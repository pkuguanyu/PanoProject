/* The member variables of MPDdata represent all the data of one MPD.
 * Function getServerData uses the pre-calculatede data stored as ".mat" file to load a MPDdata object.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MPD
{
    public class MPDdata
    {
        public static string matPath = "D://mat";
        //public int nChunk, nTile, nObject;
        //public int nLumi=5, nDiff=5, nSpeed=5;
        public byte[] tilePosition; //nChunk * nTile * 2
        public byte[] tileLumi; //nChunk * nTile
        public byte[] tileDoF; //nChunk * nTile
        public byte[] objectTraj; //nChunk * nObject * 3 * 2
        public byte[] lookupTable; //nChunk * nTile * 2
        public MPDdata(int nChunk, int nTile, int nObject)
        {
            this.nTile = nTile;
            this.nObject = nObject;
            tiles = new Tile[nTile];
            objects = new Object[nObject];
        }
        //在服务器上，给定videoID和sec，根据PSPNR等数据生成一个MPDdata
        public static MPDdata getServerData(int videoID)
        {
            //TODO
            MPDdata newData = new MPDdata(30, 10);
            try
            {
                string vidMatPath = matPath + "/" + videoID.ToString("D4");
                using (FileStream fs = new FileStream(vidMatPath + "/tilePosition.mat", FileMode.Open, FileAccess.Read))
                {
                    newData.tilePosition = new byte[fs.Length];
                    fs.Read(newData.tilePosition, 0, (int)fs.Length);
                }
                using (FileStream fs = new FileStream(vidMatPath + "/tileLumi.mat", FileMode.Open, FileAccess.Read))
                {
                    newData.tileLumi = new byte[fs.Length];
                    fs.Read(newData.tileLumi, 0, (int)fs.Length);
                }
                using (FileStream fs = new FileStream(vidMatPath + "/tileDoF.mat", FileMode.Open, FileAccess.Read))
                {
                    newData.tileDoF = new byte[fs.Length];
                    fs.Read(newData.tileDoF, 0, (int)fs.Length);
                }
                using (FileStream fs = new FileStream(vidMatPath + "/objectTraj.mat", FileMode.Open, FileAccess.Read))
                {
                    newData.objectTraj = new byte[fs.Length];
                    fs.Read(newData.objectTraj, 0, (int)fs.Length);
                }
                using (FileStream fs = new FileStream(vidMatPath + "/lookupTable.mat", FileMode.Open, FileAccess.Read))
                {
                    newData.lookupTable = new byte[fs.Length];
                    fs.Read(newData.lookupTable, 0, (int)fs.Length);
                }

                return newData;
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
