using Patterns.PipesAndFilters;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Patterns.PipesAndFilters.Tests.Filters
{
	public class Pause : AsyncFilterBase<string>
	{
		private readonly TimeSpan _timeToPause;

		public Pause(TimeSpan timeToPause)
		{
			_timeToPause = timeToPause;
		}

		protected override async Task<string> OnExecuteAsync(string input, CancellationToken cancellationToken)
		{
			await Task.Delay(_timeToPause, cancellationToken);
			return input;
		}
	}
}