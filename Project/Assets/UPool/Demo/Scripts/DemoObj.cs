using UnityEngine;

namespace UPool.Demo
{
    [RequireComponent(typeof(PoolableObject))]
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(Collider))]
    public class DemoObj : MonoBehaviour
    {
        public enum AllocationState
        {
            None,
            Allocated,
            Deallocated
        }

        private PoolableObject _poolableObj;
        private Renderer _renderer;
        private AllocationState _state = AllocationState.None;

        public AllocationState State
        {
            get { return _state; }
        }

        // Use this for initialization
        void Awake()
        {
            _poolableObj = GetComponent<PoolableObject>();
            _renderer = GetComponent<Renderer>();

            _poolableObj.OnAllocate += OnAllocate;
            _poolableObj.OnDeallocate += OnDeallocate;

            _state = AllocationState.Deallocated;
        }

        private void OnMouseDown()
        {
            _poolableObj.Recycle();
        }

        private void OnAllocate()
        {
            _renderer.material.color = new Color(Random.value, Random.value, Random.value);
            _state = AllocationState.Allocated;
        }

        private void OnDeallocate()
        {
            _state = AllocationState.Deallocated;
        }
    }
}