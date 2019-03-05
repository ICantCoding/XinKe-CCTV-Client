

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    //UI实现交互特效所需继承的抽象类
    public abstract class UIPointerEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {

        #region 子类继承的字段
        public float m_animationDuration = 0.2f; //特效动画时长
        #endregion

        #region 子类继承重写方法
        public abstract void PointerEnterEffectAction();
        public abstract void PointerExitEffectAction();
        public abstract void PointerTapEffectAction();
        #endregion

        #region 接口实现
        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnterEffectAction();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExitEffectAction();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            PointerTapEffectAction();
        }
        #endregion
    }
}