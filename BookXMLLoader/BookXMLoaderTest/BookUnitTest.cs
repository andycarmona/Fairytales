using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookXMLoaderTest
{
    using System.IO;
    using System.Xml.Serialization;

    [TestClass]
    public class BookUnitTest
    {
        private readonly book aBook;

        private int pathFirstPage ;

        public BookUnitTest()
        {
            
              var serializer = new XmlSerializer(typeof(book));
            var stream = new FileStream("C:/Users/andresc/Desktop/FairyTales/fairytales/examplebook/book.xml", FileMode.Open);
          aBook  = serializer.Deserialize(stream) as book;
            stream.Close();
        }
   [TestMethod]
        public void TestChapterExistance()
        {
        
            Assert.AreEqual(aBook.chapters[0].id, "chapter1");
        }

        [TestMethod]
        public void TestPageExistance()
        {
            bookChapterPage page1 = aBook.chapters[0].pages[0];
         
            Assert.IsNotNull(page1);
      
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
            Assert.AreEqual(aContent.objects[1].animation,"talk");
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
            Assert.AreEqual(aChoice.negative.result.success.@goto,"page3");
            Assert.AreEqual(aChoice.negative.result.failed.@goto, "frame2");
            Assert.AreEqual(aChoice.positive.result.success.@goto, "page2");
            Assert.AreEqual(aChoice.positive.result.failed.@goto, "gameOver");
        }
    }
}
