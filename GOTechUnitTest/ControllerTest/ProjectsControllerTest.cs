using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GOTech.Controllers;
using GOTech.Models;
using GOTechUnitTest.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

/* 
 * Name: Jo Lim
 * Date: Apr 6, 2019
 * Last Modified: Apr 7, 2019
 * Description: This class unit tests ProjectsController
 * Note: This Test class has to be modified as there are more functions that will be implemented in ProjectsController
 * */

namespace GOTechUnitTest.ControllerTest
{
    [TestClass]
    public class ProjectsControllerTest
    {
        // Declare mocked objects
        private Mock<DbSet<ApplicationUser>> _userSet;
        private Mock<DbSet<Review>> _reviewSet;
        private Mock<DbSet<Project>> _projectSet;
        private Mock<ApplicationDbContext> _db;

        // Declare the controller
        private ProjectsController _controller;

        // Initialize instance variables
        [TestInitialize]
        public void Initialize()
        {
            _userSet = new Mock<DbSet<ApplicationUser>>();
            _db = new Mock<ApplicationDbContext>();
            _projectSet = new Mock<DbSet<Project>>();
            _reviewSet = new Mock<DbSet<Review>>();

            // Very important to inject dependency. Otherwise, this test will use real db
            _controller = new ProjectsController(_db.Object);
        }

        [TestMethod]
        public void Index()
        {
            // ARRANGE
            var dummyProjects = new List<Project>
            {
                new Project
                {
                    ProjectId = 1,
                    Description = "Dummy Project 1"
                },
                new Project
                {
                    ProjectId = 2,
                    Description = "Dummy Project 2"
                }
            }.AsQueryable();

            _projectSet = MockingHelper.Create(dummyProjects);

            _db.Setup(c => c.Projects).Returns(_projectSet.Object);

            // ACT
            var result = _controller.Index() as ViewResult;

            // Type cast the ViewResult object to IEnumerable<T> to count elements
            var positions = (IEnumerable<Project>)result.Model;

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(2, positions.Count());
        }

        [TestMethod]
        public void DetailsWhenIdNull()
        {
            // ARRANGE
            var dummyProjects = new List<Project>
            {
                new Project
                {
                    ProjectId = 1,
                    Description = "Dummy Project 1"
                },
                new Project
                {
                    ProjectId = 2,
                    Description = "Dummy Project 2"
                }
            }.AsQueryable();

            _projectSet = MockingHelper.Create(dummyProjects);

            _db.Setup(c => c.Projects).Returns(_projectSet.Object);

            // arrange nullable int to pass as an argument
            int? nullId = null;

            // ACT
            var result = _controller.Details(nullId) as HttpStatusCodeResult;

            // ASSERT
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }


        [TestMethod]
        public void DetailsWhenIdNotNullFoundProjectNull()
        {
            // ARRANGE
            var dummyProjects = new List<Project>
            {
                new Project
                {
                    ProjectId = 1,
                    Description = "Dummy Project 1"
                },
                new Project
                {
                    ProjectId = 2,
                    Description = "Dummy Project 2"
                }
            }.AsQueryable();

            _projectSet = MockingHelper.Create(dummyProjects);

            _db.Setup(c => c.Projects).Returns(_projectSet.Object);

            // ACT
            var result = _controller.Details(3);

            // ASSERT
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }

        [TestMethod]
        public void DetailsWhenIdNotNullFoundProjectNotNull()
        {
            // ARRANGE
            var dummyProjects = new List<Project>
            {
                new Project
                {
                    ProjectId = 1,
                    Description = "Dummy Project 1"
                },
                new Project
                {
                    ProjectId = 2,
                    Description = "Dummy Project 2"
                }
            }.AsQueryable();

            _projectSet = MockingHelper.Create(dummyProjects);

            _db.Setup(c => c.Projects).Returns(_projectSet.Object);

            // ACT
            var result = _controller.Details(2) as ViewResult; ;

            // Type cast
            var project = result.Model as Project;

            // ASSERT
            Assert.AreNotEqual(null, project);
            Assert.AreEqual(project.ProjectId, 2);
            Assert.AreEqual(project.Description, "Dummy Project 2");
        }

        // Create GET
        [TestMethod]
        public void CreateGet()
        {
            // ARRANGE
            // ACT
            var result = _controller.Create() as ViewResult;
            // ASSERT
            Assert.AreNotEqual(null, result);
        }

