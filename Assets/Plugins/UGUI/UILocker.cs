using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UILocker
    {
        private static UILocker _instance;
        public static UILocker Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = GameObject.Find("MONOGameObject");

                    _instance = new UILocker();
                    _instance.Awake();
                }
                return _instance;

            }
        }
        private Dictionary<int, float> m_lockers = new Dictionary<int, float>();
        private static int s_lockerId = 1000;
        private GameObject s_eventSystemObject;

        public int Lock(float time)
        {
            var id = ++s_lockerId;
            m_lockers[id] = Time.realtimeSinceStartup + time;

            UpdateLockerState();
            return id;
        }

        public void Unlock(int lockerId)
        {
            if (m_lockers.ContainsKey(lockerId))
            {
                m_lockers.Remove(lockerId);
            }
            UpdateLockerState();

        }

        public bool IsValidLocker(int iLockerId)
        {
            return m_lockers.ContainsKey(iLockerId);
        }

        public void Update()
        {
            UpdateLockerState();
        }

        private void Awake()
        {
            if (s_eventSystemObject == null)
            {
                s_eventSystemObject = EventSystem.current.gameObject;
            }
        }

        private List<int> m_expireLocker = new List<int>();
        private void UpdateLockerState()
        {
            m_expireLocker.Clear();
            foreach (var p in m_lockers)
            {
                if (p.Value <= Time.realtimeSinceStartup)
                {
                    m_expireLocker.Add(p.Key);
                }
            }

            foreach (var k in m_expireLocker)
            {
                m_lockers.Remove(k);
            }

            if (m_lockers.Count > 0)
            {
                //lock
                if (s_eventSystemObject != null && s_eventSystemObject.activeSelf)
                {
                    s_eventSystemObject.SetActive(false);
                }
            }
            else
            {
                //unlock
                if (s_eventSystemObject != null && !s_eventSystemObject.activeSelf)
                {
                    s_eventSystemObject.SetActive(true);
                }
            }

        }

    }
}
