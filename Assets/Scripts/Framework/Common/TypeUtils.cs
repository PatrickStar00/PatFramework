using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace PAT.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class TypeIdBase
    {
        protected static int next_index = 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypeIdOf<T> : TypeIdBase
    {
        public readonly static int value = TypeIdBase.next_index++;
    }

    public class PluginTypeUtils
    {
        public static System.Reflection.Assembly ScriptAssembly;

        private static Dictionary<string, Type> s_types = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static System.Type FindType(string typeName)
        {

            if (s_types == null)
            {
                s_types = new Dictionary<string, Type>();
                var assembly = ScriptAssembly;
                var types = assembly.GetExportedTypes();
                foreach (var t in types)
                {
                    if (s_types.ContainsKey(t.Name))
                    {
                        Debug.LogError("重复的类型名" + t.Name);
                        continue;
                    }
                    s_types[t.Name] = t;
                }

                types = typeof(ComponentHolder).Assembly.GetExportedTypes();
                foreach (var t in types)
                {
                    if (s_types.ContainsKey(t.Name))
                    {
                        Debug.LogError("重复的类型名" + t.Name);
                        continue;
                    }
                    s_types[t.Name] = t;
                }
            }

            var type = s_types[typeName];
            return type;

            //var assembly = ScriptAssembly;
            //var types = assembly.GetExportedTypes();
            //foreach (var t in types)
            //{
            //    if (t.Name == typeName)
            //    {
            //        return t;
            //    }
            //}

            //types = typeof(ComponentHolder).Assembly.GetExportedTypes();
            //foreach (var t in types)
            //{
            //    if (t.Name == typeName)
            //    {
            //        return t;
            //    }
            //}

            //return null;

        }
    }
}
