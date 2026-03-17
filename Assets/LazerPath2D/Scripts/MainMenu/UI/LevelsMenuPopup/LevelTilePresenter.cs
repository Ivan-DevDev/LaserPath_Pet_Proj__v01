using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonServices.LevelsService;
using Assets.LazerPath2D.Scripts.CommonServices.SceneManagment;
using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using Assets.LazerPath2D.Scripts.CommonUI.Stars;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using System.Collections.Generic;


namespace Assets.LazerPath2D.Scripts.MainMenu.UI.LevelsMenuPopup
{
    public class LevelTilePresenter : ISubscribedPresenter
    {
        // model

        private const int FirstLevel = 1;

        private readonly CompletedLevelsService _completedLevelsService;
        
        private readonly ViewsFactory _viewsFactory;
        private readonly ProjectPresentersFactory _projectPresentersFactory;
        private SceneSwitcher _sceneSwitcher;
        private readonly PlaySound _playSound;

        private readonly int _levelNumber;
        private StarsViewInLevelPresenter _starsViewInLevelPresenter;


        // view
        private LevelTileView _levelTileView;
        private StarView _starView;

        private bool _isBlocked;

        public LevelTilePresenter(
            CompletedLevelsService completedLevelsService,
            ViewsFactory viewsFactory,
            ProjectPresentersFactory projectPresentersFactory,
            SceneSwitcher sceneSwitcher,
           
            int levelNumber,
            LevelTileView levelTileView,
            PlaySound playSound)
        {
            _completedLevelsService = completedLevelsService;
            _viewsFactory = viewsFactory;
            _projectPresentersFactory = projectPresentersFactory;
            _sceneSwitcher = sceneSwitcher;
           
            _levelNumber = levelNumber;
            _levelTileView = levelTileView;
            _playSound = playSound;
        }

        public LevelTileView LevelTileView => _levelTileView;

        public void Initialize()
        { 
            _starsViewInLevelPresenter = new StarsViewInLevelPresenter(_viewsFactory);


            _isBlocked = _levelNumber != FirstLevel && PreviousLevelCompleted() == false;

            _levelTileView.SetLevel(_levelNumber.ToString());


            if (_isBlocked)
            {
                _levelTileView.SetBlock();
            }
            else
            {
                int amountStarsInLevel = _completedLevelsService.GetStarsNodesInLevel(_levelNumber);
               
                List<StarView> starViews = _starsViewInLevelPresenter.GetSpawnStarView(_levelTileView, amountStarsInLevel);

                if (_completedLevelsService.IsLevelCompleted(_levelNumber))
                {
                    int activeStarsInLevel = _completedLevelsService.GetActiveStarsInLevel(_levelNumber).Value;

                    _starsViewInLevelPresenter.SetActiveStarsInLevel(starViews, activeStarsInLevel);


                    _levelTileView.SetComplete();
                }
                else
                {
                    _levelTileView.SetActive();
                }
            }

            _levelTileView.Clicked += OnViewClicked;
        }

        public void Dispose()
        {
            _levelTileView.Clicked -= OnViewClicked;
        }
        public void Subscribe()
        {
            _levelTileView.Clicked += OnViewClicked;
        }

        public void Unsubscribe()
        {
            _levelTileView.Clicked -= OnViewClicked;
        }

        private void OnViewClicked()
        {
            if (_isBlocked)
            {

                return;
            }

           
            _sceneSwitcher.ProcessSwitchSceneFor(new OutputMainMenuArgs(new GamePlayInputArgs(_levelNumber)));

        }

        private bool PreviousLevelCompleted() => _completedLevelsService.IsLevelCompleted(_levelNumber - 1);
       
    }
}
