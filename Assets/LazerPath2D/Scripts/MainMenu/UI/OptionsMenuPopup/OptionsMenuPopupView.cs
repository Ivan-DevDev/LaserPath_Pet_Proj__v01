using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.LazerPath2D.Scripts.CommonUI.Popup;
using DG.Tweening;
using Assets.LazerPath2D.Scripts.CommonUI.PlaySounds;

namespace Assets.LazerPath2D.Scripts.MainMenu.UI.OptionsMenuPopup
{
    public class OptionsMenuPopupView : PopupViewBase
    {
        [SerializeField] private TMP_Text _headerText;
        [SerializeField] private Button _closeButton;
        [SerializeField] private PlaySoundView _soundView;

        [SerializeField] private Image _conturBoard;
        [SerializeField] private Image _conturLightBoard;


        public Image ConturBoard => _conturBoard;
        public Image ConturLightBoard => _conturLightBoard;

        public void SetHeaderText(string headerText) => _headerText.text = headerText;

        public PlaySoundView PlaySoundView => _soundView;

        protected override void ModifyShowAnimation(Sequence animation)
        {
            base.ModifyShowAnimation(animation);

            animation
                .Append(_headerText
                .DOFade(1f, 0.2f)
                .From(0));
        }
    }
}
