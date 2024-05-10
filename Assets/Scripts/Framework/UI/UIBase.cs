using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;
using System.Reflection;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
        // public AnimatorTransition AnimatorTranser { get; private set; }
        public Canvas RootCanvas { get; private set; }
        public CanvasGroup RootCanvasGroup { get; private set; }

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
        public UIBase PrevUI { get; internal set; }
        public bool Destroyed { get; internal set; }

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
                
                AsyncOperationHandle<GameObject> textureHandle = Addressables.InstantiateAsync(string.Intern(GetFramePrefabName()));;
                textureHandle.Completed += (AsyncOperationHandle<GameObject> handle) =>
                {
                    UILocker.Instance.Unlock(iLockerId);
                    if (Destroyed) return;
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        OnLoadInit(handle.Result);
                    }
                    else
                    {
                        Debug.LogError("UIBase.OnLoadedSuccess , ui 资源错误:" + string.Intern(GetFramePrefabName()));
                        if (failed != null) failed();
                        return;
                    }
                };

                // OnLoadInit(result.Result as GameObject);

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

        public virtual void Init(GameObject gameObject)
        {
        }

        private void DoActive(bool isDockableRefresh = false)
        {
        }

        private void DoInactive(bool isDockableRefresh = false)
        {

        }

        private void OnLoadInit(GameObject asset)
        {
            if (Destroyed) return;
            var go = asset;
            if (go == null)
            {
                return;
            }
            // 屏幕适配
            var canvasScaler = go.GetComponent<CanvasScaler>();
            if (canvasScaler == null)
            {
                canvasScaler = go.AddComponent<CanvasScaler>();
            }
            canvasScaler.referenceResolution = new Vector2(1136, 640);
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            float aspecRatio = Screen.width / (float)Screen.height;
            canvasScaler.matchWidthOrHeight = aspecRatio < 1.7 ? 0 : 1;

            m_uiGameObject = go;
            // m_uiGameObject = UnityEngine.Object.Instantiate(go);
             m_uiGameObject.name = go.name;
            // AnimatorTranser = m_uiGameObject.GetComponent<AnimatorTransition>();

            var uiLayer = OwnerStack.GetUILayer();
            var uiParentTransform = OwnerStack.Group.GetCameraTransform();
            var camera = OwnerStack.Group.GetCamera();


            RootCanvas = m_uiGameObject.GetComponent<Canvas>();
            RootCanvas.renderMode = RenderMode.ScreenSpaceCamera;

            RootCanvasGroup = m_uiGameObject.GetComponent<CanvasGroup>();
            if (RootCanvasGroup == null)
            {
                RootCanvasGroup = m_uiGameObject.AddComponent<CanvasGroup>();
            }

            m_uiGameObject.transform.SetParent(uiParentTransform);
            RootCanvas.worldCamera = camera;

            m_uiGameObject.transform.localScale = Vector3.one;
            m_uiGameObject.transform.localPosition = Vector3.zero;

            GameObjectUtils.SetAllChildrenLayer(m_uiGameObject, uiLayer);

            Init(m_uiGameObject);


            // RefreshMultiLanguage();
            //     if (m_luaInitDelegate != null)
            //     {
            //         m_luaInitDelegate(m_uiGameObject, this);
            //     }
            // }

            // if (UISystem.Instance.GlobalFonts != null)
            // {
            //     RefreshFont(UISystem.Instance.GlobalFonts);
            // }

            // RefreshSortingOrder();
            // if (m_luaAfterInitDelegate != null)
            //     m_luaAfterInitDelegate();
            // else AfterInit();

            // Inited = true;
            // IsDelayDestroyMe = false;

            // ShowAllEffects(false);

            // if (m_isActive)
            // {
            //     Debug.LogFormat("set active true {0}", GetFramePrefabName());
            //     DoActive();
            // }
            // else
            // {
            //     Debug.LogFormat("set active false {0}", GetFramePrefabName());
            //     DoInactive();
            // }
        }

    }
}
