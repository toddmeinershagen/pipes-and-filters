using System.Threading;
using System.Threading.Tasks;

namespace Patterns.PipesAndFilters
{
    public interface IAsyncFilter<T>
    {
        Task<T> ExecuteAsync(T input, CancellationToken cancellationToken);
    }
}
