using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace UI
{

    public class UISystem //: FrameSingleton<UISystem> //, IUpdater, IFixedUpdater, ILateUpdater//: MonoSingleton<UISystem>
    {

        private static UISystem _instance;
        public static UISystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UISystem();
                    _instance.Init();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IFormCreater FormCreater { get; set; }
        public Font[] GlobalFonts { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        internal Dictionary<int, UIGroup> m_groups = new Dictionary<int, UIGroup>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iDepth"></param>
        /// <param name="iCullingLayer"></param>
        /// <returns></returns>
        public UIGroup CreateGroup(int iDepth, int iCullingLayer)
        {
            var group = UIGroup.Create(iDepth, iCullingLayer);
            m_groups[iDepth] = group;
            return group;
        }

        public void Init()
        {

        }

        public void ViewUpdate(float dt)
        {
            foreach (var g in m_groups)
            {
                foreach (var s in g.Value.Stacks)
                {
                    s.Value.OnUpdate(dt);
                }
            }

            UILocker.Instance.Update();
        }

    }
}
