using System.Web;

namespace BookWriterTool.Helpers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    using BookWriterTool.Models;

    public class FileOperations
    {

      //  public readonly string TemplateDirectory = "~/Content/Resources/BookTemplate/";

        public string ActualBook;
   
      //  public static readonly string UsersDirectory = "~/Content/Resources/Users/";
 

        //public static readonly string Character2DDirectory = "~/Content/Resources/Generic/character2d/anime";

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
            string[] filesPaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(GlobalVariables.ConfigResource("TemplateDirectory")));

            return filesPaths;
        }

        public string[] GetListOfUserBooks(string user)
        {
            string[] filesPaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(GlobalVariables.ConfigResource("UsersDirectory")+user+"/Books"));

            return filesPaths;
        }

        public List<BookModel> GetListOf2DCharacter()
        {
            var aListObjects = new List<BookModel>();
            string[] directoryPaths = null;
            directoryPaths = Directory.GetDirectories(HttpContext.Current.Server.MapPath(GlobalVariables.ConfigResource("Character2DRes")));

            if (directoryPaths.Length != 0)
            {
                foreach (string directoryPath in directoryPaths)
                {
                   
                    string[] filesPaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(directoryPath));
                  
                    string lastFolderName = this.GetFolderName(directoryPath);
                    var aBookModel = new BookModel { Target = lastFolderName };

                    if (filesPaths.Length != 0)
                    {
                        foreach (var aFile in filesPaths)
                        {     
                            var aObject = new ObjectModel { ImageObj = aFile };

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
                if((newFileName!=null)||(userName!=string.Empty))
                {
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(GlobalVariables.ConfigResource("UsersDirectory") + userName + "/Books/")))
                    {
                        Directory.CreateDirectory(
                            HttpContext.Current.Server.MapPath(GlobalVariables.ConfigResource("UsersDirectory") + userName + "/Books/"));
                    }
                    if (
                        !File.Exists(
                            GlobalVariables.ConfigResource("UsersDirectory") + userName + "/Books/" + newFileName
                            + ".xml"))
                    {
                        File.Copy(
                            HttpContext.Current.Server.MapPath(
                                GlobalVariables.ConfigResource("TemplateDirectory") + "empty.xml"),
                            HttpContext.Current.Server.MapPath(
                                GlobalVariables.ConfigResource("UsersDirectory") + userName + "/Books/" + newFileName
                                + ".xml"));
                    }
                    else
                    {
                        throw new IOException("ERROR:This file exist already.Please!! Choose another name. ");
                    }
                }else
                {
                    throw new IOException("ERROR:File name cannot be empty ");
                }
            }
            catch (IOException e)
            {
                mssg = e.Message;
            }
            return mssg;
        }

        public List<string> GetListOfBackgrounds(string directoryPath)
        {
            var backgroundFiles=new List<string>();
            var filesInDirectory =Directory.GetFiles( directoryPath);
            if (filesInDirectory.Length > 0)
            {
                foreach (var fileName in filesInDirectory)
                {
                    var fileinfo = new FileInfo(fileName);
                    backgroundFiles.Add(fileinfo.Name);
                }
            }
            return backgroundFiles;
        }

        public Dictionary<string, string[]> GetListOfObjects(string directoryPath)
        {
            var objectCatalog = new Dictionary<string, string[]>();
            var directoriesInRoot =Directory.GetDirectories( directoryPath);
            foreach (var aDirectory in directoriesInRoot)
            {
                var fileInfo = new FileInfo(aDirectory);
              
                var filesInDirectory = Directory.GetFiles(aDirectory);
                if (filesInDirectory.Length>0)
                {
                    var tmpFiles=new string[filesInDirectory.Length];
                   for(int i=0;i<filesInDirectory.Length;i++)
                   {
                       var tmpInfo = new FileInfo(filesInDirectory[i]);
                       tmpFiles[i] = tmpInfo.Name;

                   } 
                    objectCatalog.Add(fileInfo.Name, tmpFiles);
                }
            
            }
            return objectCatalog;
        }
      
        public string DeleteBook(string fileToDelete, string activeUser)
        {
            var mssg="";
            var pathToFile = string.Format(
                "{0}{1}/Books/{2}", GlobalVariables.ConfigResource("UsersDirectory"), activeUser, fileToDelete.Trim());
            try
            {
                File.Delete(
                    HttpContext.Current.Server.MapPath(pathToFile));
            }
            catch (IOException e)
            {
                mssg = e.Message;
            }
            return mssg;
        }
    }
}
