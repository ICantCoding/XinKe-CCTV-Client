

namespace TDFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TDFramework.Utils;

    public class ObjectManager : Singleton<ObjectManager>
    {

        #region 类对象池相关
        #region 字段和属性
        private Dictionary<Type, object> m_classObjectPoolDict = new Dictionary<Type, object>();
        #endregion

        #region 方法
        public ClassObjectPool<T> GetOrCreateClassObjectPool<T>(int maxCount)
            where T : class, new()
        {
            Type type = typeof(T);
            object obj = null;
            if (!m_classObjectPoolDict.TryGetValue(type, out obj) || obj == null)
            {
                ClassObjectPool<T> newPool = new ClassObjectPool<T>(maxCount);
                m_classObjectPoolDict.Add(type, newPool);
                return newPool;
            }
            return obj as ClassObjectPool<T>;
        }
        #endregion
        #endregion

        #region 对象池相关
        #region 字段和属性
        public Transform m_goPool; //对象池父物体节点
        public Transform m_sceneGos;
        //对象池
        public Dictionary<uint, List<GameObjectItem>> m_gameObjectItemPoolDict = new Dictionary<uint, List<GameObjectItem>>();
        //GameObjectItem的类对象池
        protected ClassObjectPool<GameObjectItem> m_gameObjectItemClassPool = null;
        //GameObjectItem的Guid为Key, GameObjectItem实例为对象的字典集合
        protected Dictionary<long, GameObjectItem> m_gameObjectItemDict = new Dictionary<long, GameObjectItem>();
        #endregion

        #region 方法
        //对象池初始化一些数据
        public void InitGoPool(Transform goPool, Transform sceneGos)
        {
            m_gameObjectItemClassPool = ObjectManager.Instance.GetOrCreateClassObjectPool<GameObjectItem>(1000);
            m_goPool = goPool;
            m_sceneGos = sceneGos;
        }
        
        //同步加载GameObject, 参数3：资源在跳转场景是否需要清空
        public GameObject Instantiate(string path, bool bClear = true)
        {
            uint crc = CrcHelper.StringToCRC32(path);
            //先尝试从缓存中取实例化Obj
            GameObjectItem gameObjectItem = GetGameObjectItemFromPool(crc);
            if (gameObjectItem == null)
            {
                gameObjectItem = m_gameObjectItemClassPool.Spawn(true);
                gameObjectItem.Crc = crc;
                gameObjectItem.Clear = bClear;
                ResourceMgr.Instance.LoadGameObjectItem(path, gameObjectItem);
                if (gameObjectItem.ResourceItem.Obj != null)
                {
                    gameObjectItem.Obj = GameObject.Instantiate(gameObjectItem.ResourceItem.Obj) as GameObject;
                }
            }
            gameObjectItem.Guid = gameObjectItem.Obj.GetInstanceID();
            if (!m_gameObjectItemDict.ContainsKey(gameObjectItem.Guid))
            {
                m_gameObjectItemDict.Add(gameObjectItem.Guid, gameObjectItem);
            }
            return gameObjectItem.Obj;
        }
        //异步加载GameObject
        public void InstantiateAsync(string path, OnAsyncResourceObjFinished dealFinish, LoadAssetPriority priority,
         bool setSceneObject = false, object param1 = null, object param2 = null, object param3 = null,
         bool bClear = true)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            uint crc = CrcHelper.StringToCRC32(path);
            GameObjectItem gameObjectItem = GetGameObjectItemFromPool(crc);
            if (gameObjectItem != null)
            {
                if (setSceneObject)
                {
                    gameObjectItem.Obj.transform.SetParent(m_sceneGos, false);
                }
                if (dealFinish != null)
                {
                    dealFinish(path, gameObjectItem.Obj, param1, param2, param3);
                }
                return;
            }
            gameObjectItem = m_gameObjectItemClassPool.Spawn(true);
            gameObjectItem.Crc = crc;
            gameObjectItem.SetSceneParent = setSceneObject;
            gameObjectItem.Clear = bClear;
            gameObjectItem.DealFinishCallback = dealFinish;
            gameObjectItem.Param1 = param1;
            gameObjectItem.Param2 = param2;
            gameObjectItem.Param3 = param3;
            //调用ResourceManager异步加载接口
            ResourceMgr.Instance.AsyncLoadGameObjectItem(path, gameObjectItem, OnAsyncLoadGameObjectFinish, priority);
        }
        //GameObject异步加载资源ResourceItem成功后的回调
        private void OnAsyncLoadGameObjectFinish(string path, GameObjectItem gameObjectItem,
         object param1 = null, object param2 = null, object param3 = null)
        {
            if (gameObjectItem == null) return;
            if (gameObjectItem.ResourceItem.Obj == null)
            {
#if UNITY_EDITOR
                Debug.Log("异步资源加载的资源为空: " + path);
#endif
            }
            else
            {
                gameObjectItem.Obj = GameObject.Instantiate(gameObjectItem.ResourceItem.Obj) as GameObject;
            }
            if(gameObjectItem.Obj != null && gameObjectItem.SetSceneParent)
            {
                gameObjectItem.Obj.transform.SetParent(m_sceneGos, false);
            }
            if(gameObjectItem.DealFinishCallback != null)
            {
                gameObjectItem.DealFinishCallback(path, gameObjectItem.Obj, 
                 gameObjectItem.Param1, gameObjectItem.Param2, gameObjectItem.Param3);
            }
        }
        //卸载资源
        public void ReleaseGameObjectItem(GameObject obj, int maxCacheCount = -1, bool destoryCache = false, bool recycleParent = true)
        {
            if (obj == null) return;
            GameObjectItem gameObjectItem = null;
            int tempGuid = obj.GetInstanceID();
            if (!m_gameObjectItemDict.TryGetValue(tempGuid, out gameObjectItem) || gameObjectItem == null)
            {
                Debug.Log(obj.name + "并非对象池技术创建,不能回收到对象池!");
                return;
            }
            if (gameObjectItem.AlreadyRelease)
            {
                Debug.Log(obj.name + "该对象已经放回对象池, 检查是否清空该对象的引用!");
                return;
            }
#if UNITY_EDITOR
            obj.name += "(Recycle)";
#endif
            if (maxCacheCount == 0) //表示不缓存
            {
                m_gameObjectItemDict.Remove(tempGuid);
                GameObject.Destroy(gameObjectItem.Obj);
                ResourceMgr.Instance.UnLoadGameObjectItem(gameObjectItem, destoryCache);
                gameObjectItem.Reset();
                m_gameObjectItemClassPool.Recycle(gameObjectItem);
            }
            else
            {
                //回收到对象池
                List<GameObjectItem> list = null;
                if (!m_gameObjectItemPoolDict.TryGetValue(gameObjectItem.Crc, out list) || list == null)
                {
                    list = new List<GameObjectItem>();
                    m_gameObjectItemPoolDict.Add(gameObjectItem.Crc, list);
                }
                if (gameObjectItem.Obj)
                {
                    if (recycleParent)
                    {
                        gameObjectItem.Obj.transform.SetParent(m_goPool);
                    }
                    else
                    {
                        gameObjectItem.Obj.SetActive(false);
                    }
                }
                if (maxCacheCount < 0 || list.Count < maxCacheCount) //<0表示可以无限缓存, <maxCacheCount表示需要放入缓存
                {
                    list.Add(gameObjectItem);
                    gameObjectItem.AlreadyRelease = true;
                    ResourceMgr.Instance.DecrementResourceRef(gameObjectItem, 1);
                }
                else //不需要缓存GameObject到对象池
                {
                    m_gameObjectItemDict.Remove(tempGuid);
                    GameObject.Destroy(gameObjectItem.Obj);
                    ResourceMgr.Instance.UnLoadGameObjectItem(gameObjectItem);
                    gameObjectItem.Reset();
                    m_gameObjectItemClassPool.Recycle(gameObjectItem);
                }
            }
        }
        //预加载GameObject对象, 路径, 预加载个数, 跳场景是否清除
        public void PreloadGameObject(string path, int count = -1, bool clear = false)
        {
            List<GameObject> tempGameObjectList = new List<GameObject>();
            for(int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(path, clear);
                tempGameObjectList.Add(obj);
            }
            for(int i = 0; i < count; i++)
            {
                GameObject obj = tempGameObjectList[i];
                ReleaseGameObjectItem(obj);
                obj = null;
            }
            tempGameObjectList.Clear();
        }
        
        //从对象池中获取对象
        public GameObjectItem GetGameObjectItemFromPool(uint crc)
        {
            List<GameObjectItem> list = null;
            if (m_gameObjectItemPoolDict.TryGetValue(crc, out list) && list != null && list.Count > 0)
            {
                ResourceMgr.Instance.IncrementResourceRef(crc, 1);
                GameObjectItem gameObjectItem = list[0];
                list.RemoveAt(0);
                GameObject go = gameObjectItem.Obj;
                if (!System.Object.ReferenceEquals(go, null))
                {
                    gameObjectItem.AlreadyRelease = false;
#if UNITY_EDITOR
                    if (go.name.EndsWith("(Recycle)"))
                    {
                        go.name = go.name.Replace("(Recycle)", "");
                    }
#endif
                }
                return gameObjectItem;
            }
            return null;
        }
        //判断游戏对象是否是对象池创建
        public bool IsCreatedByObjectManager(GameObject go)
        {
            if(System.Object.ReferenceEquals(go, null)) return false;
            long guid = go.GetInstanceID();
            return m_gameObjectItemDict.ContainsKey(guid);
        }
        #endregion
        #endregion
    }
}
