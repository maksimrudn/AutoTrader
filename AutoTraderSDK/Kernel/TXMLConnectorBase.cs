using AutoTraderSDK.Domain.InputXML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AutoTraderSDK.Kernel
{
    public abstract class TXMLConnectorBase:IDisposable
    {
        IntPtr _tConnectorDll;
        string _logpath = Globals.GetWorkFolder() + "\0";
        int _loglevel = 3;

        

        public TXMLConnectorBase(string tconFile = "txmlconnector1.dll")
        {
            _tConnectorDll = NativeMethods.LoadLibrary(tconFile);

            IntPtr initializePtr = NativeMethods.GetProcAddress(_tConnectorDll, "Initialize");
            IntPtr freeMemoryPtr = NativeMethods.GetProcAddress(_tConnectorDll, "FreeMemory");
            IntPtr setCallbackPtr = NativeMethods.GetProcAddress(_tConnectorDll, "SetCallback");
            IntPtr sendCommandPtr = NativeMethods.GetProcAddress(_tConnectorDll, "SendCommand");
            IntPtr unInitializePtr = NativeMethods.GetProcAddress(_tConnectorDll, "UnInitialize");

            _initialize = (Initialize)Marshal.GetDelegateForFunctionPointer(initializePtr, typeof(Initialize));
            _freeMemory = (FreeMemory)Marshal.GetDelegateForFunctionPointer(freeMemoryPtr, typeof(FreeMemory));
            _setCallback = (SetCallback)Marshal.GetDelegateForFunctionPointer(setCallbackPtr, typeof(SetCallback));
            _sendCommand = (SendCommand)Marshal.GetDelegateForFunctionPointer(sendCommandPtr, typeof(SendCommand));
            _unInitialize = (UnInitialize)Marshal.GetDelegateForFunctionPointer(sendCommandPtr, typeof(UnInitialize));
            _setLogLevel = (SetLogLevel)Marshal.GetDelegateForFunctionPointer(sendCommandPtr, typeof(SetLogLevel));

            // шаг 1 - инициализация
            IntPtr pPath = Kernel.MarshalUTF8.StringToHGlobalUTF8(_logpath);
            IntPtr pResult = _initialize(pPath, _loglevel);
            if (!pResult.Equals(IntPtr.Zero))
            {
                String result = Kernel.MarshalUTF8.PtrToStringUTF8(pResult);
                Marshal.FreeHGlobal(pPath);
                _freeMemory(pResult);
                log.WriteLog(result);

                throw new Exception();
            }
            else
            {
                Marshal.FreeHGlobal(pPath);
                _freeMemory(pResult);
                log.WriteLog("Initialize() OK");
            }

            _callBackDelegate = new CallBackDelegate(_inputStreamHandler);
            _callbackHandle = GCHandle.Alloc(_callBackDelegate);

            //шаг 2
            bool callbackres = _setCallback(_callBackDelegate);
            if (callbackres == false)
            {
                throw new Exception("Не удалось установить обработчик обратного вызова");
            }
        }



        protected result ConnectorSendCommand(object commandInfo, Type type)
        {
            string cmd = XMLHelper.SerializeToString(commandInfo, type);
            string res = ConnectorSendCommand(cmd);

            if (res.Contains("<error>"))
            {
                var er = (error)XMLHelper.Deserialize(res, typeof(error));

                throw new Exception(er.Text);
            }

            return (result)XMLHelper.Deserialize(res, typeof(result));

        }

        protected String ConnectorSendCommand(String command)
        {

            IntPtr pData = Kernel.MarshalUTF8.StringToHGlobalUTF8(command);
            IntPtr pResult = _sendCommand(pData);

            String result = Kernel.MarshalUTF8.PtrToStringUTF8(pResult);

            Marshal.FreeHGlobal(pData);
            _freeMemory(pResult);

            return result;
        }

        public void Dispose()
        {

        }



        // файл библиотеки TXmlConnector.dll должен находиться в одной папке с программой

        //Выполняет инициализацию библиотеки: запускает поток обработки очереди
        //обратных вызовов, инициализирует систему логирования библиотеки.
        //logPath Путь к директории, в которую будут сохраняться файлы отчетов
        //logLevel Глубина логирования
        //Предусмотрено три уровня логирования, в соответствии с
        //детализацией и размером лог-файла:
        //1 – минимальный
        //2 – стандартный (рекомендуемый);
        //3 – максимальный
        //Данная функция в качестве аргументов принимает путь к папке (const BYTE*
        //logPath), в которой будут созданы лог-файлы (XDF*.log, DSP*.txt, TS*.log), и
        //уровень логирования (int logLevel).
        //logPath должен включать в себя завершающий символ "\" и заканчиваться на
        //терминальный символ «\0». Пример:
        //logPath = "D:\\Logs\\\0";
        //Функция Initialize может быть вызвана в процессе работы с Коннектором
        //повторно для изменения директории и уровня логирования, но только в
        //случае, когда библиотека остановлена, то есть была выполнена команда
        //disconnect или соединение еще не было установлено.
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        protected delegate IntPtr Initialize(IntPtr pPath, Int32 logLevel);
        protected Initialize _initialize;

        //    Выполняет остановку внутренних потоков библиотеки, в том числе завершает
        //поток обработки очереди обратных вызовов. Останавливает систему
        //логирования библиотеки и закрывает файлы отчетов.
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        protected delegate IntPtr UnInitialize();
        protected UnInitialize _unInitialize;

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        protected delegate IntPtr SetLogLevel(Int32 logLevel);
        protected SetLogLevel _setLogLevel;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        protected delegate IntPtr FreeMemory(IntPtr pData);
        protected FreeMemory _freeMemory;

        //для передачи делегата обработчика события обратного вызова, используется в начале работы
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        protected delegate bool SetCallback(CallBackDelegate pCallback);
        protected SetCallback _setCallback;

        //отправка комманды
        //IntPtr pResult = SendCommand(IntPtr pData);

        //после отправки комманды необходимо выполнить очистку
        //Marshal.FreeHGlobal(pData); //очистка памяти от отправляемой команды
        //FreeMemory(pResult);		//очистка памяти от полученного результата

        //Возвращаемые значени
        // 1      <result success=”true”/>
        // 2      <result success=”false”>
        //            <message>error message</message>
        //        </result>
        // 3      <error> Текст сообщения об ошибке</error>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        protected delegate IntPtr SendCommand(IntPtr pData);
        protected SendCommand _sendCommand;



        //Обработчик обратных вызовов
        //IntPtr - данные обратного вызова
        //после получения данных, их нужно очистить с помощью FreeMemory(pData);
        protected delegate bool CallBackDelegate(IntPtr pData);
        protected CallBackDelegate _callBackDelegate;
        protected GCHandle _callbackHandle;

        protected bool _inputStreamHandler(IntPtr pData)
        {
            bool res = true;
            String result = Kernel.MarshalUTF8.PtrToStringUTF8(pData);
            _freeMemory(pData);

            _handleData(result);

            //_newInputData.Invoke(null, new InputStreamEventArgs() { Data = result });

            return res;
        }


        protected abstract void _handleData(String result);
    }
}
