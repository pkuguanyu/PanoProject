using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace MPD
{
    class Program
    {
        static void Main(string[] args)
        {
            //启动server
            WebServer.Main();

            //client端获得MPDdata
            MPDdata data = MPDclient.requestMPDdata(1256);

        }
    }


}
