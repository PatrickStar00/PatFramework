using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 应用程序管理模块
/// 监听以下几个函数：OnApplicationPause OnApplicationFocus OnApplicationQuit
/// 其他地方不需要重复调用，只需要监听对应的事件即可
/// </summary>
/// 
public class ApplicationManager : UnitySingleton<ApplicationManager>, IManager
{
    private bool m_init;

    private ManagerType m_priority = ManagerType.ApplicationManager;

    public Action OnExitGame;
    public Action OnRestart;

    public delegate void ApplicationPauseCallback(bool pause);
    public event ApplicationPauseCallback onApplicationPause;

    public delegate void ApplicationFocusCallback(bool focus);
    public event ApplicationFocusCallback onApplicationFocus;

    public delegate void ApplicationQuitCallback();
    public event ApplicationQuitCallback onApplicationQuit;

    public ManagerType Priority
    {
        get
        {
            return m_priority;
        }

        set
        {
            m_priority = value;
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	public void Update ()
    {
		
	}

    public void LateUpdate()
    {

    }

    public void FixedUpdate()
    {

    }

    public void Init()
    {
        if (m_init)
            return;
        m_init = true;

    }

    public void Destroy()
    {

    }

    void OnEnable()
    { 
        //捕捉异常报错
        Application.logMessageReceived += HandleLog;
    }

    /// <summary>
    /// 只弹出Exception类型的报错
    /// </summary>
    /// <param name="message"></param>
    /// <param name="stackTrace"></param>
    /// <param name="type"></param>
    void HandleLog(string message, string stackTrace, LogType type)
    {
        if(type == LogType.Exception)
        {
            // ErrorPromptEvent promptEvent = new ErrorPromptEvent();
            // promptEvent.titile = "UnityException";
            // promptEvent.content = MTString.GetString(message , " stackTrace: ", stackTrace);
            // EventCenter.Broadcast<ErrorPromptEvent>((int)MUFEventDefine.eGameEvent_ErrorPrompt, promptEvent);
        }
     
    }

    void OnApplicationPause(bool paused)
    {
        //ILog.Debug("OnApplicationPause");

        if (onApplicationPause != null)
            onApplicationPause(paused);

    }

    void OnApplicationFocus(bool isFocus)
    {
        //ILog.Debug("OnApplicationFocus");

        if (onApplicationFocus != null)
            onApplicationFocus(isFocus);  
    }

    void OnApplicationQuit()
    {
        //ILog.Debug("OnApplicationQuit");
        if(onApplicationQuit != null)
           onApplicationQuit();
    }

}
