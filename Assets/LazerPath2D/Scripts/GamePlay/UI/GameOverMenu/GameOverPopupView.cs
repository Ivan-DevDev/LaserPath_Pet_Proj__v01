using Assets.LazerPath2D.Scripts.CommonUI.Popup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LazerPath2D.Scripts.GamePlay.UI.GameOverMenu
{
    public class GameOverPopupView : PopupViewBase
    {
        [SerializeField] private TMP_Text _headerText;
        [field: SerializeField] public Button RestartButton { get; private set;}
        [field: SerializeField] public Button MainMenuButton { get; private set; }
        [field: SerializeField] public Button PlayNextLevelButton { get; private set; }

        [SerializeField] private Image _conturBody;
        [SerializeField] private Image _conturLightBody;

        public Image ConturBody => _conturBody;
        public Image ConturLightBody => _conturLightBody;


        public void SetTexHeader(string  text) => _headerText.text = text;

        
    }
}
