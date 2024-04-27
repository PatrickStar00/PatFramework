using System.Collections.Generic;
using UnityEngine;

public class GameInit : UnitySingleton<GameInit>
{

    private List<IManager> m_managerList = new List<IManager>();

    private ManagerListSort m_managerListSort = new ManagerListSort();
    private bool m_managerInited = false;

    private GameObject m_GameEntry;
    public GameObject GameEntry
    {
        get
        {
            return m_GameEntry;
        }
    }

    public void AddManager(IManager manager)
    {
        m_managerList.Add(manager);
    }

    public void Initialize(GameObject gameEntry)
    {
        m_GameEntry = gameEntry;
        InitManager();
    }

    void InitManager()
    {
        m_managerInited = false;

        //可填充式增加系统模块
        //这里的顺序无所谓，执行优先级只关注m_priority
        m_managerList.Add(FLogManager.Instance);
        m_managerList.Sort(m_managerListSort);

        foreach (var manager in m_managerList)
        {
            ILog.Debug("Manager Init ", manager.Priority.ToString());
            if (manager != null)
                manager.Init();
        }

        m_managerInited = true;
    }


    void Update()
    {
        if (m_managerInited == false)
            return;

        foreach (var manager in m_managerList)
        {
            if (manager != null)
                manager.Update();
        }
    }
    void LateUpdate()
    {
        if (m_managerInited == false)
            return;

        foreach (var manager in m_managerList)
        {
            if (manager != null)
                manager.LateUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (m_managerInited == false)
            return;

        foreach (var manager in m_managerList)
        {
            if (manager != null)
                manager.FixedUpdate();
        }

    }

}