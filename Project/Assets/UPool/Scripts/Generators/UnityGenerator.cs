using UnityEngine;

namespace UPool
{
    /// <summary>
    /// Unity specific generator that allows for hte creationg of new GameObjects based on a template
    /// </summary>
    public class UnityGenerator : IGenerator
    {
        private AbstractPool _owner;
        private GameObject _template;
        private Transform _container;

        public UnityGenerator(AbstractPool owner, GameObject template, Transform container)
        {
            _owner = owner;
            _template = template;
            _container = container;
        }

        public IPoolable CreateInstance()
        {
            GameObject obj = Object.Instantiate<GameObject>(_template, _container);
            obj.transform.localPosition = Vector3.zero;
            return obj.GetComponent<IPoolable>();
        }
    }
}
