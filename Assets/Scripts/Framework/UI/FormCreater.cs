using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI
{
    public class FormCreater : Singleton<FormCreater>, UI.IFormCreater
    {
        public UIBase CreateForm(int formId)
        {
            switch (formId)
            {
                case UIDefines.ID_WINDOWS_TEST:
                    return new Window_MainMenu();
            }
            return new UIBase();
        }
    }
}
