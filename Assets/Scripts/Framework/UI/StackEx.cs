using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI
{
    public class StackEx<T> : IEnumerable<T>
    {
        LinkedList<T> list = new LinkedList<T>();

        public int Count { get { return list.Count; } }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            if (list.Last == null)
            {
                ILog.Warn("stack is empty");
                return default(T);
            }

            return list.Last.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiid"></param>
        public void Push(T uiid)
        {
            list.AddLast(uiid);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Pop()
        {
            if (list.Last != null)
            {
                list.RemoveLast();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Exists(T value)
        {
            return list.Find(value) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public void PullOut(T v)
        {
            list.Remove(v);
        }

        public bool GetNext(T v , out T next)
        {
            next = default(T);
            for (int i = 0; i < list.Count; i++)
            {
                if (list.ElementAt(i).Equals(v))
                {
                    if (i+1 < list.Count)
                    {
                        next = list.ElementAt(i + 1);
                        return true;
                    }
                }
            }            
            return false;
        }

#if UNITY_EDITOR
        public void PrintStack()
        {
            string stack = "[";
            foreach(var t in list)
            {
                stack += t.ToString() + ",";
            }
            stack += "]";
            UnityEngine.Debug.LogFormat("===== ui stack : {0}", stack);
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < list.Count; i++)
            {
                yield return list.ElementAt(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < list.Count; i++)
            {
                yield return list.ElementAt(i);
            }
        }
    }
}
