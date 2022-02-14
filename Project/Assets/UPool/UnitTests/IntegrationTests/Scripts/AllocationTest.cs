using NUnit.Framework;
using UnityEngine;
using UPool;
using UPool.Demo;
using Assert = UnityEngine.Assertions.Assert;

/// <summary>
/// Tests the Allocate Action invokation of PoolableObject
/// </summary>
public class AllocationTest
{
    private Pool<PoolableObject> _pool;

    [Test]
    public void AllocateGameObject()
    {
        const int poolSize = 1;
        _pool = new Pool<PoolableObject>(poolSize, Resources.Load<GameObject>("AllocationTestObject"));
        
        PoolableObject item = _pool.Acquire();

        Assert.AreEqual(DemoObj.AllocationState.Allocated, item.GetComponent<DemoObj>().State, "OnAllocate action was not invoked upon PoolableObject allocation");
    }
}
