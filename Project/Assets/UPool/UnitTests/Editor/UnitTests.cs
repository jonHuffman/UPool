using NUnit.Framework;
using UnityEngine;
using UPool.Demo;

namespace UPool.Tests
{
    public class UnitTests
    {

        [Test]
        public void SizingTest()
        {
            const int poolSize = 3;

            // Test initial size
            Pool<TestItem> pool = new Pool<TestItem>(poolSize);
            Assert.AreEqual(poolSize, pool.Size, "The initial size of the pool was {0}, expected {1}", poolSize, pool.Size);

            // Test size after aquisition
            for (int i = 0; i < poolSize; ++i)
            {
                pool.Aquire();
            }
            Assert.AreEqual(poolSize, pool.Size, "The initial size of the pool was {0}, expected {1} after aquisition", poolSize, pool.Size);

            // Test size after aquiring more items that exist in the pool
            pool.Aquire();
            Assert.Greater(pool.Size, poolSize, "The pool size did not properly increase when additional items were aquired. Was {0}, expected {1} after aquisition", poolSize, pool.Size);
        }

        [Test]
        public void AllocationTest()
        {
            const int poolSize = 1;

            Pool<TestItem> pool = new Pool<TestItem>(poolSize);
            TestItem item = pool.Aquire();

            Assert.AreNotEqual(TestItem.UNALLOCATED_NUMBER, item.number, "TestItem Allocation did not occur properly. Number should not be {0}", TestItem.UNALLOCATED_NUMBER);
        }

        [Test]
        public void DeallocationTest()
        {
            const int poolSize = 1;

            Pool<TestItem> pool = new Pool<TestItem>(poolSize);
            TestItem item = pool.Aquire();
            pool.Recycle(item);

            Assert.AreEqual(TestItem.UNALLOCATED_NUMBER, item.number, "TestItem Deallocation did no occur properly. Number should be {0}", TestItem.UNALLOCATED_NUMBER);
        }

        [Test]
        public void RecycleTest()
        {
            const int poolSize = 1;
            const string itemName = "RecycledObject";

            // Test initial size
            Pool<TestItem> pool = new Pool<TestItem>(poolSize);
            TestItem item = pool.Aquire();
            item.name = itemName;
            pool.Recycle(item);
            item = pool.Aquire();

            Assert.AreEqual(itemName, item.name, "TestItem was not recycled, name should have been {0}", item.name);
        }

        [Test]
        public void DestroyAllItemsTest()
        {
            const int poolSize = 2;

            // Test that all items created by the pool get destroyed regardless of allocation
            Pool<TestItem> pool = new Pool<TestItem>(poolSize);
            TestItem item1 = pool.Aquire();
            TestItem item2 = pool.Aquire();
            pool.Recycle(item1);
            pool.Destroy();

            Assert.IsTrue(item1.isDestroyed, "Item 1 was not destroyed.");
            Assert.IsTrue(item2.isDestroyed, "Item 2 was not destroyed.");
        }

        [Test]
        public void DestroyUnallocatedItemsTest()
        {
            const int poolSize = 2;

            // Test that allocated items do not get destroyed
            Pool<TestItem> pool = new Pool<TestItem>(poolSize);
            TestItem item1 = pool.Aquire();
            TestItem item2 = pool.Aquire();
            pool.Recycle(item1);
            pool.Destroy(false);

            Assert.IsTrue(item1.isDestroyed, "Item 1 was not destroyed.");
            Assert.IsFalse(item2.isDestroyed, "Item 2 was destroyed.");
        }
    }
}