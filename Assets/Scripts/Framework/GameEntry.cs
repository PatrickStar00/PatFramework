using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntry : MonoBehaviour
{
    [HideInInspector]
    public System.Reflection.Assembly m_assembly;

    void Start()
    {
        bool bIsLoadSuccess = LoadFromScript();
        if (false == bIsLoadSuccess)
        {
            UnityEngine.Debug.LogError("Can't load scripts");
        }
    }
    bool LoadFromScript()
    {
        GameMain gameMain = new GameMain();
        gameMain.StartUp();
        return true;
    }
}
