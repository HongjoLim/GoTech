using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * Name: Jo Lim
 * Date: Apr 5, 2019
 * Last Modified: Apr 5, 2019
 * Description: This class helps to mock db set and store reusable code
 * */

namespace GOTechUnitTest.Tools
{
    public class MockingHelper
    {
        public static Mock<DbSet<T>> Create<T>(IQueryable<T> data) where T : class
        {
            var queryable = data.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            
            return dbSet;
        }
    }
}
