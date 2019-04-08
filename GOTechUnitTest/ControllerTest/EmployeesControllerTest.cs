using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GOTech;
using GOTech.Controllers;
using GOTech.Models;
using GOTechUnitTest.Tools;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

/* 
 * Name: Jo Lim
 * Date: Apr 5, 2019
 * Last Modified: Apr 7, 2019
 * Description: This class unit tests EmployeesController
 * */

namespace GOTechUnitTest.ControllerTest
{
    [TestClass]
    public class EmployeesControllerTest
    {
        // Declare mocked objects
        private Mock<DbSet<ApplicationUser>> _userSet;
        private Mock<DbSet<Province>> _provinceSet;
        private Mock<DbSet<Position>> _positionSet;
        private Mock<ApplicationDbContext> _db;
        private Mock<IUserStore<ApplicationUser>> _store;
        private Mock<ApplicationUserManager> _userManager;

        // Declare the controller
        private EmployeesController _controller; 

        // Initialize instance variables
        [TestInitialize]
        public void Initialize()
        {
            _userSet = new Mock<DbSet<ApplicationUser>>();
            _positionSet = new Mock<DbSet<Position>>();
            _db = new Mock<ApplicationDbContext>();
            _provinceSet = new Mock<DbSet<Province>>();
            _store = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new Mock<ApplicationUserManager>(_store.Object);

            // Very important to inject dependency. Otherwise, this test will use real db
            _controller = new EmployeesController(_userManager.Object, _db.Object);
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
            var employees = (IEnumerable<ApplicationUser>)result.Model;

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(1, employees.Count());
        }

        // When there is 1 employee and 1 customer in our db
        [TestMethod]
        public void IndexWhenNoEmployee()
        {
            // ARRANGE
            var dummyUsers = new List<ApplicationUser>
            {
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

            // Type cast the ViewResult object to IEnumerable<T> to count Members
            var employees = (IEnumerable<ApplicationUser>) result.Model;

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(0, employees.Count());
        }

        [TestMethod]
        public void DetailsWhenEmailNotNull()
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

            // ACT
            var resultTask = _controller.Details(dummyUsers.FirstOrDefault().Email);

            // Type cast
            var result = (ViewResult) resultTask;

            // Type cast the ViewResult object to ApplicationUser to count Members
            var employee = (ApplicationUser) result.Model;

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(dummyUsers.FirstOrDefault().Email, employee.Email);
            Assert.AreEqual(dummyUsers.FirstOrDefault().ProvinceId, employee.ProvinceId);
        }

        [TestMethod]
        public void DetailsWhenEmailNull()
        {
            // ARRANGE
            string email = null;

            // ACT
            var resultTask = _controller.Details(email);

            // Type cast
            var result = resultTask as HttpStatusCodeResult;

            // ASSERT
            Assert.AreEqual((int) HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void DetailsWhenEmailNotNullFoundUserNull()
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

            // ACT
            var resultTask = _controller.Details("czebahi@gmail.com");

            // ASSERT
            Assert.AreEqual(typeof(HttpNotFoundResult) , resultTask.GetType());
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

        // Edit GET
        [TestMethod]
        public async Task EditWhenEmailNotNullUserFoundNotNull()
        {
            var dummyUser = new ApplicationUser
            {
                UserName = "czebahi@gmail.com",
                Email = "czebahi@gmail.com",
                ProvinceId = 1,
                PositionId = 1
            };

            var dummyPosition = new Position
            {
                PositionId = 1,
                Title = "Developer"
            };

            var dummyProvince = new Province
            {
                ProvinceId = 1,
                ProvinceName = "ON"

            };

            _userSet = MockingHelper.Create(new List<ApplicationUser> { dummyUser }.AsQueryable());
            _provinceSet = MockingHelper.Create(new List<Province> { dummyProvince}.AsQueryable());
            _positionSet = MockingHelper.Create(new List<Position> { dummyPosition }.AsQueryable());

            // Set the mocked db sets in the mocked db context
            _db.Setup(c => c.Users).Returns(_userSet.Object);
            _db.Setup(c => c.Provinces).Returns(_provinceSet.Object);
            _db.Setup(c => c.Positions).Returns(_positionSet.Object);

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
            Assert.AreEqual(dummyPosition.PositionId, resultModel.PositionId);
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
                ProvinceId = 1,
                PositionId = 1
            };

            var dummyPosition = new Position
            {
                PositionId = 1,
                Title = "Developer"
            };

            var dummyProvince = new Province
            {
                ProvinceId = 1,
                ProvinceName = "ON"
            };

            _userSet = MockingHelper.Create(new List<ApplicationUser> { dummyUser }.AsQueryable());
            _provinceSet = MockingHelper.Create(new List<Province> { dummyProvince }.AsQueryable());
            _positionSet = MockingHelper.Create(new List<Position> { dummyPosition }.AsQueryable());

            // Set the mocked db sets in the mocked db context
            _db.Setup(c => c.Users).Returns(_userSet.Object);
            _db.Setup(c => c.Provinces).Returns(_provinceSet.Object);
            _db.Setup(c => c.Positions).Returns(_positionSet.Object);

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
                    ProvinceId = 1,
                    PositionId = 1
                }
            }.AsQueryable();

            var dummyPosition = new Position
            {
                PositionId = 1,
                Title = "Developer"
            };

            var dummyProvince = new Province
            {
                ProvinceId = 1,
                ProvinceName = "ON"

            };

            _userSet = MockingHelper.Create(dummyUsers);
            _provinceSet = MockingHelper.Create(new List<Province> { dummyProvince }.AsQueryable());
            _positionSet = MockingHelper.Create(new List<Position> { dummyPosition }.AsQueryable());

            // Set the mocked db sets in the mocked db context
            _db.Setup(c => c.Users).Returns(_userSet.Object);
            _db.Setup(c => c.Provinces).Returns(_provinceSet.Object);
            _db.Setup(c => c.Positions).Returns(_positionSet.Object);

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
        public async Task DeleteConfirmedWhenFoundEmployeeNotNull()
        {
            // ARRANGE
            var dummyUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "czebahi@gmail.com",
                    Email = "czebahi@gmail.com",
                    ProvinceId = 1,
                    PositionId = 1
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
            _userManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _db.Verify(x => x.SaveChanges(), Times.Once);
        }

        // DeleteConfirmed
        [TestMethod]
        public async Task DeleteConfirmedWhenFoundEmployeeNull()
        {
            // ARRANGE
            var dummyUser = new ApplicationUser
            {
                UserName = "czebahi@gmail.com",
                Email = "czebahi@gmail.com",
                ProvinceId = 1,
                PositionId = 1
            };

            ApplicationUser found = null;

            string randomEmail = "random@gmail.com";

            // Manipulate the method in and UserManager so that it cannot find the user
            _userManager.Setup(x => x.FindByEmailAsync(randomEmail)).Returns(Task.FromResult(found));

            // ACT
            var result = await _controller.DeleteConfirmed(dummyUser) as HttpStatusCodeResult;

            // ASSERT
            Assert.AreEqual((int) HttpStatusCode.BadRequest, result.StatusCode);
            _userManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
