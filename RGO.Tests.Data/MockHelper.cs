using Microsoft.EntityFrameworkCore;
using Moq;

namespace RR.Tests.Data; 

public static class MockHelper
{
    public static Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> data = null) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        if (data != null)
        {
            var asyncEnumerable = new TestAsyncEnumerable<T>(data);
            mockSet.As<IAsyncEnumerable<T>>()
                   .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                   .Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T>(data.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

        return mockSet;
    }

    public static Mock<MockableDbSetWithExtensions<T>> GetMockableDbSetWithExtensions<T>(IQueryable<T> data = null)
        where T : class
    {
        var mockSet = new Mock<MockableDbSetWithExtensions<T>>();
        if (data != null)
        {
            var asyncEnumerable = new TestAsyncEnumerable<T>(data);
            mockSet.As<IAsyncEnumerable<T>>()
                   .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                   .Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T>(data.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

        return mockSet;
    }
}