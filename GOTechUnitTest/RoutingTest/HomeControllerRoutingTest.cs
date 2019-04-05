using GOTech;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

/* 
 * Name: Jo Lim
 * Date: Apr 5, 2019
 * Last Modified: Apr 5, 2019
 * Description: This class unit tests Routing in HomeController
 * */

namespace GOTechUnitTest.RoutingTest
{
    [TestClass]
    public class HomeControllerRoutingTest
    {
        private Mock<HttpRequestBase> _request;
        private Mock<HttpContextBase> _context;
        private Mock<HttpResponseBase> _response;
        private RouteCollection _routes;

        [TestInitialize]
        public void Initialize()
        {
            _context = new Mock<HttpContextBase>();
            _request = new Mock<HttpRequestBase>();
            _response = new Mock<HttpResponseBase>();
            _routes = new RouteCollection();
        }

        // This reusable method constructs http context
        private void CreateHttpContext(string targetUrl = null,
                                                                        string httpMethod = "GET")
        {
            _request.Setup(m => m.AppRelativeCurrentExecutionFilePath)
                 .Returns(targetUrl);

            _request.Setup(m => m.HttpMethod).Returns(httpMethod);
           
            _response.Setup(m => m.ApplyAppPathModifier(
            It.IsAny<string>())).Returns<string>(s => s);

            _context.Setup(m => m.Request).Returns(_request.Object);

            _context.Setup(m => m.Response).Returns(_response.Object);

            RouteConfig.RegisterRoutes(_routes);
        }

        // https://{application base path}
        [TestMethod]
        public void NoControllerNoActionNoParameter()
        {
            // ARRANGE
            CreateHttpContext("~/");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Home", routeData.Values["controller"]);
            Assert.AreEqual("Index", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        // https://{application base path}/Home
        [TestMethod]
        public void HomeControllerNoActionNoParameter()
        {
            // ARRANGE
            CreateHttpContext("~/Home");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Home", routeData.Values["controller"]);
            Assert.AreEqual("Index", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        // https://{application base path}/Home/Index
        [TestMethod]
        public void HomeControllerIndexActionNoParameter()
        {
            // ARRANGE
            CreateHttpContext("~/Home/Index");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Home", routeData.Values["controller"]);
            Assert.AreEqual("Index", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        // https://{application base path}/Home/Index/11
        [TestMethod]
        public void HomeControllerIndexActionRandomParameter()
        {
            // ARRANGE
            CreateHttpContext("~/Home/Index/11");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Home", routeData.Values["controller"]);
            Assert.AreEqual("Index", routeData.Values["action"]);
            Assert.AreEqual("11", routeData.Values["id"]);
        }

        // https://{application base path}/Home/About
        [TestMethod]
        public void HomeControllerAboutActionNoParameter()
        {
            // ARRANGE
            CreateHttpContext("~/Home/About");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Home", routeData.Values["controller"]);
            Assert.AreEqual("About", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        // https://{application base path}/Home/About/11
        [TestMethod]
        public void HomeControllerAboutActionRandomParameter()
        {
            // ARRANGE
            CreateHttpContext("~/Home/About/11");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Home", routeData.Values["controller"]);
            Assert.AreEqual("About", routeData.Values["action"]);
            Assert.AreEqual("11", routeData.Values["id"]);
        }

        // https://{application base path}/Home/Contact
        [TestMethod]
        public void HomeControllerContactActionNoParameter()
        {
            // ARRANGE
            CreateHttpContext("~/Home/Contact");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Home", routeData.Values["controller"]);
            Assert.AreEqual("Contact", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        // https://{application base path}/Home/Contact/11
        [TestMethod]
        public void HomeControllerContactActionRandomParameter()
        {
            // ARRANGE
            CreateHttpContext("~/Home/Contact/11");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Home", routeData.Values["controller"]);
            Assert.AreEqual("Contact", routeData.Values["action"]);
            Assert.AreEqual("11", routeData.Values["id"]);
        }
    }
}
