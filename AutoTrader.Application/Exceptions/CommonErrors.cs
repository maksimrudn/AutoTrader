using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Exceptions
{
    public static class CommonErrors
    {
        public static int ServerConnectionErrorCode { get; } = 1;
        public static ExceptionData ServerConnectionError = new(ServerConnectionErrorCode, "Server returned connection error.");

        public static int AlreadyLoggedInCode { get; } = 2;
        public static ExceptionData AlreadyLoggedIn = new(AlreadyLoggedInCode, "User is logged in already");

        public static int UnAuthorizedCode { get; } = 3;        
        public static ExceptionData UnAuthorized = new(UnAuthorizedCode, "User is unauthorized");

        public static int ServerStatusWaitingTimeoutCode { get; } = 4;
        public static ExceptionData ServerStatusWaitingTimeout = new(ServerStatusWaitingTimeoutCode, "server_status response waiting timeout");

        public static int PositionsWaitingTimeoutCode { get; } = 5;
        public static ExceptionData PositionsWaitingTimeout = new(PositionsWaitingTimeoutCode, "positions response waiting timeout");
    }
}
