namespace UPool
{
    public interface IGenerator
    {
        IPoolable CreateInstance();
    }
}
