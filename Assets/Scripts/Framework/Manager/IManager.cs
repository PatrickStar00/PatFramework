using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 此枚举的作用在于各个模块的优先级划分
/// </summary>
public enum ManagerType
{
    FLogManager = 1,
    PoolManager,
    NetworkManager,
    ResourceManager,
    SettingManager,
    MultiLanguageManager,
    UIManager,
    PlatformManager,
    GameTimeManager,
    LuaManager,
    ApplicationManager,
    InstallManager,
}

/// <summary>
/// 基本模块的通用接口
/// </summary>
public interface IManager
{
    ManagerType Priority { get; set; }

    void Init();

    void Update();

    void LateUpdate();

    void FixedUpdate();

    void Destroy();
}

/// <summary>
/// 针对SystemManager进行优先级排序
/// </summary>
public class ManagerListSort : IComparer<IManager>
{
    public int Compare(IManager x, IManager y)
    {
        return PriorityCompare(x, y);
    }

    static int PriorityCompare(IManager x, IManager y)
    {
        int xPriority = (int)x.Priority;
        int yPriority = (int)y.Priority;

        if (xPriority > yPriority)
        {
            return 1;
        }
        else if (xPriority == yPriority)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }
}



