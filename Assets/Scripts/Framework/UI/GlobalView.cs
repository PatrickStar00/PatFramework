using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI;
using UnityEngine;

namespace UI
{
    public class GlobalView
    {
        internal static float PaddingLeft;
        internal static float PaddingRight;
        internal static float PaddingBottom;
        internal static float PaddingTop;
        

        public static void SetViewPadding(float left , float right , float top , float bottom)
        {
            //var cameras = GameObject.FindObjectsOfType<Camera>();
            PaddingLeft = left;
            PaddingRight = right;
            PaddingBottom = top;
            PaddingTop = bottom;

            //var realTop = 1.0f - top;
            //var realRight = 1.0f - right;
            foreach (var g in UISystem.Instance.m_groups)
            {
                var c = g.Value.m_camera;
                SetCameraView(c);
            }
        }

        public static void SetCameraView(Camera c)
        {
            var left = PaddingLeft;
            var right = PaddingRight;
            var bottom = PaddingBottom;
            var top = PaddingTop;

            bool bPortrait = Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown;

            float w = Screen.width;
            float h = Screen.height;

            if (bPortrait)
            {
                w = Screen.height;
                h = Screen.width;
            }
            var realBottom = h - PaddingBottom;
            var realRight = w - right;

            
            var rect = new Rect(left / w, top / h, (realRight - left) / w, (realBottom - top) / h);
            c.rect = rect;
        }
    }


}
