

namespace TDFramework
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //ResourceItem中包含AssetBundle
    public class ResourceItem
    {
        ////////////////////////前四个属性通过读取依赖文件获取///////////////////////////
        //资源路径的crc
        public uint Crc = 0;
        //资源名字
        public string AssetName = string.Empty;
        //资源所在AssetBundle
        public string ABName = string.Empty;
        //资源依赖的AssetBundle
        public List<string> DependABList = null;
        //////////////////////////////////////////////////////////////////////////////

        ///////////////////////通过LoadResourceItem来获取AssetBundle///////////////////
        //资源的加载完的AB包
        public AssetBundle Ab = null;
        //////////////////////////////////////////////////////////////////////////////

        //资源对象本身,从AB包中加载出来
        public Object Obj = null;
        //资源唯一标识
        public int Guid = 0;
        //最后使用该资源的时间
        public float LastUseTime = 0.0f;
        //跳转场景时,该资源是否需要清除
        public bool Clear = false;
        //该资源的引用计数
        private int refCount = 0;
        public int RefCount
        {
            get{ return refCount; } 
            set
            {
                refCount = value;
                if(refCount < 0)
                {
                    Debug.Log(AssetName + "的资源引用计数<0");
                }
            }
        }
    }
    //AssetBundleItem用于维护AssetBundle的引用计数, ResouceItem中的Ab包来源于AssetBundleItem
    public class AssetBundleItem
    {
        public AssetBundle assetBundle = null;
        public int refCount; //引用计数

        public void Reset()
        {
            assetBundle = null;
            refCount = 0;
        }
    }
    //对象池中保存的GameObjectItem数据,其中包含对象GameObject本身
    public class GameObjectItem
    {
        public uint Crc = 0; //路径对应Crc
        public ResourceItem ResourceItem; //未实例化的资源
        public GameObject Obj = null; //实例化出来的GameObject
        public bool Clear = true; //跳转场景是否删除该资源
        public long Guid = 0; //存储Guid
        public bool AlreadyRelease = false; //是否已经放回到对象池
        public bool SetSceneParent = false; //是否放到场景下边
        //游戏对象资源加载完成回调
        public OnAsyncResourceObjFinished DealFinishCallback = null;
        //异步参数
        public object Param1 = null;
        public object Param2 = null;
        public object Param3 = null;

        public void Reset()
        {
            Crc = 0;
            Obj = null;
            Clear = true;
            Guid = 0;
            ResourceItem = null;
            AlreadyRelease = false;
            SetSceneParent = false;
            DealFinishCallback = null;
            Param1 = Param2 = Param3 = null;
        }
    }

    public class AssetBundleManager : Singleton<AssetBundleManager>
    {

        #region 字段和属性
        protected Dictionary<uint, ResourceItem> m_resourceItemDict =
         new Dictionary<uint, ResourceItem>();
        protected Dictionary<uint, AssetBundleItem> m_assetBundleItemDict =
         new Dictionary<uint, AssetBundleItem>();
        //AssetBundleItem的类对象池, 先分配500个AssetBundleItem的类对象池
        protected ClassObjectPool<AssetBundleItem> m_assetBundleItemPool =
         ObjectManager.Instance.GetOrCreateClassObjectPool<AssetBundleItem>(500);
        #endregion	

        #region 构造函数
        public AssetBundleManager()
        {
            LoadAssetBundleConfig(); //初始化, AssetBundleManager一定要先初始化,把依赖文件读取出来
        }
        #endregion

        #region 方法
        public bool LoadAssetBundleConfig()
        {
            m_resourceItemDict.Clear();

            string configPath = ABPathConfig.DependenceFile4AssetBundle;
            AssetBundle configAB = AssetBundle.LoadFromFile(configPath);
            TextAsset textAsset = configAB.LoadAsset<TextAsset>(ABPathConfig.DependenceFileName);
            if (textAsset == null)
            {
                Debug.Log("AssetBundleConfig is not exists!");
                return false;
            }
            MemoryStream ms = new MemoryStream(textAsset.bytes);
            BinaryFormatter bf = new BinaryFormatter();
            AssetBundleConfig abConfig = (AssetBundleConfig)bf.Deserialize(ms);
            ms.Close();

            for (int i = 0; i < abConfig.ABList.Count; i++)
            {
                ResourceItem item = new ResourceItem();
                ABBase abBase = abConfig.ABList[i];
                item.Crc = abBase.Crc;
                item.AssetName = abBase.AssetName;
                item.ABName = abBase.ABName;
                item.DependABList = new List<string>(abBase.DependABList);
                if (m_resourceItemDict.ContainsKey(item.Crc) == false)
                {
                    m_resourceItemDict.Add(item.Crc, item);
                }
                else
                {
                    Debug.Log("重复的Crc, 资源名: " + item.AssetName + ", AB包名: " + item.ABName);
                }
            }
            return true;
        }
        //根据crc获取ResourceItem
        public ResourceItem LoadResourceItem(uint crc)
        {
            ResourceItem resourceItem = null;
            if (!m_resourceItemDict.TryGetValue(crc, out resourceItem) || resourceItem == null)
            {
                Debug.Log("没有找到Crc: " + crc.ToString() + "对应的资源!");
                return null;
            }
            if (resourceItem.Ab != null)
            {
                return resourceItem;
            }
            if (resourceItem.DependABList != null)
            {
                for (int i = 0; i < resourceItem.DependABList.Count; i++)
                {
                    LoadAssetBundle(resourceItem.DependABList[i]);
                }
            }
            resourceItem.Ab = LoadAssetBundle(resourceItem.ABName);
            return resourceItem;
        }
        //根据文件路径名获取ResourceItem
        public ResourceItem LoadResourceItem(string filePath)
        {
            uint crc = CrcHelper.StringToCRC32(filePath);
            return LoadResourceItem(crc);
        }
        public void UnLoadResourceItem(ResourceItem item)
        {
            if (item == null) return;
            if (item.DependABList != null && item.DependABList.Count > 0)
            {
                for (int i = 0; i < item.DependABList.Count; i++)
                {
                    UnLoadAssetBundle(item.DependABList[i]);
                }
            }
            UnLoadAssetBundle(item.ABName);
        }
        public AssetBundle LoadAssetBundle(string abName)
        {
            AssetBundleItem abItem = null;
            uint crc = CrcHelper.StringToCRC32(abName); //AB包名字创建crc
            if (!m_assetBundleItemDict.TryGetValue(crc, out abItem) || abItem == null)
            {
                AssetBundle assetBundle = null;
                string abFullPath = ABPathConfig.AssetBundleBuildTargetPath + "/" + abName;
                if (File.Exists(abFullPath))
                {
                    assetBundle = AssetBundle.LoadFromFile(abFullPath);
                }
                if (assetBundle == null)
                {
                    Debug.Log("Load AssetBundle Error: " + abFullPath);
                    return null;
                }
                abItem = m_assetBundleItemPool.Spawn(true);
                abItem.assetBundle = assetBundle;
                abItem.refCount++;
                m_assetBundleItemDict.Add(crc, abItem);
            }
            else
            {
                abItem.refCount++;
            }
            return abItem.assetBundle;
        }
        public void UnLoadAssetBundle(string abName)
        {
            AssetBundleItem abItem = null;
            uint crc = CrcHelper.StringToCRC32(abName);
            if (m_assetBundleItemDict.TryGetValue(crc, out abItem) && abItem != null)
            {
                abItem.refCount--;
                if (abItem.refCount <= 0 && abItem.assetBundle != null)
                {
                    abItem.assetBundle.Unload(true);
                    abItem.Reset();
                    m_assetBundleItemPool.Recycle(abItem);
                    m_assetBundleItemDict.Remove(crc);
                }
            }
        }
        public ResourceItem FindResourceItem(uint crc)
        {
            return m_resourceItemDict[crc];
        }
        #endregion
    }
}