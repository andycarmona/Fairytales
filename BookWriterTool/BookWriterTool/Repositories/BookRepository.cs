using System.Web;

namespace BookWriterTool.Repositories
{
    using System;
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
            const int NumberOfFrames = 4; //add 1 to number of frames you need
            const int NumberOfContents = 2;
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(HttpContext.Current.Server.MapPath(fileName));
            XmlNodeList chaptersNodes = xmlDocument.SelectNodes("//book/chapters/chapter");
            XmlElement elemPage = xmlDocument.CreateElement("page");
            XmlElement elemFrames = xmlDocument.CreateElement("frames");

            XmlNode parentNode = null;
            string pageIdName = string.Format("{0}{1}", "page", (this.GetNumberOfpagesInBook(fileName) + 1));
            for (int i = 1; i < NumberOfFrames; i++)
            {
                XmlElement tmpFrame = xmlDocument.CreateElement("frame");
                XmlElement tmpContents = xmlDocument.CreateElement("contents");
                tmpFrame.SetAttribute("id", "frame" + i);
                tmpFrame.SetAttribute("bordertype", "square");
                elemFrames.AppendChild(tmpFrame);
                tmpFrame.AppendChild(tmpContents);

                for (int x = 0; x < NumberOfContents; x++)
                {
                    XmlElement tmpObjects = xmlDocument.CreateElement("objects");
                    XmlElement tmpContent = xmlDocument.CreateElement("content");

                    tmpContent.SetAttribute("target", x == 0 ? "left" : "right");
                    tmpContent.SetAttribute("background", "");
                    tmpContent.AppendChild((tmpObjects));
                    tmpContents.AppendChild(tmpContent);
                }
            }
            elemPage.SetAttribute("id", pageIdName);
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
            xmlDocument.Save(HttpContext.Current.Server.MapPath(fileName));
            return this.GetAllContent();
        }

        public string GetIdFromFileName(string splittedValues)
        {
            var splittedImg = splittedValues.Split('/');
            var typeImg = splittedImg[splittedImg.Length - 1];
            
            return typeImg;
        }

        public string AddDialogBoxToContent(string[] content, string fileName)
        {
            return null;
        }

        public string AddCharacterToContent(BookModel content, string fileName)
        {

        
            var mssg = "";
            try
            {
                AddGenericObject(fileName, content, content.Objects[0].ImageObj, "character");
            }
            catch (Exception e)
            {
                mssg = e.Message;
            }

            return mssg;
        }

        public void AddGenericObject(string fileName, BookModel content, string imgVal, string type)
        {
            
            var image = this.GetIdFromFileName(imgVal);
            xmlDoc = new XmlDocument();
            XmlElement aObject = xmlDoc.CreateElement("object");
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            var objectsNodes = (XmlElement)this.xmlDoc.SelectSingleNode(String.Format("//book/chapters/chapter[@id='{0}']/" +
                "pages/page[@id='{1}']/frames/frame[@id='{2}']/contents/content[@target='{3}']/objects",
                content.ChapterId,content.PageId,content.FrameId,content.Target));

            if (objectsNodes != null)
            {
                aObject.SetAttribute("img", image);
                aObject.SetAttribute("scaleX", "25%");
                aObject.SetAttribute("scaleY", "25%");
                aObject.SetAttribute("origo", "25%");
                aObject.SetAttribute("type", type);
                aObject.SetAttribute("id", content.Objects[0].ObjectId);
                objectsNodes.AppendChild(aObject);

                xmlDoc.Save(HttpContext.Current.Server.MapPath(fileName));
            }

        }
        public string UpdateObjectPosition(BookModel content, string fileName)
        {
          //  string[] splittedValues = content[0].Split('-');
            xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            var objectsNodes = (XmlElement)this.xmlDoc.SelectSingleNode(String.Format("//book/chapters/chapter[@id='{0}']/" +
                "pages/page[@id='{1}']/frames/frame[@id='{2}']/contents/content[@target='{3}']/objects",
               content.ChapterId,content.PageId,content.FrameId,content.Target));
            return null;
        }

        public string AddBackgroundToContent(BookModel content, string fileName)
        {
            var image = this.GetIdFromFileName(content.Objects[0].ImageObj);
            var mssg = "";
            xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            var frameNode = (XmlElement)this.xmlDoc.SelectSingleNode(String.Format("//book/chapters/chapter[@id='{0}']/" +
                "pages/page[@id='{1}']/frames/frame[@id='{2}']/contents/content[@target='{3}']",
               content.ChapterId,content.PageId,content.FrameId,content.Target));

            if (frameNode != null)
            {
                frameNode.SetAttribute("background", image);
                try
                {
                    xmlDoc.Save(HttpContext.Current.Server.MapPath(fileName));
                }
                catch (DirectoryNotFoundException e)
                {
                    mssg = e.Message;
                }
            }
            return mssg;
        }

        

        public string AddFrame(BookModel contentToSearch, string fileName)
        {
            var mssg = "";
            xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            XmlNode frameNode = this.xmlDoc.SelectSingleNode(
                    "//book/chapters/chapter[@id='" + contentToSearch.ChapterId + "']/" + "pages/page[@id='" + contentToSearch.PageId
                    + "']/frames/frame[@id='" + contentToSearch.FrameId + "']");
            if (frameNode != null)
            {  
                XmlElement tmpContents = xmlDoc.CreateElement("contents");
                frameNode.RemoveAll();
                XmlAttribute xId = xmlDoc.CreateAttribute("id");
                XmlAttribute xBorderType = xmlDoc.CreateAttribute("bordertype");
                xId.Value = contentToSearch.FrameId;
                xBorderType.Value = contentToSearch.Target;
                if (frameNode.Attributes != null)
                {
                    frameNode.Attributes.Append(xBorderType);
                    frameNode.Attributes.Append(xId);
                }
                frameNode.AppendChild(tmpContents);
            
            if (contentToSearch.Target == "rectangle")
            {
                XmlElement aContent = xmlDoc.CreateElement("content");
                XmlElement tmpObjects = xmlDoc.CreateElement("objects");
                aContent.SetAttribute("target", "rectangle");
                aContent.SetAttribute("type", "");
                tmpContents.AppendChild(aContent);
                aContent.AppendChild(tmpObjects);
            }
            else if (contentToSearch.Target == "square")
            {
                for (int i = 0; i < 2; i++)
                {
                    XmlElement aContent = xmlDoc.CreateElement("content");
                    XmlElement tmpObjects = xmlDoc.CreateElement("objects");
                    if (i == 0)
                        aContent.SetAttribute("target", "left");
                    else
                        aContent.SetAttribute("target", "right");
                    aContent.SetAttribute("type", "none");
                    tmpContents.AppendChild(aContent);
                    aContent.AppendChild(tmpObjects);
                }
            }
            try
            {
                xmlDoc.Save(HttpContext.Current.Server.MapPath(fileName));
            }
            catch (DirectoryNotFoundException e)
            {
                mssg = e.Message;
            }
            }
            return mssg;
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
    }
}