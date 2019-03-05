

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;

    public class RotateEffect : UIPointerEffect
    {
        #region 字段
        private Transform m_trans = null;
        private Vector3 m_originRotationValue;
        public Vector3 m_rotationValue;
        #endregion

        #region Unity生命周期
        private void Awake()
        {
            m_trans = transform;
            m_originRotationValue = m_trans.localEulerAngles;
        }
        #endregion

        #region 抽象方法的实现
        public override void PointerEnterEffectAction()
        {
            m_trans.DOLocalRotate(m_rotationValue, m_animationDuration);
        }
        public override void PointerExitEffectAction()
        {
            m_trans.DOLocalRotate(m_originRotationValue, m_animationDuration);
        }
        public override void PointerTapEffectAction()
        {

        }
        #endregion
    }
}