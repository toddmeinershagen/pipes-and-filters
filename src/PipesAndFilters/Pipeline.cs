using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PipesAndFilters
{
    public class Pipeline<T> : IPipeline<T>
    {
        private readonly List<IAsyncFilter<T>> _filters = new List<IAsyncFilter<T>>();
        private readonly IServiceProvider _serviceProvider;

        public Pipeline(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public Pipeline()
            : this(new ActivatorServiceProvider())
        { }

        public Pipeline<T> Add(IAsyncFilter<T> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            _filters.Add(filter);
            return this;
        }

        public Pipeline<T> Add<TFilter>() where TFilter : IAsyncFilter<T>
        {
            return Add(_serviceProvider.GetService<TFilter>());
        }

        public async Task<T> ExecuteAsync(T input, CancellationToken cancellationToken = default)
        {
            T current = input;

            foreach (var filter in _filters)
            {
                current = await filter.ExecuteAsync(current, cancellationToken);
            }

            return current;
        }
    }
}
