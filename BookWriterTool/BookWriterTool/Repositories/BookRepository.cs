using System.Web;

namespace BookWriterTool.Repositories
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using BookWriterTool.Helpers;
    using BookWriterTool.Models;

    public class BookRepository : IBookRepository
    {


        private readonly FileOperations handleFiles;

        private book aBook;

        private XmlDocument xmlDoc;

        private string actualFile;

        public BookRepository()
        {

            handleFiles = new FileOperations();
        }

        public string SetActualFile(string aFileBook)
        {
            this.actualFile = aFileBook;
            string status = "";
            if ((aFileBook != null)&&(aFileBook != "/"))
            {
                try
                {
                    aBook = handleFiles.SerializeXmlToObject(aFileBook);
                }
                catch (IOException ex)
                {
                    status = ex.Message;
                }
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

        public string AddTextToContent(string model, string componentId,string fileName,string type,string form)
        {
            /*chapter1-page2-frame1-rectangle
            		 <txtbox>
                      <text id="mainText"></text>
                    </txtbox>*/
            
            var mssg = "";
            string[] splitedData = componentId.Split('-');
            var chapterId = splitedData[0];
            var pageId = splitedData[1];
            var frameId = splitedData[2];
            var target = splitedData[3];
            var txtBoxId = splitedData[4];

         xmlDoc = new XmlDocument();
            XmlElement aTextBox = xmlDoc.CreateElement("txtbox");
            XmlElement textBoxContent = xmlDoc.CreateElement("object");
          
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            var objectsNodes = (XmlElement)this.xmlDoc.SelectSingleNode(String.Format("//book/chapters/chapter[@id='{0}']/" +
                "pages/page[@id='{1}']/frames/frame[@id='{2}']/contents/content[@target='{3}']/objects",
                chapterId, pageId, frameId, target));
          
            if (objectsNodes != null)
            {
                  if (objectsNodes.ChildNodes.Count > 0)
                  {
                      foreach (XmlNode aObject in objectsNodes.ChildNodes)
                      {
                          objectsNodes.RemoveChild(aObject);
                      }
                  }
              
               XmlCDataSection txtBoxValue = xmlDoc.CreateCDataSection(model);
                textBoxContent.SetAttribute("id", componentId);
                textBoxContent.SetAttribute("type",type);
                if (type == "speechBubbla")
                {
                    textBoxContent.SetAttribute("form",form);
                }
                textBoxContent.AppendChild(txtBoxValue);
                objectsNodes.AppendChild(textBoxContent);
              
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

        public string GetIdFromFileName(string splittedValues)
        {
            var splittedImg = splittedValues.Split('/');
            var typeImg = splittedImg[splittedImg.Length - 2]+"/"+splittedImg[splittedImg.Length - 1];
            
            return typeImg;
        }

        public string AddTextToBubble(string model, string componentId, string fileName, string type, string form)
        {
            var mssg = "";
            string[] splitedData = componentId.Split('-');
            var chapterId = splitedData[0];
            var pageId = splitedData[1];
            var frameId = splitedData[2];
            var target = splitedData[3];
            var txtBoxId = splitedData[4];

            xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            var objectsNodes = (XmlElement)this.xmlDoc.SelectSingleNode(String.Format("//book/chapters/chapter[@id='{0}']/" +
                "pages/page[@id='{1}']/frames/frame[@id='{2}']/contents/content[@target='{3}']/objects/object[@id='{4}']",
                chapterId, pageId, frameId, target,txtBoxId));

            if (objectsNodes != null)
            {
                if (objectsNodes.ChildNodes.Count > 0)
                {
                    foreach (XmlNode aObject in objectsNodes.ChildNodes)
                    {
                        objectsNodes.RemoveChild(aObject);
                    }
                }

                XmlCDataSection txtBoxValue = xmlDoc.CreateCDataSection(model);
            
                objectsNodes.AppendChild(txtBoxValue);

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

        public string AddSpeechBubbleObject(BookModel content, string fileName)
        {
          //  var image = this.GetIdFromFileName(imgVal);
            var mssg = "";
            try{
            xmlDoc = new XmlDocument();
            XmlElement aObject = xmlDoc.CreateElement("object");
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            var objectsNodes = (XmlElement)this.xmlDoc.SelectSingleNode(String.Format("//book/chapters/chapter[@id='{0}']/" +
                "pages/page[@id='{1}']/frames/frame[@id='{2}']/contents/content[@target='{3}']/objects",
                content.ChapterId, content.PageId, content.FrameId, content.Target));

            if (objectsNodes != null)
            {
                //aObject.SetAttribute("img", image);
                aObject.SetAttribute("scaleX", content.Objects[0].ScaleX);
                aObject.SetAttribute("scaleY", content.Objects[0].ScaleY);
                aObject.SetAttribute("origoX", content.Objects[0].OrigoX);
                aObject.SetAttribute("origoY", content.Objects[0].OrigoY);
                aObject.SetAttribute("type", content.Objects[0].Type);
                aObject.SetAttribute("id", content.Objects[0].ObjectId);
                objectsNodes.AppendChild(aObject);

                xmlDoc.Save(HttpContext.Current.Server.MapPath(fileName));
            }
               }
            catch (Exception e)
            {
                mssg = e.Message;
            }

            return mssg;
        }

        public string DeleteObjectFromContent(BookModel content, string fileName)
        {
            var mssg = "";
            var splitId = content.Objects[0].ObjectId.Split('-');
            var objId = splitId[splitId.Length-1];
            xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            XmlNode objectsNodes = this.xmlDoc.SelectSingleNode(String.Format("//book/chapters/chapter[@id='{0}']/pages/page[@id='{1}']/frames/frame[@id='{2}']/contents/content[@target='{3}']/objects/object[@id='{4}']", content.ChapterId, content.PageId, content.FrameId, content.Target,objId));
            if (objectsNodes != null)
            {
                XmlNode parentNode = objectsNodes.ParentNode;
                if (parentNode != null)
                {
                    parentNode.RemoveChild(objectsNodes);
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

        public string AddObjectToContent(BookModel content, string fileName)
        {


            var mssg = "";
            try
            {
                AddGenericObject(fileName, content, content.Objects[0].ImageObj);
            }
            catch (Exception e)
            {
                mssg = e.Message;
            }

            return mssg;
        }

        public void AddGenericObject(string fileName, BookModel content, string imgVal)
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
                aObject.SetAttribute("scaleX", content.Objects[0].ScaleX);
                aObject.SetAttribute("scaleY", content.Objects[0].ScaleY);
                aObject.SetAttribute("origoX", content.Objects[0].OrigoX);
                aObject.SetAttribute("origoY", content.Objects[0].OrigoY);
                aObject.SetAttribute("type",content.Objects[0].Type);
                aObject.SetAttribute("id", content.Objects[0].ObjectId);
                objectsNodes.AppendChild(aObject);

                xmlDoc.Save(HttpContext.Current.Server.MapPath(fileName));
            }

        }

        public string UpdateObjectPosition(BookModel content, string fileName)
        {
            var mssg = "";
          //  string[] splittedValues = content[0].Split('-');
            xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(fileName));
            var aObject = (XmlElement)this.xmlDoc.SelectSingleNode(String.Format("//book/chapters/chapter[@id='{0}']/" +
                "pages/page[@id='{1}']/frames/frame[@id='{2}']/contents/content[@target='{3}']/objects/object[@id='{4}']",
               content.ChapterId,content.PageId,content.FrameId,content.Target,content.Objects[0].ObjectId));
            if (aObject != null)
            {
                aObject.SetAttribute("scaleX", content.Objects[0].ScaleX);
                aObject.SetAttribute("scaleY", content.Objects[0].ScaleY);
                aObject.SetAttribute("origoX", content.Objects[0].OrigoX);
                aObject.SetAttribute("origoY", content.Objects[0].OrigoY);
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