
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class StaticGameObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static GameObject GetMonoRoot()
    {
        GameObject go = GameObject.Find("MONOGameObject");

        if (go == null)
        {
            go = new GameObject("MONOGameObject");
            GameObject.DontDestroyOnLoad(go);
        }

        return go;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform GetNodeOnMonoRoot(string name)
    {
        GameObject go = GameObject.Find("MONOGameObject");

        var t = go.transform.Find(name);
        if (t != null) return t;

        var mono = GetMonoRoot();
        var newgo = new GameObject();
        newgo.transform.SetParent(mono.transform);
        return newgo.transform;
    }
    public static GameObject ALRoot { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public static GameObject Camera2DTopGameObject { get; set; }
    public static Transform Camera2DTopTransform { get; set; }
    public static Camera Camera2DTop { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public static GameObject Camera2DBottomGameObject { get; set; }
    public static Transform Camera2DBottomTransform { get; set; }
    public static Camera Camera2DBottom { get; set; }

    public static GameObject RootPool { get; set; }
    public static Transform RootPoolTransform { get; set; }

    public static Transform NetSessionsTransform { get; set; }
    public static GameObject NetSessions { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public static void Init()
    {
        //root go
        ALRoot = GameObject.Find("ALRoot");
        //ALRoot.AddComponentIfWithout<Loom>();
        //Loom
        //2d camera top
        Camera2DTopTransform = ALRoot.transform.Find("m_camera2DTop");
        Camera2DTopGameObject = Camera2DTopTransform.gameObject;
        Camera2DTop = Camera2DTopTransform.GetComponent<Camera>();

        //2d camera bottom
        Camera2DBottomTransform = ALRoot.transform.Find("m_camera2DBottom");
        Camera2DBottomGameObject = Camera2DBottomTransform.gameObject;
        Camera2DBottom = Camera2DBottomTransform.GetComponent<Camera>();

        RootPoolTransform = ALRoot.transform.Find("m_pool");
        RootPool = RootPoolTransform.gameObject;

        NetSessionsTransform = ALRoot.transform.Find("m_netSession");
        NetSessions = NetSessionsTransform.gameObject;
    }
}

    //public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    //{
    //    private static T _instance;
    //    public static T Instance
    //    {
    //        get
    //        {
    //            if (_instance == null)
    //            {
    //                GameObject go = GameObject.Find("MONOGameObject");

    //                if (go == null)
    //                {
    //                    go = new GameObject("MONOGameObject");
    //                    DontDestroyOnLoad(go);
    //                }
    //                _instance = go.AddComponent<T>();
    //            }
    //            return _instance;
    //        }
    //    }
    //    public virtual void Awake()
    //    {
    //        DontDestroyOnLoad(this.gameObject);
    //        if (_instance == null)
    //        {
    //            _instance = this as T;
    //        }
    //        else
    //        {
    //            Destroy(gameObject);
    //        }
    //        Init();
    //    }

    //    private void OnDestroy()
    //    {
    //        _instance = null;
    //    }
    //    protected virtual void Init()
    //    {

    //    }

    //    private void OnApplicationQuit()
    //    {
    //        //_instance = null;
    //    }

    //    public virtual void Dispose()
    //    { }

    //}


    
