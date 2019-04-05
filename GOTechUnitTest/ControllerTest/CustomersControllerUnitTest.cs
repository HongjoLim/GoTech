using GOTech;
using GOTech.Controllers;
using GOTech.Models;
using GOTechUnitTest.Tools;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GOTechUnitTest.ControllerTest
{
    [TestClass]
    public class CustomersControllerUnitTest
    {
        // Declare mocked objects
        private Mock<DbSet<ApplicationUser>> _userSet;
        private Mock<DbSet<Province>> _provinceSet;
        private Mock<ApplicationDbContext> _db;
        private Mock<IUserStore<ApplicationUser>> _store;
        private Mock<ApplicationUserManager> _userManager;

        // Declare the controller
        private CustomersController _controller;

        // Initialize instance variables
        [TestInitialize]
        public void Initialize()
        {
            _userSet = new Mock<DbSet<ApplicationUser>>();
            _db = new Mock<ApplicationDbContext>();
            _provinceSet = new Mock<DbSet<Province>>();
            _store = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new Mock<ApplicationUserManager>(_store.Object);
            _controller = new CustomersController(_userManager.Object, _db.Object);
        }

        // When there is 1 employee and 1 customer in our db
        [TestMethod]
        public void IndexWhenOneEmployeeOneCustomer()
        {
            var dummyUsers = new List<ApplicationUser>
            {
                // ARRANGE
                new ApplicationUser
                {
                    UserName = "czebahi1@gmail.com",
                    Email = "czebahi@gmail.com",
                    PositionId = 2,
                },
                new ApplicationUser
                {
                    UserName = "czebahi2@gmail.com",
                    Email = "czebahi2@gmail.com",
                }
            }.AsQueryable();

            _userSet = MockingHelper.Create(dummyUsers);

            _db.Setup(c => c.Users).Returns(_userSet.Object);

            // ACT
            var result = _controller.Index() as ViewResult;

            // Type cast the ViewResult object to IEnumerable<T> to count members
            var customers = (IEnumerable<ApplicationUser>)result.Model;

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(1, customers.Count());
            Assert.AreEqual(dummyUsers.ToList()[1].UserName, "czebahi2@gmail.com");
        }

        // When there is 1 employee and 1 customer in our db
        [TestMethod]
        public void IndexWhenNoCustomer()
        {
            // ARRANGE
            var dummyUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "czebahi2@gmail.com",
                    Email = "czebahi2@gmail.com",
                    PositionId = 2
                }
            }.AsQueryable();

            _userSet = MockingHelper.Create(dummyUsers);

            _db.Setup(c => c.Users).Returns(_userSet.Object);

            // ACT
            var result = _controller.Index() as ViewResult;

            // Type cast the ViewResult object to IEnumerable<T> to count Members
            var customers = (IEnumerable<ApplicationUser>)result.Model;

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(0, customers.Count());
        }
    }
}