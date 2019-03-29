
namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FPSDisplay : MonoBehaviour
    {
        #region 字段
        private float m_deltaTime = 0.0f;
        private GUIStyle m_style;
        #endregion

        #region Unity生命周期
        void Awake()
        {
            m_style = new GUIStyle();
            m_style.alignment = TextAnchor.UpperLeft;
            m_style.normal.background = null;
            m_style.fontSize = 25;
            m_style.normal.textColor = new Color(0f, 1f, 0f, 1.0f);
        }
        void Update()
        {
            m_deltaTime += (Time.deltaTime - m_deltaTime) * 0.1f;
        }
        void OnGUI()
        {
            int w = Screen.width;
            int h = Screen.height;
            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            float fps = 1.0f / m_deltaTime;
            // float fps = 1.0f / Time.deltaTime;
            string text = string.Format("   {0:0.} FPS", fps);
            GUI.Label(rect, text, m_style);
        }
        #endregion
    }
}
