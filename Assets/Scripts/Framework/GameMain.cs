using System.Collections;
using System.Collections.Generic;
using UI;
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
        RegistSystemCallbacks();
        // GameInit.Instance.AddManager(LuaManager.Instance);
        //!原版在这里初始化Lua并加载UI

        //这里考虑还需不需要GameInit
        GameInit.Instance.Initialize(gameEntry);
        // UI.UISystem.Instance.FormCreater = UI.FormCreater.Instance;
        BInited = true;
        UIStatic.InitUI();
        UIStatic.StackMain.Push(UIDefines.ID_WINDOWS_TEST);
    }

    void RegistSystemCallbacks()
    {

        //         ApplicationImpl.Instance.InitTempApplicationCallbacks();

        // #if !MUF_NO_NETWORK
        //         ILog.ReceiveMsgLogAction = GameReceiveMessage.ReceiveMsgLog;
        //         ILog.SendMsgLogAction = GameReceiveMessage.SendMsgLog;
        //         ILog.m_gErrorDelegate = LogUtility.OnErrorLog;
        // #endif

    }
}
