namespace BookWriterTool.Controllers
{
    using System.Web.Mvc;
    using BookWriterTool.Models;
    using BookWriterTool.Repositories;

    public class HomeController : Controller
    {
        public HomeController()
            : this(new BookRepository())
        {
        }

        public HomeController(IBookRepository bookRepository)
        {
        }

        public ActionResult Index(string user)
        {
            return RedirectToAction("EditBook", "Book");
        }
    }
}
