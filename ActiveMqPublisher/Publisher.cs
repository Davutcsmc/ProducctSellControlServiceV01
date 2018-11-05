using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMqControl
{
    public class Publisher : BaseClass
    {
        private string DESTINATION = "queue://service2client";
        public Publisher()
        {
        }

        public Publisher(string dest)
        {
            DESTINATION = dest;
        }

        public Publisher(string uri, string dest):base(uri)
        {
            DESTINATION = dest;
        }

        public string SendMessage(string message)
        {
            string result = string.Empty;
            try
            {
                IDestination destination = _session.GetDestination(DESTINATION);
                using (IMessageProducer producer = _session.CreateProducer(destination))
                {
                    var textMessage = producer.CreateTextMessage(message);
                    producer.Send(textMessage);
                }
                result = "Message sent successfully.";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public string Destionation
        {
            get
            {
                return DESTINATION;
            }
            set
            {
                DESTINATION = value;
            }
        }
    }
}
