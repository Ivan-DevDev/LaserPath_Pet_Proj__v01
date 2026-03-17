using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonServices.CoroutinePerformer;
using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using Assets.LazerPath2D.Scripts.CommonUI.PlaySounds;
using Assets.LazerPath2D.Scripts.CommonUI.Popup;
using Assets.LazerPath2D.Scripts.MainMenu.UI.OptionsMenuPopup;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.UI.PauseMenu
{
    public class PauseMenuPopupPresenter : PopupPresenterBase
    {
        public event Action ClickedRestartLevel;
        public event Action ClickedMainMenu;

        // model
        private const string Progress = "_Progress";
        private PlaySoundViewPresenter _playSoundViewPresenter;
        private PlaySound _playSound;
        private GameSettingsDataProvider _gameSettingsDataProvider;

        private ICoroutinePerformer _coroutinePerformer;
        private Coroutine _timerToCompleteCoroutine;

        // view
        private PauseMenuPopupView _pauseMenuPopupView;


        public PauseMenuPopupPresenter(
            PauseMenuPopupView pauseMenuPopupView,
            ICoroutinePerformer coroutinePerformer,
            PlaySound playSound,
            GameSettingsDataProvider gameSettingsDataProvider) : base(coroutinePerformer, playSound)
        {
           
            _pauseMenuPopupView = pauseMenuPopupView;
            _playSound = playSound;
            _gameSettingsDataProvider = gameSettingsDataProvider;
            _coroutinePerformer = coroutinePerformer;
        }

        protected override PopupViewBase PopupView => _pauseMenuPopupView;

        public override void Initialize()
        {
            base.Initialize();

            _playSoundViewPresenter = new PlaySoundViewPresenter(_playSound, _pauseMenuPopupView.PlaySoundView);

            _playSoundViewPresenter.Initialize();

            _pauseMenuPopupView.CloseButton.onClick.AddListener(OnCloseButtonClicked);
            _pauseMenuPopupView.RestartButton.onClick.AddListener(OnRestartButtonClicked);
            _pauseMenuPopupView.MainMenuButton.onClick.AddListener(OnMainMenuButtonCliked);

        }

        public override void Dispose()
        {
            base.Dispose();

            _playSoundViewPresenter.Dispose();

            _pauseMenuPopupView.CloseButton.onClick.RemoveListener(OnCloseButtonClicked);
            _pauseMenuPopupView.RestartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _pauseMenuPopupView.MainMenuButton.onClick.RemoveListener(OnMainMenuButtonCliked);

            if (_timerToCompleteCoroutine != null)
                _coroutinePerformer.StopPerform(_timerToCompleteCoroutine);
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            if (_pauseMenuPopupView.ConturLightBody != null)
                _pauseMenuPopupView.ConturLightBody.material.SetFloat(Progress, 0f);
        }

        protected override void OnPostShow()
        {
            base.OnPostShow();

            if (_pauseMenuPopupView.ConturLightBody != null)
                _coroutinePerformer.StartPerform(StartConturLightShow());
        }



        protected override void OnPostHide()
        {
            base.OnPostHide();

            _gameSettingsDataProvider.Save();
        }

        private void OnMainMenuButtonCliked()
        {
            _gameSettingsDataProvider.Save();

            ClickedMainMenu?.Invoke();

            OnCloseRequest();
        }

        private void OnRestartButtonClicked()
        {
            ClickedRestartLevel?.Invoke();

            OnCloseRequest();
        }

        private void OnCloseButtonClicked()
        {
            OnCloseRequest();
        }


        private IEnumerator StartConturLightShow()
        {
            float maxTime = 0.5f;
            float progress = default;

            while (progress < 1)
            {
                progress += Time.deltaTime / maxTime;

                _pauseMenuPopupView.ConturLightBody.material.SetFloat(Progress, progress);

                yield return null;
            }

            _timerToCompleteCoroutine = null;
        }
    }
}
