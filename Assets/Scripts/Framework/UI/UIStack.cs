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

        public int Prio { get; set; }
        public UIGroup Group { get; set; }
        public UIStack(int prio, UIGroup group)
        {
            Prio = prio;
            Group = group;
        }

        #region fields
        // 所有注册的UI
        private Dictionary<int, UIBase> m_uis = new Dictionary<int, UIBase>();
        // ui调用堆栈
        private StackEx<int> m_uiStack = new StackEx<int>();
        #endregion

        // 注册UI
        public void RegisterUI(UIBase ui)
        {

            Debug.Assert(!m_uis.ContainsKey(ui.GetID()), "重复的 UI ID : " + ui.ToString());
            ui.OwnerStack = this;
            m_uis.Add(ui.GetID(), ui);
        }
        public UIBase GetUIInstance(int uiid)
        {
            if (m_uis.ContainsKey(uiid))
            {
                return m_uis[uiid];
            }
            if (UISystem.Instance.FormCreater != null)
            {
                UIBase aUI = UISystem.Instance.FormCreater.CreateForm(uiid);

                if (aUI == null)
                {
                    Debug.LogFormat("GetUIInstance id : {0} NOT exists!", uiid);
                    return null;
                }

                RegisterUI(aUI);
                return aUI;
            }
            Debug.LogError("plz indicate a FormCreater");
            return null;
        }


    }
}
