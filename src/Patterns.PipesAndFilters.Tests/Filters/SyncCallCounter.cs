using Patterns.PipesAndFilters;

namespace Patterns.PipesAndFilters.Tests.Filters
{
    public class SyncCallCounter : SyncFilterBase<string>
    {
        protected override string OnExecute(string input)
        {
            Value++;
            return Value.ToString();
        }

        public int Value { get; private set; }
    }
}
