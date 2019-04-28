
namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Video;

    public class LaunchSplash : MonoBehaviour
    {

        #region 字段
        private VideoPlayer m_videoPlayer = null;
        private bool m_screenAdaptFlag = false;
        #endregion

        #region Unity生命周期
        private void Awake()
        {
            m_videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
            if (m_videoPlayer != null)
            {
                m_videoPlayer.loopPointReached += OnVideoPlayerPlayEnd;
            }
        }
        void LateUpdate()
        {
            if (m_screenAdaptFlag == false)
            {
                //读取 AppInfo信息
                SingletonMgr.GameGlobalInfo.AppInfo = AppInfo.DeserializeAppInfoFromXml();
                Screen.SetResolution(SingletonMgr.GameGlobalInfo.AppInfo.ScreenWidth, SingletonMgr.GameGlobalInfo.AppInfo.ScreenHeight, 
                    (FullScreenMode)SingletonMgr.GameGlobalInfo.AppInfo.FullMode, SingletonMgr.GameGlobalInfo.AppInfo.ScreenRefreshRate);
                m_screenAdaptFlag = true;
            }
        }
        private void OnDestroy()
        {
            if (m_videoPlayer != null)
            {
                m_videoPlayer.loopPointReached -= OnVideoPlayerPlayEnd;
            }
        }
        #endregion

        #region 回调方法
        public void OnVideoPlayerPlayEnd(VideoPlayer source)
        {
            SingletonMgr.LoadSceneMgr.LoadScene(SingletonMgr.SceneInfoMgr.AppStartScene);
        }
        #endregion
    }

}