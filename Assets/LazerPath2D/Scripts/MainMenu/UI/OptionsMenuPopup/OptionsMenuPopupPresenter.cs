using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonServices.CoroutinePerformer;
using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using Assets.LazerPath2D.Scripts.CommonUI.PlaySounds;
using Assets.LazerPath2D.Scripts.CommonUI.Popup;
using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using System.Collections;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.MainMenu.UI.OptionsMenuPopup
{
    public class OptionsMenuPopupPresenter : PopupPresenterBase
    {
        // model
        private const string _headerText = "OPTIONS";
        private const string Progress = "_Progress";

        private readonly ProjectPresentersFactory _projectPresentersFactory;
        private PlaySoundViewPresenter _playSoundViewPresenter;
        private PlaySound _playSound;
        private GameSettingsDataProvider _gameSettingsDataProvider;

        private ICoroutinePerformer _coroutinePerformer;
        private Coroutine _timerToCompleteCoroutine;

        // view
        private OptionsMenuPopupView _optionsMenuPopupView;

        protected override PopupViewBase PopupView => _optionsMenuPopupView;

        public OptionsMenuPopupPresenter(
            PlaySound playSound,
            ProjectPresentersFactory projectPresentersFactory,
            OptionsMenuPopupView optionsMenuPopupView,
            ICoroutinePerformer coroutinePerformer,
            GameSettingsDataProvider gameSettingsDataProvider) : base(coroutinePerformer, playSound)
        {
            _playSound = playSound;
            _projectPresentersFactory = projectPresentersFactory;
            _optionsMenuPopupView = optionsMenuPopupView;
            _gameSettingsDataProvider = gameSettingsDataProvider;

            _coroutinePerformer = coroutinePerformer;
        }

        public override void Initialize()
        {
            base.Initialize();

            _optionsMenuPopupView.SetHeaderText(_headerText);

            _playSoundViewPresenter = new PlaySoundViewPresenter(_playSound, _optionsMenuPopupView.PlaySoundView);

            _playSoundViewPresenter.Initialize();
        }

        public override void Dispose()
        {
            base.Dispose();

            _playSoundViewPresenter.Dispose();

            if (_timerToCompleteCoroutine != null)
                _coroutinePerformer.StopPerform(_timerToCompleteCoroutine);
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            if (_optionsMenuPopupView.ConturLightBoard != null)
                _optionsMenuPopupView.ConturLightBoard.material.SetFloat(Progress, 0);
        }

        protected override void OnPostShow()
        {
            base.OnPostShow();

            if (_optionsMenuPopupView.ConturLightBoard != null)
                _coroutinePerformer.StartPerform(StartConturLightShow());
        }

        protected override void OnPostHide()
        {
            base.OnPostHide();

            _gameSettingsDataProvider.Save();
        }

        private IEnumerator StartConturLightShow()
        {
            float maxTime = 0.5f;
            float progress = default;

            while (progress < 1)
            {
                progress += Time.deltaTime / maxTime;

                _optionsMenuPopupView.ConturLightBoard.material.SetFloat(Progress, progress);

                yield return null;
            }

            _timerToCompleteCoroutine = null;
        }
    }
}
