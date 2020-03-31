namespace Patterns.PipesAndFilters
{
    public interface IServiceProvider
    {
        T GetService<T>();
    }
}
