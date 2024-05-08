using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
public partial class Window_MainMenu : UIBase
{
    public Canvas m_canvas = null;
    public GameObject m_ShowInfoPanel = null;
    public GameObject m_InfoList = null;

	public override int GetID() { return UIDefines.ID_WINDOWS_TEST; }
	public override string GetFramePrefabName() { return "Window_MainMenu"; }
}
