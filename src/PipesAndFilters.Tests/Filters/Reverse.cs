using System;
using System.Linq;

namespace PipesAndFilters.Tests
{
    public class Reverse : SyncFilterBase<string>
	{
		protected override string OnExecute(string input)
		{
			return new string(input.ToCharArray().Reverse().ToArray());
		}
	}
}