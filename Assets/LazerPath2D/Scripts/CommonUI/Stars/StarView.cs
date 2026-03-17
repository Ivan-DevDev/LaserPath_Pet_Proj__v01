using Assets.LazerPath2D.Scripts.CommonUI.View;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LazerPath2D.Scripts.CommonUI.Stars
{
    public class StarView : MonoBehaviour, IView
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _deActiveColor;

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        public void SetImageView(Image image) => _image = image;
        public void SetSpriteView(Sprite sprite) => _image.sprite = sprite;


        public void SetActive()
        {
            _image.color = _activeColor;
        }

        public void SetDeActive()
        {
            
            _image.color = _deActiveColor;
        }
    }
}
