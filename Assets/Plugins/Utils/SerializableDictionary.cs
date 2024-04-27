using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField]
        List<TKey> keys;
        [SerializeField]
        List<TValue> values;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TValue this[TKey index]
        {
            get
            {
                return target[index];
            }
            set
            {
                target[index] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key , out TValue v)
        {            
            return target.TryGetValue(key, out v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return target.ContainsKey(key);
        }

        protected Dictionary<TKey, TValue> target;
        public Dictionary<TKey, TValue> ToDictionary() { return target; }

        public SerializableDictionary()
        {
            //        this.target = target;
            this.target = new Dictionary<TKey, TValue>();
        }

        public void OnBeforeSerialize()
        {
            keys = new List<TKey>(target.Keys);
            values = new List<TValue>(target.Values);
        }

        public void OnAfterDeserialize()
        {
            var count = Math.Min(keys.Count, values.Count);
            target = new Dictionary<TKey, TValue>(count);
            for (var i = 0; i < count; ++i)
            {
                target.Add(keys[i], values[i]);
            }
        }
    }
