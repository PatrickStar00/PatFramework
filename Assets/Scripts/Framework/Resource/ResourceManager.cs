/********************************************************************************* 
  *Description: 资源管理器，对外唯一核心类
**********************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

#pragma warning disable 0414
namespace MUF.Resource
{
    public enum ELoadingStatus
    {
        Failed,
        Successed,
        Canceled,       //TODO
        Loading,        //正在加载
    }

    public struct LoadingResult
    {

    }


    public static class ResourceManager
    {

    }
}