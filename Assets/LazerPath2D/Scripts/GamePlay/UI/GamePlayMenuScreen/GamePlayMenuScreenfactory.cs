using Assets.LazerPath2D.Scripts.CommonUI.View;
using Assets.LazerPath2D.Scripts.DI;
using Assets.LazerPath2D.Scripts.GamePlay.Managers;
using Assets.LazerPath2D.Scripts.GamePlay.Utility.Reactive;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.UI.GamePlayMenuScreen
{
    public class GamePlayMenuScreenfactory
    {
        private DIContainer _container;

        public GamePlayMenuScreenfactory(DIContainer container)
        {
            _container = container;
        }

        public GamePlayMenuScreenPresenter CreateGamePlayMenuScreenPresenter(
            IReadOnlyVariable<int> currentLevelNumber,
            IReadOnlyVariable<int> amountActiveStars,
             IReadOnlyVariable<int> maxStarsViewInLevel,
            ViewsFactory viewsFactory,
            GamePlayUIRoot gamePlayUIRoot,
            GamePlayMenuPopupService gamePlayMenuPopupService,
            GamePlayMenuScreenView gamePlayMenuScreenView,
            GamePlayManager gamePlayManager,
            Transform enviroment)
        {
            GamePlayMenuScreenPresenter gamePlayMenuScreenPresenter = new GamePlayMenuScreenPresenter(
                currentLevelNumber,
                amountActiveStars,
                maxStarsViewInLevel,
                viewsFactory,
                gamePlayUIRoot,
                gamePlayMenuPopupService,
                gamePlayMenuScreenView,
                gamePlayManager,
                enviroment);

            return gamePlayMenuScreenPresenter;
        }
    }
}
