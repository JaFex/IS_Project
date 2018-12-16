using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace ParkDACE.classes
{
    class FunctionHelper
    {
        private static string strDocuments = AppDomain.CurrentDomain.BaseDirectory+"files\\";

        public FunctionHelper()
        {
        }

        public static Boolean ValidXMLParkingLocation(string filenameXML, string filenameXSD)
        {
            if (!checkIfFileExist(filenameXML) || !checkIfFileExist(filenameXSD))
            {
                Console.WriteLine("Files not found!");
                return false;
            }

            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("", givePatch(filenameXSD));
            XmlReader rd = XmlReader.Create(givePatch(filenameXML));
            XDocument docConferm = XDocument.Load(rd);
            Boolean valid = true;
            docConferm.Validate(schema, (o, e) =>
            {
                Console.WriteLine("Invalid XML({0}) {1}", filenameXML, e.Message);
                valid = false;
            });
            return valid;
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

        public static string directory()
        {
            return strDocuments;
        }


        public static void checkIfFileExistOrFail(string filename)
        {
            if (!File.Exists(@"" + strDocuments + "" + filename))
            {
                Console.WriteLine("Fail to find the file(" + filename + ")!");
                throw new FileNotFoundException();
            }
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

        public static void ReleaseAndCloseExcel(Worksheet excelWorksheet, Workbook excelWorkbook, Application excelAplication)
        {
            excelWorkbook.Close(0);
            excelAplication.Quit();
            

            FunctionHelper.ReleaseComObjects(excelWorksheet);
            FunctionHelper.ReleaseComObjects(excelWorkbook);
            FunctionHelper.ReleaseComObjects(excelAplication);
        }

        public static void ReleaseComObjects(object obs)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obs);
                obs = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public static string givePatch(string filename)
        {
            return @directory() + "" + filename;
        }
    }
}
