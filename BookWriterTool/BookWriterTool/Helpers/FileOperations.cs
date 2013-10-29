using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookWriterTool.Helpers
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class FileOperations
    {
        private string actualBook;

        public book SerializeXmlToObject(string actualBook)
        {
            book aBook;
            var serializer = new XmlSerializer(typeof(book));
            var stream = new FileStream(HttpContext.Current.Server.MapPath(actualBook), FileMode.Open);
            aBook = serializer.Deserialize(stream) as book;
            stream.Close();
            return aBook;
        }
        public void SetActualXml(string file)
        {
            actualBook = file;
        }
        public string GetActualXml()
        {
            return actualBook;
        }
        public string DeserializeObjectToXml(book aBook)
        {
            StringWriter textWriter=new StringWriter();
            XmlSerializer xmlSerializer = new XmlSerializer(aBook.GetType());
            xmlSerializer.Serialize(textWriter,aBook);
         
            return textWriter.ToString();
        }
       /* public List<File> GetListOfFilesinFolder(string user)
        {
            FileInfo aFileinfo=  new FileInfo();
            if (aFileinfo.Directory != null)
            {
                FileInfo[] listOfFile=  aFileinfo.Directory.GetFiles()
            }
        }*/
    }
}