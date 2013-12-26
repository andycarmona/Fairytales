using System.Web.Mvc;

using System.Net.Sockets;

namespace BookWriterTool.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;

    using BookWriterTool.Helpers;
    using BookWriterTool.Models;
    using BookWriterTool.Repositories;

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

        public ActionResult FakeLogin(string userName)
        {
            var statusConn = this.CheckConnection();
            var httpSessionStateBase = this.HttpContext.Session;
            if ((httpSessionStateBase != null) && (!statusConn))
            {
                httpSessionStateBase["username"] = userName;
                return RedirectToAction("EditBook");
            }
            else return RedirectToAction("Index", "Error");
            throw new Exception("message");
        }

        public ActionResult GetChosenBook(string path, string fileName)
        {
            var actualDirectory = path;
            ViewBag.statusMsg = "No message";
            Session["ActualFile"] = fileName + ".xml";
            Session["ActualDirectory"] = actualDirectory;

            return this.RedirectToAction("EditBook");

        }

        public ActionResult AddNewBook(string newFileName)
        {


            string[] listOfBooks = null;
            systemMssg = "";
            if (Session["username"] != null)
            {
                activeUser = (string)this.Session["username"];
                try
                {
                    systemMssg = this.fileHandler.AddNewBook(newFileName, activeUser);
                    listOfBooks = this.fileHandler.GetListOfUserBooks(activeUser);
                    systemMssg = aBookRepository.SetActualFile(String.Format("{0}{1}/Books/{2}.xml", GlobalVariables.ConfigResource("UsersDirectory"), activeUser, newFileName));
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
            systemMssg = "";
            if (Session["username"] != null)
            {
                activeUser = (string)this.Session["username"];
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
            var fileName = "";
            var actualDirectory = "";

            if (Session["ActualFile"] != null)
            {
                fileName = (string)Session["ActualFile"];
            }
            if (Session["ActualDirectory"] != null)
            {
                actualDirectory = (string)Session["ActualDirectory"];
            }
            systemMssg = "";
            if (Session["username"] != null)
            {
                activeUser = (string)this.Session["username"];

                try
                {
                    string[] listOfBooks = this.fileHandler.GetListOfUserBooks(activeUser);


                    systemMssg = aBookRepository.SetActualFile(actualDirectory + "/" + fileName);
                    if (fileName != null)
                        aBook = this.aBookRepository.GetAllContent();
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

     /*   [HttpPost]
        public JsonResult GetListObjectsInframe(string resultFrame)
        {
            string[] splitResult = resultFrame.Split('-');

            string chapterId = splitResult[0];
            string pageId = splitResult[1];
            string frameId = splitResult[2];
            string target = splitResult[3];

            var listOfObject = new List<bookChapterPageFrameContentObject>();
            string msg = "";

            if (Session["ActualFile"] != null)
            {
                var fileName = (string)this.Session["ActualFile"];

                msg = aBookRepository.SetActualFile(fileName);

                book actualBook = this.aBookRepository.GetAllContent();
                listOfObject.AddRange(from aChapter in actualBook.chapters
                                      where aChapter.id == chapterId
                                      from page in aChapter.pages
                                      where page.id == pageId
                                      from frame in page.frames
                                      where frame.id == frameId
                                      where frame.contents != null
                                      from content in frame.contents
                                      where content.target == target
                                      from Object in content.objects
                                      select Object);


            }

            return Json(new
            {
                msg,
                ObjectInquiryView = this.RenderPartialView("ObjectListConfig", listOfObject),

            });
        }*/

        [HttpPost]
        public ActionResult AddPage(BookModel model)
        {
            systemMssg = "";
            if (Session["ActualFile"] != null)
            {

                var fileName = (string)this.Session["ActualFile"];
                try
                {
                    book anotherBook = this.aBookRepository.AddPage(model.ChapterId, fileName);
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
            string statusMsg = "";
                     var targetFile = this.GetTargetFile();
            if (!String.IsNullOrEmpty(targetFile))
                statusMsg = aBookRepository.AddBackgroundToContent(frameDescriptionArray, targetFile);
            return Json(statusMsg);
        }

        [HttpPost]
        public JsonResult UpdateObjectPosition(BookModel frameDescriptionArray)
        {
            string statusMsg = "";
            var targetFile = this.GetTargetFile();
            if (!String.IsNullOrEmpty(targetFile))
                statusMsg = aBookRepository.UpdateObjectPosition(frameDescriptionArray, targetFile);
            return Json(statusMsg);
        }

        [HttpPost]
        public JsonResult AddFrame(BookModel frameDescriptionArray)
        {
            string statusMsg = "";
            var targetFile = this.GetTargetFile();
            if (!String.IsNullOrEmpty(targetFile))
                statusMsg = aBookRepository.AddFrame(frameDescriptionArray, targetFile);


            return Json(statusMsg);
        }
        public string GetTargetFile()
        {
            string targetFile = "";
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
            var statusMsg = "";
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
            var statusMsg = "";
            if (Session["ActualFile"] != null)
            {

                var fileName = (string)this.Session["ActualFile"];

                statusMsg = aBookRepository.AddSpeechBubbleObject(model, fileName);


            }
            return Json(statusMsg);
        }
        [HttpPost]
        public string AddTextToBubble(string model, string componentId, string type, string form)
        {
            var statusMsg = "";
            if (Session["ActualFile"] != null)
            {

                var fileName = (string)this.Session["ActualFile"];

                statusMsg = aBookRepository.AddTextToBubble(model, componentId, fileName, type, form);


            }
            var replacedModel = Regex.Replace(model, "\n", "<br />");
            return replacedModel;
        }

        [HttpPost]
        public string AddTextToContent(string model, string componentId, string type, string form)
        {
            var statusMsg = "";
            if (Session["ActualFile"] != null)
            {

                var fileName = (string)this.Session["ActualFile"];

                statusMsg = aBookRepository.AddTextToContent(model, componentId, fileName, type, form);


            }
            var replacedModel = Regex.Replace(model, "\n", "<br />");
            return replacedModel;
        }

        public JsonResult DeleteObjectFromContent(BookModel model)
        {
            var statusMsg = "";
            if (Session["ActualFile"] != null)
            {

                var fileName = (string)this.Session["ActualFile"];

                statusMsg = aBookRepository.DeleteObjectFromContent(model, fileName);


            }
            return Json(statusMsg);
        }

    }
}
