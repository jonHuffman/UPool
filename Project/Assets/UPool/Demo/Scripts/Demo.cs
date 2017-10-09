using UnityEngine;
using UnityEngine.UI;

namespace UPool.Demo
{
    public class Demo : MonoBehaviour
    {
        [SerializeField]
        private Button _getObjButton;

        private Pool<PoolableObject> _pool;

        void Awake()
        {
            _getObjButton.onClick.AddListener(GetObjFromPool);

            _pool = new Pool<PoolableObject>(3, Resources.Load<GameObject>("DemoObject"), null, false);
        }

        private void GetObjFromPool()
        {
            _pool.Acquire().transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        }
    }
}