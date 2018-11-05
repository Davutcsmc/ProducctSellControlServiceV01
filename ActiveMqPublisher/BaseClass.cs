using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMqControl
{
    public class BaseClass
    {
        public string URI = "activemq:tcp://localhost:61616";
        public IConnectionFactory connectionFactory;
        public IConnection _connection;
        public ISession _session;

        public BaseClass()
        {
            connectionFactory = new ConnectionFactory(URI);
            if (_connection == null)
            {
                _connection = connectionFactory.CreateConnection();
                _connection.Start();
                _session = _connection.CreateSession();
            }
        }

        public BaseClass(string AMQuri)
        {
            URI = AMQuri;
            connectionFactory = new ConnectionFactory(URI);
            if (_connection == null)
            {
                _connection = connectionFactory.CreateConnection();
                _connection.Start();
                _session = _connection.CreateSession();
            }
        }
    }
}
