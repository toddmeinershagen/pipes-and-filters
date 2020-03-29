namespace PipesAndFilters.Tests
{
    public class Append : SyncFilterBase<string>
	{
		private readonly string _text;

		public Append(string text)
		{
			this._text = text;
		}

		protected override string OnExecute(string input)
		{
			return input += _text;
		}
	}
}