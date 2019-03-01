
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
        private void OnDestroy()
        {
            if(m_videoPlayer != null)
            {
                m_videoPlayer.loopPointReached -= OnVideoPlayerPlayEnd;
            }
        }
        #endregion

        #region 回调方法
        public void OnVideoPlayerPlayEnd(VideoPlayer source)
        {
            LoadSceneMgr.Instance.LoadScene(SingletonMgr.SceneInfoMgr.AppStartScene);
        }
        #endregion
    }

}