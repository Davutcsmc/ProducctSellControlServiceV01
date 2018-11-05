using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ProducctSellControlServiceV01
{
    public partial class ProductSellControlServiceV01 : ServiceBase
    {
        private int mTimerPeriod = 5000;

        ActiveMqControl.Publisher publisher;  

        public ProductSellControlServiceV01()
        {
            InitializeComponent();
        }

        public ProductSellControlServiceV01(int timerPeriod)
        {
            InitializeComponent();
            mTimerPeriod = timerPeriod;

            string amqUri = ReadConfigurationValues.GetConfiguration.Instance.GetamqUri();
            string amqPQ = ReadConfigurationValues.GetConfiguration.Instance.GetamqPublisherQName();
            string amqLQ = ReadConfigurationValues.GetConfiguration.Instance.GetamqListenerQName();

            publisher = new ActiveMqControl.Publisher(amqUri, amqPQ);
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStart.txt");

            // Set up a timer that triggers every minute.
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = mTimerPeriod; // 60 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStop.txt");

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnContinue()
        {
            System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnContinue.txt");
        }

        private int eventId = 1;
        private int uniqueIdent = 0;
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            //while (uniqueIdent <= 200)
            //{
            //    Model.Product p = produceProduct(uniqueIdent.ToString());

            //    Session.Broker brkr = new Session.Broker();

            //    brkr.Insert(p);
            //    uniqueIdent++;
            //    break;
            //}

            while (uniqueIdent <= 200)
            {
                Model.Product p = new Model.Product();

                Session.Broker brkr = new Session.Broker();

                p = brkr.GetProduct(uniqueIdent);

                uniqueIdent++;

                string pStr = string.Format(p.UniqueIdentifier + ", " + p.UniqueIdentifier + ", "
                    + p.Color + ", " + p.Model + ", " + p.Name);

                publisher.SendMessage(pStr);

                //using (System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "OnProductData.txt", true))
                //{
                //    file.WriteLine(string.Format(DateTime.Now.ToString() + "\t" + p.UniqueIdentifier + "\t" + 
                //        p.Barcode + "\t" + p.Color + "\t" + p.Model + "\t" + p.Name));
                //}
                break;
            }

            

        }

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);




        public Model.Product produceProduct(string uniqVal)
        {
            string[] productColors = { "Kırmızı", "Yeşil", "Mavi", "Siyah", "Beyaz", "Sarı" };
            string[] productNames = { "Gömlek", "Etek", "Pantolon", "Ayakkabı" };
            string[] ModelGomlek = { "Kareli", "Klasik", "Slim", "Regular" };
            string[] ModelEtek = { "Piliseli", "Uzun", "Desenli", "Kalem" };
            string[] ModelPantolon = { "Denim", "Kadife", "Slim", "Kanvas" };
            string[] ModelAyak = { "Topuklu", "Spor", "Günlük", "Sandalet" };

            Random randomNum = new Random();

            Model.Product product = new Model.Product();
            product.UniqueIdentifier = uniqVal;
            product.Barcode = generateString();

            int randValColor = randomNum.Next(6);
            product.Color = productColors[randValColor];
            int randVal = randomNum.Next(4);
            product.Name = productNames[randVal];
            int randModel = randomNum.Next(4);

            product.Model = ModelGomlek[randModel];
            switch (randVal)
            {
                case 1:
                    product.Model = ModelGomlek[randModel];
                    break;
                case 2:
                    product.Model = ModelEtek[randModel];
                    break;
                case 3:
                    product.Model = ModelPantolon[randModel];
                    break;
                case 4:
                    product.Model = ModelAyak[randModel];
                    break;
            }

            //String strProduct = product.UniqueIdentifier + " " + product.Name + " " + product.Model + " " + product.Color + "\n" + product.Barcode;

            //AppInfo appInfo = new AppInfo();
            //appInfo.applicationName = strProduct;
            //app_info.add(appInfo);

            //adapter.notifyDataSetChanged();
            return product;
        }

        public string generateString()
        {
            Guid g = Guid.NewGuid();
            string barcode = Convert.ToBase64String(g.ToByteArray());
            barcode = barcode.Replace("=", "");
            barcode = barcode.Replace("+", "");

            return barcode;
        }

    }
}
