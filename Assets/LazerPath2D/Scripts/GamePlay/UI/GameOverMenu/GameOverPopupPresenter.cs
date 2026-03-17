using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonServices.CoroutinePerformer;
using Assets.LazerPath2D.Scripts.CommonUI.Popup;
using Assets.LazerPath2D.Scripts.GamePlay.UI.PauseMenu;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.UI.GameOverMenu
{
    public class GameOverPopupPresenter : PopupPresenterBase
    {
        public event Action ClickedRestartLevel;
        public event Action ClickedMainMenu;
        public event Action ClickedPlayNextLevel;

        // model
        private const string Progress = "_Progress";

        private ICoroutinePerformer _coroutinePerformer;
        private Coroutine _timerToCompleteCoroutine;

        // view
        private GameOverPopupView _gameOverPopupView;


        public GameOverPopupPresenter(
            GameOverPopupView gameOverPopupView, 
            ICoroutinePerformer coroutinePerformer,
            PlaySound playSound) : base (coroutinePerformer, playSound)
        {
            _gameOverPopupView = gameOverPopupView;
            _coroutinePerformer = coroutinePerformer;
        }

        protected override PopupViewBase PopupView => _gameOverPopupView;

        public override void Initialize()
        {
            base.Initialize();           

            _gameOverPopupView.RestartButton.onClick.AddListener(OnRestartButtonClicked);
            _gameOverPopupView.MainMenuButton.onClick.AddListener(OnMainMenuButtonCliked);
            _gameOverPopupView.PlayNextLevelButton.onClick.AddListener(OnPlayNextLevelClicked);
        }

        public override void Dispose()
        {
            base.Dispose();

            _gameOverPopupView.RestartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _gameOverPopupView.MainMenuButton.onClick.RemoveListener(OnMainMenuButtonCliked);
            _gameOverPopupView.PlayNextLevelButton.onClick.RemoveListener(OnPlayNextLevelClicked);


            if (_timerToCompleteCoroutine != null)
                _coroutinePerformer.StopPerform(_timerToCompleteCoroutine);
        }


        protected override void OnPreShow()
        {
            base.OnPreShow();

            if (_gameOverPopupView.ConturLightBody != null)
                _gameOverPopupView.ConturLightBody.material.SetFloat(Progress, 0f);
        }

        protected override void OnPostShow()
        {
            base.OnPostShow();

            if (_gameOverPopupView.ConturLightBody != null)
                _coroutinePerformer.StartPerform(StartConturLightShow());
        }


        private void OnPlayNextLevelClicked()
        {
            ClickedPlayNextLevel?.Invoke();

            OnCloseRequest();
        }

        private void OnMainMenuButtonCliked()
        {
            ClickedMainMenu?.Invoke();

            OnCloseRequest();
        }

        private void OnRestartButtonClicked()
        {
            ClickedRestartLevel?.Invoke();

            OnCloseRequest();
        }

        private IEnumerator StartConturLightShow()
        {
            float maxTime = 0.5f;
            float progress = default;

            while (progress < 1)
            {
                progress += Time.deltaTime / maxTime;

                _gameOverPopupView.ConturLightBody.material.SetFloat(Progress, progress);

                yield return null;
            }

            _timerToCompleteCoroutine = null;
        }
    }
}
