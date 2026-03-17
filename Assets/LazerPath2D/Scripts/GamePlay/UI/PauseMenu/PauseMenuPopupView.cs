using Assets.LazerPath2D.Scripts.CommonUI.PlaySounds;
using Assets.LazerPath2D.Scripts.CommonUI.Popup;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LazerPath2D.Scripts.GamePlay.UI.PauseMenu
{
    public class PauseMenuPopupView : PopupViewBase
    {

        [field: SerializeField] private TMP_Text _headerText;
        [field: SerializeField] public Button CloseButton {  get; private set; }
        [field: SerializeField] public Button RestartButton { get; private set; }
        [field: SerializeField] public Button MainMenuButton { get; private set; }

        [SerializeField] private PlaySoundView _soundView;

        [SerializeField] private Image _conturBody;
        [SerializeField] private Image _conturLightBody;

        public PlaySoundView PlaySoundView => _soundView;

        public Image ConturBody => _conturBody;
        public Image ConturLightBody => _conturLightBody;

        public void SetHeaderText(string  text) => _headerText.text = text;

        protected override void ModifyShowAnimation(Sequence animation)
        {
            base.ModifyShowAnimation(animation);

            animation.Append(
                _headerText
                .DOFade(1f, 0.2f)
                .From(0));
        }
      
    }
}
