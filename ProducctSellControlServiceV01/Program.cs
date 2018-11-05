using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ProducctSellControlServiceV01
{
    static class Program
    {
        private static int pushDataPeriod = 5000;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            pushDataPeriod = ReadConfigurationValues.GetConfiguration.Instance.GetPushDataPeriod();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ProductSellControlServiceV01(pushDataPeriod)
            };
            ServiceBase.Run(ServicesToRun);
        }

    }
}
