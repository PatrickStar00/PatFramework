sing System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using MUF.Resource;
using UnityEngine.UI;
using System.Reflection;
using EffComponent;
using UI;

namespace UI
{
    // 所有菜单UI的基类
    public class UIBase
    {
        public object LuaTable { get; set; }
        public delegate bool BindLuaUICallback(UIBase ui, string uiname);
        public static BindLuaUICallback m_bindLuaCallback = null;
        public static void SetLuaBinder(BindLuaUICallback cb)
        {
            m_bindLuaCallback = cb;
        }

        public delegate void LuaInitDelegate(GameObject go, UIBase uics);
        private LuaInitDelegate m_luaInitDelegate;
        public void SetLuaInitCallback(LuaInitDelegate cb)
        {
            m_luaInitDelegate = cb;
        }
        public delegate void LuaAfterInitDelegate();
        private LuaAfterInitDelegate m_luaAfterInitDelegate;
        public void SetLuaAfterInitCallback(LuaAfterInitDelegate cb)
        {
            m_luaAfterInitDelegate = cb;
        }
        public delegate void LuaOnActiveDelegate();
        private LuaOnActiveDelegate m_luaOnActiveDelegate;
        public void SetLuaOnActiveCallback(LuaOnActiveDelegate cb)
        {
            m_luaOnActiveDelegate = cb;
        }

        public delegate void LuaBeforeActiveDelegate();
        private LuaBeforeActiveDelegate m_luaBeforeActiveDelegate;
        public void SetLuaBeforeActiveCallback(LuaBeforeActiveDelegate cb)
        {
            m_luaBeforeActiveDelegate = cb;
        }

        internal void ClearLuaDelegate()
        {
            m_luaInitDelegate = null;
            m_luaAfterInitDelegate = null;
            m_luaInitEventListenerDelegate = null;
            m_luaOnActiveDelegate = null;
            m_luaBeforeActiveDelegate = null;
            m_luaOnDestroyDelegate = null;
            m_luaOnFixedUpdateDelegate = null;
            m_luaIsFullScreenDelegate = null;
            m_luaOnInactiveDelegate = null;
            m_luaOnUpdateDelegate = null;
            m_luaRemoveEventListenerDelegate = null;

            RemoveAllControlCallback();
            //RebindAllControlCallback();
        }

        private void RemoveAllControlCallback()
        {
            UnbindButtonCallback();
            if (m_uiGameObject == null) return;
            var comps = m_uiGameObject.transform.GetComponentsInChildren<UnityEngine.Component>(true);
            foreach (var c in comps)
            {
                if (c is Clickable)
                {
                    (c as Clickable).RegisterCallback(null);
                }
                else if (c is InfinityGrid)
                {
                    (c as InfinityGrid).RegisterBindCallback(null);
                    (c as InfinityGrid).UnregisterButtonCallback();
                    (c as InfinityGrid).RegisterGetTemplateIndexFunc(null);
                }
                else if (c is InfinityPageList)
                {
                    (c as InfinityPageList).RegisterBindCallback(null);
                    (c as InfinityPageList).UnregisterButtonCallback();
                    (c as InfinityPageList).RegisterGetTemplateIndexFunc(null);
                }
            }
        }

        internal void RefreshFont(Font[] fonts)
        {
            if (fonts == null || fonts.Length <= 0 || m_uiGameObject == null) return;
            Text[] texts = m_uiGameObject.GetComponentsInChildren<Text>(true);
            foreach (var t in texts)
            {
                var index = 0;
                if (t is TextPro)
                {
                    index = (t as TextPro).FontIndex;
                }
                if (index >= fonts.Length)
                {
                    index = 0;
                }
                t.font = fonts[index];
            }
        }

        //internal void TryLoad(Action<int> callback)
        //{
        //    if (m_uiGameObject != null)
        //    {
        //        if (callback != null)
        //        {
        //            callback(m_transitionUiid);
        //        }
        //    }
        //    else
        //    {
        //        m_transtionCallback = callback;
        //        Load();
        //    }
        //}

