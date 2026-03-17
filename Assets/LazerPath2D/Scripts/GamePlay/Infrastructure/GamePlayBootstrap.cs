using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonServices.ConfigsManagment;
using Assets.LazerPath2D.Scripts.CommonServices.CoroutinePerformer;
using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using Assets.LazerPath2D.Scripts.CommonServices.InputHandler;
using Assets.LazerPath2D.Scripts.CommonServices.LevelsService;
using Assets.LazerPath2D.Scripts.CommonServices.LoadingScreen;
using Assets.LazerPath2D.Scripts.CommonServices.SceneManagment;
using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using Assets.LazerPath2D.Scripts.Configs.GamePlay.Levels;
using Assets.LazerPath2D.Scripts.DI;
using Assets.LazerPath2D.Scripts.GamePlay.GameState;
using Assets.LazerPath2D.Scripts.GamePlay.LaserView;
using Assets.LazerPath2D.Scripts.GamePlay.Level;
using Assets.LazerPath2D.Scripts.GamePlay.Managers;
using Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode;
using Assets.LazerPath2D.Scripts.GamePlay.ScreenClick;
using Assets.LazerPath2D.Scripts.GamePlay.UI;
using Assets.LazerPath2D.Scripts.GamePlay.UI.GamePlayMenuScreen;
using Assets.LazerPath2D.Scripts.GamePlay.Utility;
using Assets.LazerPath2D.Scripts.GamePlay.Utility.Reactive;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using System;

namespace Assets.LazerPath2D.Scripts.GamePlay.Infrastructure
{
    public class GamePlayBootstrap : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private LayerMask _interactableLayer; 
        [SerializeField] private float _maxRayLength = 10f;
        [SerializeField] private float _speedRotateNodes;
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private Transform _enviroment;

        private DIContainer _container;
        private GamePlayInputArgs _gamePlayInputArgs;

        private IInputClickHandler _inputClickHandler;
        private ScreenClickHandler _screenClickHandler;
        private ICoroutinePerformer _coroutinePerformer;
        private GamePlayManager _gamePlayManager;
        private PlaySound _playSound;
        private AudioHandler _audioHandler;

        private ReactiveVariable<int> _currentLevelNumber;
        private ReactiveVariable<int> _amountActiveStars;
        private ReactiveVariable<int> _maxStarsViewInLevel;

        private bool _isRegistered;

        public IEnumerator Run(DIContainer container, GamePlayInputArgs gamePlayInputArgs)
        {
            if (_cinemachineCamera == null)
                throw new ArgumentNullException($" No component {nameof(_cinemachineCamera)} !!!");

            _container = container;
            _gamePlayInputArgs = gamePlayInputArgs;
            _coroutinePerformer = _container.Resolve<ICoroutinePerformer>();
            _playSound =_container.Resolve<PlaySound>();
            _audioHandler = _container.Resolve<AudioHandler>();

            _currentLevelNumber = new ReactiveVariable<int>();
            _amountActiveStars = new ReactiveVariable<int>();
            _maxStarsViewInLevel = new ReactiveVariable<int>();

            ProcessRegistrations();
           

            yield return null;

            InitializeUI();

            _container.Resolve<ILoadingCurtain>().Hide();

            _isRegistered = true;

         
            if (_playSound.MusicSource.isPlaying == false)
                _playSound.ToPlayAudoiClipsInOder();

           
            _audioHandler.SetSettingsFromData();


            _gamePlayManager.ToStartGameLevel();
        }

        private void InitializeUI()
        {


        }

