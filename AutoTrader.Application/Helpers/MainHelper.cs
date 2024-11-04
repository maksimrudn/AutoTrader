using System.Reflection;

namespace AutoTrader.Application.Helpers
{
    public static class MainHelper
    {
        public static string GetWorkFolder()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "").Replace('/', '\\')) + "\\";
        }
    }

    //описание обработчика обратного вызова
    delegate bool CallBackDelegate(IntPtr pData);
}
