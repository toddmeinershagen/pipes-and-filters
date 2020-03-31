using FluentAssertions;
using NUnit.Framework;
using Patterns.PipesAndFilters;
using Patterns.PipesAndFilters.Tests.Filters;
using System;
using System.Threading.Tasks;

namespace Patterns.PipesAndFilters.Tests
{
	[TestFixture]
	public class PipelineTests
	{
		[Test]
		public void given_a_null_serviceprovider_when_constructing_then_should_throw()
		{
			Action action = () => new Pipeline<string>(null);
			action.Should()
				.Throw<ArgumentNullException>()
				.WithMessage("Value cannot be null. (Parameter 'serviceProvider')");
		}

		[Test]
		public async Task given_pipeline_with_no_filters_when_executing_then_returns_original_input()
		{
			var input = "hi";
			var pipeline = new Pipeline<string>();

			var result = await pipeline.ExecuteAsync(input);

			result.Should().Be(input);
		}

		[Test]
		public void given_pipeline_when_adding_a_null_filter_then_throws()
		{
			var pipeline = new Pipeline<string>();
			Action action = () => pipeline.Add(null as IAsyncFilter<string>);

			action.Should()
				.Throw<ArgumentNullException>()
				.WithMessage("Value cannot be null. (Parameter 'filter')");
		}

		[Test]
		public async Task given_filters_when_executing_then_should_execute_in_order()
		{
			var input = "one";
			var pipeline = new Pipeline<string>();

			pipeline
				.Add(new Append(", two"))
				.Add(new Append(", three"));
			var result = await pipeline.ExecuteAsync(input);

			result.Should().Be("one, two, three");
		}

		[Test]
		public async Task given_no_service_provider_and_filter_added_as_type_when_executing_then_should_use_default_service_provider()
		{
			var input = "one";
			var pipeline = new Pipeline<string>();
			pipeline.Add<Reverse>();

			var result = await pipeline.ExecuteAsync(input);

			result.Should().Be("eno");
		}

		[Test]
		public async Task given_pipeline_as_filter_and_additional_filters_when_executing_then_should_return_results()
		{
			var input = "";

			var numbers = new Pipeline<string>()
				.Add(new Append("1"))
				.Add(new Pause(TimeSpan.FromMilliseconds(25)))
				.Add(new Append("2"))
				.Add(new Append("3"));

			var result = await new Pipeline<string>()
				.Add(numbers)
				.Add<Reverse>()
				.ExecuteAsync(input);

			result.Should().Be("321");
		}
	}
}