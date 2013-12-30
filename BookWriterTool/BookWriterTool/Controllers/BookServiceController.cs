using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookWriterTool.Helpers;

namespace BookWriterTool.Controllers
{
    public class BookServiceController : ApiController
    {
       private FileOperations fileHandler;
      
        [Authorize]
        [HttpGet]
        public List<string> GetAllBooks(string userName)
        {
            this.fileHandler = new FileOperations();
            var listOfBooks = fileHandler.GetListOfUserBooksRelativePath(userName);
            return listOfBooks;
        }
        [Authorize]
        [AcceptVerbs("GET","POST")]
        public book GetBookModel(string userName,string fileName)
        {
            this.fileHandler = new FileOperations();
            var path = String.Format("~/Users/{0}/Books/{1}/{1}.xml",userName,fileName);
            var bookModel = fileHandler.SerializeXmlToObject(path);
            return bookModel;
        }
    }
}
