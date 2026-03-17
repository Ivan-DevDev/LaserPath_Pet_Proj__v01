using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using Assets.LazerPath2D.Scripts.CommonUI.Popup;
using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using Assets.LazerPath2D.Scripts.MainMenu.UI.LevelsMenuPopup;
using Assets.LazerPath2D.Scripts.MainMenu.UI.OptionsMenuPopup;
using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.MainMenu.UI
{
    public class MainMenuPopupService : PopupService
    {
        private readonly MainMenuUIRoot _mainMenuUIRoot;

        public MainMenuPopupService(
            ViewsFactory viewsFactory,
            ProjectPresentersFactory presentersFactory,
            MainMenuUIRoot mainMenuUIRoot,
            PlaySound playSound)
            : base(viewsFactory, presentersFactory, playSound)
        {
            _mainMenuUIRoot = mainMenuUIRoot;
        }

        protected override Transform PopupLayer => _mainMenuUIRoot.PopupsLayer;       

        public OptionsMenuPopupPresenter OpenOptionsMenuPopupPresenter(Action closedCallBack = null)
        {
            OptionsMenuPopupView view = ViewsFactory.Create<OptionsMenuPopupView>(
                ViewIDs.OptionsMenuPopupView, PopupLayer);

            OptionsMenuPopupPresenter popupPresenter = _presentersFactory.CreateOptionsMenuPopupPresenter(view);

            OnPopupCreated(popupPresenter, view, closedCallBack);

            return popupPresenter;
        }

        public LevelMenuPoupPresenter OpenLevelMenuPoupPresenter(Action closedCallBack = null)
        {
            LevelMenuPoupView poupView = ViewsFactory.Create<LevelMenuPoupView>(ViewIDs.LevelsMenuPopupView, PopupLayer);


            LevelTileListView levelTileListView = poupView.LevelTileListView;

            LevelMenuPoupPresenter levelMenuPoupPresenter = _presentersFactory.CreateLevelMenuPoupPresenter(poupView, levelTileListView);

            OnPopupCreated(levelMenuPoupPresenter, poupView, closedCallBack);

            return levelMenuPoupPresenter;
        }
    }
}
