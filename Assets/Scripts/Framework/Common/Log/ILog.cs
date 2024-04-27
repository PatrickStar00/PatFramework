using System;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Collections;
using System.Collections.Generic;


public class ILog
{
    public enum LogLevel
    {
        DebugLevel = 0,
        InfoLevel,
        WarnLevel,
        ErrorLevel
    }

    public static Action<string, int> ReceiveMsgLogAction;
    public static Action<string, int> SendMsgLogAction;

    static public Callback<bool, string, string> m_gErrorDelegate;
    static private UdpLog m_udpLog = new UdpLog();
    static public ulong m_guID = 0;
    static public string m_gstrAddInfo = "0";
    static public LogLevel m_gLogLevel = LogLevel.DebugLevel; // by default
    public static List<string> m_SendLogList = new List<string>();
    public static bool m_bReadPerf = false;
    public static bool m_bCanSendError = true;

    public static void Init()
    {
        Info(MTString.GetString("Dev: ", SystemInfo.deviceModel, " CPU: ", SystemInfo.processorType, " Mem: ", SystemInfo.systemMemorySize.ToString(), " OS: ", SystemInfo.operatingSystem));
    }

    /// <summary>
    /// 设置ILog等级
    /// </summary>
    /// <param name="level"></param>
    public static void SetILogLevel(LogLevel level)
    {
        m_gLogLevel = level;
    }

    /// <summary>
    /// 通用的ILog Debug接口
    /// </summary>
    /// <param name="message"></param>
    /// 
    static public void Debug(string message)
    {
        string strMsg = MTString.GetString("[Debug]-", message);
        SendDebug(strMsg);
    }
    /// <summary>
    /// 通用的ILog Debug接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// 
    static public void Debug(string message, string message2)
    {
        string strMsg = MTString.GetString("[Debug]-", message, message2);
        SendDebug(strMsg);
    }

    /// <summary>
    /// 通用的ILog Debug接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// 
    static public void Debug(string message, string message2, string message3)
    {
        string strMsg = MTString.GetString("[Debug]-", message, message2, message3);
        SendDebug(strMsg);
    }

    /// <summary>
    /// 通用的ILog Debug接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// <param name="message4"></param>
    /// 
    static public void Debug(string message, string message2, string message3, string message4)
    {
        string strMsg = MTString.GetString("[Debug]-", message, message2, message3, message4);
        SendDebug(strMsg);
    }
    /// <summary>
    /// 通用的ILog Debug接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// <param name="message4"></param>
    /// <param name="message5"></param>
    /// 
    static public void Debug(string message, string message2, string message3, string message4, string message5)
    {
        string strMsg = MTString.GetString("[Debug]-", message, message2, message3, message4, message5);
        SendDebug(strMsg);
    }

    static public void SendDebug(string strInfo)
    {
        if (m_gLogLevel <= LogLevel.DebugLevel)
        {
            UnityEngine.Debug.Log(strInfo);
            SendUdpLog(strInfo); // 调试日志不输出到日志服务器
        }
        LogToFile(strInfo);
    }



    /// <summary>
    /// 通用的ILog Info接口
    /// </summary>
    /// <param name="message"></param>
    /// 
    static public void Info(string message)
    {
        string strMsg = MTString.GetString("[Info]-", message);
        SendInfo(strMsg);
    }
    /// <summary>
    /// 通用的ILog Info接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// 
    static public void Info(string message, string message2)
    {
        string strMsg = MTString.GetString("[Info]-", message, message2);
        SendInfo(strMsg);
    }
    /// <summary>
    /// 通用的ILog Info接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// 
    static public void Info(string message, string message2, string message3)
    {
        string strMsg = MTString.GetString("[Info]-", message, message2, message3);
        SendInfo(strMsg);
    }
    /// <summary>
    /// 通用的ILog Info接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// <param name="message4"></param>
    /// 
    static public void Info(string message, string message2, string message3, string message4)
    {
        string strMsg = MTString.GetString("[Info]-", message, message2, message3, message4);
        SendInfo(strMsg);
    }
    /// <summary>
    /// 通用的ILog Info接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// <param name="message4"></param>
    /// <param name="message5"></param>
    /// 
    static public void Info(string message, string message2, string message3, string message4, string message5)
    {
        string strMsg = MTString.GetString("[Info]-", message, message2, message3, message4, message5);
        SendInfo(strMsg);
    }

    static public void SendInfo(string strInfo)
    {
        if (m_gLogLevel <= LogLevel.InfoLevel)
        {
            UnityEngine.Debug.Log(strInfo);
            SendUdpLog(strInfo);
        }
        LogToFile(strInfo);
    }


