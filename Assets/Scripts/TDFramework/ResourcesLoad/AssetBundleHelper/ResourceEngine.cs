
namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ResourceEngine : MonoBehaviour
    {
        #region Unity生命周期
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        #endregion

        #region 方法
        public void InitResourceEngine()
        {
            Transform poolGoTrans = GameObject.Find(GameTagMgr.PoolGos_Tag).transform;
            poolGoTrans.gameObject.SetActive(false);
            poolGoTrans.localPosition = new Vector3(-9.4f, -14.995f, -18.56f);
            GameObject.DontDestroyOnLoad(poolGoTrans.gameObject);
            Transform sceneGoTrans = GameObject.Find(GameTagMgr.SceneGos_Tag).transform;
            sceneGoTrans.gameObject.SetActive(false);
            GameObject.DontDestroyOnLoad(sceneGoTrans.gameObject);

            //初始化对象池数据
            ObjectManager.Instance.InitGoPool(poolGoTrans, sceneGoTrans); 
            
            //预加载第一种男Npc
            ObjectManager.Instance.PreloadGameObject(StringMgr.Man1_Npc, 160, false);
            //预加载第二种男Npc
            ObjectManager.Instance.PreloadGameObject(StringMgr.Man2_Npc, 160, false);
            //预加载第三种男Npc
            ObjectManager.Instance.PreloadGameObject(StringMgr.Man3_Npc, 160, false);
            //预加载第四种男Npc
            ObjectManager.Instance.PreloadGameObject(StringMgr.Man4_Npc, 160, false);
            //预加载第五种男Npc
            ObjectManager.Instance.PreloadGameObject(StringMgr.Man5_Npc, 160, false);
            //预加载第一种女Npc
            ObjectManager.Instance.PreloadGameObject(StringMgr.Woman1_Npc, 160, false);
            //预加载第二种女Npc
            ObjectManager.Instance.PreloadGameObject(StringMgr.Woman2_Npc, 160, false);
            //预加载第三种女Npc
            ObjectManager.Instance.PreloadGameObject(StringMgr.Woman3_Npc, 160, false);
            //预加载第四种女Npc
            ObjectManager.Instance.PreloadGameObject(StringMgr.Woman4_Npc, 160, false);
            //预加载第五种女Npc
            ObjectManager.Instance.PreloadGameObject(StringMgr.Woman5_Npc, 160, false);
            //预加载BigScreen
            ObjectManager.Instance.PreloadGameObject(StringMgr.RawBigScreen, 200, false);
            //预加载SmallScreen
            ObjectManager.Instance.PreloadGameObject(StringMgr.RawSmallScreen, 200, false);
            //预先加载CCTVCamera
            ObjectManager.Instance.PreloadGameObject(StringMgr.CCTVCamera, 200, false);
            //预先加载RendererTexture
            string rendererTextureName = "";
            for(int i = 1; i <= 16; ++i)
            {
                for(int j = 1; j <= 4; ++j)
                {
                    rendererTextureName = "Assets/Res/RenderTexture/New Render Texture " + i.ToString() + " " + j.ToString() + ".renderTexture";
                    ResourceMgr.Instance.PreLoadAsset(rendererTextureName);
                }
            }
        }
        #endregion
    }
}