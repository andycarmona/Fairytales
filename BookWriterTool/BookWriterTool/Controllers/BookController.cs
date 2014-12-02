using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

using BookWriterTool.Helpers;
using BookWriterTool.Models;
using BookWriterTool.Repositories;
using System.Net.Sockets;

namespace BookWriterTool.Controllers
{


    [HandleError]
    public class BookController : Controller
    {
        private readonly IBookRepository aBookRepository;

        private readonly FileOperations fileHandler;

        private book aBook;

        private string activeUser;


        private string systemMssg;

        //Boolean isConnected;

        public BookController()
            : this(new BookRepository())
        {

        }
        public BookController(IBookRepository bookRepository)
        {

            aBookRepository = bookRepository;
            this.fileHandler = new FileOperations();
         

        }

        private bool CheckConnection()
        {

            var tcpClient = new TcpClient();
            var statusConn = true;
            try
            {
                tcpClient.Connect("http://www.google.com", 80);
            }
            catch (Exception e)
            {
                statusConn = tcpClient.Connected;
            }

            return statusConn;
        }

       


        public ActionResult AddNewBook(string newFileName)
        {

            activeUser = User.Identity.Name;
            string[] listOfBooks = null;
            systemMssg = string.Empty;

            if (activeUser != null)
            {
              
                try
                {
                    systemMssg = this.fileHandler.AddNewBook(newFileName, activeUser);
                    listOfBooks = this.fileHandler.GetListOfUserBooks(activeUser);
                    systemMssg = aBookRepository.SetActualFile(String.Format("{0}{1}/Books/{2}/{3}.xml", GlobalVariables.ConfigResource("UsersDirectory"), activeUser, newFileName, newFileName));
                    if (newFileName != null)
                        aBook = this.aBookRepository.GetAllContent();

                }
                catch (Exception e)
                {
                    systemMssg = e.Message;
                }
            }
            else
            {
                systemMssg = "Couldn't add new book.";
            }
            ViewBag.statusMsg = systemMssg;
            ViewData["listOfBooks"] = listOfBooks;
            return this.PartialView("ListOfBooks");
        }


        public ActionResult DeleteBook(string fileToDelete)
        {
            string[] listOfBooks = null;
            systemMssg = string.Empty;
            activeUser = User.Identity.Name;
            if (activeUser != null)
            {
                try
                {

                    systemMssg = this.fileHandler.DeleteBook(fileToDelete, activeUser);
                    listOfBooks = this.fileHandler.GetListOfUserBooks(activeUser);
                }
                catch (Exception e)
                {
                    systemMssg = e.Message;
                }
            }
            else
            {
                systemMssg = "Couldn't find files";
            }
            ViewBag.statusMsg = systemMssg;
            ViewData["listOfBooks"] = listOfBooks;
            return this.PartialView("ListOfBooks");

        }


        public ActionResult EditBook()
        {
            var fileName = string.Empty;
            var actualDirectory = string.Empty;
            ViewBag.statusMsg = "No message";
            activeUser = "andresc";

            if ((Session["ActualFile"] != null) && (Session["ActualDirectory"] != null))
            {
                fileName = (string)Session["ActualFile"]; actualDirectory = (string)Session["ActualDirectory"];
            }

            systemMssg = string.Empty;
            if (activeUser != null)
            {
                try
                {
                    var listOfBooks = this.fileHandler.GetListOfUserBooks(activeUser);


                    systemMssg = aBookRepository.SetActualFile(actualDirectory + "/" + fileName);
                    if (fileName != null)
                        aBook = this.aBookRepository.GetAllContent();
                    var title = fileName.Split('.');
                    ViewBag.fileName = title[0];
                    ViewBag.arrayBooks = listOfBooks;
                    ViewBag.statusMsg = systemMssg;
                    ViewBag.ObjectList = this.GetObjectsInFolder(actualDirectory);
                    ViewBag.BackgroundList = this.GetBackgroundInFolder(actualDirectory);
                    return this.View(aBook);
                }
                catch (Exception e)
                {
                    systemMssg = e.Message;
                }
            }
            ViewBag.statusMsg = systemMssg;
            return this.View();
        }

        public ViewResult GetListOfBooks()
        {
            var listOfBooks = this.fileHandler.GetAllBooks();
            ViewBag.arrayBooks = listOfBooks.Where(book => book != "").ToList();
            return this.View();
        }

