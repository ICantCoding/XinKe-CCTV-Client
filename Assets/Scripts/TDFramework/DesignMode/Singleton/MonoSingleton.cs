
namespace TDFramework
{
    using System;
    using UnityEngine;
    using System.Collections;

    public class MonoSingleton<T> : MonoBehaviour
     where T : MonoSingleton<T>
    {
        #region 单例加锁
        private static readonly object lockobj = new object();
        private static T m_instance = null;
        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (lockobj)
                    {
                        m_instance = FindObjectOfType(typeof(T)) as T;
                        if (m_instance == null)
                        {
                            m_instance = new GameObject(typeof(T).Name + "_Singleton").AddComponent<T>();
                        }
                        if (m_instance == null)
                            Debug.LogError("Failed to create m_instance of " + typeof(T).FullName + ".");
                    }
                }
                return m_instance;
            }
        }
        #endregion

        #region Unity生命周期
        void OnApplicationQuit()
        {
            if (m_instance != null)
                m_instance = null;
        }
        #endregion

		#region 方法
        public static T CreateInstance()
        {
            if (Instance != null) Instance.OnCreate();
            return Instance;
        }
        protected virtual void OnCreate()
        {

        }
		#endregion

    }
}