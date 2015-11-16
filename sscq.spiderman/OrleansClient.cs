using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Net;

using Orleans;
using Orleans.Runtime.Configuration;

namespace sscq.spiderman
{
    public class OrleansClient:IClient
    {
        public static IClient Instance = new OrleansClient();

        private OrleansClient()
        { }

        public async Task<bool> Connect(string ip, int port)
        {
            return await this.Connect();
        }

        public async Task<bool> Connect()
        {

            SpiderLogger.Info("------------------启动 Orleans Client.------------------");
            try
            {
                if (!Orleans.GrainClient.IsInitialized)
                {
                    ClientConfiguration config = new ClientConfiguration();
                    config.GatewayProvider = ClientConfiguration.GatewayProviderType.Config;
                    IPEndPoint ip=new IPEndPoint(IPAddress.Parse("127.0.0.1"),30000);
                    config.Gateways.Add(ip);
                    config.StatisticsMetricsTableWriteInterval = TimeSpan.FromSeconds(30);
                    config.StatisticsLogWriteInterval = TimeSpan.FromSeconds(300);
                    config.StatisticsWriteLogStatisticsToTable = false;
                    config.StatisticsPerfCountersWriteInterval = TimeSpan.FromSeconds(30);
                    Orleans.GrainClient.Initialize(config);
                }
                IServices server = GrainClient.GrainFactory.GetGrain<IServices>(Guid.NewGuid());
                return await server.ConnectToServer(Dns.GetHostName());
                
            }
            catch (Exception ex)
            {
                SpiderLogger.Error("启动 Orleans Client 失败." + ex.Message+"\r\n");
                SpiderLogger.Error(ex.StackTrace);
                return false;

            }           
        }

        public Task<bool> DisConnect()
        {
            SpiderLogger.Info("------------------关闭 Orleans Client.------------------");
             try
             {
                 if (Orleans.GrainClient.IsInitialized)
                 {
                     Orleans.GrainClient.Uninitialize();
                 }
                 return Task.FromResult(true) ;
             }
             catch (Exception ex)
             {
                 SpiderLogger.Error("关闭 Orleans Client 失败." + ex.Message + "\r\n");
                 SpiderLogger.Error(ex.StackTrace);
                 return Task.FromResult(false);
             }   
           
        }

        public Task<bool> KeepAlived()
        {
            return Task.FromResult(true);
        }       
    }
}
