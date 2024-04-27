using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    
    public class UIGroup
    {
        private GameObject m_cameraGO;
        internal Camera m_camera;
        private Transform m_cameraTransform;

        public int Depth { get; set; }
        public int Layer { get; private set; }
        //public static EventSystem UIEventSystem { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iDepth"></param>
        /// <param name="iCullingLayer"></param>
        /// <returns></returns>
        public static UIGroup Create(int iDepth , int iCullingLayer)
        {
            Debug.Assert(iCullingLayer >= 0 && iCullingLayer < UIGlobalDefines.UILayerNames.Length, "UIGroup Creat , iCullingLayer out of range");
            var group = new UIGroup();

            GameObject go = GameObject.Find("UIRoot");

            if (go == null)
            {
                go = new GameObject("UIRoot");
                var monogo = StaticGameObjects.GetMonoRoot();
                go.transform.SetParent(monogo.transform);

//                 if(GameServerConfig.Instance.m_bAdjustSandBox == true)
//                     go.AddComponent<HUDFPS>();
// #if GAME_REPORT
//                 go.AddComponent<UIReport>();
// #endif
                go.ResetTransform();
            }

            var name = "UI_" + iDepth;
            GameObject uigo = GameObject.Find(name);
            if (uigo == null)
            {
                uigo = new GameObject(name);

                uigo.transform.SetParent(go.transform);
                uigo.ResetTransform();
            }

            group.m_camera = uigo.GetComponent<Camera>();
            if (group.m_camera == null)
            {
                group.m_camera = uigo.AddComponent<Camera>();
            }
            group.m_cameraTransform = uigo.transform;
            group.m_camera.depth = iDepth;
            
            group.m_camera.orthographic = true;
            group.m_camera.allowHDR = false;
            group.m_camera.allowMSAA = false;
            var layerName = UIGlobalDefines.UILayerNames[iCullingLayer];
            var mask = LayerMask.GetMask(layerName);
            group.Layer = LayerMask.NameToLayer(layerName);
            group.m_camera.cullingMask = mask;
            group.m_camera.clearFlags = CameraClearFlags.Depth;
            group.m_camera.backgroundColor = Color.black;

            GlobalView.SetCameraView(group.m_camera);

            //var eventSystem = GameObject.Find("EventSystem");
            if (EventSystem.current == null)
            {
                var eventSystem = new GameObject("EventSystem");

                eventSystem.transform.SetParent(go.transform);
                eventSystem.ResetTransform();
                if(eventSystem.GetComponent<EventSystem>() == false)
                   eventSystem.AddComponent<EventSystem>();
                if (eventSystem.GetComponent<StandaloneInputModule>() == false)
                    eventSystem.AddComponent<StandaloneInputModule>();
                if (eventSystem.GetComponent<BaseInput>() == false)
                    eventSystem.AddComponent<BaseInput>();
                EventSystem.current = eventSystem.AddComponent<EventSystem>();
            }
            
            return group;
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, UIStack> Stacks = new Dictionary<int, UIStack>();
        /// <summary>
        /// 
        /// </summary>
        public UIStack CreateStack(int prioStart)
        {
            var newStack = new UIStack(prioStart , this);
            if (Stacks.ContainsKey(prioStart))
            {
                ILog.Warn(string.Format("stack {0} already exists", prioStart));
                return null;
            }
            Stacks[prioStart] = newStack;
            return newStack;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prio"></param>
        /// <returns></returns>
        public UIStack GetStackByPrioStart(int prio)
        {
            UIStack res = null;
            if (Stacks.TryGetValue(prio , out res))
            {
                return res;
            }

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Camera GetCamera()
        {
            return m_camera;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Transform GetCameraTransform()
        {
            return m_cameraTransform;
        }
    }
}