        // Create POST when model state is not valid
        [TestMethod]
        public void CreateWhenModelStateValid()
        {
            // ARRANGE
            var dummyProject = new Project
            {
                ProjectId = 1,
                Description = "Dummy Project 1"
            };

            _projectSet = MockingHelper.Create(new List<Project> { dummyProject }.AsQueryable());

            _db.Setup(c => c.Projects).Returns(_projectSet.Object);

            // ACT
            var result = _controller.Create(dummyProject);

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        // Create POST when model state is not valid
        [TestMethod]
        public void CreateWhenModelStateNotValid()
        {
            // ARRANGE
            // set the model state invalid
            _controller.ModelState.AddModelError("error", "some error");

            // ACT
            var result = _controller.Create(null);


            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreNotEqual(typeof(RedirectToRouteResult), result.GetType());
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        // Edit GET
        [TestMethod]
        public void EditWhenIdNull()
        {
            // ARRANGE
            // arrange nullable int to pass as an argument
            int? nullId = null;

            // ACT
            var result = _controller.Edit(nullId) as HttpStatusCodeResult;

            // ASSERT
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        // Edit GET
        [TestMethod]
        public void EditWhenIdNotNullFoundProjectNull()
        {
            // ARRANGE
            var dummyProjects = new List<Project>
            {
                new Project
                {
                    ProjectId = 1,
                    Description = "Dummy Project 1"
                },
                new Project
                {
                    ProjectId = 2,
                    Description = "Dummy Project 2"
                }
            }.AsQueryable();

            _projectSet = MockingHelper.Create(dummyProjects);

            _db.Setup(c => c.Projects).Returns(_projectSet.Object);

            // ACT
            var result = _controller.Edit(4);

            // ASSERT
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }

        // Edit GET
        [TestMethod]
        public void EditWhenIdNotNullFoundProjectNotNull()
        {
            // ARRANGE
            var dummyProjects = new List<Project>
            {
                new Project
                {
                    ProjectId = 1,
                    Description = "Dummy Project 1"
                },
                new Project
                {
                    ProjectId = 2,
                    Description = "Dummy Project 2"
                }
            }.AsQueryable();

            _projectSet = MockingHelper.Create(dummyProjects);

            _db.Setup(c => c.Projects).Returns(_projectSet.Object);

            // ACT
            var result = _controller.Edit(2) as ViewResult;

            var project = result.Model as Project;

            // ASSERT
            Assert.AreNotEqual(null, project);
            Assert.AreEqual(project.ProjectId, 2);
            Assert.AreEqual(project.Description, "Dummy Project 2");
        }

        // Edit POST - These cases must be tested AFTER attaching review objects to a Project object
        // Needs to be tested
        [TestMethod]
        public void EditWhenModelStateValid()
        {
        }

        // Edit POST
        [TestMethod]
        public void EditWhenModelStateNotValid()
        {
           
        }

        // Delete GET
        [TestMethod]
        public void DeleteWhenIdNull()
        {
            // ARRANGE
            // arrange nullable int to pass as an argument
            int? nullId = null;

            // ACT
            var result = _controller.Delete(nullId) as HttpStatusCodeResult;

            // ASSERT
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        // Delete GET
        [TestMethod]
        public void DeleteWhenIdNotNullFoundProjectNull()
        {
            // ARRANGE
            var dummyProjects = new List<Project>
            {
                new Project
                {
                    ProjectId = 1,
                    Description = "Dummy Project 1"
                },
                new Project
                {
                    ProjectId = 2,
                    Description = "Dummy Project 2"
                }
            }.AsQueryable();

            // Mock the project DbSet
            _projectSet = MockingHelper.Create(dummyProjects);

            _db.Setup(c => c.Projects).Returns(_projectSet.Object);

            // ACT
            var result = _controller.Delete(4);

            // ASSERT
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }

        // Delete GET
        [TestMethod]
        public void DeleteWhenIdNotNullFoundProjectNotNull()
        {
            // ARRANGE
            var dummyProjects = new List<Project>
            {
                new Project
                {
                    ProjectId = 1,
                    Description = "Dummy Project 1"
                },
                new Project
                {
                    ProjectId = 2,
                    Description = "Dummy Project 2"
                }
            }.AsQueryable();

            // Mock the project DbSet
            _projectSet = MockingHelper.Create(dummyProjects);

            _db.Setup(x => x.Projects).Returns(_projectSet.Object);

            // ACT
            var result = _controller.Delete(2) as ViewResult; ;

            var project = result.Model as Project;

            // ASSERT
            Assert.AreNotEqual(null, project);
            Assert.AreEqual(project.ProjectId, 2);
            Assert.AreEqual(project.Description, "Dummy Project 2");
        }

        // DeleteConfirmed POST
        // This case must be expanded AFTER attaching review objects to a Project object
        // Assigned Reviews to a Project must be deleted too. This logic has not been implemented.
        [TestMethod]
        public void DeleteConfirmed()
        {
            // ARRANGE
            var dummyProjects = new List<Project>
            {
                new Project
                {
                    ProjectId = 1,
                    Description = "Dummy Project 1"
                },
                new Project
                {
                    ProjectId = 2,
                    Description = "Dummy Project 2"
                }
            }.AsQueryable();


            // Mock the project DbSet and ApplicationDbContext
            _projectSet = MockingHelper.Create(dummyProjects);
            _db.Setup(x => x.Projects).Returns(_projectSet.Object);

            _db.Setup(x => x.Projects.Remove(It.IsAny<Project>())).Returns(dummyProjects.ToList()[1]);

            _db.Setup(x => x.Projects.Find())
                .Returns(dummyProjects.ToList()[1]);

            _db.Setup(c => c.SaveChanges()).Returns(1);
            
            // ACT - delete the Project with id 2
            var result = _controller.DeleteConfirmed(2);

            // ASSERT
            Assert.AreNotEqual(null, result);

            // check if the method on DbSet was called
            _db.Verify(x => x.Projects.Find(It.IsAny<int>()), Times.Once);
            _db.Verify(x => x.Projects.Remove(It.IsAny<Project>()), Times.Once);
        }
    }
}