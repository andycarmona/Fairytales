using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookXMLoaderTest
{
    using System.IO;
    using System.Xml.Serialization;

    [TestClass]
    public class BookUnitTest
    {
        private book container;

        public BookUnitTest()
        {
              var serializer = new XmlSerializer(typeof(book));
            var stream = new FileStream("C:/Users/andresc/Desktop/FairyTales/fairytales/examplebook/book.xml", FileMode.Open);
          container  = serializer.Deserialize(stream) as book;
            stream.Close();
        }

        [TestMethod]
        public void TestPageExistance()
        {
          //  Assert.AreEqual(container.chapters.chapter.pages.page[0].id, "page1");
        }

        [TestMethod]
        public void TestFrameExistance()
        {
            
           Assert.AreEqual(container.chapters.chapter.pages.page.frames[0].id, "frame1");
           Assert.AreEqual(container.chapters.chapter.pages.page.frames[0].bordertype, "triangle");
        
        }

       
        [TestMethod]
        public void TestContentExistance()
        {
            Assert.AreEqual(container.chapters.chapter.pages.page.frames[0].contents[0].target, "left");
            Assert.AreEqual(container.chapters.chapter.pages.page.frames[0].contents[0].type, "none");
        }

        [TestMethod]
        public void TestObjectsExistance()
        {
            Assert.AreEqual(container.chapters.chapter.pages.page.frames[0].contents[0].objects[0].type, "character");
            Assert.AreEqual(container.chapters.chapter.pages.page.frames[0].contents[0].objects[0].id, "duck");
            Assert.AreEqual(container.chapters.chapter.pages.page.frames[0].contents[0].objects[1].animation, "talk");
            Assert.IsNull(container.chapters.chapter.pages.page.frames[0].contents[0].objects[0].animation);
        }
    }
}
