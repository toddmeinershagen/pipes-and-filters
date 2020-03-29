using System;

namespace PipesAndFilters
{
    public interface IServiceProvider
    {
        T GetService<T>();
    }
}
