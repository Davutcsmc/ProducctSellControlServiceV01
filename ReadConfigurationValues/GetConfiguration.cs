using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadConfigurationValues
{
    public class GetConfiguration
    {
        private static int m_PushDataPeriod = 30000;
        private static string m_DBConnStr;

        private static string m_amqUri;
        private static string m_amqPublisherQ;
        private static string m_amqListenerQ;

        private static GetConfiguration instance = null;
        private static readonly object padlock = new object();

        private GetConfiguration()
        {
            ReadConfigurationValues();
        }

        public static GetConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new GetConfiguration();
                        }
                    }
                }
                return instance;
            }
        }

        public static void ReadConfigurationValues()
        {
            var control = int.TryParse(ConfigurationManager.AppSettings["PushDataPeriod"], out m_PushDataPeriod);
            if (!control) m_PushDataPeriod = 5000;

            m_DBConnStr = ConfigurationManager.AppSettings["DBConnectionString"];

            m_amqUri = ConfigurationManager.AppSettings["AMQuri"];

            m_amqPublisherQ = ConfigurationManager.AppSettings["AMQPublisherQueue"];

            m_amqListenerQ = ConfigurationManager.AppSettings["AMQListenerQueue"];
            
        }

        public int GetPushDataPeriod()
        {
            return m_PushDataPeriod;
        }

        public string GetDBConnString()
        {
            return m_DBConnStr;
        }

        public string GetamqUri()
        {
            return m_amqUri;
        }

        public string GetamqPublisherQName()
        {
            return m_amqPublisherQ;
        }

        public string GetamqListenerQName()
        {
            return m_amqListenerQ;
        }

    }


}
