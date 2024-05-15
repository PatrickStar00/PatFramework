using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;


//public class AutoLayerOrderDesc : UnityComponentDataDesc<AutoLayerOrder>
//{
//    public int OrderIndex = 1;
//    public override void InitComponent(AutoLayerOrder component)
//    {

//        component.OrderIndex = OrderIndex;
//    }
//}
[AddComponentMenu("UI/Auto Layer Order")]
public class AutoLayerOrder : MonoBehaviour
{
    public int OrderIndex = 1;
#if UNITY_EDITOR

    public int GetRootOrder(Transform t)
    {
        if (t == null) return 0;
        var canvas = t.GetComponent<Canvas>();
        if (canvas != null)
            return canvas.sortingOrder;

        return GetRootOrder(t.parent);
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            RefreshSortingOrderOfGameObject(this, GetRootOrder(transform.parent));
        }
    }

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
        int minOrder = int.MaxValue;
        foreach (var p in pars)
        {
            Renderer render = p.GetComponent<Renderer>();
            if (null != render)
            {
                minOrder = Math.Min(minOrder, render.sortingOrder);
            }

        }
        foreach (var p in pars)
        {
            Renderer render = p.GetComponent<Renderer>();
            if (null != render)
            {
                render.sortingOrder = render.sortingOrder - minOrder + baseOrder + alo.OrderIndex;
            }
        }

        var mesh = alo.GetComponentsInChildren<MeshRenderer>(true);
        minOrder = int.MaxValue;
        foreach (var p in mesh)
        {
            Renderer render = p.GetComponent<Renderer>();
            if (null != render)
            {
                minOrder = Math.Min(minOrder, render.sortingOrder);
            }
        }
        foreach (var p in mesh)
        {
            Renderer render = p.GetComponent<Renderer>();
            if (null != render)
            {
                render.sortingOrder = render.sortingOrder - minOrder + baseOrder + alo.OrderIndex;
            }
        }
    }
#endif
}

