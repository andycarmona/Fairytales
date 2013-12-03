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

        public string AddCharacterToContent(string[] content, string fileName)
        {
         
            var splittedValues = content[0].Split('-');
            var splittedImg = splittedValues[4].Split('/');
            var typeImg = splittedImg[splittedImg.Length - 1];
            var idOfImg = typeImg.Split('.');
            var mssg = "";
          try
          {
              AddGenericObject(fileName, splittedValues, idOfImg[0],"character");
          }
          catch (Exception e)
          {
              mssg = e.Message;
          }
         
            return mssg;
        }

        public void AddGenericObject(string fileName,string[] splittedValues,string idOfImg,string type)
        {
            xmlDoc = new XmlDocument();
            XmlElement aObject = xmlDoc.CreateElement("object");
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            var objectsNodes =
                (XmlElement)
                this.xmlDoc.SelectSingleNode(
                    "//book/chapters/chapter[@id='" + splittedValues[0] + "']/" + "pages/page[@id='" + splittedValues[1]
                    + "']" + "/frames/frame[@id='" + splittedValues[2] + "']" + "/contents/content[@target='"
                    + splittedValues[3] + "']/objects");
            if (objectsNodes != null)
            {
                aObject.SetAttribute("type",type);
                aObject.SetAttribute("id", idOfImg);
                objectsNodes.AppendChild(aObject);
          
                    xmlDoc.Save(HttpContext.Current.Server.MapPath(fileName));
                }
            
        }

        public string AddFrame(string[] contentToSearch, string fileName)
        {
            string[] splittedValues = contentToSearch[0].Split('-');
            var mssg = "";
            xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            XmlNode frameNode = this.xmlDoc.SelectSingleNode(
                    "//book/chapters/chapter[@id='" + splittedValues[0] + "']/" + "pages/page[@id='" + splittedValues[1]
                    + "']/frames/frame[@id='" + splittedValues[2] + "']");

            XmlElement tmpContents = xmlDoc.CreateElement("contents");
          
            if (frameNode != null)
            {
                frameNode.RemoveAll();
                XmlAttribute xId = xmlDoc.CreateAttribute("id");
                XmlAttribute xBorderType = xmlDoc.CreateAttribute("bordertype");
                xId.Value = splittedValues[2];
                xBorderType.Value = splittedValues[4];
                if (frameNode.Attributes != null)
                {
                    frameNode.Attributes.Append(xBorderType);
                    frameNode.Attributes.Append(xId);
                }
                frameNode.AppendChild(tmpContents);
            }
            if (splittedValues[4] == "rectangle")
            {
                XmlElement aContent = xmlDoc.CreateElement("content");  
                XmlElement tmpObjects = xmlDoc.CreateElement("objects");
                aContent.SetAttribute("target", "rectangle");
                aContent.SetAttribute("type", "");
                tmpContents.AppendChild(aContent);
                aContent.AppendChild(tmpObjects);
            }
            else if (splittedValues[4] == "square")
            {
                for (int i = 0; i < 2; i++)
                {
                    XmlElement aContent = xmlDoc.CreateElement("content");
                    XmlElement tmpObjects = xmlDoc.CreateElement("objects");
                    if(i==0)
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