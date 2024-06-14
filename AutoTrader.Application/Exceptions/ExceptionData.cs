using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Exceptions
{
    public class ExceptionData
    {
        public ExceptionData(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        } 

        public int ErrorCode { get; set; }

        public string Message { get; set; }
    }
}
