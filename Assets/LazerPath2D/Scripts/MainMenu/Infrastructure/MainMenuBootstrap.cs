using Assets.LazerPath2D.Scripts.CommonServices.SceneManagment;
using Assets.LazerPath2D.Scripts.DI;
using UnityEngine;
using System.Collections;
using Assets.LazerPath2D.Scripts.MainMenu.UI;
using Assets.LazerPath2D.Scripts.CommonServices.AssetsManagment;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using Assets.LazerPath2D.Scripts.MainMenu.UI.MainMenuScreen;
using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;

namespace Assets.LazerPath2D.Scripts.MainMenu.Infrastructure
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        [SerializeField] Transform _environment;
        private DIContainer _container;
        private ResourcesAssetLoader _resourcesAssetLoader;
        private ViewsFactory _viewsFactory;
        private PlaySound _playSound;
        private AudioHandler _audioHandler;

        public IEnumerator Run(DIContainer container, MainMenuInputArgs mainMenuInputArgs)
        {
            _container = container;
            _resourcesAssetLoader = _container.Resolve<ResourcesAssetLoader>();
            _viewsFactory = _container.Resolve<ViewsFactory>();
            _playSound = _container.Resolve<PlaySound>();
            _audioHandler = _container.Resolve<AudioHandler>();            

            ProcessRegistrations();

            InitializeUI();
           
            if (_playSound.MusicSource.isPlaying == false)
                _playSound.ToPlayAudoiClipsInOder();
           
            _audioHandler.SetSettingsFromData();

            yield return new WaitForSeconds(0.5f);
        }

        private void InitializeUI()
        {
            MainMenuScreenPresenter mainMenuScreenPresenter = _container.Resolve<MainMenuScreenPresenter>();

            mainMenuScreenPresenter.Initialize();

        }

        private void ProcessRegistrations()
        {
            _container.RegisterAsSingle(c => new MainMenuPresenterFactory(_container));

            _container.RegisterAsSingle(c =>
            {
                MainMenuUIRoot mainMenuUIRootPrefab = _resourcesAssetLoader.LoadResource<MainMenuUIRoot>("MainMenu/UI/MainMenuUIRoot");

                MainMenuUIRoot mainMenuUIRoot = Instantiate(mainMenuUIRootPrefab);

                return mainMenuUIRoot;

            }).NonLazy();

            _container.RegisterAsSingle(c => new MainMenuPopupService(
                _viewsFactory,
                _container.Resolve<ProjectPresentersFactory>(),
                _container.Resolve<MainMenuUIRoot>(),
                _container.Resolve<PlaySound>()));

            _container.RegisterAsSingle(c =>
            {
                MainMenuScreenPresenter mainMenuScreenPresenter = _container.Resolve<MainMenuPresenterFactory>()
                .CreateMainMenuScreenPresenter(
                    _container.Resolve<ViewsFactory>(),
                    _container.Resolve<MainMenuUIRoot>(),
                    _container.Resolve<ProjectPresentersFactory>(),
                    _container.Resolve<MainMenuPopupService>(),
                    _environment);

                return mainMenuScreenPresenter;

            });

            _container.Initialize();
        }
    }
}
