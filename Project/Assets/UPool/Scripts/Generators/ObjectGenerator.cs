using System;

namespace UPool
{
    public class DefaultGenerator : IGenerator
    {
        private Type _type;

        public DefaultGenerator(Type type)
        {
            _type = type;
        }

        public IPoolable CreateInstance()
        {
            return (IPoolable)Activator.CreateInstance(_type, true);
        }
    }
}
