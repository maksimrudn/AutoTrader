using AutoTrader.Application.UnManaged;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace AutoTrader.Application.Helpers
{
    public static class XMLHelper
    {
        public static string ToXml<T>(this T obj)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", ""); // Remove xsi and xsd namespaces

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true, // Don't emit <?xml version="1.0" ... ?>
                Indent = false,             // Pretty-print the XML
                NewLineHandling = NewLineHandling.None,
                NewLineChars = "" // Ensure no newlines are inserted
            };

            using var stringWriter = new StringWriter();
            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                new XmlSerializer(typeof(T)).Serialize(xmlWriter, obj, ns);
            }

            return stringWriter.ToString();
        }
        
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
            XmlReader xr = XmlReader.Create(new StringReader(data), xs);


            xr.Read();
            return xr.Name;

        }

        public static IntPtr SerializeToIntPtr(object commandInfo, Type type)
        {
            string cmd = SerializeToString(commandInfo, type);

            IntPtr pData = MarshalUTF8.StringToHGlobalUTF8(cmd);

            return pData;
        }

        public static object Deserialize(string data, Type type)
        {
            object res = null;
            
            // 1. Try to detect [XmlRoot("...")] on the class
            var xmlRootAttr = type.GetCustomAttribute<XmlRootAttribute>();
        
            // 2. Decide the element name
            string rootName;
            if (xmlRootAttr != null && !string.IsNullOrWhiteSpace(xmlRootAttr.ElementName))
            {
                rootName = xmlRootAttr.ElementName;
            }
            else
            {
                // fallback: use the type name
                rootName = type.Name;
            }

            XmlRootAttribute xRoot = new XmlRootAttribute(rootName);
            xRoot.IsNullable = true;

            XmlSerializer xser = new XmlSerializer(type, xRoot);
            StringReader sr = new StringReader(data);
            res = xser.Deserialize(sr);

            return res;
        }
    }
}
