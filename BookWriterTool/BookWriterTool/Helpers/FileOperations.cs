using System.Web;

namespace BookWriterTool.Helpers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    using BookWriterTool.Models;

    public class FileOperations
    {

        public readonly string TemplateDirectory = "~/Content/Resources/BookTemplate/";

        public string ActualBook;

        public static readonly string UsersDirectory = "~/Content/Resources/Users/";
 

        public static readonly string Character2DDirectory = "~/Content/Resources/Generic/character2d/anime";

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

        public string[] GetListOfUserBooks(string user)
        {
            string[] filesPaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(UsersDirectory+user+"/Books"));

            return filesPaths;
        }

        public List<BookModel> GetListOf2DCharacter()
        {
            var aListObjects = new List<BookModel>();
            string[] directoryPaths = null;
            string lastFolderName = "empty";
            directoryPaths = Directory.GetDirectories(HttpContext.Current.Server.MapPath(Character2DDirectory));

            if (directoryPaths.Length != 0)
            {
                foreach (string directoryPath in directoryPaths)
                {
                    var fileInfo =new FileInfo(directoryPath);
                    var temp = fileInfo.DirectoryName;
                    string[] filesPaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(directoryPath));
                  
                    lastFolderName = this.GetFolderName(directoryPath);
                    var aBookModel = new BookModel { Target = lastFolderName };

                    if (filesPaths.Length != 0)
                    {
                        foreach (var aFile in filesPaths)
                        {     
                            var aObject = new ObjectModel();
             
                            aObject.ImageObj = aFile;
                            aBookModel.Objects.Add(aObject);
                            aListObjects.Add(aBookModel);
                        }
                    }
                }
                //var aObject = new ObjectModel();
                string path = GlobalVariables.ConfigResource("CharacterRes");
               
            } return aListObjects;
        }

        public string GetFolderName(string aFile)
        {
            string folderName=Path.GetFileName(
                            HttpContext.Current.Server.MapPath(
                                Path.GetDirectoryName(HttpContext.Current.Server.MapPath(aFile))));
            return folderName;
        }

        public string AddNewBook(string newFileName,string userName)
        {
            var mssg = "";
            try
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(UsersDirectory + userName + "/Books/")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(UsersDirectory + userName + "/Books/"));
                }
                File.Copy(
                    HttpContext.Current.Server.MapPath(TemplateDirectory + "empty.xml"),
                    HttpContext.Current.Server.MapPath(UsersDirectory + userName + "/Books/" + newFileName + ".xml"));
            }
            catch (IOException e)
            {
                mssg = e.Message;
            }
            return mssg;
        }

        public string DeleteBook(string fileToDelete, string activeUser)
        {
            var mssg="";
            try
            {
                File.Delete(HttpContext.Current.Server.MapPath(UsersDirectory + activeUser + "/Books/"+fileToDelete));
            }
            catch (IOException e)
            {
                mssg = e.Message;
            }
            return mssg;
        }
    }
}
