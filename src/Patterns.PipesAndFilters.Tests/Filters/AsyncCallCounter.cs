using Patterns.PipesAndFilters;
using System.Threading;
using System.Threading.Tasks;

namespace Patterns.PipesAndFilters.Tests.Filters
{
    public class AsyncCallCounter : AsyncFilterBase<string>
    {
        protected override async Task<string> OnExecuteAsync(string input, CancellationToken cancellationToken)
        {
            Value++;
            return await Task.FromResult(Value.ToString());
        }

        public int Value { get; private set; }
    }
}
