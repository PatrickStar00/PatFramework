
//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
    public struct ModifyWatchDog<T> where T : System.IComparable<T>
    {
        private T v;
        private bool m_inited;

        public void Init(T val)
        {
            v = val;
            m_inited = true;
        }

        public bool Check(T val , T defaultValue)
        {
            if (!m_inited)
            {
                Init(defaultValue);
            }
            if (v.CompareTo(val) == 0)
            {
                return false;
            }
            else
            {
                v = val;
                return true;
            }

        }
    }
public class GameObjectUtils
{
    /// <summary>
    /// 
    /// </summary>
    private static List<GameObject> m_vDontDestroyOnLoadList = new List<GameObject>();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    public static void DontDestroyOnLoad(UnityEngine.Object o)
    {
        if (null == o)
        {
            return;
        }
        DontDestroyOnLoad(o as MonoBehaviour);
        DontDestroyOnLoad(o as GameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    public static void DontDestroyOnLoad(MonoBehaviour o)
    {
        if (null == o)
        {
            return;
        }
        DontDestroyOnLoad(o.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    public static void DontDestroyOnLoad(GameObject o)
    {
        if (null == o)
        {
            return;
        }
        foreach (Object obj in m_vDontDestroyOnLoadList)
        {
            if (obj == o)
            {
                return;
            }
        }
        m_vDontDestroyOnLoadList.Add(o);
        Object.DontDestroyOnLoad(o);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layerName"></param>
    public static void SetAllChildrenLayer(GameObject go, string layerName)
    {
        Transform[] trans = go.GetComponentsInChildren<Transform>(true);
        int layer = LayerMask.NameToLayer(layerName);
        for (int i = 0; i < trans.Length; ++i)
        {
            trans[i].gameObject.layer = layer;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    public static void SetAllChildrenLayer(GameObject go, int layer)
    {
        Transform[] trans = go.GetComponentsInChildren<Transform>(true);
        //int layer = LayerMask.NameToLayer(layerName);
        for (int i = 0; i < trans.Length; ++i)
        {
            trans[i].gameObject.layer = layer;
        }
    }


}
