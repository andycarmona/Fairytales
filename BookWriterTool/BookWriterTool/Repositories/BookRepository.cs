using System.Web;

namespace BookWriterTool.Repositories
{
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using BookWriterTool.Helpers;
    using BookWriterTool.Models;

    public class BookRepository : IBookRepository
    {


        private readonly FileOperations handleFiles;

        private book aBook;

        private XmlDocument xmlDoc;

        public BookRepository()
        {

            handleFiles = new FileOperations();
        }

        public string SetActualFile(string aFileBook)
        {

            string status = "Executed with no errors";
            try
            {
                aBook = handleFiles.SerializeXmlToObject(aFileBook);
            }
            catch (IOException ex)
            {
                status = ex.Message;
            }
            return status;
        }

        public int GetNumberOfpagesInBook(string fileName)
        {
            this.SetActualFile(fileName);
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

        /* public book AddContentsInFrame()
         {
             XmlDocument xmlDoc = new XmlDocument();
             xmlDoc.Load(HttpContext.Current.Server.MapPath(actualBook));
             XmlNode aPageNode;
             XmlNodeList chaptersNodes = xmlDoc.SelectNodes("//book/chapters/chapter");
             return this.GetAllContent();
         }*/

        public book AddPage(string chapterId, string fileName)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            XmlNodeList chaptersNodes = xmlDoc.SelectNodes("//book/chapters/chapter");
            XmlElement elemPage = xmlDoc.CreateElement("page");
            XmlElement elemFrames = xmlDoc.CreateElement("frames");
            XmlElement elemObjectsL1 = xmlDoc.CreateElement("objects");
            XmlElement elemObjectsL2 = xmlDoc.CreateElement("objects");
            XmlElement elemObjectsL3 = xmlDoc.CreateElement("objects");
            XmlElement elemObjectsR1 = xmlDoc.CreateElement("objects");
            XmlElement elemObjectsR2 = xmlDoc.CreateElement("objects");
            XmlElement elemObjectsR3 = xmlDoc.CreateElement("objects");
            XmlElement aFrame1 = xmlDoc.CreateElement("frame");
            XmlElement aFrame2 = xmlDoc.CreateElement("frame");
            XmlElement aFrame3 = xmlDoc.CreateElement("frame");
            XmlElement elemContents1 = xmlDoc.CreateElement("contents");
            XmlElement elemContents2 = xmlDoc.CreateElement("contents");
            XmlElement elemContents3 = xmlDoc.CreateElement("contents");
            XmlElement aContentLeft1 = xmlDoc.CreateElement("content");
            XmlElement aContentRight1 = xmlDoc.CreateElement("content");
            XmlElement aContentLeft2 = xmlDoc.CreateElement("content");
            XmlElement aContentRight2 = xmlDoc.CreateElement("content");
            XmlElement aContentLeft3 = xmlDoc.CreateElement("content");
            XmlElement aContentRight3 = xmlDoc.CreateElement("content");  
            XmlNode parentNode = null;
            string pageIdName = string.Format("{0}{1}", "page", (this.GetNumberOfpagesInBook(fileName) + 1));

            aContentLeft1.SetAttribute("target", "left");
            aContentRight1.SetAttribute("target", "right");
            aContentLeft1.SetAttribute("background","");
            aContentRight1.SetAttribute("background","");
            aContentLeft2.SetAttribute("target", "left");
            aContentRight2.SetAttribute("target", "right");
            aContentLeft2.SetAttribute("background", "");
            aContentRight2.SetAttribute("background", "");
            aContentLeft3.SetAttribute("target", "left");
            aContentRight3.SetAttribute("target", "right");
            aContentLeft3.SetAttribute("background", "");
            aContentRight3.SetAttribute("background", "");
            aFrame1.SetAttribute("id", "frame1");
            aFrame1.SetAttribute("bordertype", "square");
            aFrame2.SetAttribute("id", "frame2");
            aFrame2.SetAttribute("bordertype", "square");
            aFrame3.SetAttribute("id", "frame3");
            aFrame3.SetAttribute("bordertype", "square"); 
           
            elemContents1.AppendChild(aContentLeft1);
            elemContents1.AppendChild(aContentRight1);
            elemContents2.AppendChild(aContentLeft2);
            elemContents2.AppendChild(aContentRight2);
            elemContents3.AppendChild(aContentLeft3);
            elemContents3.AppendChild(aContentRight3);
           aContentLeft1.AppendChild(elemObjectsL1);
            aContentLeft2.AppendChild(elemObjectsL2);
            aContentLeft3.AppendChild(elemObjectsL3);
            aContentRight1.AppendChild(elemObjectsR1);
            aContentRight2.AppendChild(elemObjectsR2);
            aContentRight3.AppendChild(elemObjectsR3);

            aFrame1.AppendChild(elemContents1);
            aFrame2.AppendChild(elemContents2);
            aFrame3.AppendChild(elemContents3);
       
            elemPage.SetAttribute("id", pageIdName);  
          
            elemFrames.AppendChild(aFrame1);
            elemFrames.AppendChild(aFrame2);
            elemFrames.AppendChild(aFrame3);

            elemPage.AppendChild(elemFrames);

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
            xmlDoc.Save(HttpContext.Current.Server.MapPath(fileName));
            return this.GetAllContent();
        }

        public string AddObjectToContent(string[] content, string fileName)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            XmlNode pagesNodes = this.xmlDoc.SelectSingleNode("//page[@id=page1]/frames/frame[@id=frame1]/contents/content[@target=left]");
            if (pagesNodes != null)
            {
                XmlAttributeCollection pageAttri = pagesNodes.Attributes;
                if (pageAttri != null)
                {
                    foreach (var node in pageAttri)
                    {
                        var temp=node.ToString();
                        temp = "";
                    }
                }
            }

            //   XmlNodeList frameNodes = null;
           // XmlNodeList contentNodes = null;
            return "ok";
        }

        public void AddContentToFrame(string[] contentToSearch, string fileName)
        {

            xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            XmlNodeList pagesNodes = xmlDoc.SelectNodes("//book/chapters/chapter/pages/page");
            XmlNodeList frameNodes = null;
            XmlNodeList contentNodes = null;
            if (pagesNodes != null)
            {
                string[] splittedData = contentToSearch[0].Split('-');
                var pageToUpdate = splittedData[0];
                var frameToUpdate = splittedData[1];
                var borderTypeToUpdate = splittedData[2];
                foreach (XmlNode page in pagesNodes)
                {
                    if (page != null)
                    {
                        if (page.Attributes != null && page.Attributes["id"].Value == pageToUpdate)
                        {

                            for (int i = 0; i < page.ChildNodes.Count + 1; i++)
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
                            if (borderTypeToUpdate == "rectangle")
                            {
                                for (int i = 0; i < frame.ChildNodes.Count + 1; i++)
                                {

                                    if (frame.ChildNodes[i].Name == "contents")
                                    {
                                        contentNodes = frame.ChildNodes[i].ChildNodes;
                                        if (contentNodes != null)
                                        {
                                            foreach (XmlNode content in contentNodes)
                                            {
                                                content.Attributes["target"].Value = "";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            xmlDoc.Save(HttpContext.Current.Server.MapPath(fileName));

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

        public void EditChapter(string chapterId)
        {
            throw new System.NotImplementedException();
        }

        /*     public void EditChapter(string id)
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
        }*/
    }
}