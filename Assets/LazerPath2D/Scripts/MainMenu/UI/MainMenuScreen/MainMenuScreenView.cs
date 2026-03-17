using Assets.LazerPath2D.Scripts.CommonUI.View;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LazerPath2D.Scripts.MainMenu.UI.MainMenuScreen
{
    [Serializable]
    public class MainMenuScreenView : MonoBehaviour, IView
    {
        public event Action OpenOptionsPopupButtonCliced;
        public event Action OpenLevelsPopupButtonCliced;

        [field: SerializeField] private Image _backGroundImage;
        [field: SerializeField] private Image _backGroundStarSky;

        [field: SerializeField] public Button OpenOptionsPopupButton { get; private set; }
        [field: SerializeField] public Button OpenLevelsPopupButton { get; private set; }

        public Image BackGroundStarSky => _backGroundImage;
        public void SetBackGroundImage(Sprite backGroundImage) => _backGroundImage.sprite = backGroundImage;

        private void OnEnable()
        {
            OpenOptionsPopupButton.onClick.AddListener(OnOpenOptionsPopupButtonCliced);
            OpenLevelsPopupButton.onClick.AddListener(OnOpenLevelsPopupButtonCliced);
        }

        public void OnDisable()
        {
            OpenOptionsPopupButton.onClick.RemoveListener(OnOpenOptionsPopupButtonCliced);
            OpenLevelsPopupButton.onClick.RemoveListener(OnOpenLevelsPopupButtonCliced);
        }

        private void OnOpenOptionsPopupButtonCliced()
        {
            OpenOptionsPopupButtonCliced?.Invoke();
        }

        private void OnOpenLevelsPopupButtonCliced()
        {
            OpenLevelsPopupButtonCliced?.Invoke();
        }
    }
}
