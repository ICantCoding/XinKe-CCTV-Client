

//类对象池， 用于创建类对象缓存

namespace TDFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ClassObjectPool<T> where T : class, new()
    {
        #region 字段和属性
        //池容器，栈
        protected Stack<T> m_pool = new Stack<T>();
        //池中最大类对象的个数限制(<= 0 表示不限制对象个数，但是并没有事先缓存类对象)
        protected int m_maxCount = 0;
        //没有回收的类对象个数计数器
        protected int m_noRecycleCount = 0;
        #endregion

        #region 构造函数
        public ClassObjectPool(int maxCount)
        {
            m_maxCount = maxCount;
            for(int i = 0; i < m_maxCount; i++)
            {
                m_pool.Push(new T());
            }
        }
        #endregion

        #region 方法
        //从类对象池中取出
        //isCreate表示池中没有类对象时，是否主动创建，主动创建的类对象不会被Pool管理
        public T Spawn(bool isCreate)
        {
            if(m_pool.Count > 0)
            {
                T rtn = m_pool.Pop();
                if(rtn == null && isCreate == true)
                {
                    rtn = new T();
                }
                m_noRecycleCount++;
                return rtn;
            }else
            {
                if(isCreate == true)
                {
                    T rtn = new T();
                    m_noRecycleCount++;
                    return rtn;
                }
            }
            return null;
        }
        //回收到对象池
        public bool Recycle(T obj)
        {
            if(obj == null) return false;
            m_noRecycleCount--;
            if(m_pool.Count >= m_maxCount && m_maxCount > 0)
            {
                obj = null;
                return true;
            }
            else
            {
                m_pool.Push(obj);
            }
            return true;
        }
        #endregion
    }
}