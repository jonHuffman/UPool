using System;
using NUnit.Framework;
using UnityEngine;
using UPool;
using Assert = UnityEngine.Assertions.Assert;
using Object = UnityEngine.Object;

/// <summary>
/// Tests general aquisition and recycling of PoolableObjects
/// </summary>
public class RecycleTests
{
    private Pool<PoolableObject> _disableObjPool;
    private Pool<PoolableObject> _disableColliderPool;
    private Pool<PoolableObject> _disableRendererPool;
    private GameObject _container;

    [SetUp]
    public void SetUp()
    {
        const int poolSize = 1;
        _container = new GameObject("Container");
        _container.transform.position = Pool<PoolableObject>.AUTO_CONTAINER_POSITION;

        _disableObjPool = new Pool<PoolableObject>(poolSize, Resources.Load<GameObject>("DisableObjectTestObject"), _container.transform);
        _disableColliderPool = new Pool<PoolableObject>(poolSize, Resources.Load<GameObject>("DisableColliderTestObject"));
        _disableRendererPool = new Pool<PoolableObject>(poolSize, Resources.Load<GameObject>("DisableRendererTestObject"));
    }

    [TearDown]
    public void TearDown()
    {
        _disableObjPool.DestroyAndDeallocateAll();
        _disableColliderPool.DestroyAndDeallocateAll();
        _disableRendererPool.DestroyAndDeallocateAll();
        
        Object.Destroy(_container);
    }

    [Test]
    public void AquisitionTest()
    {
        PoolableObject item = _disableObjPool.Acquire();

        Assert.IsNull(item.transform.parent, "Object Parent has not been set to the root of the Hierarchy");
        Assert.AreEqual(Vector3.zero, item.transform.position, "Object Position has not been reset to Vector3.zero");
        Assert.AreEqual(Quaternion.identity, item.transform.rotation, "Object Rotation has not been reset to Quaternion.identity");
        Assert.AreEqual(Vector3.one, item.transform.lossyScale, "Object Scale has not been reset to Vector3.one");

        _disableObjPool.Recycle(item);
    }

    [Test]
    public void RecycleTest()
    {
        PoolableObject item = _disableObjPool.Acquire();
        _disableObjPool.Recycle(item);

        Assert.AreEqual(_container.transform, item.transform.parent, "Object Parent has not been reset to the Container upon recycle");
        Assert.AreEqual(Vector3.zero, item.transform.localPosition, "Object Local Position has not been reset to Vector3.zero");
    }

    [Test]
    public void DisableObjectTest()
    {
        PoolableObject item = _disableObjPool.Acquire();

        Assert.IsTrue(item.gameObject.activeSelf, "This object was not Enabled during allocation");

        _disableObjPool.Recycle(item);
        
        Assert.IsFalse(item.gameObject.activeSelf, "This object was not Disabled during deallocation");

        Collider[] colliders = item.GetComponentsInChildren<Collider>();
        Renderer[] renderers = item.GetComponentsInChildren<Renderer>();

        Array.ForEach<Collider>(colliders, (col) => Assert.IsTrue(col.enabled, "A collider was disabled when it should not have been"));
        Array.ForEach<Renderer>(renderers, (ren) => Assert.IsTrue(ren.enabled, "A renderer was disabled when it should not have been"));
    }

    [Test]
    public void DisableCollidersTest()
    {
        PoolableObject item = _disableColliderPool.Acquire();
        _disableColliderPool.Recycle(item);

        Assert.IsTrue(item.gameObject.activeSelf, "This object was Disabled during allocation and it should not have been");

        Collider[] colliders = item.GetComponentsInChildren<Collider>();
        Renderer[] renderers = item.GetComponentsInChildren<Renderer>();

        Array.ForEach<Collider>(colliders, (col) => Assert.IsFalse(col.enabled, "A collider was not disabled when it should have been"));
        Array.ForEach<Renderer>(renderers, (ren) => Assert.IsTrue(ren.enabled, "A renderer was disabled when it should not have been"));
    }

    [Test]
    public void DisableRenderersTest()
    {
        PoolableObject item = _disableRendererPool.Acquire();
        _disableRendererPool.Recycle(item);

        Assert.IsTrue(item.gameObject.activeSelf, "This object was Disabled during allocation and it should not have been");

        Collider[] colliders = item.GetComponentsInChildren<Collider>();
        Renderer[] renderers = item.GetComponentsInChildren<Renderer>();

        Array.ForEach<Collider>(colliders, (col) => Assert.IsTrue(col.enabled, "A collider was disabled when it should not have been"));
        Array.ForEach<Renderer>(renderers, (ren) => Assert.IsFalse(ren.enabled, "A renderer was not disabled when it should have been"));
    }
}
