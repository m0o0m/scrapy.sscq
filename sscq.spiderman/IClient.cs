using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sscq.spiderman
{
    public interface IClient
    {      
        bool Connect(string ip, int port);
        bool Connect();
        bool DisConnect();
        bool KeepAlived();
    }
}
