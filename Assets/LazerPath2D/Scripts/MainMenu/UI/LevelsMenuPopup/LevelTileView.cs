using Assets.LazerPath2D.Scripts.CommonUI.View;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LazerPath2D.Scripts.MainMenu.UI.LevelsMenuPopup
{
    public class LevelTileView : MonoBehaviour, IShowableView
    {
        public event Action Clicked;

        [SerializeField] private Image _backGround;
        [SerializeField] private TMP_Text _levelNumberText;
        [SerializeField] private Button _levelButton;       
        [SerializeField] private Transform _containerStars;

        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _completedColor;
        [SerializeField] private Color _blockedColor;
        [SerializeField] private Color _blockedColorText;


        private void OnEnable()
        {
            _levelButton.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _levelButton.onClick.RemoveListener(OnClick);
        }

        public Transform ContainerStars => _containerStars;

        public void SetLevel(string  level) => _levelNumberText.text = level;

        public void SetBlock()
        {
            _levelNumberText.color = _blockedColorText;
            _backGround.color = _blockedColor;
        }

        public void SetComplete() => _backGround.color = _completedColor;

        public void SetActive() => _backGround.color = _activeColor;


        public Tween Show()
        {
            transform.DOKill();

            return transform.DOScale(1, 0.1f)
                .From(0f)
                .SetUpdate(true)
                .Play();
        }

        public Tween Hide()
        {
            transform.DOKill();

            return DOTween.Sequence();
        }

        private void OnClick()
        {
            Clicked?.Invoke();
        }

    }
}
