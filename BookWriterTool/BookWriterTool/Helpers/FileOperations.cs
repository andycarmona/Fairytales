using System.Web;

namespace BookWriterTool.Helpers
{
    using System.IO;
    using System.Xml.Serialization;

    public class FileOperations
    {

        public readonly string TemplateDirectory = "~/Content/Resources/BookTemplate";

        public string ActualBook;

        public static readonly string UsersDirectory = "~/Content/Resources/Users";

        public book SerializeXmlToObject(string actualBook)
        {
            var serializer = new XmlSerializer(typeof(book));
            var stream = new FileStream(HttpContext.Current.Server.MapPath(actualBook), FileMode.Open);
            var aBook = serializer.Deserialize(stream) as book;
            stream.Close();
            return aBook;
        }

        public void SetActualXml(string file)
        {
            this.ActualBook = file;
        }

        public string GetActualXml()
        {
            return this.ActualBook;
        }

        public string DeserializeObjectToXml(book aBook)
        {
            var textWriter = new StringWriter();
            var xmlSerializer = new XmlSerializer(aBook.GetType());
            xmlSerializer.Serialize(textWriter, aBook);

            return textWriter.ToString();
        }

        public string[] GetListOfTemplates()
        {
            string[] filesPaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(TemplateDirectory));
     
            return filesPaths;
        }

        public string[] GetListOfUserFiles(string user)
        {
            string[] filesPaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(UsersDirectory + "/" + user));
     
            return filesPaths;
        }
    }
}