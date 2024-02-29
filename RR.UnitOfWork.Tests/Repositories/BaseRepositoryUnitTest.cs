namespace RR.UnitOfWork.Tests.Repositories;

public class BaseRepositoryUnitTest
{
    public class AsyncEnumeratorWrapper<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public AsyncEnumeratorWrapper(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public T Current => _enumerator.Current;

        public ValueTask DisposeAsync()
        {
            _enumerator.Dispose();
            return new ValueTask();
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_enumerator.MoveNext());
        }
    }

    public class TestAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IEnumerable<T> _enumerable;

        public TestAsyncEnumerable(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new AsyncEnumeratorWrapper<T>(_enumerable.GetEnumerator());
        }
    }

    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _innerEnumerator;

        public TestAsyncEnumerator(IEnumerator<T> innerEnumerator)
        {
            _innerEnumerator = innerEnumerator;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_innerEnumerator.MoveNext());
        }

        public T Current => _innerEnumerator.Current;

        public ValueTask DisposeAsync()
        {
            _innerEnumerator.Dispose();
            return new ValueTask();
        }
    }
}