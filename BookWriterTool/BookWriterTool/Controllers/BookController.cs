using System.Web.Mvc;

namespace BookWriterTool.Controllers
{
    using System;
    using System.IO;

    using BookWriterTool.Helpers;
    using BookWriterTool.Models;
    using BookWriterTool.Repositories;

    public class BookController : Controller
    {
        private readonly IBookRepository aBookRepository;

        private readonly FileOperations fileHandler;

        private book aBook;

        private string activeUser;

        public BookController()
            : this(new BookRepository())
        {

        }
        public BookController(IBookRepository bookRepository)
        {

            aBookRepository = bookRepository;
            this.fileHandler = new FileOperations();

        }

        public ActionResult FakeLogin(string userName)
        {
            var httpSessionStateBase = this.HttpContext.Session;
            if (httpSessionStateBase != null)
            {
                httpSessionStateBase["username"] = userName;

            }
            return RedirectToAction("EditBook");
        }

        public ActionResult GetChosenBook(string fileName)
        {
            string msg = aBookRepository.SetActualFile(fileName);
            aBook = this.aBookRepository.GetAllContent();
            ViewBag.statusMsg = msg;
            Session["ActualFile"] = fileName;
            TempData["ActualBook"] = aBook;
            return this.RedirectToAction("EditBook");

        }

        public PartialViewResult GetAvailableBooks(string fileOption)
        {
            string statusMsg = "Got all books";
            string[] listOfBooks = null;
            bool newBook = true;
            if (Session["username"] != null)
            {
                activeUser = (string)this.Session["username"];
            }
            else
            {
                statusMsg = "Couldn't find files";
            }

            try
            {
                if (fileOption.Equals("loadBook"))
                {
                    listOfBooks = this.fileHandler.GetListOfUserFiles(activeUser);
                    var temp = new string[listOfBooks.Length + 1];
                    for (int i = 0; i < listOfBooks.Length; i++)
                    {
                        temp[i + 1] = listOfBooks[i];
                    }
                    listOfBooks = temp;
                    newBook = false;
                }
                else if (fileOption.Equals("newBook"))
                {
                    listOfBooks = this.fileHandler.GetListOfTemplates();

                }
            }
            catch (Exception e)
            {
                statusMsg = e.Message;
            }
            ViewBag.statusMsg = statusMsg;
            ViewBag.newBook = newBook;
            ViewBag.listBook = listOfBooks;
            return this.PartialView("ListOfBooks", listOfBooks);
        }

        [HttpPost]
        public JsonResult Test(string fileOption)
        {
            var question = new Question { Title = "What is a the Matrix ? " + fileOption };
            return Json(question);
        }

        public class Question
        {
            public string Title { get; set; }
        }

        public ActionResult EditBook()
        {
            string[] listOfBooks = this.fileHandler.GetListOfUserFiles(activeUser);

            if (TempData["ActualBook"] != null)
            {
                aBook = TempData["ActualBook"] as book;
            }
            ViewBag.arrayBooks = listOfBooks;

            return this.View(aBook);
        }

        [HttpPost]
        public ActionResult AddPage(string[] chapterNumber)
        {
            string statusMsg = "Page added";
            if (Session["ActualFile"] != null)
            {

                var fileName = (string)this.Session["ActualFile"];
                try
                {
                    book anotherBook = this.aBookRepository.AddPage(chapterNumber[0], fileName);
                    return this.RedirectToActionPermanent("EditBook", anotherBook);
                }
                catch (Exception e)
                {
                    statusMsg = e.Message;
                }

            }
            return Json(statusMsg);
        }

        [HttpPost]
        public JsonResult AddContentToFrame(string[] frameDescriptionArray)
        {
            string statusMsg = "Changed frame in page";
            if (Session["ActualFile"] != null)
            {
                try
                {
                    var fileName = (string)this.Session["ActualFile"];

                    aBookRepository.AddContentToFrame(frameDescriptionArray, fileName);
                }
                catch (IOException e)
                {
                    statusMsg = e.Message;
                }

            }
            return Json(statusMsg);
        }

        public ActionResult ViewBook(string bookId)
        {
            book anotherBook = this.aBookRepository.GetAllContent();
            return this.View(anotherBook);
        }

    }
}
