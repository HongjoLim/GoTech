using System;
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
 * Last Modified: Apr 6, 2019
 * Description: This class unit tests PositionsController
 * */

namespace GOTechUnitTest.ControllerTest
{
    [TestClass]
    public class PositionsControllerTest
    {
        // Declare mocked objects
        private Mock<DbSet<ApplicationUser>> _userSet;
        private Mock<DbSet<Position>> _positionSet;
        private Mock<ApplicationDbContext> _db;

        // Declare the controller
        private PositionsController _controller;

        // Initialize instance variables
        [TestInitialize]
        public void Initialize()
        {
            _userSet = new Mock<DbSet<ApplicationUser>>();
            _db = new Mock<ApplicationDbContext>();
            _positionSet = new Mock<DbSet<Position>>();

            // Very important to inject dependency. Otherwise, this test will use real db
            _controller = new PositionsController(_db.Object);
        }

        // Position Title "UnAssigned" must not appear in the list
        [TestMethod]
        public void Index()
        {
            // ARRANGE
            var dummyPositions = new List<Position>
            {
                new Position
                {
                    PositionId = 1,
                    Title = "Developer"
                },
                new Position
                {
                    PositionId = 2,
                    Title = "Designer"
                },                
                new Position
                {
                    PositionId = 3,
                    Title = "UnAssigned"
                }
            }.AsQueryable();

            _positionSet = MockingHelper.Create(dummyPositions);

            _db.Setup(c => c.Positions).Returns(_positionSet.Object);

            // ACT
            var result = _controller.Index() as ViewResult;

            // Type cast the ViewResult object to IEnumerable<T> to count members
            var positions = (IEnumerable<Position>) result.Model;

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(2, positions.Count());
            Assert.AreEqual(positions.FirstOrDefault(x=>x.Title=="UnAssigned"), null);
        }

        [TestMethod]
        public void DetailsWhenIdNull()
        {
            // ARRANGE
            var dummyPositions = new List<Position>
            {
                new Position
                {
                    PositionId = 1,
                    Title = "Developer"
                },
                new Position
                {
                    PositionId = 2,
                    Title = "Designer"
                },
                new Position
                {
                    PositionId = 3,
                    Title = "UnAssigned"
                }
            }.AsQueryable();

            _positionSet = MockingHelper.Create(dummyPositions);

            _db.Setup(c => c.Positions).Returns(_positionSet.Object);

            // arrange nullable int to pass as an argument
            int? nullId = null;

            // ACT
            var result = _controller.Details(nullId) as HttpStatusCodeResult;

            // ASSERT
            Assert.AreEqual((int) HttpStatusCode.BadRequest, result.StatusCode);
        }
        
        [TestMethod]
        public void DetailsWhenIdNotNullFoundPositionNull()
        {
            // ARRANGE
            var dummyPositions = new List<Position>
            {
                new Position
                {
                    PositionId = 1,
                    Title = "Developer"
                },
                new Position
                {
                    PositionId = 2,
                    Title = "Designer"
                },
                new Position
                {
                    PositionId = 3,
                    Title = "UnAssigned"
                }
            }.AsQueryable();

            _positionSet = MockingHelper.Create(dummyPositions);

            _db.Setup(c => c.Positions).Returns(_positionSet.Object);

            // ACT
            var result = _controller.Details(4);

            // ASSERT
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }

