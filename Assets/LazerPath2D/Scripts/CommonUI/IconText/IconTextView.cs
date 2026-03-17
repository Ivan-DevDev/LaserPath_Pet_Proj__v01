using Assets.LazerPath2D.Scripts.CommonUI.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LazerPath2D.Scripts.CommonUI.IconText
{
    public class IconTextView : MonoBehaviour, IView 
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _icon;

        public void SetText(string text) => _text.text = text;

        public void SetIcon(Image image) => _icon = image;
    }
}