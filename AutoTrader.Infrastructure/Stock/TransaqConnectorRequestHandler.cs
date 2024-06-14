using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Helpers;
using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using AutoTrader.Application.UnManaged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AutoTrader.Infrastructure.Stock
{
    public class TransaqConnectorRequestHandler: ITransaqConnectorRequestHandler
    {
        IntPtr _tConnectorDll;
        string _logpath = MainHelper.GetWorkFolder() + "\0";
        int _loglevel = 3;
        protected string _tconfFile;
        protected TransaqConnectorInputStreamHandler _inputStreamHandler;

        public TransaqConnectorRequestHandler(string tconFile, TransaqConnectorInputStreamHandler inputStreamHandler)
        {
            _inputStreamHandler = inputStreamHandler;

            _tconfFile = tconFile;
            _tConnectorDll = NativeMethods.LoadLibrary(tconFile);

            IntPtr initializePtr = NativeMethods.GetProcAddress(_tConnectorDll, "Initialize");
            IntPtr freeMemoryPtr = NativeMethods.GetProcAddress(_tConnectorDll, "FreeMemory");
            IntPtr setCallbackPtr = NativeMethods.GetProcAddress(_tConnectorDll, "SetCallback");
            IntPtr sendCommandPtr = NativeMethods.GetProcAddress(_tConnectorDll, "SendCommand");
            IntPtr unInitializePtr = NativeMethods.GetProcAddress(_tConnectorDll, "UnInitialize");
            IntPtr setLogLevelPtr = NativeMethods.GetProcAddress(_tConnectorDll, "SetLogLevel");

            if (initializePtr == IntPtr.Zero)
            {
                throw new Exception("Can't load library " + Marshal.GetLastWin32Error());
            }

            _initialize = (Initialize)Marshal.GetDelegateForFunctionPointer(initializePtr, typeof(Initialize));
            _freeUpMemory = (FreeMemory)Marshal.GetDelegateForFunctionPointer(freeMemoryPtr, typeof(FreeMemory));
            _setCallback = (SetCallback)Marshal.GetDelegateForFunctionPointer(setCallbackPtr, typeof(SetCallback));
            _sendCommand = (SendCommand)Marshal.GetDelegateForFunctionPointer(sendCommandPtr, typeof(SendCommand));
            _unInitialize = (UnInitialize)Marshal.GetDelegateForFunctionPointer(unInitializePtr, typeof(UnInitialize));
            _setLogLevel = (SetLogLevel)Marshal.GetDelegateForFunctionPointer(setLogLevelPtr, typeof(SetLogLevel));

            // шаг 1 - инициализация
            IntPtr pPath = MarshalUTF8.StringToHGlobalUTF8(_logpath);
            IntPtr pResult = _initialize(pPath, _loglevel);
            if (!pResult.Equals(IntPtr.Zero))
            {
                String result = MarshalUTF8.PtrToStringUTF8(pResult);
                Marshal.FreeHGlobal(pPath);
                _freeUpMemory(pResult);
                log.WriteLog(result);

                throw new Exception(result);
            }
            else
            {
                Marshal.FreeHGlobal(pPath);
                _freeUpMemory(pResult);
                log.WriteLog("Initialize() OK");
            }

            _callBackDelegate = new CallBackDelegate(_inputStreamCallBackHandler);
            _callbackHandle = GCHandle.Alloc(_callBackDelegate);

            //шаг 2
            bool callbackres = _setCallback(_callBackDelegate);
            if (callbackres == false)
            {
                throw new Exception("Не удалось установить обработчик обратного вызова");
            }
        }

        public result ConnectorSendCommand(command commandInfo)
        {
            string cmd = XMLHelper.SerializeToString(commandInfo, commandInfo.GetType());
            string res = ConnectorSendCommand(cmd);

            if (res.Contains("<error>"))
            {
                var er = (error)XMLHelper.Deserialize(res, typeof(error));

                throw new Exception(er.Text);
            }

            return (result)XMLHelper.Deserialize(res, typeof(result));
        }

        public String ConnectorSendCommand(String command)
        {
            IntPtr pData = MarshalUTF8.StringToHGlobalUTF8(command);
            IntPtr pResult = _sendCommand(pData);

            String result = MarshalUTF8.PtrToStringUTF8(pResult);

            Marshal.FreeHGlobal(pData);
            _freeUpMemory(pResult);

            return result;
        }

        bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }

                IntPtr pResult = _unInitialize();

                if (!pResult.Equals(IntPtr.Zero))
                {
                    String result = MarshalUTF8.PtrToStringUTF8(pResult);
                    _freeUpMemory(pResult);
                    log.WriteLog(result);
                }
                else
                {
                    log.WriteLog("UnInitialize() OK");
                }

                _disposed = true;
            }            
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
        protected FreeMemory _freeUpMemory;

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

        protected bool _inputStreamCallBackHandler(IntPtr pData)
        {
            bool res = true;
            String result = MarshalUTF8.PtrToStringUTF8(pData);
            _freeUpMemory(pData);

            _inputStreamHandler.HandleData(result);

            return res;
        }
    }
}
