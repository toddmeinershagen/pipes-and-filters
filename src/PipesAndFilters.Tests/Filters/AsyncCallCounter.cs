using System.Threading;
using System.Threading.Tasks;

namespace PipesAndFilters.Tests
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
