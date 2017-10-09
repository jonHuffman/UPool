using UnityEngine;
using UnityEngine.Assertions;
using UnityTest;
using UPool;
using UPool.Demo;

/// <summary>
/// Tests the Allocate Action invokation of PoolableObject
/// </summary>
[RequireComponent(typeof(TestComponent))]
public class AllocationTest : MonoBehaviour
{
    private Pool<PoolableObject> _pool;
    private TestComponent _testCmp;

    private void Awake()
    {
        _testCmp = GetComponent<TestComponent>();

        const int poolSize = 1;
        _pool = new Pool<PoolableObject>(poolSize, Resources.Load<GameObject>("AllocationTestObject"));
    }

    private void Start()
    {
        PoolableObject item = _pool.Acquire();

        Assert.AreEqual(DemoObj.AllocationState.Allocated, item.GetComponent<DemoObj>().State, "OnAllocate action was not invoked upon PoolableObject allocation");


        IntegrationTest.Pass();
    }
}
