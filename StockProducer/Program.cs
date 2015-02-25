using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockProducer {
    class Program {
        public static IDictionary<String, IList<string>> StocksOnIndrxes = new ConcurrentDictionary<string, IList<string>>();
        public static IDictionary<int, String> Indexs = new ConcurrentDictionary<int, string>();

        static void Main(string[] args) {
            MessageQueue mq = new MessageQueue(@".\Private$\Stock");
            mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(Stock) });
            mq.Purge();

            Indexs.Add(0,"NASDAQ");
            Indexs.Add(1,"S&P 500");
            Indexs.Add(2, "Dow Jones");
            // NASDAQ
            IList<String> stockNmaes=new[] {"Apple Inc.", "NVIDIA Corporat", "O'Reilly Autom"};
            StocksOnIndrxes.Add("NASDAQ",stockNmaes);

            // S&P 500
            stockNmaes=new[] {"Adobe Systems Inc", "Accenture PLC", "Actavis PLC"};
            StocksOnIndrxes.Add("S&P 500",stockNmaes);

            // Dow Jones
            stockNmaes=new[] {"3M Co", "Cisco Systems Inc", "Home Depot Inc"};
            StocksOnIndrxes.Add("Dow Jones",stockNmaes);

            while(true) {
                Stock s=GenerateRandomStockQoute();
                Console.WriteLine("SENDING STOCK TO QUEUE: " + s);
                mq.Send(s);
                
                Thread.Sleep(2000);
            }
        }

        public static Stock GenerateRandomStockQoute() {
            Random r = new Random();
            Stock s = new Stock();
            String indexName;
            Indexs.TryGetValue(r.Next(3), out indexName);
            String stockName=StocksOnIndrxes[indexName][r.Next(3)];
            s.Ask=r.NextDouble()*1000;
            s.Change=r.NextDouble()%0.10;
            s.LastValue=r.NextDouble()*1000;
            s.Offer=r.NextDouble()*1000;
            s.TimeStamp=DateTime.Now;
            s.Index=indexName;
            s.StockName=stockName;
            return s;
        }
    }
}
