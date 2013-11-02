﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookWriterTool;
using BookWriterTool.Controllers;

namespace BookWriterTool.Tests.Controllers
{
    [TestClass]
    public class BookControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", result.ViewBag.Message);
        }

        [TestMethod]
        public void AddPage()
        {
            BookController controller = new BookController();
            string[] chapters= new string[1];
            chapters[0] = "chapter1";
            ActionResult result=controller.AddPage(chapters);


        }


       
    }
}