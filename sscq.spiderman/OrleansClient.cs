using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using Orleans;

namespace sscq.spiderman
{
    public class OrleansClient:IClient
    {
        public static IClient Instance = new OrleansClient();

        private OrleansClient()
        { }        

        public bool Connect(string ip, int port)
        {
            return this.Connect();
        }

        public bool Connect()
        {
            if (!Orleans.GrainClient.IsInitialized)
            {
                Orleans.GrainClient.Initialize("DevTestClientConfiguration.xml");
                System.Threading.Thread.Sleep(5 * 1000);
            }
            IServices server = GrainClient.GrainFactory.GetGrain<IServices>(Guid.NewGuid());
            server.ConnectToServer(Dns.GetHostName());
            return true;
        }

        public bool DisConnect()
        {
            if (Orleans.GrainClient.IsInitialized)
            {
                Orleans.GrainClient.Uninitialize();
            }
            return true ;
        }

        public bool KeepAlived()
        {
            return true;
        }       
    }
}
