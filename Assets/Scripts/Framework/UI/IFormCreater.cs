using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI
{
    public interface IFormCreater
    {
        UI.UIBase CreateForm(int formId);
    }
}
