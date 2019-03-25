

//自定义数据结构-双向链表
namespace TDFramework.TDStruct
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    //双向链表节点
    public class TDDoubleLinkedListNode<T> where T : class, new()
    {
        //前一个节点
        public TDDoubleLinkedListNode<T> prev = null;
        //后一个节点
        public TDDoubleLinkedListNode<T> next = null;
        //当前节点
        public T t = null;
    }

    //双向链表结构
    public class TDDoubleLinkedList<T> where T : class, new()
    {
        //表头
        public TDDoubleLinkedListNode<T> Head = null;
        //表尾
        public TDDoubleLinkedListNode<T> Tail = null;
        //双向链表节点类对象池
        protected ClassObjectPool<TDDoubleLinkedListNode<T>> m_doubleLinkedListNodePool =
            ObjectManager.Instance.GetOrCreateClassObjectPool<TDDoubleLinkedListNode<T>>(500);
        //节点个数
        protected int m_count = 0;
        public int Count
        {
            get { return m_count; }
        }

        #region 方法
        //添加一个节点到头部
        public TDDoubleLinkedListNode<T> AddToHeader(TDDoubleLinkedListNode<T> pNode)
        {
            if(pNode == null) return null;
            pNode.prev = null;
            if(Head == null)
            {
                Head = Tail = pNode;
            }
            else
            {
                pNode.next = Head;
                Head.prev = pNode.next;
                Head = pNode;
            }
            m_count++;
            return pNode;
        }
        public TDDoubleLinkedListNode<T> AddToHeader(T t)
        {
            TDDoubleLinkedListNode<T> node = m_doubleLinkedListNodePool.Spawn(true);
            node.next = null;
            node.prev = null;
            node.t = t;
            return AddToHeader(node);
        }
        //添加一个节点到尾部
        public TDDoubleLinkedListNode<T> AddToTail(TDDoubleLinkedListNode<T> pNode)
        {
            if(pNode == null) return null;
            pNode.next = null;
            if(Tail == null)
            {
                Head = Tail = pNode;
            }
            else
            {
                pNode.prev = Tail;
                Tail.next = pNode;
                Tail = pNode;
            }
            m_count++;
            return pNode;
        }
        public TDDoubleLinkedListNode<T> AddToTail(T t)
        {
            TDDoubleLinkedListNode<T> node = m_doubleLinkedListNodePool.Spawn(true);
            node.prev = null;
            node.next = null;
            node.t = t;
            return AddToTail(node);
        }
        //移除某个节点
        public void RemoveNode(TDDoubleLinkedListNode<T> pNode)
        {
            if(pNode == null) return;
            if(pNode == Head)
            {
                Head = pNode.next;
            }
            if(pNode == Tail)
            {
                Tail = pNode.prev;
            }
            if(pNode.prev != null)
            {
                pNode.prev.next = pNode.next;
            }
            if(pNode.next != null)
            {
                pNode.next.prev = pNode.prev;
            }
            pNode.prev = pNode.next = null;
            pNode.t = null;
            m_doubleLinkedListNodePool.Recycle(pNode);
            m_count--;
        }
        //把某个节点移动到头部
        public void MoveToHead(TDDoubleLinkedListNode<T> pNode)
        {
            if(pNode == null) return;
            if(pNode == Head) return;
            if(pNode.prev == null && pNode.next == null) return;
            if (pNode == Tail)
            {
                Tail = pNode.prev;
            }
            if (pNode.prev != null)
            {
                pNode.prev.next = pNode.next;
            }
            if (pNode.next != null)
            {
                pNode.next.prev = pNode.prev;
            }
            pNode.prev = null;
            pNode.next = Head;
            Head.prev = pNode;
            Head = pNode;
            if (Tail == null)
            {
                Tail = Head;
            }
        }
        #endregion
    }

    public class TDMapList<T> where T : class, new()
    {
        #region 字段和属性
        private TDDoubleLinkedList<T> m_doubleLinkedList = new TDDoubleLinkedList<T>();
        private Dictionary<T, TDDoubleLinkedListNode<T>> m_doubleLinkedNodeDict =
         new Dictionary<T, TDDoubleLinkedListNode<T>>();
        #endregion

        #region 构造和析构函数
        ~TDMapList()
        {
            Clear();
        }
        #endregion

        public void Clear()
        {
            while(m_doubleLinkedList.Tail != null)
            {
                m_doubleLinkedList.RemoveNode(m_doubleLinkedList.Tail);
            }
        }
        public void Insert(T t)
        {
            TDDoubleLinkedListNode<T> node = null;
            if(m_doubleLinkedNodeDict.TryGetValue(t, out node) && node != null)
            {
                m_doubleLinkedList.AddToHeader(node);
            }
            else
            {
                m_doubleLinkedList.AddToHeader(t);
                m_doubleLinkedNodeDict.Add(t, m_doubleLinkedList.Head);
            }
        }
        public void Remove(T t)
        {
            TDDoubleLinkedListNode<T> node = null;
            if(m_doubleLinkedNodeDict.TryGetValue(t, out node) && node != null)
            {
                m_doubleLinkedList.RemoveNode(node);
                m_doubleLinkedNodeDict.Remove(t);
            }
        }
        public void Pop()
        {
            if(m_doubleLinkedList.Tail != null)
            {
                Remove(m_doubleLinkedList.Tail.t);
            }
        }
        public T Back()
        {
            return m_doubleLinkedList.Tail == null ? null : m_doubleLinkedList.Tail.t;
        }
        public int Size()
        {
            return m_doubleLinkedList.Count;
        }
        public bool Find(T t)
        {
            TDDoubleLinkedListNode<T> node = null;
            if(m_doubleLinkedNodeDict.TryGetValue(t, out node) && node != null)
            {
                return true;
            }
            return false;
        }
        //刷新某个节点，把该节点移动到头部
        public bool Refresh(T t)
        {
            TDDoubleLinkedListNode<T> node = null;
            if(m_doubleLinkedNodeDict.TryGetValue(t, out node) && node != null)
            {
                m_doubleLinkedList.MoveToHead(node);
                return true;
            }
            return false;
        }
    }

}
