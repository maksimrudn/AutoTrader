using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Models
{
    public class TXMLEventArgs<T> : EventArgs
    {
        public TXMLEventArgs(T data)
        {
            this.data = data;
        }
        public T data { get; set; }
    }
}
