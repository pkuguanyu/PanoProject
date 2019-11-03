/* Function requestMPDdata calls WebClient.HttpDownload to download XML file,
 * then it calls fucntion readXml to extract MPDdata from XML.
 */

using System;
using System.Text;
using System.Xml;

namespace MPD
{
    public class MPDclient
    {
        private static string path = "D:\\ClientRoot";
        private static string host = "127.0.0.1:8765";

        public MPDclient()
        {

        }
        public static MPDdata requestMPDdata(int videoID)
        {
            string fileName = videoID.ToString("D4") + ".xml";
            if(WebClient.HttpDownload(host + fileName, path))
            {
                return readXml(path + "/" + fileName);
            }
            else
            {
                return null;
            }
        }
        public static MPDdata readXml(string XmlPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (!System.IO.File.Exists(XmlPath))
            {
                return null;
            }
            xmlDoc.Load(XmlPath);
            //从Xml提取到data
            XmlNode nodeMPD = xmlDoc.SelectSingleNode("MPD");
            XmlNode nodeTilePosition = nodeMPD.SelectSingleNode("tilePosition");
            XmlNode nodeTileLumi = nodeMPD.SelectSingleNode("tileLumi");
            XmlNode nodeTileDoF = nodeMPD.SelectSingleNode("tileDoF");
            XmlNode nodeObjectTraj = nodeMPD.SelectSingleNode("objectTraj");
            XmlNode nodeLookupTable = nodeMPD.SelectSingleNode("lookupTable");
            MPDdata data = new MPDdata();
            data.tilePosition = StringToUTF8ByteArray(((XmlCDataSection)nodeTilePosition).InnerText.Trim());
            data.tileLumi = StringToUTF8ByteArray(((XmlCDataSection)nodeTileLumi).InnerText.Trim());
            data.tileDoF = StringToUTF8ByteArray(((XmlCDataSection)nodeTileDoF).InnerText.Trim());
            data.tileObjectTraj = StringToUTF8ByteArray(((XmlCDataSection)nodeObjectTraj).InnerText.Trim());
            data.tileLookupTable = StringToUTF8ByteArray(((XmlCDataSection)nodeLookupTable).InnerText.Trim());
            return data;
        }
        //将string转化为byte数据流
        //TODO 同样需要识别两个字节的byte，转为一个字节
        private static byte[] StringToUTF8ByteArray(string pXmlString)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(pXmlString);
            return byteArray;
        }
    }
}