using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MS_MQ_selective {
    class Program {
        static void Main(string[] args) {
            MessageQueue mq = new MessageQueue(@".\Private$\Stock");
            mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(Stock) });
            SelectiveMessageSystem sms = new SelectiveMessageSystem("NASDAQ",mq);
            sms.MessageReceivedEvent += SmsOnMessageReceivedEvent;
            Console.WriteLine("STARTED SELECTIVE MESSAGE SYSTEM!");

            sms.Start();

            Console.ReadLine();
        }

        private static void SmsOnMessageReceivedEvent(object sender, StockMessageEventArgs stockMessageEventArgs) {
            Console.WriteLine("Got message on desired filter: " + stockMessageEventArgs.Stock);
        }
    }
}
