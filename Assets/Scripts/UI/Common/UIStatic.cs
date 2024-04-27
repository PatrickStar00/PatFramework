using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace UI
{   
    public class UIStatic
    {
        static UIGroup m_mainGroup;

        public static UIStack StackMain { get; private set; }
        public static UIStack StackFlow { get; private set; }
        public static UIStack StackPopup { get; private set; }
        public static UIStack StackTop { get; private set; }

        public static void InitUI()
        {
            //创建UI group
            if (m_mainGroup == null)
            {
                m_mainGroup = UISystem.Instance.CreateGroup(32, UIGlobalDefines.UI_2DTOP);
                //主stack
                StackMain = m_mainGroup.CreateStack(1000);
                StackFlow = m_mainGroup.CreateStack(2000);
                StackPopup = m_mainGroup.CreateStack(3000);
                StackTop = m_mainGroup.CreateStack(9999);
            }

        }
    }
}
