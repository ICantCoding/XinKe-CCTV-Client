namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class LoadingScene : MonoBehaviour
    {

        #region 数据字段
        private AsyncOperation op = null;
        #endregion

        #region UI字段
        private Slider slider;
        private Text processValueTxt;
        #endregion

        #region Unity生命周期
        private void Awake()
        {
            slider = Util.FindObject<Slider>(transform, "ProcessBar");
            slider.onValueChanged.AddListener(OnSliderValueChange);
            processValueTxt = Util.FindObject<Text>(transform, "ProcessValueTxt");
        }
        private void Start()
        {
            if (slider)
                slider.value = 0.0f;
            LoadScene();
        }
        #endregion

        #region 方法
        public void LoadScene()
        {
            if (SingletonMgr.SceneInfoMgr.NextSceneInfo != null)
            {
                LoadSceneBySceneId();
            }
        }
        public void LoadSceneBySceneId()
        {
            op = SceneManager.LoadSceneAsync(SingletonMgr.SceneInfoMgr.NextSceneInfo.Index);
            //不允许加载完毕自动切换场景, 因为有时候加载太快就看不到加载进度条UI效果了. 
            op.allowSceneActivation = false;
            StartCoroutine(ProcessLoad());
        }
        public void LoadSceneBySceneName()
        {
            op = SceneManager.LoadSceneAsync(SingletonMgr.SceneInfoMgr.NextSceneInfo.Name);
            op.allowSceneActivation = false;
            StartCoroutine(ProcessLoad());
        }
        IEnumerator ProcessLoad()
        {
            int displayProgress = 0;
            int toProgress = 0;
            while (op.progress < 0.9f)
            {
                toProgress = (int)op.progress * 100;
                while (displayProgress < toProgress)
                {
                    ++displayProgress;
                    SetLoadingPercentage(displayProgress);
                    yield return new WaitForEndOfFrame();
                }
            }
            toProgress = 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                SetLoadingPercentage(displayProgress);
                yield return new WaitForEndOfFrame();
            }
            op.allowSceneActivation = true;
        }
        #endregion

        #region UI事件处理
        void OnSliderValueChange(float value)
        {
            processValueTxt.text = ((int)(value * 100)).ToString() + "%";
        }
        void SetLoadingPercentage(float value)
        {
            slider.value = value / 100;
        }
        #endregion
    }
}

