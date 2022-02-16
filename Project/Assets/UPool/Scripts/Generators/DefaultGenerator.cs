using System;
using System.Reflection;
using UnityEngine.Assertions;

namespace UPool
{
    /// <summary>
    /// Default object generator for the Pool
    /// </summary>
    public class DefaultGenerator<T> : IGenerator
    {
        private AbstractPool _owner;

        public DefaultGenerator(AbstractPool owner)
        {
            _owner = owner;
        }

        public IPoolable CreateInstance()
        {
            if (_owner.PoolType.IsAbstract)
            {
                throw new ArgumentException($"Cannot create an instance of Abstract type {_owner.PoolType.Name}");
            }

            ConstructorInfo constructorInfo = _owner.PoolType.GetConstructor(Type.EmptyTypes);
            Assert.IsNotNull(constructorInfo, $"Type {_owner.PoolType.Name} must have a parameterless constructor in order to be created by the Default Generator");

            return (IPoolable)constructorInfo.Invoke(null);
        }
    }
}
