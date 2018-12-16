using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace ParkSS.classes
{
    class FunctionHelper
    {
        private static string strDocuments = AppDomain.CurrentDomain.BaseDirectory + "files\\";

        public FunctionHelper()
        {
        }

        public static Boolean ValidXML(string xmlString, string filenameXSD)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            return ValidXML(doc, filenameXSD);
        }

        public static Boolean ValidXML(XmlDocument doc, string filenameXSD)
        {
            if (!checkIfFileExist(filenameXSD))
            {
                Console.WriteLine("Files not found! "+filenameXSD);
                return false;
            }

            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("", givePatch(filenameXSD));
            XDocument docConferm = XDocument.Parse(doc.OuterXml);
            Boolean valid = true;
            docConferm.Validate(schema, (o, e) =>
            {
                Console.WriteLine("Invalid XML: {0}", e.Message);
                valid = false;
            });
            return valid;
        }

        public static Boolean checkIfFileExist(string filename)
        {
            if (File.Exists(@"" + strDocuments + "" + filename))
            {
                return true;
            }
            Console.WriteLine("Fail to find the file(" + @"" + strDocuments + "" + filename + ")!");
            return false;
        }

        public static string directory()
        {
            return strDocuments;
        }

        public static string givePatch(string filename)
        {
            return @directory() + "" + filename;
        }

        public static string formatXmlToUnminifierString(XmlDocument doc)
        {
            var stringBuilder = new StringBuilder();

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                doc.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }
    }
}
