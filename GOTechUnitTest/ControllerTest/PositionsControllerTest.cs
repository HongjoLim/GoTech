using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public void CreatePostWhenModelStateValid()
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

        //// Create POST when model state is not valid
        //[TestMethod]
        //public void CreatePostWhenModelStateNotValid()
        //{
        //    // ARRANGE
        //    var dummyPosition = new Position
        //    {
        //        PositionId = 1,
        //        Title = null
        //    };

        //    // Manipulate the model state "Invalid"
        //    var validationContext = new ValidationContext(dummyPosition, null, null);
        //    var validationResults = new List<ValidationResult>();
        //    Validator.TryValidateObject(dummyPosition, validationContext, validationResults, true);
        //    foreach (var validationResult in validationResults)
        //    {
        //        _controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
        //    }

        //    // ACT
        //    var result = _controller.Create(dummyPosition);


        //    // ASSERT
        //    Assert.AreNotEqual(null, result);
        //    Assert.AreNotEqual(typeof(RedirectToRouteResult), result.GetType());
        //    Assert.AreEqual(typeof(ViewResult), result.GetType());
        //}

    }
}
