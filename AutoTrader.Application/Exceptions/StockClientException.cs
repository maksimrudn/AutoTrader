namespace AutoTrader.Application.Exceptions
{
    public class StockClientException:Exception
    {
        public StockClientException(string commonMessage):base(commonMessage) 
        {
            
        }

        public StockClientException(ExceptionData exceptionData)
                                    :base($"[{exceptionData.ErrorCode}] {exceptionData.Message}")
        {
            ErrorCode = exceptionData.ErrorCode;
            CommonMessage = exceptionData.Message;
        }

        public StockClientException(ExceptionData exceptionData, string responseMessage) 
                                    :base($"[{exceptionData.ErrorCode}] {exceptionData.Message} {responseMessage}")
        {
            ErrorCode = exceptionData.ErrorCode;
            CommonMessage = exceptionData.Message;
            ResponseMessage = responseMessage;
        }

        public int ErrorCode { get; set; }

        public string CommonMessage { get; set; }

        public string ResponseMessage { get; set; }
    }
}
