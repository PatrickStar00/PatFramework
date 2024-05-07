using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;
using System.Reflection;

namespace UI
{
    // 所有菜单UI的基类
    public class UIBase
    {

        public virtual int GetID()
        {
            return 0;
        }

        public UIStack OwnerStack { get; set; }


        public GameObject m_uiGameObject { get; set; }

        private bool m_isActive;
        public bool IsActive { get { return m_uiGameObject != null && m_isActive; } private set { m_isActive = value; } }

        public void SetActive(bool active)
        {
            if (active == m_isActive)
            {
                return;
            }

            m_isActive = active;
            if (active)
            {
                if (m_uiGameObject != null)
                {
                    DoActive();
                }
            }
            else
            {
                if (m_uiGameObject != null)
                {
                    DoInactive();
                }
            }
        }
        public Canvas RootCanvas { get; private set; }
        
        public UIBase PrevUI { get; internal set; }

        private int m_sortingOrder;
        public int SortingOrder
        {
            get { return m_sortingOrder; }
            set
            {
                if (m_sortingOrder != value)
                {
                    m_sortingOrder = value;
                    if (m_uiGameObject != null && RootCanvas != null)
                    {
                        RootCanvas.sortingOrder = value;
                    }
                }
            }
        }

        private void DoActive(bool isDockableRefresh = false)
        {
        }

        private void DoInactive(bool isDockableRefresh = false)
        {

        }

    }
}
