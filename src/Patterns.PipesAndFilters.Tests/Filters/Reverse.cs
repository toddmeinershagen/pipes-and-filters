using Patterns.PipesAndFilters;
using System;
using System.Linq;

namespace Patterns.PipesAndFilters.Tests.Filters
{
	public class Reverse : SyncFilterBase<string>
	{
		protected override string OnExecute(string input)
		{
			return new string(input.ToCharArray().Reverse().ToArray());
		}
	}
}