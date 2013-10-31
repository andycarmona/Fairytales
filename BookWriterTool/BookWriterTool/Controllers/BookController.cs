using System.Web.Mvc;

namespace BookWriterTool.Controllers
{
    using BookWriterTool.Models;
    using BookWriterTool.Repositories;

    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public static readonly string actualBook = "/Content/Resources/Users/andresc/bookNoMetadata2.xml";

        public BookController()
            : this(new BookRepository())
        {
            
        }
        public BookController(IBookRepository bookRepository)
        {
           
            this._bookRepository = bookRepository; 
            this._bookRepository.SetActualFile(actualBook);
        }
        
        public ActionResult Index()
        {
            book  aBook = this._bookRepository.GetAllContent();
      
            return this.View(aBook);
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
        public ActionResult AddContentToFrame(string[] frameDescriptionArray)
        {
            book aBook = this._bookRepository.AddContentToFrame(frameDescriptionArray);

            return this.RedirectToActionPermanent("EditBook", aBook);
        }

        [HttpPost]
        public JsonResult EditBookContent(string[] frameDescriptionArray)
        {
            book aBook = this._bookRepository.GetAllContent();
            return this.Json(new { success = true, msg = "Saved ok" });
        }
        public ActionResult ViewBook(string bookId)
        {
            book aBook = this._bookRepository.GetAllContent();
            return this.View(aBook);
        }

        public ActionResult GetChapter()
        {
            bookChapter aChapter = this._bookRepository.GetChapterById("chapter1");
            string pageName = "No page";
            if (aChapter != null)
            {
                pageName = aChapter.pages[0].id;
            }
            this.ViewBag.PageName = pageName;
            return this.View(); 
        }
        public ActionResult Edit()
        {
            this._bookRepository.EditChapter("chapter1");

            return this.View();
        }
    }
}
