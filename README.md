[![version](https://img.shields.io/badge/package-download-brightgreen.svg)](https://github.com/jonHuffman/UPool/raw/master/Package/UPool_v1.0.unitypackage)
[![version](https://img.shields.io/badge/version-v1.0-blue.svg)](https://github.com/jonHuffman/UPool)
[![license](https://img.shields.io/badge/license-MIT-red.svg)](https://github.com/jonHuffman/UPool/blob/master/LICENSE.md)
# UPool
## Description
UPool is a Generic Object Pooling system for Unity3d.

## Features
* Pools both GameObjects and C# Objects
* Pre-define the size of your Pool to control object instantiation
* Pool size increases as needed, instantiating additional instances on demand
* Allocation and Deallocation methods allow you to run logic upon Aquiring and Recycling objects
* Make any existing GameObject poolable by attaching the PoolableObject component
* Define your GameObject pool locations within the scene by providing containers
* Pool syntax is simalar to existing C# Collections making it easy to adopt
* Unit tested

## Usage
The system exists within the UPool namespace and is driven by the `Pool` class and `IPoolable` interface.

### Pool

#### Creating a Pool
Creating the actual Pool is very simple and the API around it is very straight forward. Where you need to take special care is with the objects you are pooling; Anything that you may wish to pool must implement IPoolable.
```csharp
using UPool

public class Demo
{
    private Pool<DemoObj> _pool;
    
    public class Demo()
    {
        _pool = new Pool<DemoObj>(3);
    }
}
```

#### Aquisition
The Pool will always be able to provide you with an object. In the event that you call Aquire and there are no available instances within the Pool, a new one will be created.

Calling **Aquire** will cause the **OnAllocation** method to be called on the pooled object.

```csharp
_pool.Aquire();
```

#### Recycling
In order to take full advantage of the Pool you must remember to recycle your objects when done with them. Once recycled an item will become available for aquisition through the **Aquire** method.

Calling **Recycle** will cause the **OnDeallocation** method to be called on the pooled obect.

_Remember to clear any cached references when an object is recycled!_

```csharp
_pool.Recycle(obj);
```

#### Pooling GameObjects
When creating a Pool for GameObjects there are a few other arguments that you need to provide to the constructor:
1. **initialSize (int)** : The initial size of the pool
2. **template (GameObject)** : The GameObject that will act as a template for the instantiation of additional objects. This GameObject _must_ have a component that matches the type of the Pool.
	* You are free to provide this template however you are most comfortable with. In this example I load it from Resources.
3. **container (Transform)** : An optional argument that allows you to specify a Transform that objects will live under when not in use. If none is provided one will be created for you. 
	* Null by default.
4. **hideContainerInHierarchy (bool)** : An optional argument that allows you to specify whether the container and its schildren should appear in the Hierachy. 
	* True by default.
```csharp
_pool = new Pool<PoolableObject>(3, Resources.Load<GameObject>("DemoObject"), null, false);
```

### IPoolable

```csharp
public class DemoObj : IPoolable
{
    public void Init(AbstractPool owner) { }    
    public void Destroy() { }

    public void OnAllocate()
    {
        // Called when object is Aquired from the pool
    }

    public void OnDeallocate()
    {
        // Called when object is Recycled
    }
}
```
#### Init(AbstractPool owner)
The **Init** method is called each time a new instance is created by the Pool. It is up to you how you choose to use this method. The _owner_ argument is the Pool object that owns this instance, you are free to store this reference and use it later.

#### Destroy()
The **Destroy** method self-axplanitory. This method is called when the Pool's **Destroy** method is called, and provides you a location to perform and cleanup that may need to occur.

#### OnAllocate()
The **OnAllocate** method is called when this instance is retrieved from the Pool by the **Aquire** method. Any aquisition related logic should go here.

#### OnDeallocate()
The **OnDeallocate** method is called when this instance is recycled by the Pool via the **Recycle** method. Any logic that you wish to run when the object is "put away" should occur here.


### Using PoolableObject
PoolableObject is a pre-built component for you to use. It allows for one to quickly make any GameObject poolable without too much hassle.

To use the component simply attach it to your GameObject and create the Pool with PoolableObject as the Type. In the event that you need access to any of the other Components on the object, just use GetComponent after your aquisition.
```csharp
public class Demo : MonoBehaviour
{
    private Pool<PoolableObject> _pool;

    void Awake()
    {
    	_pool = new Pool<PoolableObject>(3, Resources.Load<GameObject>("DemoObject"), null, false);
    }

    private void GetObjFromPool()
    {
    	DemoObj demotObj = _pool.Aquire().GetComponent<DemoObj>();
    }
}
```
PoolableObject is a sealed class. It is designed to provide you with a basic implementation for pooling objects without hassle. Should you wish to perform more advanced logic, simply create a new Class that extends MonoBehaviour and implement the IPoolable interface. Once this is done you can pool your new Component just like you would PoolableObject.

#### Serialized Fields
PoolableObject exposes three options that provide some ease of use and optimization for you.
1. **Disable Object** : Disables the object when it is in the Pool. If enabled the other options are ignored.
2. **Disable Colliders** : Disables all Colliders on the object and its children when in the Pool. Only runs if the _Disable Object_ option is disabled.
3. **Disable Renderers** : Disables all Renderers on the object and its children when in the Pool. Only runs if the _Disable Object_ option is disabled.

#### OnAllocate
PoolableObject contains a public OnAllocate Action that other Components may subscribe to in order to run logic upon Aquisition from the Pool.

#### OnDeallocate
PoolableObject contains a public OnDeallocate Action that other Components may subscribe to in order to run logic upon returning to the Pool.

#### Recycle
PoolableObject has its own **Recycle** method that can be used to recycle the object. It is a shortcut that allows you to recycle the object without having a reference to the Pool.

_Remember to clear any cached references when an object is recycled!_

### Tips
* Store a reference to the _owner_ argument passed through the **Init** method in IPoolable and use it in a self-recycling method
  ```csharp
  public void Recycle()
  {
  	_owner.Recycle(this);
  }
  ```
	_Remember to clear any cached references when an object is recycled!_
* If your GameObject doesn't _need_ to be disabled when resting in the Pool, consider using the **Disable Colliders** and **Disable Renderers** options provided by PoolableObject instead. Performance on just enabling/disabling these components tends to be better than enabling/disabling the entire object. 
	* PoolableObject caches references to your Colliders and Renderers on Awake in order to minimize overhead performance on these calls.

## Meta
Created by Jon Huffman [[twitter](https://twitter.com/AtticusMarkane) &bull; [github](https://github.com/ByronMayne) &bull; [Website](http://jonhuffman.com/)]

Released under the [MIT License](http://www.opensource.org/licenses/mit-license.php).
