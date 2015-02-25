using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MS_MQ_selective {
    class SelectiveMessageSystem {
        private MessageQueue _messageQueue;
        private string _indexFilter;
        public event EventHandler<StockMessageEventArgs> MessageReceivedEvent;

        public SelectiveMessageSystem(String indexFilter,MessageQueue mq) {
            if(string.IsNullOrEmpty(indexFilter)) {
                return;
            }
            _indexFilter=indexFilter;
            _messageQueue=mq;
        }

        public void Start() {
            Thread t = new Thread(Run);
            t.Start();
        }

        private void Run() {
            while(true) {
                Message m = _messageQueue.Receive();
                Stock s=m.Body as Stock;
                if(s != null) {
                    if(s.Index.Equals(_indexFilter)) {
                        MessageReceivedEvent.Invoke(this,new StockMessageEventArgs(s));
                    }
                }
            }
        }

    }
}
