using System;
using System.Collections.Generic;

namespace UPool
{
    public abstract class AbstractPool
    {
        protected IGenerator _generator;
        protected List<IPoolable> _pool;
        protected Queue<IPoolable> _availableObjects;

        /// <summary>
        /// The total size of the Object Pool
        /// </summary>
        public int Size
        {
            get { return _pool.Count; }
        }

        /// <summary>
        /// The total number of available Objects in the pool.
        /// Should you run out, additional Objects will be created for you as needed.
        /// </summary>
        public int AvailableItems
        {
            get { return _availableObjects.Count; }
        }

        /// <summary>
        /// The Type of the Pool that is being created
        /// </summary>
        protected abstract Type PoolType
        {
            get;
        }

        /// <summary>
        /// Creates a pool of the specified size for PoolType
        /// </summary>
        /// <param name="initialSize">Initial size of the pool</param>
        public AbstractPool(int initialSize)
        {
            _generator = new DefaultGenerator(PoolType);
            _pool = new List<IPoolable>(initialSize);
            _availableObjects = new Queue<IPoolable>();

            for (int i = 0; i < initialSize; ++i)
            {
                CreateNewInstance();
            }
        }

        /// <summary>
        /// Returns an object to the pool making it available for re-use.
        /// </summary>
        /// <param name="obj">The object to return to the pool. Must have originated from the pool.</param>
        public void Recycle(IPoolable obj)
        {
            if (_pool == null)
            {
                throw new InvalidOperationException("The Pool has been destroyed. Objects can no longer be Recycled into it.");
            }

            if (_pool.Contains(obj))
            {
                obj.OnDeallocate();
                _availableObjects.Enqueue(obj);
            }
            else
            {
                throw new InvalidOperationException("Cannot Recycle Objects into a Pool that did not originate from it.");
            }
        }

        /// <summary>
        /// Cleans up the Pool, prepping it for garbage collection.
        /// </summary>
        /// <param name="destroyAllocatedObjects">
        /// If true, destroys all objects managed by the pool regardless of their allocation status. 
        /// If false, only unallocated objects will be destroyed. Allocated objects will be orphaned.
        /// </param>
        public void Destroy(bool destroyAllocatedObjects = true)
        {
            if (destroyAllocatedObjects)
            {
                for (int i = 0, count = _pool.Count; i < count; ++i)
                {
                    _pool[i].Destroy();
                }
            }
            else
            {
                while (_availableObjects.Count > 0)
                {
                    _availableObjects.Dequeue().Destroy();
                }
            }

            _pool = null;
        }

        /// <summary>
        /// Aquires an unallocated object from the pool and provides it for use. If no unallocated objects are available, a new one will be created.
        /// </summary>
        /// <returns>An object of type IPoolable for use</returns>
        protected IPoolable Aquire()
        {
            if (_pool == null)
            {
                throw new InvalidOperationException("The Pool has been destroyed. Objects can no longer be Aquired from it.");
            }

            if (_availableObjects.Count == 0)
            {
                CreateNewInstance();
            }

            IPoolable obj = _availableObjects.Dequeue();
            obj.OnAllocate();
            return obj;
        }


        /// <summary>
        /// Creates and adds a new instance of the poolable object to the pool.
        /// Once this is done the size of the pool will have increased by one and a new object will be available within it.
        /// </summary>
        protected void CreateNewInstance()
        {
            IPoolable newObj = _generator.CreateInstance();
            _pool.Add(newObj);
            _availableObjects.Enqueue(newObj);
        }
    }
}
