using UnityEngine;

namespace UPool.Demo
{
    [RequireComponent(typeof(PoolableObject))]
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(Collider))]
    public class DemoObj : MonoBehaviour
    {
        private PoolableObject _poolableObj;
        private Renderer _renderer;        

        // Use this for initialization
        void Awake()
        {
            _poolableObj = GetComponent<PoolableObject>();
            _renderer = GetComponent<Renderer>();

            _poolableObj.OnAllocate += OnAllocate;
        }

        private void OnMouseDown()
        {
            _poolableObj.Recycle();
        }

        private void OnAllocate()
        {
            _renderer.material.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}