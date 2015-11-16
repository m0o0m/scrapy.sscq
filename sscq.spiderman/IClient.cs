using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sscq.spiderman
{
    public interface IClient
    {      
        Task<bool> Connect(string ip, int port);
        Task<bool> Connect();
        Task<bool> DisConnect();
        Task<bool> KeepAlived();
    }
}
