using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;
using System.Reflection;
using UnityEngine.AddressableAssets;

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

        // 设置使用的ui prefab资源
        public virtual string GetFramePrefabName()
        {
            return string.Empty;
        }

        public void TryLoadFrame(Action<UIBase> success, Action failed)
        {
            if (m_uiGameObject != null)
            {
                //已加载
                if (success != null) success(this);
            }
            else
            {
                int iLockerId = UILocker.Instance.Lock(3.0f);
                Addressables.InstantiateAsync(string.Intern(GetFramePrefabName()));
                // ResourceManager.LoadAssetAsync(string.Intern(GetFramePrefabName()), ResourceType.UI, (result) =>
                // {
                //     UILocker.Instance.Unlock(iLockerId);
                //     if (Destroyed) return;
                //     if (result.Status == ELoadingStatus.Successed)
                //     {
                //         //OnLoadedSuccess(result.Result as GameObject);
                //         OnLoadInit(result.Result as GameObject);
                //         if (success != null) success(this);
                //     }
                //     else
                //     {
                //         Debug.LogError("UIBase.OnLoadedSuccess , ui 资源错误:" + Address);
                //         if (failed != null) failed();
                //         return;
                //     }
                // });

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
