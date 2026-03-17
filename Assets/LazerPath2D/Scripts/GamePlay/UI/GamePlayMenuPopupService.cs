using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonUI.Popup;
using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using Assets.LazerPath2D.Scripts.GamePlay.UI.GameOverMenu;
using Assets.LazerPath2D.Scripts.GamePlay.UI.PauseMenu;
using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.UI
{
    public class GamePlayMenuPopupService : PopupService
    {
        public event Action<PauseMenuPopupPresenter> OpenedPauseMenu;
        public event Action<PauseMenuPopupPresenter> ClosedPauseMenu;

        public event Action<GameOverPopupPresenter> OpenedGameOverPopup;
        public event Action<GameOverPopupPresenter> ClosedGameOverPopup;

        private readonly GamePlayUIRoot _gamePlayUIRoot;

        public GamePlayMenuPopupService(
            ViewsFactory viewsFactory, 
            ProjectPresentersFactory presentersFactory,
            GamePlayUIRoot gamePlayUIRoot,
            PlaySound playSound) : base(viewsFactory, presentersFactory, playSound)
        {
            _gamePlayUIRoot = gamePlayUIRoot;
           
        }


        protected override Transform PopupLayer => _gamePlayUIRoot.PopupsLayer;
       

        public PauseMenuPopupPresenter OpenPauseMenuPopupPresenter()
        {
            PauseMenuPopupView pauseMenuPopupView = ViewsFactory.Create<PauseMenuPopupView>(ViewIDs.PauseMenuPopupView, PopupLayer);
            

            PauseMenuPopupPresenter pauseMenuPopupPresenter = _presentersFactory.CreatePauseMenuPopupPresenter(pauseMenuPopupView);

            OnPopupCreated(pauseMenuPopupPresenter, pauseMenuPopupView, () => ClosedPauseMenu?.Invoke(pauseMenuPopupPresenter));

            OpenedPauseMenu?.Invoke(pauseMenuPopupPresenter);

            return pauseMenuPopupPresenter;
        }

        public GameOverPopupPresenter OpenedGameOverPopupPresenter(Action callBack = null ) 
        {
            GameOverPopupView gameOverPopupView = ViewsFactory.Create<GameOverPopupView>(ViewIDs.GameOverPopupView, PopupLayer);

            GameOverPopupPresenter gameOverPopupPresenter = _presentersFactory.CreateGameOverPopupPresenter(gameOverPopupView);

            OnPopupCreated(gameOverPopupPresenter, gameOverPopupView, () => ClosedGameOverPopup?.Invoke(gameOverPopupPresenter));

            OpenedGameOverPopup?.Invoke(gameOverPopupPresenter);

            return gameOverPopupPresenter;
        }
    }
}
