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

  
        private readonly FileOperations handleFiles;
        private string actualBook; 
        private  book aBook;
        private int pathFirstPage ;

        public BookRepository()
        {
           
            handleFiles=new FileOperations();
        }
        
        public void SetActualFile(string actualBook)
        {
            this.actualBook = actualBook;
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

        public book AddPage(string chapterId)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(actualBook));
            XmlNode aPageNode;
            XmlNodeList chaptersNodes = xmlDoc.SelectNodes("//book/chapters/chapter");
            if (chaptersNodes != null)
            {
           
                foreach (XmlNode chaptersNode in chaptersNodes)
                {
                    //test
                  //  XmlNodeList temp = chaptersNode.ChildNodes;
                    
                    //test
                    if (chaptersNode.Attributes != null)
                    {
                       if (chaptersNode.Attributes["id"].Value == chapterId)
                       {
                           XmlElement elemPage = xmlDoc.CreateElement("page");
                           XmlElement elemFrames = xmlDoc.CreateElement("frames");
                           XmlElement aFrame = xmlDoc.CreateElement("frame");
                           elemPage.SetAttribute("id", "page3");
                           elemPage.AppendChild(elemFrames);
                           aFrame.SetAttribute("id", "frame1");
                           aFrame.SetAttribute("bordertype", "triangle");
                           elemFrames.AppendChild(aFrame);
                           chaptersNode.ChildNodes[2].AppendChild(elemPage);
                       }


                    }
                }
            }
            xmlDoc.Save(HttpContext.Current.Server.MapPath(actualBook));
            return this.GetAllContent();
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
            xmlDoc.Load(HttpContext.Current.Server.MapPath(actualBook));
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