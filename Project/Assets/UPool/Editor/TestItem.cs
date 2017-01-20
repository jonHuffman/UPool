using UnityEngine;

namespace UPool.Tests
{
    public class TestItem : IPoolable
    {
        public const int UNALLOCATED_NUMBER = -1;

        public string name = string.Empty;
        public int number = UNALLOCATED_NUMBER;
        public bool isDestroyed = false;
        
        public void Destroy()
        {
            isDestroyed = true;
        }

        public void Init(AbstractPool owner)
        {
        }

        public void OnAllocate()
        {
            number = Random.Range(1, 100);
        }

        public void OnDeallocate()
        {
            number = UNALLOCATED_NUMBER;
        }
    }
}
