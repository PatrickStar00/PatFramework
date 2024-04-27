
using PAT.Common;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class ComponentConfigData : ScriptableObject
{
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="gameObject"></param>
    //public virtual void OnAwake(GameObject gameObject)
    //{

    //}
}


/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class ComponentConfigData<T> : ComponentConfigData　where　T: Component
{

}

public class ComponentHolder : MonoBehaviour
{
    public string ComponentName;
    //public int[] tests;
    //[SerializeField]
    //public Test a = new Test();
    [HideInInspector]
    public string DataBuff = null;
    //[NonSerialized]
    public ComponentConfigData ConfigData ;
//#if UNITY_EDITOR
//    [NonSerialized]
//    public SerializedObject ConfigDataSerializedObject = null;
//    [System.NonSerialized]
//    public bool ShowProperty = true;
//#endif

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
//    public void RefreshDataObject(Type type)
//    {
//        var obj = ScriptableObject.CreateInstance(type); //type.Assembly.CreateInstance(type.FullName);
//        //var obj = type.Assembly.CreateInstance(type.FullName);
////        ConfigData = obj as ComponentConfigData;// System.Activator.CreateInstance(type) as ComponentConfigData;
////#if UNITY_EDITOR
////        ConfigDataSerializedObject = new SerializedObject(ConfigData);
////#endif

//        Serialize();
//    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
//    public void TryDesializeDataObject(Type type)
//    {
//        try
//        {
//            ConfigData = ScriptableObject.CreateInstance(type) as ComponentConfigData;
//            //ConfigData = type.Assembly.CreateInstance(type.FullName) as ComponentConfigData;
//            JsonUtility.FromJsonOverwrite(DataBuff, ConfigData);
//#if UNITY_EDITOR
//            ConfigDataSerializedObject = new SerializedObject(ConfigData);
//#endif
//        }
//        catch(Exception e)
//        {
//            Debug.LogError("反序列化失败 : " + type.FullName + "\n" + e.Message);
//        }
//    }

    /// <summary>
    /// 
    /// </summary>
//    public void ClearDataObject()
//    {
//        ConfigData = null;// System.Activator.CreateInstance(type) as ComponentConfigData;
//#if UNITY_EDITOR
//        ConfigDataSerializedObject = null;
//#endif
//        DataBuff = null;
//    }

    /// <summary>
    /// 
    /// </summary>
    //public void Serialize()
    //{
    //    if (ConfigData != null)
    //    {
    //        DataBuff = JsonUtility.ToJson(ConfigData, false);
    //    }
    //    else
    //    {
    //        DataBuff = null;
    //    }
    //}

    /// <summary>
    /// 运行阶段添加组件
    /// </summary>
    private void Awake()
    {
        var type = PluginTypeUtils.FindType(ComponentName);
        if (type == null) return;
        var typeData = PluginTypeUtils.FindType(ComponentName + "ConfigData");
        if (typeData == null) return;

        var comp = gameObject.AddComponent(type);
        ConfigData =  ScriptableObject.CreateInstance(typeData) as ComponentConfigData;
        //ConfigData = type.Assembly.CreateInstance(type.FullName) as ComponentConfigData;
        JsonUtility.FromJsonOverwrite(DataBuff, ConfigData);

        var field = comp.GetType().GetField("ConfigData");
        if (field != null)
        {
            field.SetValue(comp, ConfigData);
        }
        var onawake = comp.GetType().GetMethod("OnAwake");
        if (onawake != null)
        {
            onawake.Invoke(comp , null);
        }
        Debug.Log("++++ ComponentHolder.Awake ");
        UnityEngine.Object.Destroy(this, 0.1f);
        //ConfigData.OnAwake(gameObject);

        //TryDesializeDataObject(type);
        //var comp = gameObject.AddComponent(type);
        // awake 已经处理

    }
}

