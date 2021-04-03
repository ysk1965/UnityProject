using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ObjectPool;

public class AnimationPoolObject : PoolObject
{
    //////////////////////////////////////////////////////////////////////////////
    //public

    public void OnFinish()
    {
        this.ReturnObject();
    }

    //////////////////////////////////////////////////////////////////////////////
    //private
}
