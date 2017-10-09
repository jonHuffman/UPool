using System;
using UnityEngine;

namespace UPool
{
    /// <summary>
    /// A Generic Object Pool that can handle both traditional C# objects and 
    /// </summary>
    /// <typeparam name="T">An IPoolable</typeparam>
    public class Pool<T> : AbstractPool where T : IPoolable
    {
        public static readonly Vector3 AUTO_CONTAINER_POSITION = new Vector3(-9999, 0, 0);

        private Transform _container;
        private bool _isGameObject = false;

        public override Type PoolType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Creates a pool of the specified size for the given object type
        /// </summary>
        /// <param name="initialSize">Initial size of the pool</param>
        public Pool(int initialSize)
        {
            if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException(string.Format("Type {0} derives from MonoBehaviour. In order to Pool MonoBehaviours you must provide a template; use Pool(int, GameObject).", typeof(T).ToString()));
            }

            _generator = new DefaultGenerator(this);

            InitializePool(initialSize);
        }

        /// <summary>
        /// Creates a pool of the specified size for the given GameObject template
        /// </summary>
        /// <param name="initialSize">Initial size of the pool</param>
        /// <param name="template">The GameObject that will be pooled</param>
        /// <param name="container">The container that the pooled GameObjects will reside in when not in use. If one is not provided a container will be generated</param>
        /// <param name="hideContainerInHierarchy">If True, hides the container GameObject in the scene hierarchy to avoid clutter</param>
        public Pool(int initialSize, GameObject template, Transform container = null, bool hideContainerInHierarchy = true)
        {
            if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)) == false)
            {
                throw new ArgumentException(string.Format("Type {0} does not derive from MonoBehaviour. In order to Pool GameObjects, T should be a MonoBehaviour derivative.", typeof(T).ToString()));
            }
            if (template == null)
            {
                throw new ArgumentException("The template GameObject cannot be null.");
            }
            if (template.GetComponent<T>() == null)
            {
                throw new ArgumentException(string.Format("The template GameObject must have a Component of type {0}", typeof(T).ToString()));
            }

            _container = container != null ? container : CreateContainer(template.name);

            if (hideContainerInHierarchy)
            {
                _container.hideFlags = HideFlags.HideInHierarchy;
            }

            _generator = new UnityGenerator(this, template, _container);
            _isGameObject = true;

            InitializePool(initialSize);
        }

        [Obsolete("Use Acquire instead.")]
        public T Aquire()
        {
            return Acquire();
        }

        /// <summary>
        /// Aquires an unallocated object from the pool and provides it for use. If no unallocated objects are available, a new one will be created.
        /// </summary>
        /// <returns>An object of type T for use. If T is a MonoBehaviour, the Transform will be reset.</returns>
        public new T Acquire()
        {
            T obj = (T)base.Acquire();

            if (_isGameObject)
            {
                (obj as MonoBehaviour).transform.SetParent(null);
                (obj as MonoBehaviour).transform.position = Vector3.zero;
                (obj as MonoBehaviour).transform.rotation = Quaternion.identity;
                (obj as MonoBehaviour).transform.localScale = Vector3.one;
            }

            return obj;
        }

        /// <summary>
        /// Returns an object to the pool making it available for re-use.
        /// </summary>
        /// <param name="obj">The object to return to the pool. Must have originated from the pool.</param>
        public override void Recycle(IPoolable obj)
        {
            base.Recycle(obj);

            if (_isGameObject)
            {
                (obj as MonoBehaviour).transform.SetParent(_container);
                (obj as MonoBehaviour).transform.localPosition = Vector3.zero;
            }
        }

        /// <summary>
        /// Creates a container that the pooled GameObjects will reside in when not in use.
        /// </summary>
        /// <param name="objectName">Name of the GameObject that is being pooled.</param>
        /// <returns>A reference to the container Transform</returns>
        private Transform CreateContainer(string objectName)
        {
            GameObject container = new GameObject(string.Format("{0}Pool", objectName));
            container.transform.position = AUTO_CONTAINER_POSITION;
            return container.transform;
        }
    }
}