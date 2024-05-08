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

        public UIBase GetTopUI()
        {
            if (m_uiStack.Count <= 0) return null;
            var ui = m_uis[m_uiStack.Peek()];
            return ui;
        }

        public Dictionary<int, UIBase> UIs { get { return m_uis; } }
        public int GetUILayer()
        {
            return this.Group.Layer;
        }

        public void TryLoadUI(int uiid, Action<UIBase> success, Action Failed)
        {
            var ui = GetUIInstance(uiid);
            ui.TryLoadFrame(success, Failed);
        }

        public void Push(int uiid)
        {
            if (m_uiStack.Count > 0 && m_uiStack.Peek() == uiid)
            {
                Debug.LogWarning("Push uiid 与当前UI 相同，忽略本次操作");
                return;
            }

            if (InUsing(uiid))
            {
                //在堆栈中
                m_uiStack.PullOut(uiid);
                ResortOrder();
            }

            UIBase topUI = GetTopUI();
            UIBase ui = GetUIInstance(uiid);
            ui.PrevUI = topUI;

            ui.SortingOrder = GetTopSortOrder();
            m_uiStack.Push(uiid);
            ui.SetActive(true);
#if UNITY_EDITOR
            m_uiStack.PrintStack();
#endif        
            TryLoadUI(uiid, OnPushLoadSuccess, OnPushLoadFailed);
        }

        private void OnPushLoadFailed()
        {
            //failed
            //m_locker = 0;
        }

        private void OnPushLoadSuccess(UIBase ui)
        {


        }

        private void AdjustVisibleOfBelowUI()
        {

        }

        public void Replace(int uiid)
        {
            m_replaceFlag = ReplaceFlag.None;
            _Replace(uiid);
        }

        public void _Replace(int uiid)
        {
            _Pop(false, true, true);
            Push(uiid);
        }

        private void OnReplaceLoadFailed()
        {

        }

        private enum ReplaceFlag
        {
            None,
            PopAll,
            DestroyAll,
        }

        private ReplaceFlag m_replaceFlag;

        public void PopAllAndReplace(int uiid)
        {
            bool bNeedAnim = true;
            while (m_uiStack.Count > 0)
            {
                var topUI = GetTopUI();
                _Pop(false, bNeedAnim);
                //bNeedAnim = topUI.IsFullScreen();
            }

            Push(uiid);
        }

        public void DestroyAllAndReplace(int uiid)
        {
            DestroyAll();
            Push(uiid);
        }

        public void Pop()
        {
            _Pop(false);

        }

        public void _Pop(bool destroy, bool anim = true, bool lockPrevAnim = false)
        {

        }

        public void PopAll()
        {
            bool bNeedAnim = true;
            while (m_uiStack.Count > 0)
            {
                var topUI = GetTopUI();
                _Pop(false, bNeedAnim);
            }
        }

        List<int> _deleteds = new List<int>();
        public void DestroyAll()
        {
            while (m_uiStack.Count > 0)
            {
                DestroyUI(m_uiStack.Peek());
            }
        }

        private void OnAnimClosed(object obj)
        {
            UIBase ui = obj as UIBase;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiid"></param>
        /// <param name="parent"></param>
        public void Dock(int uiid, Transform parent, UIBase dockerUi)
        {
            var ui = GetUIInstance(uiid);

        }



        private void OnDockLoaded(UIBase obj)
        {
            var uiid = obj.GetID();
            if (InUsing(uiid))
            {
                return;
            }
            var ui = GetUIInstance(uiid);
            if (ui != null)
            {
                ui.SetActive(true);
            }
        }

        public void HideUI(int uiid)
        {
            UIBase ui = null;
            m_uis.TryGetValue(uiid, out ui);
            if (ui != null)
            {
                ui.SetActive(false);
            }
        }
        public void ShowUI(int uiid)
        {
            UIBase ui = null;
            m_uis.TryGetValue(uiid, out ui);
            if (ui != null)
            {
                ui.SetActive(true);
            }
        }
        public void RemoveUIFromStack(int uiid)
        {
            if (InUsing(uiid))
            {

            }
        }


        public void DestroyUI(int uiid, bool useAnim = true)
        {

        }
        Dictionary<int, UIBase> m_needDestoyUis = new Dictionary<int, UIBase>();

        private void MarkDestroyUI(UIBase ui)
        {
            //Debug.LogFormat("================================= cs mark : {0}", ui.GetFramePrefabName());
            m_needDestoyUis[ui.GetID()] = ui;
        }

        public void OnUpdate(float dt)
        {

        }

        public void OnFixedUpdate(float dt)
        {

        }

        public void ResortOrder()
        {

        }

        public int GetTopSortOrder()
        {
            return 0;
        }

        public UIBase GetUIById(int uiid)
        {
            UIBase res;
            if (m_uis.TryGetValue(uiid, out res))
            {
                return res;
            }
            return null;
        }

        public object ContainsUIObjectLua(int uiid)
        {
            if (m_uis.ContainsKey(uiid) && m_uis[uiid].IsActive)
            {
                return true;
            }
            return false;
        }

        internal bool InUsing(int id)
        {
            return m_uiStack.Exists(id);
        }
    }
}
