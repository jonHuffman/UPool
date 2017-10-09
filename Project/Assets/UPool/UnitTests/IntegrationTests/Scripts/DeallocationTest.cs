using UnityEngine;
using UnityEngine.Assertions;
using UnityTest;
using UPool;
using UPool.Demo;

/// <summary>
/// Tests the OnDeallocate Action invokation of PoolableObject
/// </summary>
[RequireComponent(typeof(TestComponent))]
public class DeallocationTest : MonoBehaviour
{
    private Pool<PoolableObject> _pool;
    private TestComponent _testCmp;
    private GameObject _container;

    private void Awake()
    {
        _testCmp = GetComponent<TestComponent>();

        const int poolSize = 1;
        _container = new GameObject("Container");
        _container.transform.position = Pool<PoolableObject>.AUTO_CONTAINER_POSITION;

        _pool = new Pool<PoolableObject>(poolSize, Resources.Load<GameObject>("AllocationTestObject"), _container.transform);
    }

    private void Start()
    {
        PoolableObject item = _pool.Acquire();
        _pool.Recycle(item);        
        
        Assert.AreEqual(DemoObj.AllocationState.Deallocated, item.GetComponent<DemoObj>().State, "OnDeallocate action was not invoked upon PoolableObject allocation");

        IntegrationTest.Pass();
    }
}
