using GOTech;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

/* 
 * Name: Jo Lim
 * Date: Apr 5, 2019
 * Last Modified: Apr 7, 2019
 * Description: This class unit tests Routing in CustomersController
 * */
namespace GOTechUnitTest.RoutingTest
{
    [TestClass]
    public class PositionsControllerRoutingTest
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

        // https://{application base path}/Positions
        [TestMethod]
        public void IndexNoParameter()
        {
            // ARRANGE
            CreateHttpContext("~/Positions");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Positions", routeData.Values["controller"]);
            Assert.AreEqual("Index", routeData.Values["action"]);
        }

        // https://{application base path}/Positions
        [TestMethod]
        public void CreateGET()
        {
            // ARRANGE
            CreateHttpContext("~/Positions/Create");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Positions", routeData.Values["controller"]);
            Assert.AreEqual("Create", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        // https://{application base path}/Positions
        [TestMethod]
        public void CreatePOST()
        {
            // ARRANGE
            CreateHttpContext("~/Positions/Create", "POST");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Positions", routeData.Values["controller"]);
            Assert.AreEqual("Create", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        // https://{application base path}/Positions/Details
        [TestMethod]
        public void DetailsNoParameter()
        {
            // ARRANGE
            CreateHttpContext("~/Positions/Details");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.AreEqual("Positions", routeData.Values["controller"]);
            Assert.AreEqual("Details", routeData.Values["action"]);
        }

        // https://{application base path}/Positions/Details/1
        [TestMethod]
        public void Details()
        {
            // ARRANGE
            CreateHttpContext("~/Positions/Details/1");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Positions", routeData.Values["controller"]);
            Assert.AreEqual("Details", routeData.Values["action"]);
        }

        [TestMethod]
        public void EditNoParameterGET()
        {
            // ARRANGE
            CreateHttpContext("~/Positions/Edit");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.AreEqual("Positions", routeData.Values["controller"]);
            Assert.AreEqual("Edit", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        // https://{application base path}/Positions/Edit/1
        [TestMethod]
        public void EditGET()
        {
            // ARRANGE
            CreateHttpContext("~/Positions/Edit/1");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Positions", routeData.Values["controller"]);
            Assert.AreEqual("Edit", routeData.Values["action"]);
            Assert.AreEqual("1", routeData.Values["id"]);
        }

        // https://{application base path}/Positions/Edit
        // POST method
        [TestMethod]
        public void EditPost()
        {
            // ARRANGE
            CreateHttpContext("~/Positions/Edit", "POST");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Positions", routeData.Values["controller"]);
            Assert.AreEqual("Edit", routeData.Values["action"]);
        }

        // https://{application base path}/Positions/Delete/1
        [TestMethod]
        public void DeleteGET()
        {
            // ARRANGE
            CreateHttpContext("~/Positions/Delete/1");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Positions", routeData.Values["controller"]);
            Assert.AreEqual("Delete", routeData.Values["action"]);
            Assert.AreEqual("1", routeData.Values["id"]);
        }

        // https://{application base path}/Positions/Delete
        // POST method
        [TestMethod]
        public void DeleteConfirmedPost()
        {
            // ARRANGE
            CreateHttpContext("~/Positions/Delete", "POST");

            // ACT
            RouteData routeData = _routes.GetRouteData(_context.Object);

            // ASSERT
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Positions", routeData.Values["controller"]);
            Assert.AreEqual("Delete", routeData.Values["action"]);
        }
    }
}