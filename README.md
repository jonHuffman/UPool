# UPool
## Description
UPool is a Generic Object Pooling system for Unity3d.

## Features
* Pools both GameObjects and C# Objects
* Pre-define the size of your Pool to control object instantiation
* Pool size increases as needed, instantiating additional instances on demand
* Allocation and Deallocation callbacks allow you to run logic on Aquiring and Recycling objects
* Make any existing GameObject poolable by attaching the PoolableObject component
* Define your GameObject pool locations within the scene by providing containers
* Pool syntax is simalar to existing C# Collections making it easy to adopt
* Unit tested

## Usage
The system exists within the UPool namespace and is driven by the `Pool` class and `IPoolable` interface.

### Creating a Pool
```csharp
using UPool

public class Demo
{
    private Pool<DemoObj> _pool;
    
    public class Demo()
    {
        _pool = new Pool<DemoObj>(3);
    }

    public DemoObj GetNewDemoObj()
    {
        return _pool.Aquire();
    }
}

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

### Using PoolableObject

### Tips

## Meta
Created by Jon Huffman [[twitter](https://twitter.com/AtticusMarkane) &bull; [github](https://github.com/ByronMayne) &bull; [Website](http://jonhuffman.com/)]

Released under the [MIT License](http://www.opensource.org/licenses/mit-license.php).
