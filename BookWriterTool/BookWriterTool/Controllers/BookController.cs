using System.Web.Mvc;

namespace BookWriterTool.Controllers
{
    using System;

    using BookWriterTool.Helpers;
    using BookWriterTool.Models;
    using BookWriterTool.Repositories;

    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        private FileOperations FileHandler;

        private book aBook;

        private string activeUser;

        public BookController()
            : this(new BookRepository())
        {

        }
        public BookController(IBookRepository bookRepository)
        {

            _bookRepository = bookRepository;
            FileHandler = new FileOperations();

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
            string msg = _bookRepository.SetActualFile(fileName);
            aBook = this._bookRepository.GetAllContent();
            ViewBag.statusMsg = msg;
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
                    listOfBooks = FileHandler.GetListOfUserFiles(activeUser);
                    string[] temp = new string[listOfBooks.Length+1];
                    for (int i = 0; i < listOfBooks.Length; i++)
                    {
                        temp[i+1] = listOfBooks[i];
                    }
                    listOfBooks = temp;
                        newBook = false;
                }
                else if (fileOption.Equals("newBook"))
                {
                    listOfBooks = FileHandler.GetListOfTemplates();

                }
            }
            catch (Exception e)
            {
                statusMsg = e.Message;
            }
            ViewBag.statusMsg = statusMsg;
            ViewBag.newBook = newBook;
            ViewBag.listBook = listOfBooks;
            return this.PartialView("ListOfBooks",listOfBooks);
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
            string[] listOfBooks = FileHandler.GetListOfUserFiles(activeUser);

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
            book aBook = this._bookRepository.AddPage(chapterNumber[0]);

            return this.RedirectToActionPermanent("EditBook", aBook);
        }

        [HttpPost]
        public void AddContentToFrame(string[] frameDescriptionArray)
        {
            _bookRepository.AddContentToFrame(frameDescriptionArray);
        }

        public ActionResult ViewBook(string bookId)
        {
            book aBook = this._bookRepository.GetAllContent();
            return this.View(aBook);
        }

    }
}
