using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Assets.LazerPath2D.Scripts.CommonUI.Popup
{
    public class PopupAnimationsCreator
    {

        public static Sequence CreateShowAnimation(
            Transform body,
            Image antiClicker,
            PopupAnimationTypes popupAnimationType,
            float antiClickerMaxAlpha)
        {
            switch (popupAnimationType)
            {
                case PopupAnimationTypes.None:
                    {
                        return DOTween.Sequence();
                    }

                case PopupAnimationTypes.Expand:
                    {
                        float time = 0.5f;

                        Sequence animation = DOTween.Sequence();

                        animation
                            .Append(antiClicker
                                .DOFade(antiClickerMaxAlpha, 0.1f)
                                .From(0))
                            .Join(body
                                .DOScale(1, time)
                                .From(0)
                                .SetEase(Ease.OutBack));

                        return animation;
                    }

                default:
                    throw new ArgumentException(nameof(popupAnimationType));
            }
        }

        public static Sequence CreateHideAnimation(
            Transform body,
            Image antiClicker,
            PopupAnimationTypes popupAnimationType,
            float antiClickerMaxAlpha)
        {
            return DOTween.Sequence();
        }
    }
}
