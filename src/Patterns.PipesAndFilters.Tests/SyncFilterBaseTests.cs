using FluentAssertions;
using NUnit.Framework;
using Patterns.PipesAndFilters.Tests.Filters;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Patterns.PipesAndFilters.Tests
{
    [TestFixture]
    public class given_a_derived_sync_filter_and_cancelled_token_when_executing
    {
        private SyncCallCounter _callCounter;
        private Task<string> _resultingTask;

        [OneTimeSetUp]
        public void Before()
        {
            _callCounter = new SyncCallCounter();
            var source = new CancellationTokenSource();
            var cancellationToken = source.Token;
            source.Cancel();

            Func<Task<string>> function = async () => await _callCounter.ExecuteAsync("", cancellationToken);
            _resultingTask = function();
        }

        [Test]
        public void then_task_should_not_be_marked_completedsuccessfully()
        {
            _resultingTask.IsCompletedSuccessfully.Should().BeFalse();
        }

        [Test]
        public void then_task_should_be_marked_canceled()
        {
            _resultingTask.IsCanceled.Should().BeTrue();
        }

        [Test]
        public void then_should_not_call_derived_code()
        {
            _callCounter.Value.Should().Be(0);
        }
    }

    [TestFixture]
    public class given_a_derived_sync_filter_and_noncancelled_token_when_executing
    {
        private SyncCallCounter _callCounter;
        private Task<string> _resultingTask;

        [OneTimeSetUp]
        public void Before()
        {
            _callCounter = new SyncCallCounter();
            var source = new CancellationTokenSource();
            var cancellationToken = source.Token;

            Func<Task<string>> function = async () => await _callCounter.ExecuteAsync("", cancellationToken);
            _resultingTask = function();
        }

        [Test]
        public void then_task_should_be_marked_completedsuccessfully()
        {
            _resultingTask.IsCompletedSuccessfully.Should().BeTrue();
        }

        [Test]
        public void then_task_should_not_be_marked_canceled()
        {
            _resultingTask.IsCanceled.Should().BeFalse();
        }

        [Test]
        public void then_should_call_derived_code()
        {
            _callCounter.Value.Should().Be(1);
        }

        [Test]
        public void then_should_return_result_from_derived_code()
        {
            _resultingTask.Result.Should().Be(1.ToString());
        }
    }
}
