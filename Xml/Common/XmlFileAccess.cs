using DotNEET.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DotNEET.Xml
{
    public class XmlFileAccess<T>
    {
        private static readonly XmlWriterSettings settings = new XmlWriterSettings()
        {
#if DEBUG
            Indent = true,
#else
            Indent = false,
#endif
            IndentChars = "\t",
            NewLineChars = Environment.NewLine,
#if DEBUG
            NewLineOnAttributes = true,
#else
            NewLineOnAttributes = false,
#endif
        };

        private readonly string xmlFilePath;

        public XmlFileAccess(string xmlFilePath)
        {
            this.xmlFilePath = xmlFilePath;
        }

        public void Write(T item)
        {
            try
            {
                using (var writer = XmlWriter.Create(this.xmlFilePath, XmlFileAccess<T>.settings))
                {
                    new XmlSerializer(typeof(T)).Serialize(writer, item);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(@"Can't write " + item.ToString() + @" of type + " + typeof(T).ToString() + @" into file " + this.xmlFilePath + @"exception : " + e.ToString());
                throw;
            }
        }

        public T Read()
        {
            try
            {
                using (var writer = XmlReader.Create(this.xmlFilePath))
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(writer);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(@"Can't read type " + typeof(T).ToString() + @"into file " + this.xmlFilePath + @"exception : " + e.ToString());
                throw;
            }
        }

        public bool Exists()
        {
            return File.Exists(this.xmlFilePath);
        }
    }

    public static class XmlFileAccessExts
    {
        public static T Read<T>(this string xmlFilePath)
        {
            return new XmlFileAccess<T>(xmlFilePath).Read();
        }

        public static void Write<T>(this T obj, string xmlFilePath)
        {
            new XmlFileAccess<T>(xmlFilePath.ThrowIfNull()).Write(obj.ThrowIfNull());
        }
    }
}