    /// <summary>
    /// 通用的ILog Warn接口
    /// </summary>
    /// <param name="message"></param>
    /// 
    static public void Warn(string message)
    {
        string strMsg = MTString.GetString("[Warn]-", message);
        SendWarn(strMsg);
    }
    /// <summary>
    /// 通用的ILog Warn接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// 
    static public void Warn(string message, string message2)
    {
        string strMsg = MTString.GetString("[Warn]-", message, message2);
        SendWarn(strMsg);
    }
    /// <summary>
    /// 通用的ILog Warn接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// 
    static public void Warn(string message, string message2, string message3)
    {
        string strMsg = MTString.GetString("[Warn]-", message, message2, message3);
        SendWarn(strMsg);
    }
    /// <summary>
    /// 通用的ILog Warn接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// <param name="message4"></param>
    /// 
    static public void Warn(string message, string message2, string message3, string message4)
    {
        string strMsg = MTString.GetString("[Warn]-", message, message2, message3, message4);
        SendWarn(strMsg);
    }
    /// <summary>
    /// 通用的ILog Warn接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// <param name="message4"></param>
    /// <param name="message5"></param>
    /// 
    static public void Warn(string message, string message2, string message3, string message4, string message5)
    {
        string strMsg = MTString.GetString("[Warn]-", message, message2, message3, message4, message5);
        SendWarn(strMsg);
    }

    static public void SendWarn(string strInfo)
    {
        if (m_gLogLevel <= LogLevel.WarnLevel)
        {
            UnityEngine.Debug.LogWarning(strInfo);
            SendUdpLog(strInfo, "Warn");
        }
        LogToFile(strInfo);
    }


    /// <summary>
    /// 通用的ILog Error接口
    /// </summary>
    /// <param name="message"></param>
    /// 
    static public void Error(string message)
    {
        string strMsg = MTString.GetString("[Error]-", message);
        SendError(strMsg);
    }
    /// <summary>
    /// 通用的ILog Error接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// 
    static public void Error(string message, string message2)
    {
        string strMsg = MTString.GetString("[Error]-", message, message2);
        SendError(strMsg);
    }
    /// <summary>
    /// 通用的ILog Error接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// 
    static public void Error(string message, string message2, string message3)
    {
        string strMsg = MTString.GetString("[Error]-", message, message2, message3);
        SendError(strMsg);
    }
    /// <summary>
    /// 通用的ILog Error接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// <param name="message4"></param>
    /// 
    static public void Error(string message, string message2, string message3, string message4)
    {
        string strMsg = MTString.GetString("[Error]-", message, message2, message3, message4);
        SendError(strMsg);
    }
    /// <summary>
    /// 通用的ILog Error接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// <param name="message4"></param>
    /// <param name="message5"></param>
    /// 
    static public void Error(string message, string message2, string message3, string message4, string message5)
    {
        string strMsg = MTString.GetString("[Error]-", message, message2, message3, message4, message5);
        SendError(strMsg);
    }
    /// <summary>
    /// 通用的ILog Error接口 需要传参的特殊重构
    /// </summary>
    /// <param name="message"></param>
    /// 
    static public void Report(string message)
    {
        string strMsg = MTString.GetString("[Report]-", message);
        SendError(strMsg, true);
    }

    /// <summary>
    /// 通用的ILog Error接口 需要传参的特殊重构
    /// </summary>
    /// <param name="message"></param>
    /// <param name="stackTrace"></param>
    /// 
    static public void Report(string message, string stackTrace = "")
    {
        string strMsg = MTString.GetString("[Report]-", message, stackTrace);
        SendError(strMsg, true, stackTrace);
    }

    static public void SendError(string strInfo, bool bErrorReport = false, string stackTrace = "")
    {

#if !UNITY_EDITOR
        if (!IsCanSendError(strInfo)) //send limit
            return;
#endif
#if UNITY_EDITOR
        UnityEngine.Debug.LogError(strInfo);
#else
        if (m_gErrorDelegate != null)
        {
            m_gErrorDelegate(bErrorReport, strInfo, stackTrace);
        }
        UnityEngine.Debug.LogWarning(strInfo);
#endif
        ILog.LogToFile(MTString.GetString(strInfo, "====>\n", stackTrace));
        SendUdpLog(strInfo, "Error");
    }

    static public bool IsCanSendError(string strLog)
    {
        if (!m_bReadPerf)
        {
            if (PlayerPrefs.HasKey("Key_ErrorSend"))
            {
                m_bCanSendError = PlayerPrefs.GetInt("Key_ErrorSend") == 1 ? true : false;
            }
            m_bReadPerf = true;
        }
        if (!m_bCanSendError || m_SendLogList.Contains(strLog))
            return false;
        else
        {
            if (m_SendLogList.Count > 5)
                m_SendLogList.Clear();
            m_SendLogList.Add(strLog);
        }
        return true;
    }

