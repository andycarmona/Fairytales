using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookWriterTool.Repositories
{
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using BookWriterTool.Helpers;
    using BookWriterTool.Models;

    public class BookRepository:IBookRepository
    {

        private  book aBook;

        private readonly FileOperations handleFiles;
        private string actualBook;

        public BookRepository()
        {
           
    
        }

        public BookRepository(string actualBook)
        {
            // TODO: Complete member initialization
            this.actualBook = actualBook;        
            handleFiles=new FileOperations();
            aBook = handleFiles.SerializeXmlToObject(actualBook);
        }
        

        public bookChapter GetChapterById(string chapterId)
        {
            var aChapter=new bookChapter();
            foreach (bookChapter t in this.aBook.chapters)
            {
                if (t.id == chapterId)
                {
                    aChapter = t;
                }
            }
            return aChapter;
        }
        public book GetAllContent()
        {
            var sw = new StringWriter();
            var xmlSer = new XmlSerializer(typeof(book));
            var noNamespaces = new XmlSerializerNamespaces();
            noNamespaces.Add("", "");
            xmlSer.Serialize(sw, aBook, noNamespaces);
            return aBook;
        }

      /*  public book GetAllContent()
        {
            var serializer = new XmlSerializer(typeof(book));
            try
            {
                var stream = new FileStream(
                    "C:/Users/andresc/Desktop/Fairy-tales/fairytales/examplebook/book.xml", FileMode.Open);
                aBook = serializer.Deserialize(stream) as book; 
                stream.Close();
            }
            catch (Exception e)
            {
                
            }
          
            return aBook;
        }*/

        public void EditChapter(string id)
        {
            XmlDocument xmlDoc= new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(handleFiles.GetActualXml()));
            XmlNodeList chaptersNodes = xmlDoc.SelectNodes("//book/chapters/chapter");
            if (chaptersNodes != null)
            {
                var listOfChaptersId=new List<string>();
                foreach (XmlNode chaptersNode in chaptersNodes)
                {
                    if (chaptersNode.Attributes != null)
                    {
                        listOfChaptersId.Add(chaptersNode.Attributes["id"].Value);
                    }
                }
            }
        }
    }
}