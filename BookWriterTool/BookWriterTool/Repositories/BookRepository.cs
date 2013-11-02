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

    public class BookRepository : IBookRepository
    {


        private readonly FileOperations handleFiles;
        private string actualBook;
        private book aBook;
        private int pathFirstPage;

        public BookRepository()
        {

            handleFiles = new FileOperations();
        }

        public void SetActualFile(string actualBook)
        {
            this.actualBook = actualBook;
            aBook = handleFiles.SerializeXmlToObject(actualBook);
        }
        public int GetNumberOfpagesInBook()
        {
            bookChapterPage[] apage = aBook.chapters[0].pages;
            return apage.Length;
        }
        public bookChapter GetChapterById(string chapterId)
        {
            var aChapter = new bookChapter();
            foreach (bookChapter t in this.aBook.chapters)
            {
                if (t.id == chapterId)
                {
                    aChapter = t;
                }
            }
            return aChapter;
        }

        public book AddContentsInFrame()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(actualBook));
            XmlNode aPageNode;
            XmlNodeList chaptersNodes = xmlDoc.SelectNodes("//book/chapters/chapter");
            return this.GetAllContent();
        }

        public book AddPage(string chapterId)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(actualBook));
            XmlNode aPageNode;
            XmlNodeList chaptersNodes = xmlDoc.SelectNodes("//book/chapters/chapter");
            XmlElement elemPage = xmlDoc.CreateElement("page");
            XmlElement elemFrames = xmlDoc.CreateElement("frames");
            XmlElement aFrame = xmlDoc.CreateElement("frame");
            aFrame.SetAttribute("id", "frame1");
            aFrame.SetAttribute("bordertype", "triangle");
            XmlNode parentNode = null;
            string pageIdName = string.Format("{0}{1}", "page", (this.GetNumberOfpagesInBook() + 1));
            elemPage.SetAttribute("id", pageIdName);
            elemPage.AppendChild(elemFrames);

            elemFrames.AppendChild(aFrame);
            if (chaptersNodes != null)
            {
                foreach (XmlNode chaptersNode in chaptersNodes)
                {
                    if (chaptersNode.Attributes != null)
                    {
                        if (chaptersNode.Attributes["id"].Value == chapterId)
                        {
                            parentNode = chaptersNode.ChildNodes[2];

                        }
                    }
                }

                if (parentNode != null)
                {
                    parentNode.AppendChild(elemPage);
                }
            }
            xmlDoc.Save(HttpContext.Current.Server.MapPath(actualBook));
            return this.GetAllContent();
        }

        public void AddContentToFrame(string[] content)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(actualBook));
            XmlNodeList pagesNodes = xmlDoc.SelectNodes("//book/chapters/chapter/pages/page");
            XmlNodeList frameNodes = null;
            if (pagesNodes != null)
            {
                string[] splittedData = content[0].Split('-');
                var pageToUpdate = splittedData[0];
                var frameToUpdate = splittedData[1];
                var borderTypeToUpdate = splittedData[2];
                foreach (XmlNode page in pagesNodes)
                {
                    if (page != null)
                    {
                        if (page.Attributes != null && page.Attributes["id"].Value == pageToUpdate)
                        {
                        
                            for(int i=0;i < page.ChildNodes.Count+1;i++)
                            {

                                if (page.ChildNodes[i].Name == "frames")
                                {
                                    frameNodes = page.ChildNodes[i].ChildNodes;
                                    break;
                                }
                            }

                        }
                    }
                }

                if (frameNodes != null)
                {
                    foreach (XmlNode frame in frameNodes)
                    {

                        if (frame.Attributes != null && frameToUpdate == frame.Attributes["id"].Value)
                        {
                            frame.Attributes["bordertype"].Value = borderTypeToUpdate;
                            // ((XmlElement)frame).SetAttribute("bordertype", borderTypeToUpdate);
                        }
                    }
                }
            }
            xmlDoc.Save(HttpContext.Current.Server.MapPath(actualBook));
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


        public void EditChapter(string id)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(actualBook));
            XmlNodeList chaptersNodes = xmlDoc.SelectNodes("//book/chapters/chapter");
            if (chaptersNodes != null)
            {
                var listOfChaptersId = new List<string>();
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