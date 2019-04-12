using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http.Results;
using GOTech.Controllers.API;
using GOTech.Models;
using GOTech.Models.API;
using GOTechUnitTest.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

/* 
 * Name: Jo Lim
 * Date: Apr 8, 2019
 * Last Modified: Apr 8, 2019
 * Description: This class unit tests ReviewsController, which is a WebAPI controller
 * */

namespace GOTechUnitTest.APITest
{
    [TestClass]
    public class ReviewsControllerTest
    {
        // Declare mocked objects
        private Mock<ApplicationDbContext> _db;
        private Mock<DbSet<Review>> _reviewSet;
        private ReviewsController _controller;
        private IReviewBL _reviewBL;

        [TestInitialize]
        public void Initialize()
        {
            _db = new Mock<ApplicationDbContext>();
            _reviewSet = new Mock<DbSet<Review>>();

            _reviewBL = new ReviewBL(_db.Object);
            _controller = new ReviewsController(_reviewBL);

        }

        // This reusable method is used to generate reviews
        private IQueryable<Review> GenerateReviews()
        {
            var reviews = new List<Review>
            {
                new Review
                {
                    ReviewId = 1,
                    ReviewBody = "Random comment1",
                    PostDate = DateTime.Now,
                },
                new Review
                {
                    ReviewId = 2,
                    ReviewBody = "Random comment2",
                    PostDate = DateTime.Now,
                },
            }.AsQueryable();

            return reviews;
        }

        [TestMethod]
        public void GetReviewsReturnNull()
        {
            // ARRANGE - generate empty List
            IQueryable<Review> dummyReviews = new List<Review> ().AsQueryable();

            var _reviewSet = MockingHelper.Create(dummyReviews);

            _db.Setup(x => x.Reviews).Returns(_reviewSet.Object);

            // ACT
            var result = _controller.GetReviews();

            // ASSERT
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetReviewsReturnNotNull()
        {
            // ARRANGE - make actual list of reviews 
            var dummyReviews = GenerateReviews();

            var _reviewSet = MockingHelper.Create(dummyReviews);

            _db.Setup(x => x.Reviews).Returns(_reviewSet.Object);

            // ACT
            var result = _controller.GetReviews();

            // ASSERT
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetReviewByIdWhenFoundReviewNull()
        {
            // ARRANGE - make actual list of reviews 
            var dummyReviews = GenerateReviews();

            var _reviewSet = MockingHelper.Create(dummyReviews);

            _db.Setup(x => x.Reviews).Returns(_reviewSet.Object);

            var dummyId = 3;

            // ACT
            var result = _controller.GetReview(dummyId);

            // ASSERT - if the review cannot be found, the return result type is NotFoundResult
            Assert.AreEqual(typeof(NotFoundResult), result.GetType());
        }

        [TestMethod]
        public void GetReviewByIdWhenFoundReviewNotNull()
        {
            // ARRANGE - make actual list of reviews 
            var dummyReviews = GenerateReviews();

            var _reviewSet = MockingHelper.Create(dummyReviews);

            _db.Setup(x => x.Reviews).Returns(_reviewSet.Object);

            // Manipulate the db to return the estimated review
            _db.Setup(x => x.Reviews.Find(It.IsAny<int>())).Returns(dummyReviews.ToList()[0]);

            var dummyId = 1;

            // ACT
            var result = _controller.GetReview(dummyId);
            var modelId = ((OkNegotiatedContentResult<Review>)result).Content.ReviewId;

            // ASSERT - if the review can be found, check the type and the id of the entity
            Assert.AreEqual(typeof(OkNegotiatedContentResult<Review>), result.GetType());
            Assert.AreEqual(1, modelId);

        }

        [TestMethod]
        public void PutReview()
        { }


        [TestMethod]
        public void DeleteReview()
        { }

        [TestMethod]
        public void PostReview()
        { }

        [TestMethod]
        public void ReviewExists()
        { }

    }
}