        [TestMethod]
        public void DetailsWhenIdNotNullFoundPositionNotNull()
        {
            // ARRANGE
            var dummyPositions = new List<Position>
            {
                new Position
                {
                    PositionId = 1,
                    Title = "Developer"
                },
                new Position
                {
                    PositionId = 2,
                    Title = "Designer"
                },
                new Position
                {
                    PositionId = 3,
                    Title = "UnAssigned"
                }
            }.AsQueryable();

            _positionSet = MockingHelper.Create(dummyPositions);

            _db.Setup(c => c.Positions).Returns(_positionSet.Object);

            // ACT
            var result = _controller.Details(2) as ViewResult; ;

            // Type cast the ViewResult object to IEnumerable<T> to count members
            var position = result.Model as Position;

            // ASSERT
            Assert.AreNotEqual(null, position);
            Assert.AreEqual(position.PositionId, 2);
            Assert.AreEqual(position.Title, "Designer");
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
            var dummyPosition = new Position
            {
                PositionId = 1,
                Title = "Developer"
            };

            _positionSet = MockingHelper.Create(new List<Position> { dummyPosition }.AsQueryable());

            _db.Setup(c => c.Positions).Returns(_positionSet.Object);

            // ACT
            var result = _controller.Create(dummyPosition);

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
        public void EditWhenIdNotNullFoundPositionNull()
        {
            // ARRANGE
            var dummyPositions = new List<Position>
            {
                new Position
                {
                    PositionId = 1,
                    Title = "Developer"
                },
                new Position
                {
                    PositionId = 2,
                    Title = "Designer"
                },
                new Position
                {
                    PositionId = 3,
                    Title = "UnAssigned"
                }
            }.AsQueryable();

            _positionSet = MockingHelper.Create(dummyPositions);

            _db.Setup(c => c.Positions).Returns(_positionSet.Object);

            // ACT
            var result = _controller.Edit(4);

            // ASSERT
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }

        // Edit GET
        [TestMethod]
        public void EditWhenIdNotNullFoundPositionNotNull()
        {
            // ARRANGE
            var dummyPositions = new List<Position>
            {
                new Position
                {
                    PositionId = 1,
                    Title = "Developer"
                },
                new Position
                {
                    PositionId = 2,
                    Title = "Designer"
                },
                new Position
                {
                    PositionId = 3,
                    Title = "UnAssigned"
                }
            }.AsQueryable();

            _positionSet = MockingHelper.Create(dummyPositions);

            _db.Setup(c => c.Positions).Returns(_positionSet.Object);

            // ACT
            var result = _controller.Edit(2) as ViewResult; ;
            
            var position = result.Model as Position;

            // ASSERT
            Assert.AreNotEqual(null, position);
            Assert.AreEqual(position.PositionId, 2);
            Assert.AreEqual(position.Title, "Designer");
        }

        // Edit POST
        // Needs to be tested
        [TestMethod]
        public void EditWhenModelStateValid()
        {
        //    // ARRANGE
        //    var dummyPositions = new List<Position>
        //    {
        //        new Position
        //        {
        //            PositionId = 1,
        //            Title = "Developer"
        //        },
        //        new Position
        //        {
        //            PositionId = 2,
        //            Title = "Designer"
        //        },
        //        new Position
        //        {
        //            PositionId = 3,
        //            Title = "UnAssigned"
        //        }
        //    }.AsQueryable();

        //    _positionSet = MockingHelper.Create(dummyPositions);
        //    var chagnedModel = new Position
        //    {
        //        PositionId = 2,
        //        Title = "Design"
        //    };

        //    _db.Setup(c => c.Positions).Returns(_positionSet.Object);
        //    //Assert.IsTrue(dbContext.Object.ChangeTracker.Entries().Any(entry =>
        //    //entry.State == EntityState.Modified
        //    _db.Setup(c => c.ChangeTracker.Entries().Any).Returns(EntityState.Modified);

        //    // ACT
        //    var result = _controller.Edit(chagnedModel) as ViewResult;

        //    // ASSERT
        //    Assert.AreNotEqual(null, result);
        //    Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        // Edit POST
        [TestMethod]
        public void EditWhenModelStateNotValid()
        {
            // ARRANGE
            var dummyPosition = new Position
            {
                PositionId = 1,
                Title = "Manager"
            };

            _positionSet = MockingHelper.Create(new List<Position> { dummyPosition }.AsQueryable());

            _db.Setup(c => c.Positions).Returns(_positionSet.Object);
            _controller.ModelState.AddModelError("error", "some error");

            // ACT
            var result = _controller.Edit(1);
            var resultView = result as ViewResult;
            
            // the position object will be returned to the view
            Position returnedPosition = resultView.Model as Position;

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreNotEqual(typeof(RedirectToRouteResult), result.GetType());
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            Assert.AreEqual(1, returnedPosition.PositionId);
            Assert.AreEqual("Manager", returnedPosition.Title);
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
        public void DeleteWhenIdNotNullFoundPositionNull()
        {
            // ARRANGE
            var dummyPositions = new List<Position>
            {
                new Position
                {
                    PositionId = 1,
                    Title = "Developer"
                },
                new Position
                {
                    PositionId = 2,
                    Title = "Designer"
                },
                new Position
                {
                    PositionId = 3,
                    Title = "UnAssigned"
                }
            }.AsQueryable();

            _positionSet = MockingHelper.Create(dummyPositions);

            _db.Setup(c => c.Positions).Returns(_positionSet.Object);

            // ACT
            var result = _controller.Delete(4);

            // ASSERT
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }

        // Delete GET
        [TestMethod]
        public void DeleteWhenIdNotNullFoundPositionNotNull()
        {
            // ARRANGE
            var dummyPositions = new List<Position>
            {
                new Position
                {
                    PositionId = 1,
                    Title = "Developer"
                },
                new Position
                {
                    PositionId = 2,
                    Title = "Designer"
                },
                new Position
                {
                    PositionId = 3,
                    Title = "UnAssigned"
                }
            }.AsQueryable();

            _positionSet = MockingHelper.Create(dummyPositions);

            _db.Setup(c => c.Positions).Returns(_positionSet.Object);

            // ACT
            var result = _controller.Delete(2) as ViewResult; ;

            var position = result.Model as Position;

            // ASSERT
            Assert.AreNotEqual(null, position);
            Assert.AreEqual(position.PositionId, 2);
            Assert.AreEqual(position.Title, "Designer");
        }

        // DeleteConfirmed POST
        // Case1: There IS employee that had the positionId (user wants to delete)
        // Not fully tested. Not really test-friendly method
        [TestMethod]
        public void DeleteConfirmedWhenEmployeeWithThatPositionId()
        {
            // ARRANGE
            var dummyPositions = new List<Position>
                {
                    new Position
                    {
                        PositionId = 1,
                        Title = "Developer"
                    },
                    new Position
                    {
                        PositionId = 3,
                        Title = "UnAssigned"
                    }
                }.AsQueryable();

            var dummyEmployees = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "czebahi1@gmail.com",
                    Email = "czebahi@gmail.com",
                    PositionId = 1,
                },
                new ApplicationUser
                {
                    UserName = "czebahi2@gmail.com",
                    Email = "czebahi2@gmail.com",
                    PositionId = 1
                },
                new ApplicationUser
                {
                    UserName = "czebahi2@gmail.com",
                    Email = "czebahi2@gmail.com",
                    PositionId = 2
                }
            }.AsQueryable();

            // Mock the position and user DbSet
            _positionSet = MockingHelper.Create(dummyPositions);
            _userSet = MockingHelper.Create(dummyEmployees);

            _db.Setup(c => c.Positions).Returns(_positionSet.Object);
            _db.Setup(c => c.Users).Returns(_userSet.Object);

            // ACT - delete the positionId for Developer
            var result = _controller.DeleteConfirmed(1);

            // Check if the employees' position chagned to "UnAssigned"
            var employees = _db.Object.Users.Where(x => x.PositionId == 3).ToList();

            // ASSERT
            Assert.AreEqual(2, employees.Count());
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        // DeleteConfirmed POST
        // Case2: There is NO employee that had the positionId (user wants to delete)
        // Not fully tested. Not really test-friendly method
        [TestMethod]
        public void DeleteConfirmedWhenNoEmployeeWithThatPositionId()
        {
            // ARRANGE
            var dummyPositions = new List<Position>
            {
                new Position
                {
                    PositionId = 1,
                    Title = "Salesmen"
                },
                new Position
                {
                    PositionId = 2,
                    Title = "Developer"
                },
                new Position
                {
                    PositionId = 3,
                    Title = "UnAssigned"
                }
            }.AsQueryable();

            var dummyEmployees = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "czebahi1@gmail.com",
                    Email = "czebahi@gmail.com",
                    PositionId = 1,
                },
                new ApplicationUser
                {
                    UserName = "czebahi2@gmail.com",
                    Email = "czebahi2@gmail.com",
                    PositionId = 1
                },
                new ApplicationUser
                {
                    UserName = "czebahi2@gmail.com",
                    Email = "czebahi2@gmail.com",
                    PositionId = 1
                }
            }.AsQueryable();

            // Mock the position and user DbSet
            _positionSet = MockingHelper.Create(dummyPositions);
            _userSet = MockingHelper.Create(dummyEmployees);

            _db.Setup(x => x.Positions).Returns(_positionSet.Object);
            _db.Setup(x => x.Users).Returns(_userSet.Object);

            // ACT - delete the positionId for Developer
            var result = _controller.DeleteConfirmed(2);

            // Check if the employees' position chagned to "UnAssigned"
            var employees = _db.Object.Users.Where(x => x.PositionId == 3).ToList();

            var salesmen = _db.Object.Users.Where(x => x.PositionId == 1).ToList();

            // ASSERT
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(0, employees.Count());
            Assert.AreEqual(3, salesmen.Count());
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

    }
}
