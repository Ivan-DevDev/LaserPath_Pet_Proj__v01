using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonServices.ConfigsManagment;
using Assets.LazerPath2D.Scripts.CommonServices.CoroutinePerformer;
using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using Assets.LazerPath2D.Scripts.CommonServices.LevelsService;
using Assets.LazerPath2D.Scripts.CommonServices.SceneManagment;
using Assets.LazerPath2D.Scripts.CommonUI.PlaySounds;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using Assets.LazerPath2D.Scripts.DI;
using Assets.LazerPath2D.Scripts.GamePlay.UI.GameOverMenu;
using Assets.LazerPath2D.Scripts.GamePlay.UI.PauseMenu;
using Assets.LazerPath2D.Scripts.MainMenu.UI.LevelsMenuPopup;
using Assets.LazerPath2D.Scripts.MainMenu.UI.OptionsMenuPopup;

namespace Assets.LazerPath2D.Scripts.CommonUI.Presenter
{
    public class ProjectPresentersFactory
    {
        private DIContainer _container;

        public ProjectPresentersFactory(DIContainer container)
        {
            _container = container;
        }

        public OptionsMenuPopupPresenter CreateOptionsMenuPopupPresenter(
            OptionsMenuPopupView optionsMenuPopupView)
        {
            return new OptionsMenuPopupPresenter(
                _container.Resolve<PlaySound>(),
                this,
                optionsMenuPopupView,
                _container.Resolve<ICoroutinePerformer>(),
                _container.Resolve<GameSettingsDataProvider>());
        }

        public LevelTilePresenter CreateLevelTilePresenter(int levelNumber, LevelTileView levelTileView)
        {
            LevelTilePresenter levelTilePresenter = new LevelTilePresenter(

              _container.Resolve<CompletedLevelsService>(),
              _container.Resolve<ViewsFactory>(),
              this,
              _container.Resolve<SceneSwitcher>(),              
                levelNumber,
                levelTileView,
                _container.Resolve<PlaySound>());

            return levelTilePresenter;
        }

        public LevelMenuPoupPresenter CreateLevelMenuPoupPresenter(LevelMenuPoupView levelMenuPoupView, LevelTileListView levelTileListView)
        {
            LevelMenuPoupPresenter levelMenuPoupViewPresenter = new LevelMenuPoupPresenter(
                this,
                _container.Resolve<ViewsFactory>(),
                _container.Resolve<ConfigsProviderService>().LevelConfigList,
                levelMenuPoupView,
                levelTileListView,
                _container.Resolve<ICoroutinePerformer>(),
                _container.Resolve<PlaySound>());

            return levelMenuPoupViewPresenter;
        }

        public StarsViewInLevelPresenter CreateStarsViewInLevelPresenter()
        {
            StarsViewInLevelPresenter starsViewInLevelPresenter = new StarsViewInLevelPresenter(
                _container.Resolve<ViewsFactory>());
               
            return starsViewInLevelPresenter;
        }

        public PauseMenuPopupPresenter CreatePauseMenuPopupPresenter(PauseMenuPopupView pauseMenuPopupView)
        {
            PauseMenuPopupPresenter pauseMenuPopupPresenter = new PauseMenuPopupPresenter(
                pauseMenuPopupView,
                _container.Resolve<ICoroutinePerformer>(),
                _container.Resolve<PlaySound>(),
                _container.Resolve<GameSettingsDataProvider>());

            return pauseMenuPopupPresenter;
        }

        public GameOverPopupPresenter CreateGameOverPopupPresenter(GameOverPopupView gameOverPopupView)
        {
            GameOverPopupPresenter gameOverPopupPresenter = new GameOverPopupPresenter(
                gameOverPopupView,
                _container.Resolve<ICoroutinePerformer>(), 
                _container.Resolve<PlaySound>());
            
            return gameOverPopupPresenter;
        }

        public PlaySoundViewPresenter CreatePlaySoundViewPresenter(PlaySound playSound, PlaySoundView playSoundView)
        {
            PlaySoundViewPresenter playSoundViewPresenter = new PlaySoundViewPresenter(playSound, playSoundView);

            return playSoundViewPresenter;
        }
    }
}
