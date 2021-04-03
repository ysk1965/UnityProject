using UnityEngine;
using System.Collections;

public abstract class GameObjectSingletonBase : MonoBehaviour
{
    public abstract bool AllowMultiInstance { get; }
    public abstract void SetAsSingleton(bool set);
}

public class GameObjectSingleton<T> : GameObjectSingletonBase where T : GameObjectSingleton<T>
{
    public static bool Loaded
    {
        get
        {
            return __inst != null && __inst.Valid;
        }
    }

    public static T Instance
    {
        get { return __inst; }
    }

    protected static T _inst
    {
        get
        {
            return __inst;
        }
    }

    /////////////////////////////////////////////////////////////////
    // instance member
    public override bool AllowMultiInstance
    {
        get
        {
            return false;
        }
    }

    public override void SetAsSingleton(bool set)
    {
        if (!this.AllowMultiInstance)
        {
            Debug.LogError(typeof(T).Name + " : AllowMultiInstance is false");
            return;
        }

        if (set)
        {
            if (object.ReferenceEquals(__inst, this))
                return;

            __inst = this as T;
            this.OnAttached();
        }
        else
        {
            if (!object.ReferenceEquals(__inst, this))
                return;

            __inst = null;
            this.OnDetached();
        }
    }

    protected virtual bool Valid
    {
        get
        {
            return __inst != null;
        }
    }

    protected virtual void Awake()
    {
        if (__inst != null)
        {
            if (!this.AllowMultiInstance)
                Debug.LogError(typeof(T).Name + " is already attached");

            return;
        }

        __inst = this as T;

        this.OnAttached();
    }

    protected virtual void OnDestroy()
    {
        if (object.ReferenceEquals(this, __inst))
        {
            __inst = null;
            this.OnDetached();
        }
    }

    protected virtual void OnAttached()
    {

    }

    protected virtual void OnDetached()
    {

    }

    public static void SetAsSingleton(GameObject obj, bool set)
    {
        foreach (GameObjectSingletonBase comp in obj.GetComponents<GameObjectSingletonBase>())
            comp.SetAsSingleton(set);
    }

    /////////////////////////////////////////////////////////////////
    // private
    protected static T __inst = null;
}