        private void ProcessRegistrations()
        {
            // InputSystem
            _container.RegisterAsSingle<IInputClickHandler>(c => new InputMouseHandler()).NonLazy();
            _inputClickHandler = _container.Resolve<IInputClickHandler>();

            _container.RegisterAsSingle(c => new ScreenClickHandler(_inputClickHandler, _interactableLayer));
            _screenClickHandler = _container.Resolve<ScreenClickHandler>();

            // GamePlay
            _container.RegisterAsSingle<IReflectionNodeCalculator>(c => new ReflectionDirectionNodeCalculator());
            _container.RegisterAsSingle<IDetectorNode>(c => new RayTracerToDetectNode());

            _container.RegisterAsSingle(c => new HandlerDetectorDataForLaserViewPoints());

            _container.RegisterAsSingle(c => new LevelFactory(_container));

            _container.RegisterAsSingle(c => new LaserVisualizerFactory());

            _container.RegisterAsSingle(c => new GameStateMachineFactory(_screenClickHandler));

            _container.RegisterAsSingle(c => new LevelSpawner(c.Resolve<LevelFactory>()));

            _container.RegisterAsSingle(c => new CameraManager(_cinemachineCamera, c.Resolve<ConfigsProviderService>().CameraConfig));

            // GamePlay UI
            _container.RegisterAsSingle(c => new GamePlayMenuScreenfactory(c));

            _container.RegisterAsSingle(c =>
            {
                GamePlayUIRoot gamePlayUIRootPrefab = Resources.Load<GamePlayUIRoot>("GamePlay/UI/GamePlayUIRoot");
                GamePlayUIRoot gamePlayUIRoot = Instantiate(gamePlayUIRootPrefab);

                return gamePlayUIRoot;
            }).NonLazy();

            GamePlayMenuScreenView gamePlayMenuScreenView = _container.Resolve<ViewsFactory>()
                .Create<GamePlayMenuScreenView>(
                ViewIDs.GamePlayMenuScreenView,
                _container.Resolve<GamePlayUIRoot>().HUDLayer);


            _container.RegisterAsSingle(c => new GamePlayMenuPopupService(
                _container.Resolve<ViewsFactory>(),
                _container.Resolve<ProjectPresentersFactory>(),
                _container.Resolve<GamePlayUIRoot>(),
                _container.Resolve<PlaySound>()));


            _container.RegisterAsSingle(c =>
            {
                GamePlayMenuScreenPresenter gamePlayMenuScreenPresenter =
                   _container.Resolve<GamePlayMenuScreenfactory>().CreateGamePlayMenuScreenPresenter(
                       _currentLevelNumber,
                       _amountActiveStars,
                       _maxStarsViewInLevel,
                    _container.Resolve<ViewsFactory>(),
                    _container.Resolve<GamePlayUIRoot>(),
                    _container.Resolve<GamePlayMenuPopupService>(),
                    gamePlayMenuScreenView,
                    _container.Resolve<GamePlayManager>(),
                    _enviroment);

                return gamePlayMenuScreenPresenter;

            }).NonLazy();


            // GameManager           
            _container.RegisterAsSingle(c => new GamePlayManager(
                _interactableLayer,
                _maxRayLength,
                _speedRotateNodes,
                _screenClickHandler,
                _playSound,
                _container.Resolve<GameStateMachineFactory>(),
                 _container.Resolve<LaserVisualizerFactory>(),
                 _container.Resolve<LevelSpawner>(),

                 _gamePlayInputArgs,
                 _currentLevelNumber,
                 _amountActiveStars,
                 _maxStarsViewInLevel,

                 _container.Resolve<IReflectionNodeCalculator>(),
                 _container.Resolve<IDetectorNode>(),
                 _container.Resolve<HandlerDetectorDataForLaserViewPoints>(),
                _container.Resolve<GamePlayMenuPopupService>(),
                 _container.Resolve<ICoroutinePerformer>(),
                 _container.Resolve<CompletedLevelsService>(),
                 _container.Resolve<PlayerDataProvider>(),
                 _container.Resolve<SceneSwitcher>(),
                 _container.Resolve<ConfigsProviderService>(),
                 _container.Resolve<CameraManager>()                
                 ));

            _gamePlayManager = _container.Resolve<GamePlayManager>();

           
            _container.Initialize();
        }

        private void Update()
        {
            if (_isRegistered == false)
                return;

            float deltaTime = Time.deltaTime;

            _inputClickHandler.Update();
            _gamePlayManager.ToUpdate(deltaTime);
        }
    }
}
