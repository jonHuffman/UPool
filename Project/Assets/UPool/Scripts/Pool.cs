using System;

namespace UPool
{
    /// <summary>
    /// A Generic Object Pool that can handle both traditional C# objects and 
    /// </summary>
    /// <typeparam name="T">An IPoolable</typeparam>
    public class Pool<T> : AbstractPool where T : IPoolable
    {
        protected override Type PoolType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Creates a pool of the specified size for the given object type
        /// </summary>
        /// <param name="initialSize">Initial size of the pool</param>
        public Pool(int initialSize) : base(initialSize)
        {
        }

        /// <summary>
        /// Aquires an unallocated object from the pool and provides it for use. If no unallocated objects are available, a new one will be created.
        /// </summary>
        /// <returns>An object of type T for use</returns>
        public new T Aquire()
        {
            return (T)base.Aquire();
        }
    }
}