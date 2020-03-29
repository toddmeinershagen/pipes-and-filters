using System.Threading;
using System.Threading.Tasks;

namespace PipesAndFilters
{
    public interface IPipeline<T> : IAsyncFilter<T>
    {
        Pipeline<T> Add(IAsyncFilter<T> filter);
        Pipeline<T> Add<TFilter>() where TFilter : IAsyncFilter<T>;
    }
}