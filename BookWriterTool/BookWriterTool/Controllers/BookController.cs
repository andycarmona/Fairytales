using System.Web.Mvc;

using System.Net.Sockets;

namespace BookWriterTool.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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

        Boolean isConnected;

        public BookController()
            : this(new BookRepository())
        {

        }
        public BookController(IBookRepository bookRepository)
        {

            aBookRepository = bookRepository;
            this.fileHandler = new FileOperations();

        }

        private void CheckConnection()
        {

            var tcpClient = new TcpClient();
            tcpClient.Connect("www.google.com", 80);
            isConnected = tcpClient.Connected;
        }
        public ActionResult FakeLogin(string userName)
        {
            this.CheckConnection();
            var httpSessionStateBase = this.HttpContext.Session;
            if ((httpSessionStateBase != null) || (!isConnected))
            {
                httpSessionStateBase["username"] = userName;
                return RedirectToAction("EditBook");
            }
            throw new Exception("message");
        }

        public ActionResult GetChosenBook(string fileName)
        {
            ViewBag.statusMsg = "No message";
            Session["ActualFile"] = fileName;
            return this.RedirectToAction("EditBook");

        }

        public ActionResult AddNewBook(string newFileName)
        {

            systemMssg = this.fileHandler.AddNewBook(newFileName);
            ViewBag.statusMsg = systemMssg;
            return RedirectToAction("EditBook");
        }



    /*    public PartialViewResult GetAvailableBooks()
        {
            string[] listOfBooks = null;
            systemMssg = "";
            if (Session["username"] != null)
            {
                activeUser = (string)this.Session["username"];
                try
                {

                    listOfBooks = this.fileHandler.GetListOfUserFiles(activeUser);
              

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
            ViewBag.arrayBooks = listOfBooks;
            return this.PartialView("ListOfBooks", listOfBooks);
        }*/

        public ActionResult EditBook()
        {
            systemMssg = "";
            if (Session["username"] != null)
            {
                activeUser = (string)this.Session["username"];

                try
                {
                    string[] listOfBooks = this.fileHandler.GetListOfUserFiles(activeUser);
                    var fileName = (string)Session["ActualFile"];
                    aBookRepository.SetActualFile(fileName);
                    if(fileName!=null)
                    aBook = this.aBookRepository.GetAllContent();
                    ViewBag.arrayBooks = listOfBooks;

                    return this.View(aBook);
                }
                catch (Exception e)
                {
                    systemMssg = e.Message;
                }
            }
            return this.View();
        }

        [HttpPost]
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
        }

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
            if (Session["ActualFile"] != null)
            {
                var fileName = (string)this.Session["ActualFile"];

                statusMsg = aBookRepository.AddBackgroundToContent(frameDescriptionArray, fileName);

            }
            return Json(statusMsg);
        }

        [HttpPost]
        public JsonResult UpdateObjectPosition(BookModel frameDescriptionArray)
        {
            string statusMsg = "";
            if (Session["ActualFile"] != null)
            {
                var fileName = (string)this.Session["ActualFile"];

                statusMsg = aBookRepository.UpdateObjectPosition(frameDescriptionArray, fileName);

            }
            return Json(statusMsg);
        }

        [HttpPost]
        public JsonResult AddFrame(BookModel frameDescriptionArray)
        {
            string statusMsg = "";
            if (Session["ActualFile"] != null)
            {
                var fileName = (string)this.Session["ActualFile"];

                statusMsg = aBookRepository.AddFrame(frameDescriptionArray, fileName);

            }
            return Json(statusMsg);
        }

        public JsonResult AddObjectToContent(BookModel model)
        {
            var statusMsg = "";
            if (Session["ActualFile"] != null)
            {

                var fileName = (string)this.Session["ActualFile"];

                statusMsg = aBookRepository.AddObjectToContent(model, fileName);


            }
            return Json(statusMsg);
        }

        public JsonResult AddCharacter2DToContent(BookModel model)
        {
            var statusMsg = "";
            if (Session["ActualFile"] != null)
            {

                var fileName = (string)this.Session["ActualFile"];

                statusMsg = aBookRepository.AddCharacter2DToContent(model, fileName);


            }
            return Json(statusMsg);
        }

        public JsonResult AddExpressionToContent(BookModel model)
        {
            var statusMsg = "";
            if (Session["ActualFile"] != null)
            {

                var fileName = (string)this.Session["ActualFile"];

                statusMsg = aBookRepository.AddGenericObjectToContent(model, fileName);


            }
            return Json(statusMsg);
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
