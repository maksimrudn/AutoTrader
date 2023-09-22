using AutoTraderSDK.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderSDK
{
    public static class Globals
    {
        public static string GetWorkFolder()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "").Replace('/', '\\')) + "\\";
        }
    }

    //описание обработчика обратного вызова
     delegate bool CallBackDelegate(IntPtr pData);
}
