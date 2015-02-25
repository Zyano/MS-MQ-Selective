using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_MQ_selective {
    class StockMessageEventArgs : EventArgs {
        public Stock Stock { get; set; }

        public StockMessageEventArgs(Stock s) {
            Stock=s;
        }
    }
}
