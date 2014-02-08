using System.Web;


namespace BookWriterTool.Helpers
{
    using System;
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
            string[] filesPaths =
                Directory.GetFiles(
                    HttpContext.Current.Server.MapPath(GlobalVariables.ConfigResource("TemplateDirectory")));

            return filesPaths;
        }

        public string[] GetListOfUserBooks(string user)
        {
            string[] directoriesPaths =
                Directory.GetDirectories(
                    HttpContext.Current.Server.MapPath(
                        GlobalVariables.ConfigResource("UsersDirectory") + user + "/Books"));

            return directoriesPaths;
        }

        public void AddUserFolder(string folderName)
        {
            var actualDirectory =
                HttpContext.Current.Server.MapPath(GlobalVariables.ConfigResource("UsersDirectory") + folderName);
            if (!Directory.Exists(actualDirectory)) Directory.CreateDirectory(actualDirectory);
        }

        public List<string> GetListOfUserBooksRelativePath(string user)
        {
            var physicalPaths =
                Directory.GetDirectories(
                    HttpContext.Current.Server.MapPath(
                        GlobalVariables.ConfigResource("UsersDirectory") + user + "/Books"));
            var relativePaths = new List<string>();
            foreach (var path in physicalPaths)
            {
                var fileInfo = new FileInfo(path);
                var fileName = fileInfo.Name;
                relativePaths.Add(fileName);
            }
            return relativePaths;
        }

        public List<BookModel> GetListOf2DCharacter(string bookName, string user)
        {
            var aListObjects = new List<BookModel>();
            string[] directoryPaths = null;
            directoryPaths =
                Directory.GetDirectories(
                    HttpContext.Current.Server.MapPath(
                        GlobalVariables.ConfigResource("UsersDirectory") + user + "/Books/" + bookName
                        + GlobalVariables.ConfigResource("Character2DRes")));

            if (directoryPaths.Length != 0)
            {
                foreach (var directoryPath in directoryPaths)
                {

                    var filesPaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(directoryPath));
                    var lastFolderName = this.GetFolderName(directoryPath);
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

            }
            return aListObjects;
        }

        public string GetFolderName(string aFile)
        {
            string folderName =
                Path.GetFileName(
                    HttpContext.Current.Server.MapPath(Path.GetDirectoryName(HttpContext.Current.Server.MapPath(aFile))));
            return folderName;
        }

        public void CopyBookToPublic(string sourcePath, string destinationPath)
        {
            var sourceRelPath = HttpContext.Current.Server.MapPath(sourcePath);
            var destinationRelPath = HttpContext.Current.Server.MapPath(destinationPath);
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceRelPath);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: " + sourcePath);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destinationRelPath))
            {
                Directory.CreateDirectory(destinationRelPath);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destinationRelPath, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destinationRelPath, subdir.Name);
                this.CopyBookToPublic(destinationPath+"/"+subdir.Name, temppath);
            }
        }


public string AddNewBook(string newFileName, string userName)
        {
            var mssg = "";

            try
            {
                if ((newFileName != null) || (userName != string.Empty))
                {
                    var actualDirectory = HttpContext.Current.Server.MapPath(GlobalVariables.ConfigResource("UsersDirectory") + userName + "/Books/");
                    if (!Directory.Exists(actualDirectory + newFileName))
                    {
                        Directory.CreateDirectory(actualDirectory + newFileName);
                    }

                    if (
                        !File.Exists(actualDirectory + newFileName + "/" + newFileName + ".xml"))
                    {
                        //Create new xml file from template
                        File.Copy(
                            HttpContext.Current.Server.MapPath(
                                GlobalVariables.ConfigResource("TemplateDirectory") + "empty.xml"), actualDirectory + newFileName + "/" + newFileName + ".xml");
                        //Create new image
                        File.Copy(
                            HttpContext.Current.Server.MapPath(
                                GlobalVariables.ConfigResource("Images") + "noimage.jpg"), actualDirectory + newFileName + "/" + newFileName + ".jpg");
                        //Create background folder
                        Directory.CreateDirectory(
                               actualDirectory + newFileName + "/Generic/background/");
                        //Create character folders
                        foreach (var genericCharDir in GlobalVariables.GenericCharacterDirectory())
                        {
                            foreach (var dirName in genericCharDir)
                            {
                                Directory.CreateDirectory(
                                    actualDirectory + newFileName + "/Generic/character/" + dirName);
                            }
                        }
                        //Create Character2D folders
                        foreach (KeyValuePair<string, List<string[]>> genericChar2DDir in GlobalVariables.GenericCharacter2Directory())
                        {
                            foreach (var dirName in genericChar2DDir.Value)
                            {
                                var rootDirectory = actualDirectory + newFileName + "/Generic/character2d/"
                                                      + genericChar2DDir.Key + "/";
                                foreach (var subdirectory in dirName)
                                {
                                    Directory.CreateDirectory(rootDirectory + subdirectory);
                                }
                            }

                        }
                    }
                    else
                    {
                        throw new IOException("ERROR:This file exist already.Please!! Choose another name. ");
                    }
                }
                else
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
            var backgroundFiles = new List<string>();
            var filesInDirectory = Directory.GetFiles(HttpContext.Current.Server.MapPath(directoryPath));

            if (filesInDirectory.Length > 0)
            {
                foreach (var fileName in filesInDirectory)
                {
                    var fileinfo = new FileInfo(fileName);
                    backgroundFiles.Add(directoryPath + fileinfo.Name);
                }
            }
            return backgroundFiles;
        }

        public Dictionary<string, string[]> GetListOfObjects(string directoryPath)
        {

            var objectCatalog = new Dictionary<string, string[]>();
            var directoriesInRoot = Directory.GetDirectories(HttpContext.Current.Server.MapPath(directoryPath));

            foreach (var aDirectory in directoriesInRoot)
            {
                var fileInfo = new FileInfo(aDirectory);

                var filesInDirectory = Directory.GetFiles(aDirectory);
                if (filesInDirectory.Length <= 0)
                {
                    continue;
                }
                var tmpFiles = new string[filesInDirectory.Length];
                for (var i = 0; i < filesInDirectory.Length; i++)
                {
                    var tmpInfo = new FileInfo(filesInDirectory[i]);
                    var actualDirectory = tmpInfo.Directory;

                    if (actualDirectory != null)
                    {
                        tmpFiles[i] = string.Format("{0}{1}/{2}", directoryPath, actualDirectory.Name, tmpInfo.Name);
                    }
                }
                objectCatalog.Add(fileInfo.Name, tmpFiles);
            }
            return objectCatalog;
        }

        public string DeleteBook(string fileToDelete, string activeUser)
        {
            var mssg = "";
            var pathToFile = string.Format(
                "{0}{1}/Books/{2}", GlobalVariables.ConfigResource("UsersDirectory"), activeUser, fileToDelete.Trim());
            try
            {
                Directory.Delete(HttpContext.Current.Server.MapPath(pathToFile), true);
            }
            catch (IOException e)
            {
                mssg = e.Message;
            }
            return mssg;
        }
    }
}
