using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Exceptions
{
    public class StockClientException:Exception
    {
        public StockClientException(ExceptionData exceptionData)
                                    :base($"[{exceptionData.ErrorCode}] {exceptionData.Message}")
        {
            ErrorCode = exceptionData.ErrorCode;
            DefaultMessage = exceptionData.Message;
        }

        public StockClientException(ExceptionData exceptionData, string responseMessage) 
                                    :base($"[{exceptionData.ErrorCode}] {exceptionData.Message} {responseMessage}")
        {
            ErrorCode = exceptionData.ErrorCode;
            DefaultMessage = exceptionData.Message;
            ResponseMessage = responseMessage;
        }

        public int ErrorCode { get; set; }

        public string DefaultMessage { get; set; }

        public string ResponseMessage { get; set; }
    }
}
