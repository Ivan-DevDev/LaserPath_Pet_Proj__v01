using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using Assets.LazerPath2D.Scripts.DI;
using Assets.LazerPath2D.Scripts.MainMenu.Enviroment;
using Assets.LazerPath2D.Scripts.MainMenu.UI.LevelsMenuPopup;
using Assets.LazerPath2D.Scripts.MainMenu.UI.OptionsMenuPopup;
using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.MainMenu.UI.MainMenuScreen
{
    public class MainMenuScreenPresenter : IInitializable, IDisposable, IPresenter
    {
        // model
        private readonly ViewsFactory _viewsFactory;
        private readonly MainMenuUIRoot _mainMenuUIRoot;

        private readonly ProjectPresentersFactory _projectPresentersFactory;
        private readonly MainMenuPopupService _mainMenuPopupService;       

        private OptionsMenuPopupPresenter _optionsMenuPopupPresenter;
        private LevelMenuPoupPresenter _levelMenuPoupPresenter;
        private Transform _environment;

        // view
        private MainMenuScreenView _mainMenuScreenView;
        private EnvirMainMenuObjView _mainMenuObjView;

        public MainMenuScreenPresenter(
            ViewsFactory viewFactory,
            MainMenuUIRoot mainMenuUIRoot,
            ProjectPresentersFactory projectPresentersFactory,
            MainMenuPopupService mainMenuPopupService,
            Transform environment)           
        {
            _viewsFactory = viewFactory;
            _mainMenuUIRoot = mainMenuUIRoot;
            _projectPresentersFactory = projectPresentersFactory;
            _mainMenuPopupService = mainMenuPopupService;
            _environment = environment;
        }

        public void Initialize()
        {
            _mainMenuScreenView = _viewsFactory.Create<MainMenuScreenView>(
                ViewIDs.MainMenuScreenView, _mainMenuUIRoot.HUDLayer);

            _mainMenuScreenView.OpenOptionsPopupButtonCliced += OnOpenOptionsPopup;
            _mainMenuScreenView.OpenLevelsPopupButtonCliced += OnOpenLevelsPopup;

            SpawnBackgroundView();
        }

        public void Dispose()
        {
            _mainMenuScreenView.OpenOptionsPopupButtonCliced -= OnOpenOptionsPopup;
            _mainMenuScreenView.OpenLevelsPopupButtonCliced -= OnOpenLevelsPopup;
        }

        private void OnOpenOptionsPopup()
        {
            _mainMenuPopupService.OpenOptionsMenuPopupPresenter();
        }

        private void OnOpenLevelsPopup()
        {
            _mainMenuPopupService.OpenLevelMenuPoupPresenter();
        }

        private void SpawnBackgroundView()
        {
            _mainMenuObjView = _viewsFactory.Create<EnvirMainMenuObjView>(ViewIDs.EnvirMainMenuObjView,_environment);
        }
    }
}
