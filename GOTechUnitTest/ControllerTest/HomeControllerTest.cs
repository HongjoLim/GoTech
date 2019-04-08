using System.Web.Mvc;
using GOTech.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/* 
 * Name: Jo Lim
 * Date: Apr 6, 2019
 * Last Modified: Apr 6, 2019
 * Description: This class unit tests HomeController - not much to test
 * */
namespace GOTechUnitTest.ControllerTest
{
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController _controller;

        [TestInitialize]
        public void Initilize()
        {
            // ARRANGE
            _controller = new HomeController();
        }

        [TestMethod]
        public void Index()
        {
            // ACT
            var result = _controller.Index();
            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(typeof(ViewResult), result.GetType());

        }

        [TestMethod]
        public void About()
        {
            // ACT
            var result = _controller.About();

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void Contact()
        {
            // ACT
            var result = _controller.Index();

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }
    }
}
