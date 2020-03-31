using System;

namespace Patterns.PipesAndFilters
{
    public class ActivatorServiceProvider : IServiceProvider
    {
        public T GetService<T>()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }
    }
}
