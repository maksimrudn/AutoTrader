using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Models
{
    public class TransaqEventArgs<T> : EventArgs
    {
        public TransaqEventArgs(T data)
        {
            this.data = data;
        }
        public T data { get; set; }
    }
}
