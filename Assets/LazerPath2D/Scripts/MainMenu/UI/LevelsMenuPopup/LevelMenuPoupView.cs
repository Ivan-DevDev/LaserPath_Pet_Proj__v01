using Assets.LazerPath2D.Scripts.CommonUI.Popup;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.MainMenu.UI.LevelsMenuPopup
{
    public class LevelMenuPoupView : PopupViewBase
    {
        [SerializeField] private TMP_Text _headerTitleText;
        [SerializeField] private LevelTileListView _levelTileListView;

        public LevelTileListView LevelTileListView => _levelTileListView;

        public void SetHeaderTitleText(string titleText) => _headerTitleText.text = titleText;

        protected override void ModifyShowAnimation(Sequence animation)
        {
            base.ModifyShowAnimation(animation);

           
            animation.Append(_headerTitleText.DOFade(1f, 0.2f).From(0f));

            foreach (LevelTileView levelTileView in _levelTileListView.Elements)
            {
                animation.Append(levelTileView.Show());// добавляем в конец очереди к tween , ещё анимацию
                animation.AppendInterval(0.1f);
            }
        }
    }
}
