using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;

public interface IObjectPoolHandler
{
    void ReturnObjectToPool(GameObject go);
}
