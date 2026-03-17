using Assets.LazerPath2D.Scripts.CommonUI.View;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LazerPath2D.Scripts.CommonUI.Popup
{
    [Serializable]
    public abstract class PopupViewBase : MonoBehaviour, IShowableView
    {
        public event Action CloseRequest;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _antiClicker;
        [SerializeField] private Transform _body;
        //[SerializeField] private CanvasGroup _bodyCanvasGroup;

        [SerializeField] private PopupAnimationTypes _animationType;

        [SerializeField] private float _antiClickerDefaultAlpha;

        private Tween _currentAnimation;

        private void Awake()
        {
            _antiClickerDefaultAlpha = _antiClicker.color.a;
            _canvasGroup.alpha = 0;
        }

        private void OnDestroy()
        {
            KillCurrentAnimation();
        }

        public void OnCloseButtonClick() => CloseRequest?.Invoke();//будем дёргать из инспектора

        public Tween Show()
        {
            KillCurrentAnimation();

            OnPreShow();

            // анимация
            _canvasGroup.alpha = 1f;
           
            Sequence animation = PopupAnimationsCreator
                .CreateShowAnimation(_body, _antiClicker, _animationType, _antiClickerDefaultAlpha);

            ModifyShowAnimation(animation);// передаём аним для возможности изменения в дочерних классах

            animation.OnComplete(OnPostShow);// callBack анимации, после завершения анимации
                                             // вызываем метод "OnPostShow"
            
           return _currentAnimation = animation.SetUpdate(true).Play();            
        }

        public Tween Hide()
        {
            KillCurrentAnimation();

            OnPreHide();

            // анимация 
            Sequence animation = PopupAnimationsCreator
                .CreateHideAnimation(_body, _antiClicker, _animationType, _antiClickerDefaultAlpha);

            ModifyHideAnimation(animation);

            animation.OnComplete(OnPostHide);

          return  _currentAnimation = animation.SetUpdate(true).Play();
        }

        protected virtual void ModifyShowAnimation(Sequence animation) { }
        protected virtual void ModifyHideAnimation(Sequence animation) { }

        protected virtual void OnPreShow() { }

        protected virtual void OnPostShow() { }

        protected virtual void OnPreHide() { }

        protected virtual void OnPostHide() { }


        private void KillCurrentAnimation()
        {
            if (_currentAnimation != null)
                _currentAnimation.Kill();
        }
    }
}
