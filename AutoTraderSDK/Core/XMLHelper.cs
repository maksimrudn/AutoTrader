using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AutoTraderSDK.Core
{
    public static class XMLHelper
    {
        public static string SerializeToString(object commandInfo, Type type)
        {
            string res;

            XmlSerializer xser = new XmlSerializer(type);
            MemoryStream ms = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            XmlWriter writer = XmlWriter.Create(ms, settings);

            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add("", "");

            xser.Serialize(writer, commandInfo, names);

            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);

            res = sr.ReadToEnd();

            return res;
        }

        public static string GetNodeName(string data)
        {
            log.WriteLog("ServerData: " + data);


            XmlReaderSettings xs = new XmlReaderSettings();
            xs.IgnoreWhitespace = true;
            xs.ConformanceLevel = ConformanceLevel.Fragment;
            xs.ProhibitDtd = false;
            XmlReader xr = XmlReader.Create(new System.IO.StringReader(data), xs);


            xr.Read();
            return xr.Name;

        }

        public static IntPtr SerializeToIntPtr(object commandInfo, Type type)
        {
            string cmd = SerializeToString(commandInfo, type);

            IntPtr pData = Core.MarshalUTF8.StringToHGlobalUTF8(cmd);

            return pData;
        }

        public static object Deserialize(string data, Type type)
        {
            object res = null;

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = type.Name;
            xRoot.IsNullable = true;

            XmlSerializer xser = new XmlSerializer(type, xRoot);
            StringReader sr = new StringReader(data);
            res = xser.Deserialize(sr);

            return res;
        }
    }
}
