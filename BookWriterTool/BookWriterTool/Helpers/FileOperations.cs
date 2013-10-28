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
        public book SerializeXmlToObject()
        {
            book aBook;
            var serializer = new XmlSerializer(typeof(book));
            var stream = new FileStream(HttpContext.Current.Server.MapPath("/Content/Resources/bookNoMetadata2.xml"), FileMode.Open);
            aBook = serializer.Deserialize(stream) as book;
            stream.Close();
            return aBook;
        }

        public string DeserializeObjectToXml(book aBook)
        {
            StringWriter textWriter=new StringWriter();
            XmlSerializer xmlSerializer = new XmlSerializer(aBook.GetType());
            xmlSerializer.Serialize(textWriter,aBook);
         
            return textWriter.ToString();
        }
      
    }
}