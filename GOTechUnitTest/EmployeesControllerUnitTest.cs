using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using GOTech;
using GOTech.Controllers;
using GOTech.Models;
using GOTechUnitTest.Tools;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

/* 
 * Name: Jo Lim
 * Date: Apr 5, 2019
 * Last Modified: Apr 5, 2019
 * Description: This class unit tests EmployeesController
 * */

namespace GOTechUnitTest
{
    [TestClass]
    public class EmployeesControllerUnitTest
    {
        // Declare mocked objects
        private Mock<DbSet<ApplicationUser>> _userSet;
        private Mock<DbSet<Province>> _provinceSet;
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
            _db = new Mock<ApplicationDbContext>();
            _provinceSet = new Mock<DbSet<Province>>();
            _store = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new Mock<ApplicationUserManager>(_store.Object);
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

            // Type cast the Result from TasdummyProvincesk<ActionResult> method to ViewResult
            var result = (ViewResult) resultTask;

            // Type cast the ViewResult object to ApplicationUser to count Members
            var employee = (ApplicationUser) result.Model;

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(dummyUsers.FirstOrDefault().Email, employee.Email);
            Assert.AreEqual(dummyUsers.FirstOrDefault().ProvinceId, employee.ProvinceId);
        }
    }
}
