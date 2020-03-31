using Patterns.PipesAndFilters;

namespace Patterns.PipesAndFilters.Tests.Filters
{
	public class Append : SyncFilterBase<string>
	{
		private readonly string _text;

		public Append(string text)
		{
			_text = text;
		}

		protected override string OnExecute(string input)
		{
			return input += _text;
		}
	}
}