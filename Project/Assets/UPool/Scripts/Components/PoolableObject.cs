using System;
using UnityEngine;

namespace UPool
{
    public sealed class PoolableObject : MonoBehaviour, IPoolable
    {
        [SerializeField]
        private bool _disableObject = false;
        [SerializeField]
        private bool _disableColliders = true;
        [SerializeField]
        private bool _disableRenderers = false;

        /// <summary>
        /// Called whenever the object is retrieved from the pool for use.
        /// </summary>
        public Action OnAllocate;
        /// <summary>
        /// Called whenever the object is Recycled
        /// </summary>
        public Action OnDeallocate;

        private AbstractPool _owner;
        private Collider[] _colliders;
        private Collider2D[] _2dColliders;
        private Renderer[] _renderers;

        void IPoolable.Init(AbstractPool owner)
        {
            _owner = owner;

            _colliders = GetComponentsInChildren<Collider>();
            _2dColliders = GetComponentsInChildren<Collider2D>();
            _renderers = GetComponentsInChildren<Renderer>();
        }

        void IPoolable.OnAllocate()
        {
            if(_disableObject)
            {
                gameObject.SetActive(true);
            }
            else
            {
                if(_disableColliders)
                {
                    EnableColliders();
                }

                if(_disableRenderers)
                {
                    EnableRenderers();
                }
            }

            if(OnAllocate != null)
            {
                OnAllocate();
            }
        }

        void IPoolable.OnDeallocate()
        {
            if (_disableObject)
            {
                gameObject.SetActive(false);
            }
            else
            {
                if (_disableColliders)
                {
                    DisableColliders();
                }

                if (_disableRenderers)
                {
                    DisableRenderers();
                }
            }

            if (OnDeallocate != null)
            {
                OnDeallocate();
            }
        }

        void IPoolable.Destroy()
        {
            _owner = null;
            _colliders = null;
            _2dColliders = null;
            _renderers = null;

            OnAllocate = null;
            OnDeallocate = null;
        }

        /// <summary>
        /// Recycles this object calling OnDeallocate and returning it to the Pool
        /// </summary>
        public void Recycle()
        {
            _owner.Recycle(this);
        }

        /// <summary>
        /// Enables all colliders for this object and its children
        /// </summary>
        private void EnableColliders()
        {
            for (int i = 0, iCount = _colliders.Length; i < iCount; ++i)
            {
                _colliders[i].enabled = true;
            }

            for (int j = 0, jCount = _2dColliders.Length; j < jCount; ++j)
            {
                _2dColliders[j].enabled = true;
            }
        }

        /// <summary>
        /// Disables all colliders for this object and its children
        /// </summary>
        private void DisableColliders()
        {
            for (int i = 0, iCount = _colliders.Length; i < iCount; ++i)
            {
                _colliders[i].enabled = false;
            }

            for (int j = 0, jCount = _2dColliders.Length; j < jCount; ++j)
            {
                _2dColliders[j].enabled = false;
            }
        }

        /// <summary>
        /// Enables all renderers for this object and its children
        /// </summary>
        private void EnableRenderers()
        {
            for (int i = 0, count = _renderers.Length; i < count; ++i)
            {
                _renderers[i].enabled = true;
            }
        }

        /// <summary>
        /// Disables all renderers for this object and its children
        /// </summary>
        private void DisableRenderers()
        {
            for (int i = 0, count = _renderers.Length; i < count; ++i)
            {
                _renderers[i].enabled = false;
            }
        }
    }
}
