using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using Assets.LazerPath2D.Scripts.DI;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.MainMenu.UI.MainMenuScreen
{
    public class MainMenuPresenterFactory
    {
        private DIContainer _container;       
        public MainMenuPresenterFactory(DIContainer container)
        {
            _container = container;            
        }

        public MainMenuScreenPresenter CreateMainMenuScreenPresenter(
            ViewsFactory viewsFactory,
            MainMenuUIRoot mainMenuUIRoot,
            ProjectPresentersFactory projectPresentersFactory,
            MainMenuPopupService mainMenuPopupService,
            Transform environment)
        {
            MainMenuScreenPresenter mainMenuScreenPresenter = new MainMenuScreenPresenter(
                viewsFactory,
                mainMenuUIRoot,
                projectPresentersFactory,
                mainMenuPopupService,
                environment);

            return mainMenuScreenPresenter;
        }
    }
}
