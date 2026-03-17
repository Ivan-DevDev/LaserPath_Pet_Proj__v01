using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using Assets.LazerPath2D.Scripts.CommonUI.Stars;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using Assets.LazerPath2D.Scripts.DI;
using Assets.LazerPath2D.Scripts.GamePlay.Enviroment;
using Assets.LazerPath2D.Scripts.GamePlay.GameState.States;
using Assets.LazerPath2D.Scripts.GamePlay.Managers;
using Assets.LazerPath2D.Scripts.GamePlay.UI.Background;
using Assets.LazerPath2D.Scripts.GamePlay.Utility.Reactive;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LazerPath2D.Scripts.GamePlay.UI.GamePlayMenuScreen
{
    public class GamePlayMenuScreenPresenter : IInitializable, IDisposable, IPresenter
    {
        // model
        private const string _titleLevel = "LEVEL ";
        private IReadOnlyVariable<int> _currentLevelNumber;
        private IReadOnlyVariable<int> _amountActiveStars;
        private IReadOnlyVariable<int> _maxStarsViewInLevel;

        private readonly ViewsFactory _viewsFactory;
        private GamePlayUIRoot _gamePlayUIRoot;

        private GamePlayMenuPopupService _gamePlayMenuPopupService;
        private GamePlayManager _gamePlayManager;

        private Transform _enviroment;

        // view
        private GamePlayMenuScreenView _view;
        private List<StarView> _starViews = new();
        private GamePlayBackgroundView _gamePlayBackgroundView;
        private EnvirBackgroundObjView _envirBackgroundObjView;

        public GamePlayMenuScreenPresenter(
            IReadOnlyVariable<int> currentLevelNumber,
            IReadOnlyVariable<int> amountActiveStars,
            IReadOnlyVariable<int> maxStarsViewInLevel,
            ViewsFactory viewsFactory,
            GamePlayUIRoot gamePlayUIRoot,
            //IReadOnlyVariable<int> currentLevelNumber,
            GamePlayMenuPopupService gamePlayMenuPopupService,
            GamePlayMenuScreenView gamePlayMenuScreenView,
            GamePlayManager gamePlayManager,
            Transform enviroment)
        {
            _currentLevelNumber = currentLevelNumber;
            _amountActiveStars = amountActiveStars;
            _maxStarsViewInLevel = maxStarsViewInLevel;

            _viewsFactory = viewsFactory;
            _gamePlayUIRoot = gamePlayUIRoot;
            _gamePlayMenuPopupService = gamePlayMenuPopupService;
            _view = gamePlayMenuScreenView;

            _gamePlayManager = gamePlayManager;
            _enviroment = enviroment;
        }

        public void Initialize()
        {
            _currentLevelNumber.Changed += OnChangedCurrentLevelNumber; ;
            _amountActiveStars.Changed += OnChangedActiveStars;
            _maxStarsViewInLevel.Changed += OnChangedMaxStarsViewInLevel;

            _view.OpenPausePopupButtonClicked += OnOpenPausePopupButtonClicked;

            _gamePlayManager.ChangedBackgroundColor += OnChangedBackgroundColor;

            SpawnBackgroundView();
        }


        public void Dispose()
        {
            _view.OpenPausePopupButtonClicked -= OnOpenPausePopupButtonClicked;
            _gamePlayManager.ChangedBackgroundColor -= OnChangedBackgroundColor;

            foreach (StarView starView in _starViews)
            {
                _viewsFactory.Release(starView);
            }

            _starViews.Clear();


            if (_gamePlayBackgroundView != null)
                _viewsFactory.Release(_gamePlayBackgroundView);


            if (_currentLevelNumber != null)
                _currentLevelNumber.Changed -= OnChangedCurrentLevelNumber;

            if (_amountActiveStars != null)
                _amountActiveStars.Changed += OnChangedActiveStars;
        }

        public void OnOpenPausePopupButtonClicked()
        {
            if (_gamePlayManager.GameStateMachine != null)
            {
                if (_gamePlayManager.GameStateMachine.CurrentState is GameOverState)
                    return;
            }

            _gamePlayMenuPopupService.OpenPauseMenuPopupPresenter();
        }



        public void SetBackgroundViewColor(Color color) => _gamePlayBackgroundView.SetBackgroundColor(color);
        public void SetBackgroundViewImage(Image image) => _gamePlayBackgroundView.SetBackgroundImage(image);
        private void OnChangedBackgroundColor(Color color)
        {
            _gamePlayBackgroundView.SetBackgroundColor(color);
        }
        public EnvirBackgroundObjView EnvirBackgroundObjView => _envirBackgroundObjView;



        public void RemoveStarView()
        {
            if (_starViews.Count > 0)
            {
                foreach (StarView starView in _starViews)
                    _viewsFactory.Release(starView);

                _starViews.Clear();
            }
        }

        public void UpdateStarsSpawner()
        {
            RemoveStarView();

            SpawnStarView();
        }

        public void UpdateActiveStarsView(int currentActiveStarsValue)
        {
            if (_starViews.Count == 0)
                return;

            if (currentActiveStarsValue > _maxStarsViewInLevel.Value)
                throw new ArgumentOutOfRangeException(nameof(currentActiveStarsValue));

            foreach (StarView starView in _starViews)
                starView.SetDeActive();

            for (int i = 0; i < currentActiveStarsValue; i++)
            {
                _starViews[i].SetActive();
            }
        }

        private void SpawnStarView()
        {
            for (int i = 0; i < _maxStarsViewInLevel.Value; i++)
            {
                Transform parent = _view.StarsViewContainer;

                StarView starView = _viewsFactory.Create<StarView>(ViewIDs.StarGamePlayView, parent);

                starView.SetDeActive();

                _starViews.Add(starView);
            }
        }

        private void OnChangedActiveStars(int preveousValue, int currentActiveStars)
        {
            UpdateActiveStarsView(currentActiveStars);
        }

        private void OnChangedCurrentLevelNumber(int previousValue, int currentValue)
        {
            UpdateTitleLevelText(currentValue);


        }

        private void OnChangedMaxStarsViewInLevel(int previousValue, int currentvalue)
        {
            UpdateStarsSpawner();
            
        }


        private void UpdateTitleLevelText(int currentLevelNumber)
        {
            _view.SetTitleLevelText(_titleLevel + $"{currentLevelNumber.ToString()}");
        }

        private void SpawnBackgroundView()
        {
            _gamePlayBackgroundView = _viewsFactory.Create<GamePlayBackgroundView>(ViewIDs.GamePlayBackgroundView, _enviroment);

            _gamePlayBackgroundView.Initialize();

            _envirBackgroundObjView = _viewsFactory.Create<EnvirBackgroundObjView>(ViewIDs.EnvirBackgroundObjView, _enviroment);
        }
    }
}
