using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Service.Interface;

namespace Service
{
    public class SendMessage: ISendMessage
    {
        public void SendMessageToContractor(string message1, long phonenumber)
        {
            var accountSid = "AC1ddfefa9ad316bf025cc4229c0362fee";
            var authToken = "804bbb888ee86949df86a07beda3b28c";
            TwilioClient.Init(accountSid, authToken);
            string phone1 = Convert.ToString(phonenumber);
            string countryid = "+91";
            string concat = countryid + phone1;
            var messageOptions = new CreateMessageOptions(
                new PhoneNumber(concat));
            messageOptions.MessagingServiceSid = "MGd4d8963b65f2e7ac1eb40d55babdfcf0";
            messageOptions.Body = message1;
            var message = MessageResource.Create(messageOptions);
        }
    }
}
