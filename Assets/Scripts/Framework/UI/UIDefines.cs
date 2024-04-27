using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI
{
    
    public class UIGlobalDefines
    {
        public const int UI_2DTOP = 0;        
        public const int UI_2DBOTTOM = 1;
        public const int UI_3D = 2;

        public static string[] UILayerNames = new string[3]
        {
            "UI","UI_2DBOTTOM","UI_3D"
        };
    }
}
