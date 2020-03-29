using System.Threading;
using System.Threading.Tasks;

namespace PipesAndFilters
{
    public abstract class AsyncFilterBase<T> : IAsyncFilter<T>
    {
        public async Task<T> ExecuteAsync(T input, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return await Task.FromCanceled<T>(cancellationToken);
            }

            return await OnExecuteAsync(input, cancellationToken);
        }

        protected abstract Task<T> OnExecuteAsync(T input, CancellationToken cancellationToken);
    }
}
