using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;

public class ObjectPoolManager : GameObjectSingleton<ObjectPoolManager>
{

    //////////////////////////////////////////////////////////////////////////////
    //public

    [Header("Editing Pool Info value at runtime has no effect")]
    public PoolInfo[] poolInfo;
    public UIPoolInfo[] uiPoolInfo;

    public bool IsPoolReady { get => this.isPoolReady; }

    /* Returns an available object from the pool 
    OR 
    null in case the pool does not have any object available & can grow size is false.
    */
    public GameObject GetObjectFromPool(PoolObjectType poolType, Vector3 position, Quaternion? rotation = null)
    {
        return this.GetObjectFromPool(poolType.ToString(), position, rotation);
    }

    public GameObject GetObjectFromPool(UIPoolObjectType poolType, Vector3 position, Quaternion? rotation = null)
    {
        return this.GetObjectFromPool(poolType.ToString(), position, rotation);
    }

    public GameObject GetObjectFromPool(string name, Vector3 position, Quaternion? rotation)
    {
        GameObject result = null;

        if (poolDictionary.ContainsKey(name))
        {
            Pool pool = poolDictionary[name];
            result = pool.NextAvailableObject(position, rotation == null ? Quaternion.Euler(Vector3.zero) : rotation.Value);
            //scenario when no available object is found in pool
            if (result == null)
            {
                Debug.LogWarning("No object available in pool. Consider setting fixedSize to false.: " + name);
            }

        }
        else
        {
            Debug.LogError("Invalid pool name specified: " + name);
        }

        return result;
    }

    public void ReturnObjectToPool(GameObject go)
    {
        PoolObject po = go.GetComponent<PoolObject>();
        if (po == null)
        {
            Debug.LogWarning("Specified object is not a pooled instance: " + go.name);
        }
        else
        {
            if (poolDictionary.ContainsKey(po.poolName.ToString()))
            {
                Pool pool = poolDictionary[po.poolName.ToString()];
                pool.ReturnObjectToPool(po);
            }
            else
            {
                Debug.LogWarning("No pool available with name: " + po.poolName);
            }
        }
    }

    public void ReturnObjectToPool(GameObject go, float t)
    {
        this.StartCoroutine(this.ReturnObjectToPoolCoroutine(go, t));
    }

    public void AddCustomPool(PoolInfo custom)
    {
        Pool pool = new Pool(custom.poolType.ToString(), custom.prefab, custom.poolSize, custom.fixedSize);

        if (this.showLog) Debug.Log("Creating custom pool: " + custom.poolType);
        poolDictionary[custom.poolType.ToString()] = pool;
    }

    public void AddCustomPool(UIPoolInfo custom)
    {
        Pool pool = new Pool(custom.poolType.ToString(), custom.prefab, custom.poolSize, custom.fixedSize);

        if (this.showLog) Debug.Log("Creating custom UI pool: " + custom.poolType);
        poolDictionary[custom.poolType.ToString()] = pool;
    }

    //////////////////////////////////////////////////////////////////////////////
    //protected

    // Use this for initialization
    protected void Start()
    {
        //check for duplicate names
        this.CheckForDuplicatePoolNames();
        //create pools
        this.CreatePools();

        this.isPoolReady = true;
    }


    //////////////////////////////////////////////////////////////////////////////
    //private

    //mapping of pool name vs list
    [SerializeField] private bool showLog;
    [SerializeField] private Dictionary<string, Pool> poolDictionary = new Dictionary<string, Pool>();

    private bool isPoolReady = false;
    private void CheckForDuplicatePoolNames()
    {
        for (int index = 0; index < poolInfo.Length; index++)
        {
            PoolObjectType poolType = poolInfo[index].poolType;
            for (int internalIndex = index + 1; internalIndex < poolInfo.Length; internalIndex++)
            {
                if (poolType.Equals(poolInfo[internalIndex].poolType))
                {
                    Debug.LogError(string.Format("Pool {0} & {1} have the same name. Assign different names.", index, internalIndex));
                }
            }
        }
    }

    private void CreatePools()
    {
        foreach (PoolInfo currentPoolInfo in this.poolInfo)
        {
            Pool pool = new Pool(currentPoolInfo.poolType.ToString(), currentPoolInfo.prefab, currentPoolInfo.poolSize, currentPoolInfo.fixedSize);

            if (this.showLog) Debug.Log("Creating pool: " + currentPoolInfo.poolType);
            poolDictionary[currentPoolInfo.poolType.ToString()] = pool;
        }

        foreach (UIPoolInfo currentPoolInfo in this.uiPoolInfo)
        {
            Pool pool = new Pool(currentPoolInfo.poolType.ToString(), currentPoolInfo.prefab, currentPoolInfo.poolSize, currentPoolInfo.fixedSize);

            if (this.showLog) Debug.Log("Creating ui pool: " + currentPoolInfo.poolType);
            poolDictionary[currentPoolInfo.poolType.ToString()] = pool;
        }
    }

    private IEnumerator ReturnObjectToPoolCoroutine(GameObject go, float t)
    {
        yield return new WaitForSeconds(t);
        this.ReturnObjectToPool(go);
    }
}