        public ActionResult ViewBookFlip(string fileName)
        {
            systemMssg = string.Empty;
            activeUser = "andresc";
            if (activeUser != null)
            {
                if (Session["ActualDirectory"] == null)
                {
                    Session["ActualDirectory"] = String.Format("../Users/{0}/Books/{1}", activeUser, fileName);
                }
                try
                {
                    string actualPath = String.Format("../Users/{0}/Books/{1}/{1}.xml", activeUser, fileName);
                    systemMssg = aBookRepository.SetActualFile(actualPath);
                    if (fileName != null)
                        aBook = this.aBookRepository.GetAllContent();
                    ViewBag.fileName = fileName;
                    //  ViewBag.arrayBooks = listOfBooks;
                    ViewBag.statusMsg = systemMssg;
                    return this.View(aBook);
                }
                catch (Exception e)
                {
                    systemMssg = e.Message;
                }
            }
            ViewBag.statusMsg = systemMssg;
            return this.View(aBook);
        }

        public ActionResult ViewBook(string fileName)
        {
        
            var actualDirectory = string.Empty;
            ViewBag.statusMsg = "No message";
            activeUser = "andresc";

            if ((Session["ActualFile"] != null) && (Session["ActualDirectory"] != null))
            {
                fileName = (string)Session["ActualFile"]; actualDirectory = (string)Session["ActualDirectory"];
            }

            systemMssg = string.Empty;
            if (activeUser != null)
            {
                try
                {
                    var listOfBooks = this.fileHandler.GetListOfUserBooks(activeUser);


                    systemMssg = aBookRepository.SetActualFile(actualDirectory + "/" + fileName);
                    if (fileName != null)
                        aBook = this.aBookRepository.GetAllContent();
                    var title = fileName.Split('.');
                    ViewBag.fileName = title[0];
                    ViewBag.arrayBooks = listOfBooks;
                    ViewBag.statusMsg = systemMssg;
                    //ViewBag.ObjectList = this.GetObjectsInFolder(actualDirectory);
                    //ViewBag.BackgroundList = this.GetBackgroundInFolder(actualDirectory);
                    this.GetChosenBook(
                        GlobalVariables.ConfigResource("UsersDirectory") + activeUser + "/Books/" + fileName,
                        fileName);
                    return this.View(aBook);
                }
                catch (Exception e)
                {
                    systemMssg = e.Message;
                }
            }
            ViewBag.statusMsg = systemMssg;
            return this.View();
        }

        public ActionResult GetChosenBook(string path, string fileName)
        {
            var actualDirectory = path;
            ViewBag.statusMsg = "No message";
            Session["ActualFile"] = fileName + ".xml";
            Session["ActualDirectory"] = actualDirectory;

            return this.RedirectToAction("EditBook");

        }
        [HttpPost]
        public ActionResult AddPage(BookModel model)
        {
            systemMssg = string.Empty;


            var targetFile = this.GetTargetFile();
            if (!String.IsNullOrEmpty(targetFile))
            {
                try
                {
                    book anotherBook = this.aBookRepository.AddPage(model.ChapterId, targetFile);
                    return this.RedirectToActionPermanent("EditBook", anotherBook);
                }
                catch (Exception e)
                {
                    systemMssg = e.Message;
                }
            }
            return Json(systemMssg);
        }


        [HttpPost]
        public JsonResult AddBackgroundToFrame(BookModel frameDescriptionArray)
        {
            string statusMsg = string.Empty;
            var targetFile = this.GetTargetFile();
            if (!String.IsNullOrEmpty(targetFile))
                statusMsg = aBookRepository.AddBackgroundToContent(frameDescriptionArray, targetFile);
            return Json(statusMsg);
        }

        [HttpPost]
        public JsonResult UpdateObjectPosition(BookModel frameDescriptionArray)
        {
            string statusMsg = string.Empty;
            var targetFile = this.GetTargetFile();
            if (!String.IsNullOrEmpty(targetFile))
                statusMsg = aBookRepository.UpdateObjectPosition(frameDescriptionArray, targetFile);
            return Json(statusMsg);
        }


        [HttpPost]
        public JsonResult AddFrame(BookModel frameDescriptionArray)
        {
            string statusMsg = string.Empty;
            var targetFile = this.GetTargetFile();
            if (!String.IsNullOrEmpty(targetFile))
                statusMsg = aBookRepository.AddFrame(frameDescriptionArray, targetFile);


            return Json(statusMsg);
        }

