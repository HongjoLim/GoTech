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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

/* 
 * Name: Jo Lim
 * Date: Apr 5, 2019
 * Last Modified: Apr 5, 2019
 * Description: This class unit tests CustomersController
 * */

namespace GOTechUnitTest.ControllerTest
{
    [TestClass]
    public class CustomersControllerTest
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
            // ARRANGE
            var dummyUsers = new List<ApplicationUser>
            {
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

        [TestMethod]
        public async Task DetailsWhenEmailNull()
        {
            // ARRANGE
            string email = null;

            // ACT
            var resultTask = await _controller.Details(email);

            // Type cast
            var result = resultTask as HttpStatusCodeResult;

            // ASSERT
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        // CASE: When ProvinceId IS NULL - Cannot be tested becuase it includes extention method
        [TestMethod]
        public async Task DetailsWhenEmailNotNullFoundUserNotNullFoundProvinceIdNotNull()
        {
            // ARRANGE
            var dummyUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "czebahi2@gmail.com",
                    Email = "czebahi2@gmail.com",
                    ProvinceId = 1
                }
            }.AsQueryable();

            var dummyProvinces = new List<Province>
            {
                new Province{
                    ProvinceId = 1,
                    ProvinceName = "ON"
                }
            }.AsQueryable();

            _userSet = MockingHelper.Create(dummyUsers);
            _provinceSet = MockingHelper.Create(dummyProvinces);

            _db.Setup(c => c.Users).Returns(_userSet.Object);
            _db.Setup(c => c.Provinces).Returns(_provinceSet.Object);

            var user = dummyUsers.ToList()[0];
            var email = user.Email;

            // Manipulate the method in IUserStore and UserManager so that it cannot find the user
            _store.Setup(x => x.FindByNameAsync(email)).Returns(Task.FromResult(user));
            _userManager.Setup(x => x.FindByNameAsync(email)).Returns(Task.FromResult(user));

            //ACT
            // Type cast
            var result = await _controller.Details(email);
            var resultView = result as ViewResult;

            // Type cast the ViewResult object to ApplicationUser
            var customer = (ApplicationUser) resultView.Model;

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(dummyUsers.FirstOrDefault().Email, customer.Email);
            Assert.AreEqual(dummyUsers.FirstOrDefault().ProvinceId, customer.ProvinceId);
        }

