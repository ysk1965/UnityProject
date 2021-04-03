using UnityEngine;
using System.Collections;

namespace ObjectPool
{
    public class PoolObject : MonoBehaviour
    {
        [HideInInspector]
        public string poolName;
        //defines whether the object is waiting in pool or is in use
        public bool isPooled;

        protected void ReturnObject()
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(this.gameObject);
        }

        protected void OnParticleSystemStopped()
        {
            this.ReturnObject();
        }
    }
}