        private string GetTargetFile()
        {
            string targetFile = string.Empty;
            if ((Session["ActualFile"] != null) && (Session["ActualDirectory"] != null))
            {
                var fileName = (string)this.Session["ActualFile"];
                var actualDirectory = (string)this.Session["ActualDirectory"];
                targetFile = String.Format("{0}/{1}", actualDirectory, fileName);
            }
            return targetFile;
        }


        public JsonResult AddObjectToContent(BookModel model)
        {
            var statusMsg = string.Empty;
            var targetFile = this.GetTargetFile();
            if (!String.IsNullOrEmpty(targetFile))
                statusMsg = aBookRepository.AddObjectToContent(model, targetFile);
            return Json(statusMsg);
        }

        private string GetPathWithoutFile(string input)
        {
            int index = input.LastIndexOf("/", System.StringComparison.Ordinal);
            if (index > 0)
                input = input.Substring(0, index);
            return input;
        }


        public Dictionary<string, string[]> GetObjectsInFolder(string actualDirectory)
        {
            var resultObjects = new Dictionary<string, string[]>();
            if ((actualDirectory != null) && (!String.IsNullOrEmpty(actualDirectory)))
            {

                Dictionary<string, string[]> characterObj =
                    this.fileHandler.GetListOfObjects(actualDirectory + GlobalVariables.ConfigResource("CharacterRes"));

                Dictionary<string, string[]> character2DObj =
                    fileHandler.GetListOfObjects(actualDirectory + GlobalVariables.ConfigResource("Character2DRes"));

                resultObjects = characterObj.Union(character2DObj).ToDictionary(k => k.Key, v => v.Value);

            } return resultObjects;
        }


        public List<string> GetBackgroundInFolder(string actualDirectory)
        {
            var backgroundFiles = new List<string>();
            if (!String.IsNullOrEmpty(actualDirectory))
            {

                backgroundFiles =
                       this.fileHandler.GetListOfBackgrounds(actualDirectory + GlobalVariables.ConfigResource("BackgroundRes"));
            }
            return backgroundFiles;
        }


        [HttpPost]
        public ActionResult UploadObject(string selectedFolder, HttpPostedFileBase file)
        {

            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath(selectedFolder), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("EditBook");
        }

       

        public JsonResult AddSpeechBubbleObject(BookModel model)
        {
            var statusMsg = string.Empty;
            if (Session["ActualDirectory"] != null)
            {
                var actualDirectory = (string)Session["ActualDirectory"];
                var fileName = (string)this.Session["ActualFile"];

                statusMsg = aBookRepository.AddSpeechBubbleObject(model, actualDirectory + "/" + fileName);


            }
            return Json(statusMsg);
        }

        [HttpPost]
        public string AddTextToBubble(string model, string componentId, string type, string form)
        {

            if (Session["ActualDirectory"] != null)
            {
                var actualDirectory = (string)Session["ActualDirectory"];
                var fileName = (string)this.Session["ActualFile"];

                aBookRepository.AddTextToBubble(model, componentId, actualDirectory + "/" + fileName, type, form);


            }
            var replacedModel = Regex.Replace(model, "\n", "<br />");
            return replacedModel;
        }

        [HttpPost]
        public string AddTextToContent(string model, string componentId, string type, string form)
        {

            if (Session["ActualDirectory"] != null)
            {
                var actualDirectory = (string)Session["ActualDirectory"];

                var fileName = (string)this.Session["ActualFile"];

                aBookRepository.AddTextToContent(model, componentId, actualDirectory + "/" + fileName, type, form);


            }
            var replacedModel = Regex.Replace(model, "\n", "<br />");
            return replacedModel;
        }

        public JsonResult DeleteObjectFromContent(BookModel model)
        {
            var statusMsg = string.Empty;
            if ((Session["ActualFile"] != null) && (Session["ActualDirectory"] != null))
            {

                var fileName = (string)this.Session["ActualFile"];
                var actualDirectory = (string)Session["ActualDirectory"];

                statusMsg = aBookRepository.DeleteObjectFromContent(model, actualDirectory + "/" + fileName);


            }
            return Json(statusMsg);
        }

    }
}
