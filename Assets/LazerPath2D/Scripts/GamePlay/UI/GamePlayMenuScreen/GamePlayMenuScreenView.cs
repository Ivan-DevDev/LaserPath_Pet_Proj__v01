using Assets.LazerPath2D.Scripts.CommonUI.View;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LazerPath2D.Scripts.GamePlay.UI.GamePlayMenuScreen
{
    public class GamePlayMenuScreenView : MonoBehaviour, IView
    {
        public event Action OpenPausePopupButtonClicked;


        [field: SerializeField] private Image _backGroundImage;

        [field: SerializeField] public TMP_Text TitleLevelText { get; private set; }
        [field: SerializeField] public Image HeaderBackgroundImage { get; private set; }
        [field: SerializeField] public Transform StarsViewContainer { get; private set; }

        [field: SerializeField] public Button OpenPausePopupButton { get; private set; }

        public void SetBackgroundImage(Image image) => _backGroundImage = image;


        public void SetTitleLevelText(string titleLevelText) => TitleLevelText.text = titleLevelText;
        public void SetHeaderBackgroundImage(Image image) => HeaderBackgroundImage = image;


        private void OnEnable()
        {
            OpenPausePopupButton.onClick.AddListener(OnOpenPausePopupButton);
        }
        private void OnDisable() 
        {
            OpenPausePopupButton.onClick.RemoveListener(OnOpenPausePopupButton);
        }

        private void OnOpenPausePopupButton()
        {
            OpenPausePopupButtonClicked?.Invoke();
        }

    }
}
