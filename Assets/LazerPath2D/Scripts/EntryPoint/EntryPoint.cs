using Assets.LazerPath2D.Scripts.CommonServices.AssetsManagment;
using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonServices.ConfigsManagment;
using Assets.LazerPath2D.Scripts.CommonServices.CoroutinePerformer;
using Assets.LazerPath2D.Scripts.CommonServices.DataManagment;
using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.Repositories;
using Assets.LazerPath2D.Scripts.CommonServices.LevelsService;
using Assets.LazerPath2D.Scripts.CommonServices.LoadingScreen;
using Assets.LazerPath2D.Scripts.CommonServices.SceneManagment;
using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using Assets.LazerPath2D.Scripts.DI;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.LazerPath2D.Scripts.EntryPoint
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Bootstrap _gameBootstrap;
        [SerializeField] private AudioMixer _audioMixer;

        private void Awake()
        {
            SetupAppSettings();

            DIContainer projectContainer = new DIContainer();           

            RegisterResourcesAssetLoader(projectContainer);
            RegisterCoroutinePerformer(projectContainer);
            RegisterLoadingCurtain(projectContainer);
            RegisterSceneLoader(projectContainer);
            RegisterSeneSwitcher(projectContainer);

            RegisterSaveLoadService(projectContainer);
            RegisterPlayerDataProvider(projectContainer);

            RegisterCompletedLevelsService(projectContainer);
            RegisterConfigsProviderService(projectContainer);

            RegisterProjectPresentersFactory(projectContainer);
            RegisterViewsFactory(projectContainer);
            RegisterAudioHandler(projectContainer);
            RegisterPlaySound(projectContainer);

            RegisterGameSettingsDataProvider(projectContainer);


            projectContainer.Initialize();

            projectContainer.Resolve<ICoroutinePerformer>().StartPerform(_gameBootstrap.Run(projectContainer));
        }


        private void SetupAppSettings()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        private void RegisterGameSettingsDataProvider(DIContainer container)
        {
            container.RegisterAsSingle(c => new GameSettingsDataProvider(c.Resolve<ISaveLoadService>()));
        }

        private void RegisterAudioHandler(DIContainer container)
        {
            container.RegisterAsSingle(c => new AudioHandler(_audioMixer,c.Resolve<GameSettingsDataProvider>()));
        }

        private void RegisterPlaySound(DIContainer container)
        {
            container.RegisterAsSingle( c => 
            {
                PlaySound playSoundPrefab = container.Resolve<ResourcesAssetLoader>().LoadResource<PlaySound>("Sound/PlaySound");
                PlaySound playSound = Instantiate(playSoundPrefab);
                
                return playSound;
            });
        }

        private void RegisterProjectPresentersFactory(DIContainer container)
        {
            container.RegisterAsSingle(c => new ProjectPresentersFactory(c));
        }

        private void RegisterViewsFactory(DIContainer container)
        {
            container.RegisterAsSingle(c => new ViewsFactory(c.Resolve<ResourcesAssetLoader>()));
        }

        private void RegisterConfigsProviderService(DIContainer container)
        {
            container.RegisterAsSingle(c => new ConfigsProviderService(c.Resolve<ResourcesAssetLoader>()));
        }

        private void RegisterCompletedLevelsService(DIContainer container)
        {
            container.RegisterAsSingle(c => new CompletedLevelsService(
                c.Resolve<PlayerDataProvider>(), 
                c.Resolve<ConfigsProviderService>())).NonLazy();
        }

        private void RegisterPlayerDataProvider(DIContainer container)
        {
            container.RegisterAsSingle(c => new PlayerDataProvider(c.Resolve<ISaveLoadService>()));
        }

        private void RegisterSaveLoadService(DIContainer container)
        {
            container.RegisterAsSingle<ISaveLoadService>(
                c => new SaveLoadService(new JsonSerializer(), new LocalDataRepository()));
        }

        private void RegisterSeneSwitcher(DIContainer container)
        {
            container.RegisterAsSingle(c => new SceneSwitcher(
                c.Resolve<ICoroutinePerformer>(),
                c.Resolve<ILoadingCurtain>(),
                c.Resolve<ISceneLoader>(),
                c));
        }

        private void RegisterSceneLoader(DIContainer container)

          => container.RegisterAsSingle<ISceneLoader>(c => new DefaultSceneLoader());


        private void RegisterResourcesAssetLoader(DIContainer container)
            => container.RegisterAsSingle(c => new ResourcesAssetLoader());

        private void RegisterCoroutinePerformer(DIContainer container)
        {
            container.RegisterAsSingle<ICoroutinePerformer>(c =>
            {
                ResourcesAssetLoader resourcesAssetLoader = c.Resolve<ResourcesAssetLoader>();
                CoroutinePerformer coroutinePerformerPrefab = resourcesAssetLoader.LoadResource<CoroutinePerformer>(InfrastructureAssetPaths.CoroutinePerformerPath);

                return Instantiate(coroutinePerformerPrefab);
            });
        }

        private void RegisterLoadingCurtain(DIContainer container)
        {
            container.RegisterAsSingle<ILoadingCurtain>(c =>
            {
                ResourcesAssetLoader resourcesAssetLoader = c.Resolve<ResourcesAssetLoader>();
                StandartLoadingCurtain standartLoadingCurtainPrefab = resourcesAssetLoader.LoadResource<StandartLoadingCurtain>(InfrastructureAssetPaths.LoadingCurtainPath);

                return Instantiate(standartLoadingCurtainPrefab);
            });
        }
    }
}
