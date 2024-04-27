using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Playables;
using PAT.Common;

/// <summary>
/// 
/// </summary>
public static class GameObjectExtension
{

    //public static void Find<T, P>(this SortedDictionary<T, P> obj)
    //{

    //}
    //static System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

    //static System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

    //public struct ComponentCache
    //{
    //    public static class ComponentCacheDelegate<T> where T : Component
    //    {
    //        public delegate T GetCachedComponent(ref ComponentCache cache);
    //        public delegate void SetCachedComponent(ref ComponentCache cache, T value);
    //        public static GetCachedComponent GetCachedComponentFunc;
    //        public static SetCachedComponent SetCachedComponentFunc;
    //    }
    //}
    private static Dictionary<int, Dictionary<int, Component[]>> m_dictChildrenComponents = new Dictionary<int, Dictionary<int, Component[]>>();
    private static Dictionary<int, Dictionary<int, Component>> m_dictComponent = new Dictionary<int, Dictionary<int, Component>>();
    private static Dictionary<int, Dictionary<string, Transform>> m_dictChildrenTransform = new Dictionary<int, Dictionary<string, Transform>>();
    //obj有可能是null的
    public static void CheckSetActive(this GameObject obj, bool bActive)
    {

        if (obj != null && obj.activeSelf != bActive)
        {
            //stopWatch.Reset();
            //stopWatch.Start();
            obj.SetActive(bActive);
            //stopWatch.Stop();
            //if (stopWatch.ElapsedTicks > 50)
            //{
            //    Debug.LogWarning(obj.name + ",active:" + bActive + ",takes ticks:" + stopWatch.ElapsedTicks);
            //}
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void ResetTransform(this GameObject obj)
    {
        var t = obj.transform;
        t.localPosition = Vector3.zero;
        t.localScale = Vector3.one;
        t.localRotation = Quaternion.identity;
    }

    //移除屏幕外
    public static void SetPositionActive(this GameObject obj, bool value)
    {
        //stopWatch.Reset();
        //stopWatch.Start();

        //自定义active，默认已激活状态
        //if (obj.activeSelf != true)
        //{
        //    obj.CheckSetActive(true);
        //}
        var transform = obj.UseCacheGetComponent<Transform>();// .transform;
        var position = transform.position;
        var yAix = position.y;
        if (value)
        {
            if (yAix > 1500)
            {
                position.y = yAix - 3000;
                transform.position = position;
            }
        }
        else
        {
            if (yAix <= 1500)
            {
                position.y = yAix + 3000;
                transform.position = position;
            }
        }
        //stopWatch.Stop();
        //if (stopWatch.ElapsedTicks > 50)
        //{
        //    Debug.LogWarning(obj.name + ",custom active:" + value + ",takes ticks:" + stopWatch.ElapsedTicks);
        //}

    }

    //缩小
    public static void SetScaleActive(this GameObject obj, bool value)
    {
        //stopWatch.Reset();
        //stopWatch.Start();

        //自定义active，默认已激活状态
        //if (obj.activeSelf != true)
        //{
        //    obj.CheckSetActive(true);
        //}

        var transform = obj.UseCacheGetComponent<Transform>(); //obj.transform;
        if (value)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = Vector3.zero;
        }
        //stopWatch.Stop();
        //if (stopWatch.ElapsedTicks > 50)
        //{
            //Debug.LogWarning(obj.name + ",scale active:" + value + ",takes ticks:" + stopWatch.ElapsedTicks);
        //}
    }


    public static bool IsScaleActive(this GameObject obj)
    {
        if (obj.activeSelf != true)
        {
            return false;
        }

        var localScale = obj.transform.localScale;
        if (!(localScale == Vector3.zero))
        {
            return true;
        }

        return false;
    }


    public static UnityEngine.UI.Text[] GetTextComponentsInChildren(this GameObject obj)
    {
        return obj.GetComponentsInChildren<UnityEngine.UI.Text>(true);
    }

    public static AnimationState GetAnimState(this Animation anim, string clipName)
    {
        return anim ? anim[clipName] : null;
    }

    public static void SetPositionXYZ(this GameObject go, float x, float y, float z)
    {
        var position = go.transform.position;
        position.x = x;
        position.y = y;
        position.z = z;
        go.transform.position = position;
    }
    public static void SetLocalPositionXYZ(this GameObject go, float x, float y, float z)
    {
        var position = go.transform.localPosition;
        position.x = x;
        position.y = y;
        position.z = z;
        go.transform.localPosition = position;
    }

    public static void TryRunTimeline(this GameObject go)
    {
        var pd = go.GetComponentInChildren<PlayableDirector>();
        if (pd != null)
        {
            pd.Play();
        }
    }

    public static void SetRotationXYZW(this GameObject go, float x, float y, float z, float w)
    {
        var rotation = go.transform.rotation;
        rotation.x = x;
        rotation.y = y;
        rotation.z = z;
        rotation.w = w;
        go.transform.rotation = rotation;
    }

    public static void SetRotationEulerXYZ(this GameObject go, float x, float y, float z)
    {
        var rotation = go.transform.localRotation;
        //rotation.x = x;
        //rotation.y = y;
        //rotation.z = z;
        go.transform.localRotation = Quaternion.Euler(x, y, z);
    }

    public static float GetPositionXYZ(this GameObject go, out float y, out float z)
    {
        var position = go.transform.position;
        y = position.y;
        z = position.z;
        return position.x;
    }

    public static float GetRotationXYZW(this GameObject go, out float y, out float z, out float w)
    {
        var rotation = go.transform.rotation;
        y = rotation.y;
        z = rotation.z;
        w = rotation.w;
        return rotation.x;
    }

    public static bool Valid(this GameObject go)
    {
        return go != null;
    }

    public static bool Valid(this UnityEngine.Object go)
    {
        return go != null;
    }

    public static UnityEngine.GameObject CustomInstantiate(UnityEngine.Object original)
    {
        //stopWatch.Reset();
        //stopWatch.Start();
        var go = (UnityEngine.GameObject)UnityEngine.GameObject.Instantiate(original);
        //stopWatch.Stop();

        //Debug.LogWarning(original.name + ",CustomInstantiate:" + ",takes ticks:" + stopWatch.ElapsedTicks);
        return go;
    }

    public static Component[] UseCacheGetComponentsInChildren<T>(this GameObject go, bool includeInactive = false) where T : Component
    {
        int instanceId = go.GetInstanceID();
        int typeId = TypeIdOf<T>.value;
        if (!m_dictChildrenComponents.ContainsKey(instanceId))
        {
            m_dictChildrenComponents[instanceId] = new Dictionary<int, Component[]>();
            T[] components = go.GetComponentsInChildren<T>(includeInactive);
            m_dictChildrenComponents[instanceId][typeId] = components;
        }
        else
        {
            if (!m_dictChildrenComponents[instanceId].ContainsKey(typeId))
            {
                T[] components = go.GetComponentsInChildren<T>(includeInactive);
                m_dictChildrenComponents[instanceId][typeId] = components;
            }
        }
        return m_dictChildrenComponents[instanceId][typeId];
    }

    public static T UseCacheGetComponent<T>(this GameObject go) where T : Component
    {
        int instanceId = go.GetInstanceID();
        int typeId = TypeIdOf<T>.value;
        if (!m_dictComponent.ContainsKey(instanceId))
        {
            m_dictComponent[instanceId] = new Dictionary<int, Component>();
            T component = go.GetComponent<T>();
            m_dictComponent[instanceId][typeId] = component;
        }
        else
        {
            if (!m_dictComponent[instanceId].ContainsKey(typeId))
            {
                T component = go.GetComponent<T>();
                m_dictComponent[instanceId][typeId] = component;
            }
        }
        return m_dictComponent[instanceId][typeId] as T;
    }

    public static Transform UseCacheGetChildTransform(this GameObject go, string childName)
    {
        int instanceId = go.GetInstanceID();
        Transform transform = null;
        if (!m_dictChildrenTransform.ContainsKey(instanceId))
        {
            transform = go.transform.Find(childName);
            m_dictChildrenTransform[instanceId] = new Dictionary<string, Transform>();
            m_dictChildrenTransform[instanceId][childName] = transform;
        }
        else if (!m_dictChildrenTransform[instanceId].ContainsKey(childName))
        {
            transform = go.transform.Find(childName);
            m_dictChildrenTransform[instanceId][childName] = transform;
        }
        else
        {
            transform = m_dictChildrenTransform[instanceId][childName];
        }
        return transform;
    }

    public static T AddComponentIfWithout<T>(this GameObject go) where T : Component
    {
        T c = go.UseCacheGetComponent<T>();
        if (c == null)
        {
            c = go.AddComponent<T>();
        }

        if (c != null)
        {
            int instanceId = go.GetInstanceID();
            int typeId = TypeIdOf<T>.value;
            m_dictComponent[instanceId][typeId] = c;
        }

        return c;
    }

    public static void Clear()
    {
        m_dictChildrenComponents.Clear();
        m_dictChildrenTransform.Clear();
        m_dictComponent.Clear();
        m_originalMaterials.Clear();
    }

    private static Dictionary<int, Material> m_originalMaterials = new Dictionary<int, Material>();
    private static Material m_grayMaterial;
    public static void SetGray(this GameObject go , bool gray)
    {
        if (m_grayMaterial == null)
        {
            Shader shader = null;
            shader = ShaderUtils.LoadShader("UI/Gray");

            if (shader == null)
                return;
            m_grayMaterial = new Material(shader);
        }
        var gs = go.GetComponentsInChildren<Graphic>();
        if (gray)
        {
            foreach (var g in gs)
            {
                var insid = g.GetInstanceID();
                m_originalMaterials[insid] = g.material;
                g.material = m_grayMaterial;
            }
        }
        else
        {
            foreach (var g in gs)
            {
                var insid = g.GetInstanceID();
                Material old = null; 
                if (m_originalMaterials.TryGetValue(insid, out old))
                {
                    g.material = old;
                    m_originalMaterials.Remove(insid);
                }
            }
        }
    }
    public static void ClearGrayMaterialCache(this GameObject go)
    {
        if (m_originalMaterials.Count == 0)
            return;
        var gs = go.GetComponentsInChildren<Graphic>();
        foreach (var g in gs)
        {
            var insid = g.GetInstanceID();
            m_originalMaterials.Remove(insid);
        }
    }
};

