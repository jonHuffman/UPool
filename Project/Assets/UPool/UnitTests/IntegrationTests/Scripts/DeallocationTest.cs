using NUnit.Framework;
using UnityEngine;
using UPool;
using UPool.Demo;
using Assert = UnityEngine.Assertions.Assert;

/// <summary>
/// Tests the OnDeallocate Action invokation of PoolableObject
/// </summary>
public class DeallocationTest
{
    private Pool<PoolableObject> _pool;
    private GameObject _container;

    [SetUp]
    public void SetUp()
    {
        const int poolSize = 1;
        _container = new GameObject("Container");
        _container.transform.position = Pool<PoolableObject>.AUTO_CONTAINER_POSITION;

        _pool = new Pool<PoolableObject>(poolSize, Resources.Load<GameObject>("AllocationTestObject"), _container.transform);
    }

    [Test]
    public void DeallocateGameObject()
    {
        PoolableObject item = _pool.Acquire();
        _pool.Recycle(item);        
        
        Assert.AreEqual(DemoObj.AllocationState.Deallocated, item.GetComponent<DemoObj>().State, "OnDeallocate action was not invoked upon PoolableObject allocation");
    }
}
