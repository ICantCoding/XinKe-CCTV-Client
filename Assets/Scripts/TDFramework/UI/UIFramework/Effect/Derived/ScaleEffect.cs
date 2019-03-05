

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;

    public class ScaleEffect : UIPointerEffect
    {
        #region 字段
        private Transform m_trans;
        private Vector3 m_originScaleValue;
        public Vector3 m_scaleValue; //缩放倍数
        #endregion

        #region Unity生命周期
        private void Awake()
        {
            m_trans = this.transform;
            m_originScaleValue = m_trans.localScale;
        }
        #endregion

        #region 抽象方法实现
        public override void PointerEnterEffectAction()
        {
            //进入缩放
            if(m_trans != null)
            {
                m_trans.DOScale(m_scaleValue, m_animationDuration);
            }
        }
        public override void PointerExitEffectAction()
        {
           //退出还原
            if(m_trans != null)
            {
                m_trans.DOScale(m_originScaleValue, m_animationDuration);
            }
        }
        public override void PointerTapEffectAction()
        {
            Debug.Log("Tap Tap...");
        }
        #endregion
    }
}