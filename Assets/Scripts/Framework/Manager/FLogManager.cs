using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
/// 文本LOG写入
/// (!!!禁止ILog类以外调用!!!)
/// </summary>
public class FLogManager : Singleton<FLogManager>, IManager
{
    StreamWriter m_fileWriter;

    public bool m_isMsgLogTurnOn = true;

    public Action<string, int> receiveMsgLogDelegate;
    public Action<string, int> senMsgLogDelegate;

    static public ILog.LogLevel m_gFLogLevel = ILog.LogLevel.InfoLevel;

    private CultureInfo m_cultureInfo = CultureInfo.CreateSpecificCulture("en-US");

    private List<object> m_fLogList = new List<object>();
    private List<object> m_fLogTempList = new List<object>();

    //c# 获取当前线程id
    public int m_mainThreadId;

    public readonly string m_flogExtention = ".log";

    public string m_currentLogFilePath;

    //FLog子线程
    private Thread m_fLogThread = null;
    //手动唤起 自动挂起 第一次尝试继续就会被阻塞
    private AutoResetEvent m_fLogEvent = new AutoResetEvent(false);

    private ManagerType m_priority = ManagerType.FLogManager;

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


    /// <summary>
    /// 编译环境下默认开启FLog,其他情况需要手动开启
    /// </summary>
    public void Init()
    {
        //#if UNITY_EDITOR
        InitFLog();
        //#endif

    }

    /// <summary>
    /// 初始化FLOG接口
    /// </summary>
    public void InitFLog()
    {
        m_mainThreadId = Thread.CurrentThread.ManagedThreadId;

        if (m_fileWriter != null) return;

        m_fLogList.Clear();

        string path = Path.Combine(GetFlogPath(), DateTime.Now.ToString("u", m_cultureInfo).Replace(':', '-') + m_flogExtention);
        try
        {
            string folder = Directory.GetParent(path).FullName;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            m_fileWriter = new StreamWriter(new FileStream(path, FileMode.Create), Encoding.UTF8);
            m_currentLogFilePath = path;
            ;
            ILog.Info(MTString.GetString("Start up Flog ========>>>>>", path));
        }
        catch (Exception e)
        {
            ILog.Error(e.ToString());
            ILog.Error(MTString.GetString("Start up Flog Fail !!!!!!", path));
        }

        InitFLogThread();

        TryDeleteMoreLog();

        // FlogUploader.Instance.CheckFlogWhenStartUpGame();

        receiveMsgLogDelegate = ILog.ReceiveMsgLogAction;

        senMsgLogDelegate = ILog.SendMsgLogAction;
    }

    public void Destroy()
    {

    }

    public void Update()
    {

    }

    public void LateUpdate()
    {

    }

    public void FixedUpdate()
    {

    }

    /// <summary>
    /// 初始化子线程
    /// </summary>
    private void InitFLogThread()
    {
        if (m_fLogThread != null)
            return;

        m_fLogThread = new Thread(FLogSubThread);
        m_fLogThread.Start();
    }

    /// <summary>
    /// 外接唤醒FLog的子线程
    /// </summary>
    public void FLogThreadWakeUp()
    {
        m_fLogEvent.Set();
    }

    /// <summary>
    /// 调用FLog子线程,进行I/O操作
    /// </summary>
    private void FLogSubThread()
    {
        while (true)
        {
            FLogFileWriteThread();
            //执行完挂起等待手动唤醒
            m_fLogEvent.WaitOne();
        }
    }

    /// <summary>
    /// 关闭FLOG
    /// </summary>
    public void Close()
    {
        //进程结束时，需要销毁子线程
        CloseFLogThread();

        if (m_fileWriter == null) return;
        SaveFile();
    }

    /// <summary>
    /// 需要销毁子线程
    /// </summary>
    private void CloseFLogThread()
    {
        if (m_fLogThread != null)
            m_fLogThread.Abort();
    }

    /// <summary>
    /// 获取FLOG地址
    /// </summary>
    /// <returns></returns>
    public string GetFlogPath()
    {
        string persistentDataPath = MTString.GetString(Application.dataPath, "/PatFramwork/");
        string persistentDataAssetsPath = MTString.GetString(persistentDataPath, "assets/");

        return Path.Combine(persistentDataAssetsPath, "Flog/");
    }

    /// <summary>
    /// 保存文件
    /// </summary>
    public void SaveFile()
    {
        if (m_fileWriter != null) return;

        //关闭时检测还在列表中的数据，进行阻塞式写入
        if (m_fLogList.Count > 0)
        {
            for (int i = 0; i < m_fLogList.Count; i++)
            {
                FileWriter(m_fLogList[i]);
            }
        }

        try
        {
            m_fileWriter.Flush();
            m_fileWriter.Close();
        }
        catch (Exception e)
        {
            m_fileWriter = null;
            Debug.LogError(e.ToString());
        }
        m_fileWriter = null;
        m_fLogList.Clear();
    }


