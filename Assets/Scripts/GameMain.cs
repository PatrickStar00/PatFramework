using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏主逻辑入口
/// </summary>
public class GameMain
{
    public static bool DYMODULE = false;
    private static bool m_Inited = false;

    public static bool BInited
    {
        get
        {
            return m_Inited;
        }
        set
        {
            m_Inited = value;
        }
    }

    public void StartUp()
    {
        UnityEngine.GameObject gameEntry = GameObject.Find("GameEntry");
        if (gameEntry == null) return;
        Start();
    }

    void Start()
    {
        GameObject gameEntry = GameObject.Find("GameEntry");
        BInited = true;
    }
}