        //internal void SetTransitionParams(int uiid)
        //{
        //    m_transitionUiid = uiid;
        //}

        public void BindCallback(Button btn, Action callback)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => { if (callback != null) callback(); });
        }
        public void UnBindCallback(Button btn)
        {
            btn.onClick.RemoveAllListeners();
        }

        internal void RebindLuaDelegate()
        {
            TryBindLua();
        }

        public delegate void LuaOnInactiveDelegate();
        private LuaOnInactiveDelegate m_luaOnInactiveDelegate;
        public void SetLuaOnInactiveCallback(LuaOnInactiveDelegate cb)
        {
            m_luaOnInactiveDelegate = cb;
        }

        public delegate void LuaOnDestroyDelegate();
        private LuaOnDestroyDelegate m_luaOnDestroyDelegate;
        public void SetLuaOnDestroyCallback(LuaOnDestroyDelegate cb)
        {
            m_luaOnDestroyDelegate = cb;
        }

        public delegate void LuaInitEventListenerDelegate();
        private LuaInitEventListenerDelegate m_luaInitEventListenerDelegate;
        public void SetLuaInitEventListenerCallback(LuaInitEventListenerDelegate cb)
        {
            m_luaInitEventListenerDelegate = cb;
        }

        public delegate void LuaRemoveEventListenerDelegate();
        private LuaRemoveEventListenerDelegate m_luaRemoveEventListenerDelegate;
        public void SetLuaRemoveEventListenerCallback(LuaRemoveEventListenerDelegate cb)
        {
            m_luaRemoveEventListenerDelegate = cb;
        }

        public delegate void LuaOnUpdateDelegate(float dt);
        private LuaOnUpdateDelegate m_luaOnUpdateDelegate;
        public void SetLuaOnUpdateCallback(LuaOnUpdateDelegate cb)
        {
            m_luaOnUpdateDelegate = cb;
        }

        public delegate void LuaOnFixedUpdateDelegate(float dt);
        private LuaOnFixedUpdateDelegate m_luaOnFixedUpdateDelegate;
        public void SetLuaOnFixedUpdateCallback(LuaOnFixedUpdateDelegate cb)
        {
            m_luaOnFixedUpdateDelegate = cb;
        }

        public delegate bool LuaIsFullScreenDelegate();
        private LuaIsFullScreenDelegate m_luaIsFullScreenDelegate;
        public void SetLuaIsFullScreenCallback(LuaIsFullScreenDelegate cb)
        {
            m_luaIsFullScreenDelegate = cb;
        }

        public static string AddressPattern = "UIs/Prefabs/{0}.prefab";
        private string m_address;
        public string Address
        {
            get
            {
                if (m_address == null || m_address == string.Empty)
                {
                    m_address = string.Format(AddressPattern, GetFramePrefabName());
                }
                return m_address;
            }
        }
        public virtual int GetID()
        {
            return 0;
        }

        public virtual bool IsFullScreen()
        {
            if (m_luaIsFullScreenDelegate != null) return m_luaIsFullScreenDelegate();
            return true;
        }

        public GameObject m_uiGameObject { get; set; }
        public AnimatorTransition AnimatorTranser { get; private set; }
        public Canvas RootCanvas { get; private set; }
        public CanvasGroup RootCanvasGroup { get; private set; }

        private bool m_isActive;
        public bool IsActive { get { return m_uiGameObject != null && m_isActive; } private set { m_isActive = value; } }
        public UIStack OwnerStack { get; set; }
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
        public bool Inited { get; private set; }
        public Transform Docker { get; internal set; }
        public UIBase DockerUI { get; internal set; }
        public bool IsDelayDestroyMe { get; set; }
        public bool ForbidonAnimation { get; internal set; }
        public bool m_bFullScreen { get; set; }

        public void GetOpenAnimation()
        {
        }
        // 设置使用的ui prefab资源
        public virtual string GetFramePrefabName()
        {
            return string.Empty;
        }
        public virtual void Init(GameObject gameObject)
        {
        }
        // 获得UI层
        public virtual int GetUILayer()
        {
            return UIGlobalDefines.UI_2DTOP;
        }

        // 加载 UI 
        //private void Load()
        //{
        //    ResourceManager.LoadAssetAsync(string.Intern(GetFramePrefabName()), ResourceType.UI,OnLoadCompleted);
        //}

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

            m_uiGameObject = UnityEngine.Object.Instantiate(go);
            m_uiGameObject.name = go.name;
            AnimatorTranser = m_uiGameObject.GetComponent<AnimatorTransition>();


            var uiLayer = OwnerStack.GetUILayer();
            var uiParentTransform = OwnerStack.Group.GetCameraTransform();//StaticGameObjects.Camera2DTopTransform;
            var camera = OwnerStack.Group.GetCamera();


            RootCanvas = m_uiGameObject.GetComponent<Canvas>();
            RootCanvas.renderMode = RenderMode.ScreenSpaceCamera;

            //var canvasScaler = m_uiGameObject.GetComponent<CanvasScaler>();
            //if (canvasScaler == null)
            //{
            //    canvasScaler = m_uiGameObject.AddComponent<CanvasScaler>();
            //}
            //canvasScaler.referenceResolution = new Vector2(1136, 640);
            //canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            //canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            //float aspecRatio = Screen.width / (float)Screen.height;
            //canvasScaler.matchWidthOrHeight = aspecRatio < 1.61 ? 0 : 1;

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

            //TryBindLua();
            Init(m_uiGameObject);

            
            RefreshMultiLanguage();

            //TryBindLua();
            //if (luaImpl != null)
            {
                if (m_luaInitDelegate != null)
                {
                    m_luaInitDelegate(m_uiGameObject, this);
                }
            }
            //else
            //{
            //    
            //}
            if (UISystem.Instance.GlobalFonts != null)
            {
                RefreshFont(UISystem.Instance.GlobalFonts);
            }

            RefreshSortingOrder();
            //Debug.Log("1");
            if (m_luaAfterInitDelegate != null)
                m_luaAfterInitDelegate();
            else AfterInit();

            Inited = true;
            IsDelayDestroyMe = false;

            ShowAllEffects(false);

            if (m_isActive)
            {
                Debug.LogFormat("set active true {0}", GetFramePrefabName());
                DoActive();
            }
            else
            {
                Debug.LogFormat("set active false {0}", GetFramePrefabName());
                DoInactive();
            }
        }

        public float TryRunCloseAnimation(string toForm, Action<object> callback)
        {
            ShowAllEffects(false);
            if (AnimatorTranser == null || !AnimatorTranser.isActiveAndEnabled)
            {
                if (callback != null) callback(this);
                return 0;
            }
            return AnimatorTranser.TryRunCloseAnimation(toForm, callback);
        }

        public float TryRunCoverdAnimation(string toForm, Action<object> callback)
        {
            if (AnimatorTranser == null || !AnimatorTranser.isActiveAndEnabled)
            {
                if (callback != null) callback(this);
                return 0;
            }
            return AnimatorTranser.TryRunCoverdAnimation(toForm, callback);
        }

        public void ManualRunOpenAnimation()
        {
            AnimatorTranser.ManualRunOpenAnimation();
        }

        public bool TryRunOpenAnimation(string formForm, float forceDelay, Action<object> callback)
        {
            if (AnimatorTranser == null || !AnimatorTranser.isActiveAndEnabled)
            {
                if (callback != null) callback(this);
                return false;
            }
            return AnimatorTranser.TryRunOpenAnimation(formForm, forceDelay, callback);
        }

        public bool TryRunUncoverdAnimation(string formForm, float forceDelay, Action<object> callback)
        {
            if (AnimatorTranser == null || !AnimatorTranser.isActiveAndEnabled)
            {
                if (callback != null) callback(this);
                return false;
            }
            return AnimatorTranser.TryRunUncoverdAnimation(formForm, forceDelay, callback);
        }


        internal void TryBindLua()
        {
            if (LuaTable != null)
                return;
            if (EngineConfig.UseLua)
            {
                if (m_bindLuaCallback != null)
                {
                    m_bindLuaCallback(this, GetFramePrefabName());
                }
            }
        }

        private void OnLoadedFailed(object obj)
        {
            ILog.Warn(MTString.GetString("UIBase.OnLoadedSuccess , ui 资源错误:", Address));
            return;
        }

        //private void OnLoadedSuccess(GameObject obj)
        //{
        //    if (Destroyed) return;
        //    var gotemp = obj;
        //    if (gotemp == null)
        //    {
        //        ILog.Warn(MTString.GetString("UIBase.OnLoadedSuccess , ui 资源错误:", Address));
        //        return;
        //    }

        //    //m_uiGameObject = UnityEngine.Object.Instantiate(go);
        //    m_uiGameObject = GameObject.Instantiate(gotemp);
        //    AnimatorTranser = m_uiGameObject.GetComponent<AnimatorTransition>();            


        //    m_uiGameObject.name = gotemp.name;

        //    var uiLayer = OwnerStack.GetUILayer();
        //    var uiParentTransform = OwnerStack.Group.GetCameraTransform();//StaticGameObjects.Camera2DTopTransform;
        //    var camera = OwnerStack.Group.GetCamera();


        //    RootCanvas = m_uiGameObject.GetComponent<Canvas>();
        //    RootCanvas.renderMode = RenderMode.ScreenSpaceCamera;

        //    RootCanvasGroup = m_uiGameObject.GetComponent<CanvasGroup>();
        //    if (RootCanvasGroup == null)
        //    {
        //        RootCanvasGroup = m_uiGameObject.AddComponent<CanvasGroup>();
        //    }

        //    m_uiGameObject.transform.SetParent(uiParentTransform);
        //    RootCanvas.worldCamera = camera;

        //    m_uiGameObject.transform.localScale = Vector3.one;
        //    m_uiGameObject.transform.localPosition = Vector3.zero;

        //    GameObjectUtils.SetAllChildrenLayer(m_uiGameObject, uiLayer);

        //    Init(m_uiGameObject);

        //    RefreshMultiLanguage();

        //    TryBindLua();
        //    //if (luaImpl != null)
        //    {
        //        if (m_luaInitDelegate != null)
        //        {
        //            m_luaInitDelegate(m_uiGameObject, this);
        //        }
        //    }
        //    //else
        //    //{
        //    //    
        //    //}
        //    if (UISystem.Instance.GlobalFont != null)
        //    {
        //        RefreshFont(UISystem.Instance.GlobalFont);
        //    }

        //    RefreshSortingOrder();
        //    //Debug.Log("1");
        //    if (m_luaAfterInitDelegate != null)
        //        m_luaAfterInitDelegate();
        //    else AfterInit();

        //    Inited = true;
        //    //Debug.Log("10");
        //    //m_uiGameObject.CheckSetActive(true);
        //    //this.OwnerStack.GetTopSortOrder();

        //    if (IsActive)
        //    {
        //        DoActive();
        //    }
        //    else
        //    {
        //        DoInactive();
        //    }

        //    //if (m_transtionCallback != null)
        //    //{
        //    //    m_transtionCallback(m_transitionUiid);
        //    //}

        //}

        List<AutoLayerOrder> m_autoLayers;
        public void RefreshSortingOrder()
        {
            if (m_uiGameObject == null) return;
            var baseOrder = RootCanvas.sortingOrder;
            //if (m_autoLayers == null)
            {
                var alos = m_uiGameObject.transform.GetComponentsInChildren<AutoLayerOrder>(true);
                m_autoLayers = new List<AutoLayerOrder>(alos);
            }
            foreach (var alo in m_autoLayers)
            {
                RefreshSortingOrderOfGameObject(alo, baseOrder);
                //Component.Destroy(alo);
            }
        }

        //单主线程环境下重复利用数组，减少GC
        private readonly static List<int> SortingOrders = new List<int>();
        private readonly static List<Renderer> SortingRenderers = new List<Renderer>();

        private void RefreshSortingOrderOfGameObject(AutoLayerOrder alo, int baseOrder)
        {
            var canvas = alo.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.pixelPerfect = true;
                canvas.overrideSorting = true;
                canvas.sortingOrder = baseOrder + alo.OrderIndex;
            }

            var pars = alo.GetComponentsInChildren<ParticleSystem>(true);

            SortingRenderers.Clear();
            SortingOrders.Clear();
            foreach (var p in pars)
            {
                Renderer render = p.GetComponent<Renderer>();
                if (null != render)
                {
                    SortingRenderers.Add(render);
                    if (!SortingOrders.Contains(render.sortingOrder))
                        SortingOrders.Add(render.sortingOrder);
                }
            }

            SortingOrders.Sort();
            foreach (var p in pars)
            {
                Renderer render = p.GetComponent<Renderer>();
                if (null != render)
                {
                    render.sortingOrder = baseOrder + alo.OrderIndex + SortingOrders.IndexOf(render.sortingOrder);
                }
            }

            var mesh = alo.GetComponentsInChildren<MeshRenderer>(true);

            SortingRenderers.Clear();
            SortingOrders.Clear();
            foreach (var p in mesh)
            {
                Renderer render = p.GetComponent<Renderer>();
                if (null != render)
                {
                    SortingRenderers.Add(render);
                    if (!SortingOrders.Contains(render.sortingOrder))
                        SortingOrders.Add(render.sortingOrder);
                }
            }
            SortingOrders.Sort();
            foreach (var p in mesh)
            {
                Renderer render = p.GetComponent<Renderer>();
                if (null != render)
                {
                    render.sortingOrder = baseOrder + alo.OrderIndex + SortingOrders.IndexOf(render.sortingOrder);
                }
            }

            SortingRenderers.Clear();
            SortingOrders.Clear();
        }
        
        public void AddEffectByBaseOrder(GameObject effect, Transform parent, int layerOrder)
        {
            if (m_autoLayers == null)
            {
                m_autoLayers = new List<AutoLayerOrder>();
            }
            effect.transform.SetParent(parent);
            var alo = effect.AddComponent<AutoLayerOrder>();
            alo.OrderIndex = layerOrder;
            m_autoLayers.Add(alo);
            var baseOrder = RootCanvas.sortingOrder;
            RefreshSortingOrderOfGameObject(alo, baseOrder);
        }

        // 刷新多語言
        private void RefreshMultiLanguage()
        {
            if (m_uiGameObject == null) return;
            Text[] txts = m_uiGameObject.GetComponentsInChildren<Text>(true);
            foreach (var t in txts)
            {
                if (t.name.Length > 2 && ((t.name.Substring(0, 2) == "m_") && (t.name.Substring(0, 3) != "m_z"))) continue;
                string keyName = MTString.GetString(m_uiGameObject.name, "_", t.name);
                var mt = MultiLanguageManager.Instance.GetUILanguage(keyName);
                if (mt != null) t.text = mt.Replace("|", "\n");
            }
        }

        public void ResetLua()
        {
            LuaTable = null;
            m_luaInitDelegate = null;
            m_luaAfterInitDelegate = null;
            m_luaInitEventListenerDelegate = null;
            m_luaRemoveEventListenerDelegate = null;
            m_luaBeforeActiveDelegate = null;
            m_luaOnActiveDelegate = null;
            m_luaOnDestroyDelegate = null;
            m_luaOnFixedUpdateDelegate = null;
            m_luaIsFullScreenDelegate = null;
            m_luaOnInactiveDelegate = null;
            m_luaOnUpdateDelegate = null;
        }
        protected virtual void AfterInit()
        {
        }

        public virtual void OnInactive()
        {
        }
        public virtual void BeforeActive()
        {
        }
        public virtual void OnActive()
        {
        }
        public virtual void OnDestroy()
        {
        }
        internal void DoDestroy()
        {
            Inited = false;
            _inLoading = false;
            m_dockedUIs.Clear();

            if (m_luaOnDestroyDelegate != null)
                m_luaOnDestroyDelegate();
            else
                OnDestroy();

            if (LuaTable != null)
                LuaTable = null;
            m_luaOnFixedUpdateDelegate = null;
            m_luaIsFullScreenDelegate = null;
            m_luaOnUpdateDelegate = null;
            m_luaRemoveEventListenerDelegate = null;
            m_luaInitEventListenerDelegate = null;
            m_luaOnDestroyDelegate = null;
            m_luaOnInactiveDelegate = null;

            if (m_uiGameObject != null)
            {
                SetActive(false);
                RemoveAllControlCallback();
                //MonoMethodUtil.StartCoroutine(DestroyAssets());
                GameObject.Destroy(m_uiGameObject);
                ResourceManager.UnloadAsset(string.Intern(GetFramePrefabName()), ResourceType.UI);
                m_uiGameObject = null;
                Destroyed = true;
            }
        }

        private WaitForSeconds _wait05 = new WaitForSeconds(0.5f);
        private IEnumerator DestroyAssets()
        {
            yield return _wait05;
            GameObject.Destroy(m_uiGameObject);
            m_uiGameObject = null;
            yield return null;
            ResourceManager.UnloadAsset(string.Intern(GetFramePrefabName()), ResourceType.UI);
        }
        private void DoInactive(bool isDockableRefresh = false)
        {

            foreach (var p in m_dockedUIs)
            {
                p.Value.DoInactive(true);
            }
            if (!isDockableRefresh)
            {
                //RootCanvasGroup.alpha = 0.0f;
                //RootCanvasGroup.blocksRaycasts = false;
                //RootCanvasGroup.interactable = false;
                m_uiGameObject.CheckSetActive(false);
            }
            //m_uiGameObject.transform.localScale = Vector3.zero;

            if (m_luaOnInactiveDelegate != null) m_luaOnInactiveDelegate();
            else OnInactive();

            ShowAllEffects(false);
            
            if (m_luaRemoveEventListenerDelegate != null) m_luaRemoveEventListenerDelegate();
            else RemoveEventListener();
        }

        private Dictionary<int, UIBase> m_dockedUIs = new Dictionary<int, UIBase>();
        private Vector2 m_oriSize;
        private bool _inLoading;

        //private int m_transitionUiid;
        //private Action<int> m_transtionCallback;

        private void DoActive(bool isDockableRefresh = false)
        {
            foreach (var p in m_dockedUIs)
            {
                p.Value.DoActive(true);
            }
            if (Docker == null)
            {
                //foreach (var p in OwnerStack.TransDelayCloseUIs)
                //{
                //    p.Value.SetActive(false);
                //}
                //OwnerStack.TransDelayCloseUIs.Clear();
                RootCanvas.sortingOrder = SortingOrder;
            }
            else
            {
                m_uiGameObject.transform.SetParent(Docker);
                var rt = m_uiGameObject.GetComponent<RectTransform>();
                var tgtSize = m_uiGameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
                rt.sizeDelta = tgtSize;
                var size = Docker.GetComponent<RectTransform>().sizeDelta;
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.localPosition = Vector3.zero;
                rt.localScale = new Vector3(size.x / tgtSize.x, size.y / tgtSize.y, 1);

                if (!DockerUI.m_dockedUIs.ContainsKey(GetID()))
                {
                    DockerUI.m_dockedUIs[GetID()] = this;
                }
            }

            if (!isDockableRefresh)
            {
                m_uiGameObject.CheckSetActive(true);
            }

            RefreshSortingOrder();

            if (m_luaBeforeActiveDelegate != null)
            {
                m_luaBeforeActiveDelegate();
            }
            else
            {
                BeforeActive();
            }

            if (m_luaOnActiveDelegate != null)
                m_luaOnActiveDelegate();
            else
                OnActive();

            if (m_luaInitEventListenerDelegate != null)
                m_luaInitEventListenerDelegate();
            else
                InitEventListener();
        }
        //void OnLoadCompleted(LoadingResult result)
        //{
        //if (result.Status == ELoadingStatus.Successed)
        //{
        //    OnLoadedSuccess(result.Result as GameObject);
        //}
        //else
        //{
        //    OnLoadedFailed(result.Result as string);
        //}
        //}

        private static readonly List<ParticleSysMgr> m_particleSysMgrs = new List<ParticleSysMgr>();

        public void ShowAllEffects(bool show)
        {
            if (m_uiGameObject == null)
            {
                return;
            }

            m_particleSysMgrs.Clear();
            m_uiGameObject.GetComponentsInChildren<ParticleSysMgr>(true, m_particleSysMgrs);
            if (show)
            {
                foreach (var comp in m_particleSysMgrs)
                {
                    if (comp.gameObject.layer == LayerMask.NameToLayer("Hide_Effect"))
                        GameObjectUtils.SetAllChildrenLayer(comp.gameObject, LayerMask.NameToLayer("UI_3D"));
                }
            }
            else
            {
                foreach (var comp in m_particleSysMgrs)
                {
                    GameObjectUtils.SetAllChildrenLayer(comp.gameObject, LayerMask.NameToLayer("Hide_Effect"));
                }
            }
            m_particleSysMgrs.Clear();
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
                
                if (!_inLoading)
                {
                    _inLoading = true;
                    int iLockerId = UILocker.Instance.Lock(3.0f);
                    ResourceManager.LoadAssetAsync(string.Intern(GetFramePrefabName()), ResourceType.UI, (result) =>
                    {
                        UILocker.Instance.Unlock(iLockerId);
                        if (Destroyed) return;
                        if (result.Status == ELoadingStatus.Successed)
                        {
                            //OnLoadedSuccess(result.Result as GameObject);
                            OnLoadInit(result.Result as GameObject);
                            if (success != null) success(this);
                            _inLoading = false;
                        }
                        else
                        {
                            Debug.LogError("UIBase.OnLoadedSuccess , ui 资源错误:" + Address);
                            if (failed != null) failed();
                            _inLoading = false;
                            return;
                        }
                    });
                }

            }
        }

        public void SetActive(bool active)
        {
            //if (active && OwnerStack.TransDelayCloseUIs.ContainsKey(GetID()))
            //{
            //    OwnerStack.TransDelayCloseUIs.Remove(GetID());
            //}

            if (active == m_isActive)
            {
                return;
            }

            m_isActive = active;
            if (active)
            {

                //if (m_uiGameObject == null)
                //{
                //    //Load();
                //    return;
                //}
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

        public virtual void InitEventListener()
        {

        }

        public virtual void RemoveEventListener()
        {
        }

        internal void DoUpdate(float dt)
        {
            if (m_luaOnUpdateDelegate != null)
                m_luaOnUpdateDelegate(dt);
            else
                OnUpdate(dt);

            foreach (var p in m_dockedUIs)
            {
                p.Value.DoUpdate(dt);
            }
        }

        internal void DoFixedUpdate(float dt)
        {

            if (m_luaOnFixedUpdateDelegate != null) m_luaOnFixedUpdateDelegate(dt);
            else OnFixedUpdate(dt);

            foreach (var p in m_dockedUIs)
            {
                p.Value.DoFixedUpdate(dt);
            }
        }
        public virtual void OnUpdate(float dt)
        {
        }
        public virtual void OnFixedUpdate(float dt)
        {

        }
        public void DestroyMe(bool useAnim = true)
        {
            foreach (var p in m_dockedUIs)
            {
                OwnerStack.DestroyUI(p.Value.GetID());
            }
            m_dockedUIs.Clear();
            //Debug.LogFormat("================================= cs destroy : {0}", GetFramePrefabName());
            OwnerStack.DestroyUI(this.GetID() , useAnim);
        }

        //public void DesroyMeAfterAnimationPlayOver()
        //{
        //    foreach (var p in m_dockedUIs)
        //    {
        //        p.Value.IsDelayDestroyMe = true;
        //        OwnerStack.DelayDestroyUIAsset(p.Value.GetID());
        //    }
        //    m_dockedUIs.Clear();
        //    IsDelayDestroyMe = true;
        //    OwnerStack.DelayDestroyUIAsset(this.GetID());
        //}

        public string GetText(string id)
        {
            return "UIBase.GetText NEED IMPL";// TableDataReadHelper.GetText(id);
        }
        public virtual void OnDefaultBack()
        {
            var eventName = UI.UISoundCenter.GetSfxEventName(GetNodeID("m_btnBack"));
            if (!string.IsNullOrEmpty(eventName))
            {
                PlaySfx(eventName);
            }
            OwnerStack.Pop();
        }
        public string GetNodeID(string name)
        {
            return string.Format("{0}_{1}", GetFramePrefabName(), name);
        }
        public static void PlaySfx(string eventName)
        {
        }
        public static void PlaySfxByID(string id)
        {
            var eventName = UI.UISoundCenter.GetSfxEventName(id);
            if (!string.IsNullOrEmpty(eventName))
            {
                PlaySfx(eventName);
            }
        }
        public void TryBindButtonCallback(Button btn, string name)
        {
            btn.onClick.RemoveAllListeners();
            var mi = this.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (mi != null)
            {
                var eventName = UI.UISoundCenter.GetSfxEventName(GetNodeID(btn.name));
                if (!string.IsNullOrEmpty(eventName))
                {
                    btn.onClick.AddListener(() => { PlaySfx(eventName); });
                }
                var _delegate = System.Delegate.CreateDelegate(typeof(UnityEngine.Events.UnityAction), this, mi) as UnityEngine.Events.UnityAction;
                if (_delegate != null)
                    btn.onClick.AddListener(_delegate);
            }
        }

        public void TryRegisterClickableCallback(Clickable btn, string name)
        {
            var mi = this.GetType().GetMethod(name);
            if (mi != null)
            {
                var _delegate = System.Delegate.CreateDelegate(typeof(System.Action), this, mi) as System.Action;
                if (_delegate != null)
                    btn.RegisterCallback(_delegate);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiid"></param>
        /// <param name="closeCurrent"></param>
        public void Push(int uiid)
        {
            OwnerStack.Push(uiid);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiid"></param>
        /// <param name="closeCurrent"></param>
        public void Replace(int uiid)
        {
            OwnerStack.Replace(uiid);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Pop()
        {
            OwnerStack.Pop();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void UnbindButtonCallback()
        {

            var go = m_uiGameObject;
            var formName = GetFramePrefabName();
            if (go == null)
            {
                Debug.LogWarningFormat("UnbindButtonCallback error ! ui NOT loaded : {0}", formName);
                return;
            }

            var btns = go.GetComponentsInChildren<Button>();
            foreach (var btn in btns)
            {
                btn.onClick.RemoveAllListeners();
            }
        }
        public void Dock(int uiid, Transform parent)
        {
            if (parent != null)
            {
                foreach (var p in m_dockedUIs)
                {
                    p.Value.SetActive(false);
                }

                if (m_dockedUIs.ContainsKey(uiid))
                {
                    m_dockedUIs[uiid].SetActive(true);
                    return;
                }
                OwnerStack.Dock(uiid, parent, this);
            }
        }

        public UI.SpriteCamp GetSpriteCamp()
        {
            if (m_uiGameObject != null)
            {
                return m_uiGameObject.GetComponent<UI.SpriteCamp>();
            }
            return null;
        }
    }
}
