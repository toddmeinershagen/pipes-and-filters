using System.Threading;
using System.Threading.Tasks;

namespace PipesAndFilters
{
    public abstract class SyncFilterBase<T> : AsyncFilterBase<T>
    {
        protected override async Task<T> OnExecuteAsync(T input, CancellationToken cancellationToken)
        {
            return await Task.FromResult(OnExecute(input));
        }

        protected abstract T OnExecute(T input);
    }
}