    /// <summary>
    /// 普通LOG写入
    /// </summary>
    /// <param name="str"></param>
    /// <param name="strLine"></param>
    public void Log(object str)
    {
        if (m_fileWriter == null) return;
        if (!m_isMsgLogTurnOn)
        {
            return;
        }

        AddFileStr(str);

    }

    /// <summary>
    /// 接收消息LOG
    /// </summary>
    /// <param name="sSocketType"></param>
    /// <param name="iMsgID"></param>
    public void ReceiveMsgLog(string sSocketType, int iMsgID)
    {
        if (m_fileWriter == null) return;
        if (!m_isMsgLogTurnOn)
        {
            return;
        }
        if (receiveMsgLogDelegate != null)
        {
            receiveMsgLogDelegate(sSocketType, iMsgID);
        }
        else
        {
            ReceiveMsgLogByType<int>(sSocketType, iMsgID);
        }
    }

    /// <summary>
    /// 接收消息LOG根据类型来写入
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sSocketType"></param>
    /// <param name="id"></param>
    public void ReceiveMsgLogByType<T>(string sSocketType, T id)
    {
        if (m_fileWriter == null) return;
        if (!m_isMsgLogTurnOn)
        {
            return;
        }
        try
        {
            AddFileStr(MTString.GetString("<--[", sSocketType, "]", id.ToString(), "\r\n"));
        }
        catch (System.Exception ex)
        {
            Close();
            ILog.Error(ex.ToString());
        }
    }


    /// <summary>
    /// 发送协议LOG根据类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="socketTypeStr"></param>
    /// <param name="id"></param>
    public void SendMsgLogByType<T>(string socketTypeStr, T id)
    {
        if (m_fileWriter == null) return;
        if (!m_isMsgLogTurnOn)
        {
            return;
        }
        try
        {
            AddFileStr(MTString.GetString("-->[", socketTypeStr, "]", id.ToString(), "\r\n"));
        }
        catch (System.Exception ex)
        {
            Close();
            ILog.Error(ex.ToString());
        }
    }

    /// <summary>
    /// 将LOG内容存进m_fLogLinkedList，并唤起FLOG子线程
    /// </summary>
    /// <param name="str"></param>
    /// <param name="strLine"></param>
    private void AddFileStr(object str)
    {
        string frameCount = "";
        if (Thread.CurrentThread.ManagedThreadId == m_mainThreadId)
            frameCount = Time.frameCount.ToString();
        string fileStr = MTString.GetString("[FLog]- [", DateTime.Now.ToString("u", m_cultureInfo), " ", frameCount, "] ", str as string);
        m_fLogList.Add(fileStr);

        FLogThreadWakeUp();
    }

    /// <summary>
    /// 字段写入工作
    /// </summary>
    /// <param name="str"></param>
    private void FileWriter(object fileStr)
    {
        try
        {
            m_fileWriter.Write(fileStr);
            m_fileWriter.Flush();
        }
        catch (System.Exception ex)
        {
            Close();
            ILog.Error(ex.ToString());
        }
    }

    /// <summary>
    /// fLogLinkedList交替，并写入LOG到文件中
    /// </summary>
    public void FLogFileWriteThread()
    {
        //需要锁住，避免文件写入错乱，因为交替耗时很短，所以阻塞压力不大
        lock (m_fLogList)
        {
            if (m_fLogList.Count <= 0)
                return;
            var tempList = m_fLogTempList;
            m_fLogTempList = m_fLogList;
            m_fLogList = tempList;
        }

        for (int i = 0; i < m_fLogTempList.Count; i++)
        {
            FileWriter(m_fLogTempList[i]);
        }
        m_fLogTempList.Clear();
    }

    /// <summary>
    /// 删除过多的FLogFile(20条)
    /// </summary>
    public void TryDeleteMoreLog()
    {
        try
        {
            string flogDir = GetFlogPath();
            if (!Directory.Exists(flogDir))
            {
                return;
            }
            string[] files = Directory.GetFiles(flogDir, "*" + m_flogExtention, SearchOption.TopDirectoryOnly);
            if (files == null || files.Length == 0)
            {
                return;
            }
            if (files.Length > 20)
            {
                List<string> fileList = new List<string>(files.Length);
                fileList.AddRange(files);
                fileList.Sort();
                for (int i = 0; i < 10; i++)
                {
                    if (File.Exists(fileList[i]))
                    {
                        File.Delete(fileList[i]);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }



}

