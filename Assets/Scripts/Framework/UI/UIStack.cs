using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UI
{
    // 菜单管理类
    public enum TransitionType
    {
        PUSH,
        REPLACE,
        POP,
    }

    public class UIStack
    {
        public const int SORTING_STEP = 40;
        //private int m_locker = 0;
        public int Prio { get; set; }
        public UIGroup Group { get; set; }
        public UIStack(int prio, UIGroup group)
        {
            Prio = prio;
            Group = group;
        }

    }
}