    /// <summary>
    /// 写入文件对外通用接口
    /// </summary>
    /// <param name="str"></param>
    /// <param name="strLine"></param>
    /// 
    public static void LogToFile(string message)
    {
        string strMsg = MTString.GetString(message, "\n");
        FLogManager.Instance.Log(strMsg);
    }
    /// <summary>
    /// 写入文件对外通用接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// 
    public static void LogToFile(string message, string message2)
    {
        string strMsg = MTString.GetString(message, message2, "\r\n");
        FLogManager.Instance.Log(strMsg);
    }
    /// <summary>
    /// 写入文件对外通用接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// 
    public static void LogToFile(string message, string message2, string message3)
    {
        string strMsg = MTString.GetString(message, message2, message3, "\r\n");
        FLogManager.Instance.Log(strMsg);
    }
    /// <summary>
    /// 写入文件对外通用接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// <param name="message4"></param>
    /// 
    public static void LogToFile(string message, string message2, string message3, string message4)
    {
        string strMsg = MTString.GetString(message, message2, message3, message4, "\r\n");
        FLogManager.Instance.Log(strMsg);
    }
    /// <summary>
    /// 写入文件对外通用接口
    /// </summary>
    /// <param name="message"></param>
    /// <param name="message2"></param>
    /// <param name="message3"></param>
    /// <param name="message4"></param>
    /// <param name="message5"></param>
    /// 
    public static void LogToFile(string message, string message2, string message3, string message4, string message5)
    {
        string strMsg = MTString.GetString(message, message2, message3, message4, message5, "\r\n");
        FLogManager.Instance.Log(strMsg);
    }
    /// <summary>
    /// 收/发 服务器消息
    /// </summary>
    /// <param name="sSocketType"></param>
    /// <param name="iMsgID"></param>
    /// 
    public static void ReceiveMsgLog(string sSocketType, int iMsgID)
    {
        FLogManager.Instance.ReceiveMsgLog(sSocketType, iMsgID);
    }
    public static void ReceiveMsgLogByType<T>(string iSocketType, T id)
    {
        FLogManager.Instance.ReceiveMsgLogByType(iSocketType, id);
    }
    public static void SendMsgLogByType<T>(string sSocketType, T id)
    {
        FLogManager.Instance.SendMsgLogByType(sSocketType, id);
    }

    /// <summary>
    /// 关闭Log
    /// </summary>
    public static void Close()
    {
        FLogManager.Instance.Close();
    }


    /// <summary>
    /// 封装好数据后发送UDPLog
    /// </summary>
    /// <param name="message"></param>
    /// <param name="logType"></param>
    /// 
    private static void SendUdpLog(object message, string logType = "")
    {
#if !MUF_NO_NETWORK
        m_udpLog.SendLog(GetLogBytes(message, logType));
#endif
    }
#if !MUF_NO_NETWORK
    private static byte[] GetLogBytes(object message, string logType)
    {
        string strVerInfo = logType;
        // if (GameServerConfig.Instance != null && GameServerConfig.Instance.m_loginServerInfo != null && GameServerConfig.Instance.m_loginServerInfo.m_sClientLocalVersion != null)
        // {
        //     strVerInfo = MTString.GetString(GameServerConfig.Instance.m_loginServerInfo.m_sClientLocalVersion,strVerInfo);
        // }
        string strInfo = message as string;
        strInfo = MTString.GetString("[", ILog.m_guID.ToString(), "]", "[", strVerInfo, "]", strInfo);// debug info

        byte[] sendBytes = Encoding.UTF8.GetBytes(strInfo);

        return sendBytes;
    }

    /// <summary>
    /// Application.RegisterLogCallback 回调函数
    /// </summary>
    /// <param name="logstring"></param>
    /// <param name="stackTrace"></param>
    /// <param name="type"></param>
    public void BuglyHandleLog(string logstring, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            Report(logstring, stackTrace);
        }
    }
#endif

}

/// <summary>
/// UdpLog模块
/// </summary>
class UdpLog
{
    public UdpClient m_udpClient = null;
    int m_nServerPort = 0;
    string m_strServerIP = "0";

    IPEndPoint m_hostEndPoint = null;

    public void InitLog(string strServerIP, int nServerPort)
    {
        m_nServerPort = nServerPort;
        m_strServerIP = strServerIP;
        if (m_udpClient == null)
        {
            m_udpClient = new UdpClient();

            IPAddress hostIP = IPAddress.Parse(m_strServerIP);
            m_hostEndPoint = new IPEndPoint(hostIP, m_nServerPort);
        }
    }

    // 发送数据
    public void SendLog(byte[] data)
    {
        if (data.Length > 0)
        {
            if (m_udpClient != null)
            {
                m_udpClient.BeginSend(data, data.Length, m_hostEndPoint, null, null);
            }
        }
    }
}