        [TestMethod]
        public async Task DetailsWhenEmailNotNullFoundUserNull()
        {
            // ARRANGE
            var dummyUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "czebahi2@gmail.com",
                    Email = "czebahi2@gmail.com",
                    ProvinceId = 1
                }
            }.AsQueryable();

            _userSet = MockingHelper.Create(dummyUsers);

            ApplicationUser foundUser = null;
            var email = "random@gmail.com";

            _db.Setup(c => c.Users).Returns(_userSet.Object);

            // Manipulate the method in IUserStore and UserManager so that it cannot find the user
            _store.Setup(x => x.FindByNameAsync(email)).Returns(Task.FromResult(foundUser));
            _userManager.Setup(x => x.FindByNameAsync(email)).Returns(Task.FromResult(foundUser));

            // ACT
            var resultTask = await  _controller.Details(email);

            // ASSERT
            Assert.AreEqual(typeof(HttpNotFoundResult), resultTask.GetType());
        }

        // Edit GET
        [TestMethod]
        public async Task EditWhenEmailNull()
        {
            // ARRANGE
            string email = null;

            // ACT
            var resultTask = _controller.Edit(email);

            // Type cast
            var result = await resultTask as HttpStatusCodeResult;

            // ASSERT
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        // Edit GET
        [TestMethod]
        public async Task EditWhenEmailNotNullUserFoundNull()
        {
            // ARRANGE
            var dummyUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "czebahi@gmail.com",
                    Email = "czebahi@gmail.com",
                    ProvinceId = 1
                }
            }.AsQueryable();

            _userSet = MockingHelper.Create(dummyUsers);

            _db.Setup(c => c.Users).Returns(_userSet.Object);

            // declare an email and assign a random email. Pass it as the argument
            var email = "random@gmail.com";

            // This value will get returned from UserManager
            ApplicationUser user = null;

            // Manipulate the method in IUserStore and UserManager so that it cannot find the user
            _store.Setup(x => x.FindByNameAsync(email)).Returns(Task.FromResult(user));
            _userManager.Setup(x => x.FindByNameAsync(email)).Returns(Task.FromResult(user));

            // ACT
            var result = await _controller.Edit(email);

            // ASSERT
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }

        // Edit GET - When ProvinceId IS NULL case cannot be tested because it includes extention method
        [TestMethod]
        public async Task EditWhenEmailNotNullUserFoundNotNullFoundProvinceIdNotNull()
        {
            var dummyUser = new ApplicationUser
            {
                UserName = "czebahi@gmail.com",
                Email = "czebahi@gmail.com",
                ProvinceId = 1,
            };

            var dummyProvince = new Province
            {
                ProvinceId = 1,
                ProvinceName = "ON"

            };

            _userSet = MockingHelper.Create(new List<ApplicationUser> { dummyUser }.AsQueryable());
            _provinceSet = MockingHelper.Create(new List<Province> { dummyProvince }.AsQueryable());

            // Set the mocked db sets in the mocked db context
            _db.Setup(c => c.Users).Returns(_userSet.Object);
            _db.Setup(c => c.Provinces).Returns(_provinceSet.Object);

            // Manipulate the method in IUserStore and UserManager so that it can find the user
            _store.Setup(x => x.FindByNameAsync(dummyUser.Email)).Returns(Task.FromResult(dummyUser));
            _userManager.Setup(x => x.FindByNameAsync(dummyUser.Email)).Returns(Task.FromResult(dummyUser));

            // ACT
            var result = await _controller.Edit(dummyUser.Email);
            var resultView = result as ViewResult;
            var resultModel = resultView.Model as ApplicationUser;

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            Assert.IsNotNull(resultView.Model);
            Assert.AreEqual(dummyUser.Email, resultModel.Email);
            Assert.AreEqual(dummyProvince.ProvinceId, resultModel.ProvinceId);
        }

        // Edit Post - needs to be tested
        [TestMethod]
        public async Task EditWhenModelValid()
        {
        }

        // Edit Post
        [TestMethod]
        public async Task EditWhenModelNotValid()
        {
            var dummyUser = new ApplicationUser
            {
                UserName = "czebahi@gmail.com",
                Email = "czebahi@gmail.com",
                ProvinceId = 1
            };

            var dummyProvince = new Province
            {
                ProvinceId = 1,
                ProvinceName = "ON"
            };

            _userSet = MockingHelper.Create(new List<ApplicationUser> { dummyUser }.AsQueryable());
            _provinceSet = MockingHelper.Create(new List<Province> { dummyProvince }.AsQueryable());

            // Set the mocked db sets in the mocked db context
            _db.Setup(c => c.Users).Returns(_userSet.Object);
            _db.Setup(c => c.Provinces).Returns(_provinceSet.Object);

            // Set the ModelState INVALID
            _controller.ModelState.AddModelError("error", "some error");

            // ACT
            var result = await _controller.Edit(dummyUser);
            var resultView = result as ViewResult;
            var resultModel = resultView.Model as ApplicationUser;

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            Assert.IsNotNull(resultView.Model);
            Assert.AreEqual(dummyUser.Email, resultModel.Email);
            Assert.AreEqual(dummyUser.ProvinceId, resultModel.ProvinceId);
        }

        [TestMethod]
        public async Task DeleteWhenEmailNull()
        {
            // ARRANGE
            string email = null;

            // ACT
            var result = await _controller.Delete(email) as HttpStatusCodeResult;

            // ASSERT
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);

            // verify that the method was not called
            _userManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Never);
        }

        // Delete GET
        [TestMethod]
        public async Task DeleteWhenEmailNotNullFoundUserNull()
        {
            // ARRANGE
            var dummyUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "czebahi@gmail.com",
                    Email = "czebahi@gmail.com",
                    ProvinceId = 1
                }
            }.AsQueryable();

            _userSet = MockingHelper.Create(dummyUsers);

            _db.Setup(c => c.Users).Returns(_userSet.Object);

            // declare an email and assign a random email. Pass it as the argument
            var randomEmail = "random@gmail.com";

            // This value will get returned from UserManager
            ApplicationUser user = null;

            // Manipulate the method in and UserManager so that it cannot find the user
            _userManager.Setup(x => x.FindByEmailAsync(randomEmail)).Returns(Task.FromResult(user));

            // ACT
            var result = await _controller.Delete(randomEmail);

            // ASSERT
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
            _userManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);

        }

        // Delete GET
        [TestMethod]
        public async Task DeleteWhenEmailNotNullUserNotNull()
        {
            // ARRANGE
            var dummyUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "czebahi@gmail.com",
                    Email = "czebahi@gmail.com",
                    ProvinceId = 1
                }
            }.AsQueryable();

            var dummyProvince = new Province
            {
                ProvinceId = 1,
                ProvinceName = "ON"

            };

            _userSet = MockingHelper.Create(dummyUsers);
            _provinceSet = MockingHelper.Create(new List<Province> { dummyProvince }.AsQueryable());

            // Set the mocked db sets in the mocked db context
            _db.Setup(c => c.Users).Returns(_userSet.Object);
            _db.Setup(c => c.Provinces).Returns(_provinceSet.Object);

            // This value will get returned from UserManager
            ApplicationUser user = dummyUsers.ToList()[0];
            var email = user.Email;

            // Manipulate the method in and UserManager so that it cannot find the user
            _userManager.Setup(x => x.FindByEmailAsync(email)).Returns(Task.FromResult(user));

            // ACT
            var result = await _controller.Delete(email);
            var resultView = result as ViewResult;
            var resultViewModel = resultView.Model as ApplicationUser;

            // ASSERT
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            _userManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(user.UserName, resultViewModel.UserName);
            Assert.AreEqual(user.Email, resultViewModel.Email);
        }

        // DeleteConfirmed
        [TestMethod]
        public async Task DeleteConfirmedWhenFoundUserNotNull()
        {
            // ARRANGE
            var dummyUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "czebahi@gmail.com",
                    Email = "czebahi@gmail.com",
                    ProvinceId = 1
                }
            }.AsQueryable();

            _userSet = MockingHelper.Create(dummyUsers);

            // Set the mocked db sets in the mocked db context
            _db.Setup(c => c.Users).Returns(_userSet.Object);

            // This value will get returned from UserManager
            ApplicationUser user = dummyUsers.ToList()[0];
            var email = user.Email;

            // Manipulate the method in and UserManager so that it cannot find the user
            _userManager.Setup(x => x.FindByEmailAsync(email)).Returns(Task.FromResult(user));

            _db.Setup(x => x.SaveChanges()).Returns(3);

            // ACT
            var result = await _controller.DeleteConfirmed(user);

            // ASSERT
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
            _userManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            _userManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _db.Verify(x => x.SaveChanges(), Times.Once);
        }

        // DeleteConfirmed
        [TestMethod]
        public async Task DeleteConfirmedWhenFoundUserNull()
        {
            // ARRANGE
            var dummyUser = new ApplicationUser
            {
                UserName = "czebahi@gmail.com",
                Email = "czebahi@gmail.com",
                ProvinceId = 1
            };

            ApplicationUser found = null;

            string randomEmail = "random@gmail.com";

            // Manipulate the method in and UserManager so that it cannot find the user
            _userManager.Setup(x => x.FindByEmailAsync(randomEmail)).Returns(Task.FromResult(found));

            // ACT
            var result = await _controller.DeleteConfirmed(dummyUser) as HttpStatusCodeResult;

            // ASSERT
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            _userManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        }
    }
}