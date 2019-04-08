using GOTech;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

/* 
 * Name: Jo Lim
 * Date: Apr 7, 2019
 * Last Modified: Apr 5, 2019
 * Description: This class unit tests Routing in ProjectsController
 * */
namespace GOTechUnitTest.RoutingTest
{
    [TestClass]
    public class ProjectsControllerRoutingTest
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

        // https://{application base path}/Projects
        [TestMethod]
        public void IndexNoParameter()
        {
            // ARRANGE
            CreateHttpContext("~/Projects");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Projects", routeData.Values["controller"]);
            Assert.AreEqual("Index", routeData.Values["action"]);
        }

        // https://{application base path}/Projects
        [TestMethod]
        public void CreateGET()
        {
            // ARRANGE
            CreateHttpContext("~/Projects/Create");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Projects", routeData.Values["controller"]);
            Assert.AreEqual("Create", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        // https://{application base path}/Projects
        [TestMethod]
        public void CreatePOST()
        {
            // ARRANGE
            CreateHttpContext("~/Projects/Create", "POST");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Projects", routeData.Values["controller"]);
            Assert.AreEqual("Create", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        // https://{application base path}/Projects/Details
        [TestMethod]
        public void DetailsNoParameter()
        {
            // ARRANGE
            CreateHttpContext("~/Projects/Details");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.AreEqual("Projects", routeData.Values["controller"]);
            Assert.AreEqual("Details", routeData.Values["action"]);
        }

        // https://{application base path}/Projects/Details/1
        [TestMethod]
        public void Details()
        {
            // ARRANGE
            CreateHttpContext("~/Projects/Details/1");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Projects", routeData.Values["controller"]);
            Assert.AreEqual("Details", routeData.Values["action"]);
        }

        [TestMethod]
        public void EditNoParameterGET()
        {
            // ARRANGE
            CreateHttpContext("~/Projects/Edit");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.AreEqual("Projects", routeData.Values["controller"]);
            Assert.AreEqual("Edit", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        // https://{application base path}/Projects/Edit/1
        [TestMethod]
        public void EditGET()
        {
            // ARRANGE
            CreateHttpContext("~/Projects/Edit/1");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Projects", routeData.Values["controller"]);
            Assert.AreEqual("Edit", routeData.Values["action"]);
            Assert.AreEqual("1", routeData.Values["id"]);
        }

        // https://{application base path}/Projects/Edit
        // POST method
        [TestMethod]
        public void EditPost()
        {
            // ARRANGE
            CreateHttpContext("~/Projects/Edit", "POST");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Projects", routeData.Values["controller"]);
            Assert.AreEqual("Edit", routeData.Values["action"]);
        }

        // https://{application base path}/Projects/Delete/1
        [TestMethod]
        public void DeleteGET()
        {
            // ARRANGE
            CreateHttpContext("~/Projects/Delete/1");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Projects", routeData.Values["controller"]);
            Assert.AreEqual("Delete", routeData.Values["action"]);
            Assert.AreEqual("1", routeData.Values["id"]);
        }

        // https://{application base path}/Projects/Delete
        // POST method
        [TestMethod]
        public void DeleteConfirmedPost()
        {
            // ARRANGE
            CreateHttpContext("~/Projects/Delete", "POST");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Projects", routeData.Values["controller"]);
            Assert.AreEqual("Delete", routeData.Values["action"]);
        }
    }
}