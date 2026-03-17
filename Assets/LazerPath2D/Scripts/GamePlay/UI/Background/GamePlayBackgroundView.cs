using Assets.LazerPath2D.Scripts.CommonUI.View;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LazerPath2D.Scripts.GamePlay.UI.Background
{
    public class GamePlayBackgroundView : MonoBehaviour, IView
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _opacityPanel;

        public void Initialize()
        {
            _canvas.worldCamera = Camera.main;
        }

        public Image BackgroundImage => _backgroundImage;
        public Image OpacityPanel => _opacityPanel;

        public void SetBackgroundImage(Image backgroundImage) => _backgroundImage = backgroundImage;
        public void SetBackgroundColor(Color color) => _backgroundImage.color = color;

        public void Show() => this.gameObject.SetActive(true);

        public void Hide() => this.gameObject.SetActive(false);
    }
}
