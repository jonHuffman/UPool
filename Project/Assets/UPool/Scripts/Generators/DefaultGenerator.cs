using System;

namespace UPool
{
    /// <summary>
    /// Default object generator for the Pool
    /// </summary>
    public class DefaultGenerator : IGenerator
    {
        private AbstractPool _owner;

        public DefaultGenerator(AbstractPool owner)
        {
            _owner = owner;
        }

        public IPoolable CreateInstance()
        {
            return (IPoolable)Activator.CreateInstance(_owner.PoolType, true);
        }
    }
}
