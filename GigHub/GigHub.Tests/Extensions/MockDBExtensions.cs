using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Tests.Extensions
{
    public static class MockDBExtensions
    {
        public static void SetSource<T>(this Mock<DbSet<T>> mockSet, IList<T> Source) where T : class
        {
            var data = Source.AsQueryable();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        }
    }
}
