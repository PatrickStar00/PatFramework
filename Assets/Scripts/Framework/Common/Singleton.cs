using UnityEngine;
using System.Collections;

public abstract class Singleton<T> where T : new()
{
    private static T _instance;
    static object _lock = new object();
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}



public class UnitySingleton<T> : MonoBehaviour
    where T : Component
{
    private static T _instance;
    private static bool _applicationIsQuitting = false;
    public static T Instance
    {
        get
        {
            if (_instance == null && _applicationIsQuitting == false)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    //obj.name = typeof(T).Name;
                    obj.hideFlags = HideFlags.HideInHierarchy;
                    GameObject.DontDestroyOnLoad(obj);
                    _instance = (T)obj.AddComponent(typeof(T));
                }
            }
            return _instance;
        }
    }
    public virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnDestroy()
    {
        _applicationIsQuitting = true;
    }


}


public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = GameObject.Find("MONOGameObject");

                if (go == null)
                {
                    go = new GameObject("MONOGameObject");
                    DontDestroyOnLoad(go);
                }
                _instance = go.AddComponent<T>();
            }
            return _instance;
        }
    }
    public virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
        Init();
    }

    private void OnDestroy()
    {
        _instance = null;
    }
    protected virtual void Init()
    {

    }

    private void OnApplicationQuit()
    {

    }

    public virtual void Dispose()
    { }

}

public class CoroutineHelper : MonoSingleton<CoroutineHelper>
{
    public static WaitForEndOfFrame EndOfFrame = new WaitForEndOfFrame();
}