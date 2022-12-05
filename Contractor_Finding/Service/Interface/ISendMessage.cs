using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ISendMessage
    {
        void SendMessageToContractor(string message1, long phonenumber);
    }
}
