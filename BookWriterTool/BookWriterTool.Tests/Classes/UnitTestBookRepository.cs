using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWriterTool.Tests.Classes
{
    using System.IO;
    using System.Web;
    using System.Xml.Serialization;
    using BookWriterTool.Helpers;
    using BookWriterTool.Models;
    using BookWriterTool.Repositories;

    [TestClass]
    public class UnitTestBookRepository
    {
        private readonly book aBook;

        private readonly bookChapter chapter1;
       // public static readonly string actualBook = "/Content/Resources/Test/bookNoMetadata2.xml";
        private  bookChapter chapter2;
        private readonly bookChapterPage aPage;
        private readonly FileOperations handleFiles;
        int numberOfChapters ;
        int numberOfPages ;

        private IBookRepository bookRepository;

        private int pathFirstPage;
        public UnitTestBookRepository()
        {
             bookRepository= new BookRepository();
            aBook = new book();
            var serializer = new XmlSerializer(typeof(book));
            var stream = new FileStream("C:/Users/andresc/Desktop/Fairy-tales/fairytales/BookWriterTool/BookWriterTool/Content/Resources/Test/bookNoMetadata2.xml", FileMode.Open);
            aBook = serializer.Deserialize(stream) as book;
            stream.Close();
            chapter1 = aBook.chapters[0];
            aPage = new bookChapterPage();
            bookChapterPage[] apage = aBook.chapters[0].pages;
            bookChapter[] achapter = aBook.chapters;
            numberOfChapters = achapter.Length;
            numberOfPages = apage.Length;
        }
        [TestMethod]
        public void TestAddChapter()
        {
            chapter2=new bookChapter();
            chapter2.id = "chapter2";
            aBook.chapters.SetValue(chapter1,1);
        }
        [TestMethod]
        public void TestChapterExistance()
        {
            Assert.AreEqual(aBook.chapters[1].id, "chapter2");
            Assert.AreEqual(aBook.chapters[0].id, "chapter1");
        }
    
      
        [TestMethod]
        public void TestPageExistance()
        {
             aPage.id = "page3";
            chapter1.pages.SetValue(aPage, 2);
            bookChapterPage page1 = aBook.chapters[0].pages[0];
            bookChapterPage page2 = aBook.chapters[0].pages[1];
            bookChapterPage page3 = aBook.chapters[0].pages[2];
          
            Assert.IsNotNull(page1);
                Assert.IsNotNull(page2);
                Assert.IsNotNull(page3);

            Assert.AreEqual(page1.id, "page1");

        }

        [TestMethod]
        public void TestFrameExistance()
        {
            bookChapterPageFrame aFrame = aBook.chapters[0].pages[0].frames[0];
            Assert.IsNotNull(aFrame);
            Assert.AreEqual(aFrame.id, "frame1");
            Assert.AreEqual(aFrame.bordertype, "triangle");

        }

        [TestMethod]
        public void TestContentExistance()
        {
            bookChapterPageFrameContent aContent = aBook.chapters[0].pages[0].frames[0].contents[0];
            Assert.IsNotNull(aContent);
            Assert.AreEqual(aContent.target, "left");
            Assert.AreEqual(aContent.type, "none");
            Assert.AreEqual(aContent.objects[0].type, "character");
            Assert.AreEqual(aContent.objects[1].animation, "talk");
        }

        [TestMethod]
        public void TestObjectsExistance()
        {
            bookChapterPageFrameContentObject aObject1 = aBook.chapters[0].pages[0].frames[0].contents[0].objects[0];
            bookChapterPageFrameContentObject aObject2 = aBook.chapters[0].pages[0].frames[0].contents[0].objects[1];
            Assert.IsNotNull(aObject1);
            Assert.AreEqual(aObject1.type, "character");
            Assert.AreEqual(aObject1.id, "duck");
            Assert.AreEqual(aObject2.animation, "talk");
            Assert.AreEqual(aObject2.@string, "dia1");
        }
        [TestMethod]
        public void TestChoiceExistance()
        {
            bookChapterPageFrameContentChoice aChoice = aBook.chapters[0].pages[0].frames[1].contents[0].choice;
            Assert.IsNotNull(aChoice);
            Assert.AreEqual(aChoice.negative.result.success.@goto, "page3");
            Assert.AreEqual(aChoice.negative.result.failed.@goto, "frame2");
            Assert.AreEqual(aChoice.positive.result.success.@goto, "page2");
            Assert.AreEqual(aChoice.positive.result.failed.@goto, "gameOver");
        }
   
       
        [TestMethod]
        public void TestAddContent()
        {
        }
        [TestMethod]
        public void TestAddFrame()
        {
        }
        [TestMethod]
        public void TestAddObjectOncontent()
        {
        }
        [TestMethod]
        public void TestGetSerializeModel()
        {
        }
        [TestMethod]
        public void TestDeserializeXml()
        {
        }
        [TestMethod]
        public void DeletePage()
        {
        }
        [TestMethod]
        public void DeleteChapter()
        {
        }
        [TestMethod]
        public void DeleteFrame()
        {
        }
        [TestMethod]
        public void DeleteContent()
        {
        }

    }
}
