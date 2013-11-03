using System.Web.Mvc;

namespace BookWriterTool.Controllers
{

    using BookWriterTool.Helpers;
    using BookWriterTool.Models;
    using BookWriterTool.Repositories;

    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        private FileOperations FileHandler;


        public BookController()
            : this(new BookRepository())
        {
            
        }
        public BookController(IBookRepository bookRepository)
        {
           
            _bookRepository = bookRepository; 
             FileHandler= new FileOperations();
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
            string msg = _bookRepository.SetActualFile(HttpContext.Server.MapPath(fileName));
           
               return this.RedirectToAction("EditBook",new{statusMsg=msg});

        }
  
        public PartialViewResult GetAvailableBooks(string fileOption)
        {
            string[] listOfBooks=null;
            var activeUser=(string)this.Session["username"];
            if (Session["username"] == null)
            {
                activeUser = "andresc";
            }

            if (fileOption.Equals("loadBook"))
            {
                listOfBooks = FileHandler.GetListOfUserFiles(activeUser);
            }
            else if (fileOption.Equals("newBook"))
            {
                listOfBooks = FileHandler.GetListOfTemplates();
            }
            
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
            book aBook = this._bookRepository.GetAllContent();
            return this.View(aBook);
        }
        [HttpPost]
        public ActionResult AddPage(string[] chapterNumber)
        {
            book aBook = this._bookRepository.AddPage(chapterNumber[0]);
            
            return this.RedirectToActionPermanent("EditBook",aBook);
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
