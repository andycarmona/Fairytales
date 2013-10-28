using System.Web.Mvc;

namespace BookWriterTool.Controllers
{
    using BookWriterTool.Models;
    using BookWriterTool.Repositories;

    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        

        public BookController()
            : this(new BookRepository())
        {
            
        }
        public BookController(IBookRepository bookRepository)
        {

            _bookRepository = bookRepository;
        }
        
        public ActionResult Index()
        {
            book  aBook = _bookRepository.GetAllContent();
       
            return View(aBook);
        }
        public ActionResult EditBook(string bookId)
        {
            book aBook = _bookRepository.GetAllContent();
            return this.View(aBook);
        }
        public ActionResult ViewBook(string bookId)
        {
            book aBook = _bookRepository.GetAllContent();
            return this.View(aBook);
        }

        public ActionResult GetChapter()
        {
            bookChapter aChapter = _bookRepository.GetChapterById("chapter1");
            string pageName = "No page";
            if (aChapter != null)
            {
                pageName = aChapter.pages[0].id;
            }
            ViewBag.PageName = pageName;
            return View(); 
        }
        public ActionResult Edit()
        {
            _bookRepository.EditChapter("chapter1");

            return this.View();
        }

    }
}
