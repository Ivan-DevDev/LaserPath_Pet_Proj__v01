using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonServices.CoroutinePerformer;
using Assets.LazerPath2D.Scripts.CommonUI.Popup;
using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using Assets.LazerPath2D.Scripts.Configs.GamePlay.Levels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.MainMenu.UI.LevelsMenuPopup
{
    public class LevelMenuPoupPresenter : PopupPresenterBase
    {
        //model
        private const string HeaderTitleText = "LEVEL SELECTION";
        private const string Progress = "_Progress";

        private readonly ProjectPresentersFactory _projectPresentersFactory;
        private readonly ViewsFactory _viewsFactory;
        private readonly LevelConfigList _levelConfigList;

        private List<LevelTilePresenter> _levelTilePresenterList = new();

        private ICoroutinePerformer _coroutinePerformer;
        private Coroutine _timerToCompleteCoroutine;

        //view
        private LevelMenuPoupView _levelMenuPoupView;
        private LevelTileListView _levelTileListView;

        public LevelMenuPoupPresenter(
            ProjectPresentersFactory projectPresentersFactory,
            ViewsFactory viewsFactory,
            LevelConfigList levelConfigList,
            LevelMenuPoupView levelMenuPoupView,
            LevelTileListView levelTileListView,
            ICoroutinePerformer coroutinePerformer,
            PlaySound playSound) : base(coroutinePerformer, playSound)
        {
            _projectPresentersFactory = projectPresentersFactory;
            _viewsFactory = viewsFactory;
            _levelConfigList = levelConfigList;
            _coroutinePerformer = coroutinePerformer;

            _levelMenuPoupView = levelMenuPoupView;
            _levelTileListView = levelTileListView;
        }

        protected override PopupViewBase PopupView => _levelMenuPoupView;

        public override void Initialize()
        {
            base.Initialize();

            _levelMenuPoupView.SetHeaderTitleText(HeaderTitleText);

            for (int i = 0; i < _levelConfigList.MaxLevelNumber; i++)
            {
                LevelTileView levelTileView = _viewsFactory.Create<LevelTileView>(ViewIDs.LevelTileView, _levelTileListView.Parent);

                _levelTileListView.AddElement(levelTileView);

                int levelNumber = i + 1;

                LevelTilePresenter levelTilePresenter = _projectPresentersFactory.CreateLevelTilePresenter(levelNumber, levelTileView);

                levelTilePresenter.Initialize();

                _levelTilePresenterList.Add(levelTilePresenter);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (LevelTilePresenter levelTilePresenter in _levelTilePresenterList)
            {
                _levelTileListView.RemoveElement(levelTilePresenter.LevelTileView);

                _viewsFactory.Release(levelTilePresenter.LevelTileView);

                levelTilePresenter.Dispose();
            }

            _levelTilePresenterList.Clear();

            _viewsFactory.Release(_levelMenuPoupView);

            if (_timerToCompleteCoroutine != null)
                _coroutinePerformer.StopPerform(_timerToCompleteCoroutine);

        }

        protected override void OnPreShow()
        {
            base.OnPreShow();
           
            foreach (LevelTilePresenter levelTilePresenter in _levelTilePresenterList)
            {
                levelTilePresenter.Subscribe();
            }

            if (_levelTileListView.ConturLight != null)
                _levelTileListView.ConturLight.material.SetFloat(Progress, 0);
        }

        protected override void OnPostShow()
        {
            base.OnPostShow();
           
            if (_levelTileListView.ConturLight != null)
                _coroutinePerformer.StartPerform(StartConturLightShow());
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            foreach (LevelTilePresenter levelTilePresenter in _levelTilePresenterList)
            {
                levelTilePresenter.Unsubscribe();
            }
        }

        private IEnumerator StartConturLightShow()
        {
            float maxTime = 0.5f;
            float progress = default;

            while (progress < 1)
            {
                progress += Time.deltaTime / maxTime;

                _levelTileListView.ConturLight.material.SetFloat(Progress, progress);

                yield return null;
            }

            _timerToCompleteCoroutine = null;
        }
    }
}
