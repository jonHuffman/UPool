# UPool
## Description
UPool is a Generic Object Pooling system for Unity3d.

## Features
* Pools both GameObjects and C# Objects
* Pre-define the size of your Pool to control object instantiation
* Pool size increases as needed, instantiating additional instances on demand
* Allocation and Deallocation methods allow you to run logic on Aquiring and Recycling objects
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

```csharp
_pool.Aquire();
```

#### Recycling

```csharp
_pool.Recycle(obj);
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

### Tips
* Store a reference to the _owner_ argument passed through the **Init** method in IPoolable and use it in a self-recycling method
  ```csharp
  public void Recycle()
  {
  	_owner.Recycle(this);
  }
  ```

## Meta
Created by Jon Huffman [[twitter](https://twitter.com/AtticusMarkane) &bull; [github](https://github.com/ByronMayne) &bull; [Website](http://jonhuffman.com/)]

Released under the [MIT License](http://www.opensource.org/licenses/mit-license.php).
