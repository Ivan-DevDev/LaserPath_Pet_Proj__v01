using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonServices.ConfigsManagment;
using Assets.LazerPath2D.Scripts.CommonServices.CoroutinePerformer;
using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using Assets.LazerPath2D.Scripts.CommonServices.LevelsService;
using Assets.LazerPath2D.Scripts.CommonServices.SceneManagment;
using Assets.LazerPath2D.Scripts.GamePlay.GameState;
using Assets.LazerPath2D.Scripts.GamePlay.Infrastructure;
using Assets.LazerPath2D.Scripts.GamePlay.LaserView;
using Assets.LazerPath2D.Scripts.GamePlay.Level;
using Assets.LazerPath2D.Scripts.GamePlay.Node;
using Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode;
using Assets.LazerPath2D.Scripts.GamePlay.ScreenClick;
using Assets.LazerPath2D.Scripts.GamePlay.UI;
using Assets.LazerPath2D.Scripts.GamePlay.UI.GameOverMenu;
using Assets.LazerPath2D.Scripts.GamePlay.UI.PauseMenu;
using Assets.LazerPath2D.Scripts.GamePlay.Utility;
using Assets.LazerPath2D.Scripts.GamePlay.Utility.Reactive;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Managers
{
    public class GamePlayManager : IDisposable, IUpdatable
    {
        public event Action<Color> ChangedBackgroundColor;

        private LayerMask _interactableLayer;
        private float _maxRayLength = 10f;
        private float _speedRotation;

        private ScreenClickHandler _screenClickHandler;
        private PlaySound _playSound;

        private GameStateMachineFactory _gameStateMachineFactory;
        private LaserVisualizerFactory _laserVisualizerFactory;

        private LevelSpawner _levelSpawner;

        private GamePlayInputArgs _gamePlayInputArgs;

        private ReactiveVariable<int> _currentLevelNumber;
        private ReactiveVariable<int> _amountActiveStars;
        private ReactiveVariable<int> _maxStarsViewInLevel;

        private int _maxLevelNumber;

        private NodesDetector _nodesDetector;
        private IReflectionNodeCalculator _reflectionDirectionNodeCalculator;
        private IDetectorNode _rayDetectorNode;
        private HandlerDetectorDataForLaserViewPoints _handlerDetectorDataForLaserViewPoints;

        private ICoroutinePerformer _coroutinePerformer;
        private Coroutine _coroutineDelayStartNewLevel;

        private GameStateMachine _gameStateMachine;

        private GamePlayMenuPopupService _gamePlayMenuPopupService;

        private SceneSwitcher _sceneSwitcher;
        private CompletedLevelsService _completedLevelsService;
        private PlayerDataProvider _playerDataProvider;

        private IReadOnlyList<INode> _nodes;
        private List<LaserVisualizer> _laserVisualizers;
        private ConfigsProviderService _configsProviderService;


        private Color _laserColor = default;
        private Color _contourColor = default;
        private Color _activeFillColor = default;
        private Color _deactiveFillColor = default;
        private Color _otherColor = default;

        private Color _gamePlayBackgroundColor;

        private CameraManager _cameraManager;
        private Camera _cameraMain;

        public GamePlayManager(
            LayerMask interactableLayer,
            float maxRayLength,
            float speedRotation,
            ScreenClickHandler screenClickHandler,
            PlaySound playSound,
            GameStateMachineFactory gameStateMachineFactory,
            LaserVisualizerFactory laserVisualizerFactory,
            LevelSpawner levelSpawner,

            GamePlayInputArgs gamePlayInputArgs,
            ReactiveVariable<int> currentLevelNumber,
            ReactiveVariable<int> amountActiveStars,
            ReactiveVariable<int> maxStarsViewInLevel,

            IReflectionNodeCalculator reflectionDirectionNodeCalculator,
            IDetectorNode rayDetectorNode,
            HandlerDetectorDataForLaserViewPoints handlerDetectorDataForLaserViewPoints,
            GamePlayMenuPopupService gamePlayMenuPopupService,
            ICoroutinePerformer coroutinePerformer,
            CompletedLevelsService completedLevelsService,
            PlayerDataProvider playerDataProvider,
            SceneSwitcher sceneSwitcher,
            ConfigsProviderService configsProviderService,
            CameraManager cameraManager)

        {
            _interactableLayer = interactableLayer;
            _maxRayLength = maxRayLength;
            _speedRotation = speedRotation;

            _screenClickHandler = screenClickHandler;
            _playSound = playSound;
            _gameStateMachineFactory = gameStateMachineFactory;
            _laserVisualizerFactory = laserVisualizerFactory;

            _levelSpawner = levelSpawner;

            _gamePlayInputArgs = gamePlayInputArgs;

            _currentLevelNumber = currentLevelNumber;
            _amountActiveStars = amountActiveStars;
            _maxStarsViewInLevel = maxStarsViewInLevel;

            _maxLevelNumber = _levelSpawner.MaxLevelNumber;


            _reflectionDirectionNodeCalculator = reflectionDirectionNodeCalculator;
            _rayDetectorNode = rayDetectorNode;
            _handlerDetectorDataForLaserViewPoints = handlerDetectorDataForLaserViewPoints;

            _gamePlayMenuPopupService = gamePlayMenuPopupService;

            _coroutinePerformer = coroutinePerformer;

            _completedLevelsService = completedLevelsService;
            _playerDataProvider = playerDataProvider;
            _sceneSwitcher = sceneSwitcher;
            _configsProviderService = configsProviderService;
            _cameraManager = cameraManager;

            _screenClickHandler.ClickedOnNode += OnClickedOnNode;

            _gamePlayMenuPopupService.OpenedPauseMenu += OnOpenedPauseMenu;
            _gamePlayMenuPopupService.ClosedPauseMenu += OnClosedPauseMenu;

            _gamePlayMenuPopupService.OpenedGameOverPopup += OnOpenedGameOverPopup;
            _gamePlayMenuPopupService.ClosedGameOverPopup += OnClosedGameOverPopup;

            if (_speedRotation == 0)
                _speedRotation = 100f;

            _cameraMain = Camera.main;
        }

        public GameStateMachine GameStateMachine => _gameStateMachine;

        public void Dispose()
        {
            _screenClickHandler.ClickedOnNode -= OnClickedOnNode;

            _gamePlayMenuPopupService.OpenedPauseMenu -= OnOpenedPauseMenu;
            _gamePlayMenuPopupService.ClosedPauseMenu -= OnClosedPauseMenu;

            _gamePlayMenuPopupService.OpenedGameOverPopup += OnOpenedGameOverPopup;
            _gamePlayMenuPopupService.ClosedGameOverPopup += OnClosedGameOverPopup;

            if (_coroutineDelayStartNewLevel != null)
                _coroutinePerformer.StopPerform(_coroutineDelayStartNewLevel);            
        }

        public void ToStartGameLevel()
        {
            if (_currentLevelNumber.Value == default)
            {
                _currentLevelNumber.Value = _gamePlayInputArgs.LevelNumber;
            }

            _levelSpawner.SpawnLevel(_currentLevelNumber.Value);

            _nodes = _levelSpawner.Nodes;

            SetCameraZoom();

            SetLevelColor(_currentLevelNumber.Value);
            ChangedBackgroundColor?.Invoke(_gamePlayBackgroundColor);

            int amountLaserEmiters = _levelSpawner.CurrentLevel.LaserEmiters.Count;

            _maxStarsViewInLevel.Value = _levelSpawner.CurrentLevel.StarNodes.Count;

            _laserVisualizers = _laserVisualizerFactory.CreateLaserVisualizerList(amountLaserEmiters);

            foreach (LaserVisualizer laserVisualizer in _laserVisualizers)
            {
                laserVisualizer.SetColorLaser(_laserColor);
            }

            _nodesDetector = new NodesDetector(
                 _nodes,
                 _laserVisualizers,
                _reflectionDirectionNodeCalculator,
                _rayDetectorNode,
                _handlerDetectorDataForLaserViewPoints,
                 _interactableLayer,
                _maxRayLength);

            _gameStateMachine = _gameStateMachineFactory.CreateGameStateMachine(
                _nodes,
                _gamePlayMenuPopupService,
                _nodesDetector,
                _amountActiveStars,
                _currentLevelNumber,
                _completedLevelsService,
                _playerDataProvider,
                _cameraManager);

            foreach (INode node in _nodes)
            {
                node.Initialize(
                    _nodesDetector,
                    _playSound,
                    _speedRotation,
                    _contourColor,
                    _activeFillColor,
                    _deactiveFillColor,
                    _otherColor,
                    _laserColor);
            }

            if (_nodes.All(Node => Node.IsInintialized))
            {
                foreach (INode node in _nodes)
                {

                    if (node is EmiterRay emiterRay)
                        emiterRay.StartUpdate();
                }
            }
            else
            {
                throw new ArgumentException(nameof(_nodes));
            }
        }

        public void ToRestartGameLevel()
        {
            _levelSpawner.RemoveSpawnedLevel();

            for (int i = _laserVisualizers.Count - 1; i >= 0; i--)
                UnityEngine.Object.Destroy(_laserVisualizers[i].gameObject);

            _laserVisualizers.Clear();

            _gameStateMachine.Dispose();

            _nodesDetector = null;
            _nodes = null;
        }

        public void GoToMainMenu()
        {
            _sceneSwitcher.ProcessSwitchSceneFor(new OutputGamePlayArgs(new MainMenuInputArgs()));
        }

        public void ToUpdate(float deltaTime)
        {
            _gameStateMachine.ToUpdate(deltaTime);
        }

        private void OnClickedOnNode(INode clickedNode)
        {
            clickedNode.OnClicked();
        }


        private void OnOpenedGameOverPopup(GameOverPopupPresenter presenter)
        {
            presenter.ClickedRestartLevel += OnClickedRestartLevel;
            presenter.ClickedPlayNextLevel += OnClickedPlayNextLevel;
            presenter.ClickedMainMenu += OnClickedMainMenu;
        }

        private void OnClosedGameOverPopup(GameOverPopupPresenter presenter)
        {
            presenter.ClickedRestartLevel -= OnClickedRestartLevel;
            presenter.ClickedPlayNextLevel -= OnClickedPlayNextLevel;
            presenter.ClickedMainMenu -= OnClickedMainMenu;
        }



        private void OnOpenedPauseMenu(PauseMenuPopupPresenter pauseMenuPresenter)
        {
            pauseMenuPresenter.ClickedRestartLevel += OnClickedRestartLevel;
            pauseMenuPresenter.ClickedMainMenu += OnClickedMainMenu;
        }

        private void OnClosedPauseMenu(PauseMenuPopupPresenter pauseMenuPresenter)
        {
            pauseMenuPresenter.ClickedRestartLevel -= OnClickedRestartLevel;
            pauseMenuPresenter.ClickedMainMenu -= OnClickedMainMenu;
        }


        private void OnClickedMainMenu()
        {
            GoToMainMenu();
        }

        private void OnClickedRestartLevel()
        {
            ToRestartGameLevel();

            _coroutineDelayStartNewLevel = _coroutinePerformer.StartPerform(ToDelayStartNewLevel());
        }

        private void OnClickedPlayNextLevel()
        {
            _currentLevelNumber.Value++;

            if (_currentLevelNumber.Value > _maxLevelNumber)
                _currentLevelNumber.Value = 1;

            ToRestartGameLevel();
            _coroutineDelayStartNewLevel = _coroutinePerformer.StartPerform(ToDelayStartNewLevel());
        }

        private IEnumerator ToDelayStartNewLevel()
        {
            yield return new WaitForSeconds(0.5f);

            ToStartGameLevel();
        }

        private void SetCameraZoom()
        {
            if (_cameraMain.orthographic == false)
            {
                _cameraManager.SetCameraPerspectiveFieldOfView(_levelSpawner.CurrentLevel.CameraPerspectiveFieldOfView);
            }
            else
            {
                _cameraManager.SetCameraOrthographicSize(_levelSpawner.CurrentLevel.CameraOrthographicSize);
            }
        }

        private void SetLevelColor(int levelNumber)
        {
            if (_configsProviderService.LevelConfigList.LevelsColorList.Count < 3)
                throw new ArgumentOutOfRangeException($" Not found configs Color {_configsProviderService.LevelConfigList.LevelsColorList.Count} ");

            switch (levelNumber)
            {

                case <= 3:
                    {
                        _laserColor = _configsProviderService.LevelConfigList.LevelsColorList[0].LaserColor;
                        _contourColor = _configsProviderService.LevelConfigList.LevelsColorList[0].ContourColor;
                        _activeFillColor = _configsProviderService.LevelConfigList.LevelsColorList[0].ActiveFillColor;
                        _deactiveFillColor = _configsProviderService.LevelConfigList.LevelsColorList[0].DeactiveFillColor;
                        _otherColor = _configsProviderService.LevelConfigList.LevelsColorList[0].OtherColor;
                        _gamePlayBackgroundColor = _configsProviderService.LevelConfigList.LevelsColorList[0].GamePlayBackgroundColor;

                        break;
                    }
                case <= 6:
                    {
                        _laserColor = _configsProviderService.LevelConfigList.LevelsColorList[1].LaserColor;
                        _contourColor = _configsProviderService.LevelConfigList.LevelsColorList[1].ContourColor;
                        _activeFillColor = _configsProviderService.LevelConfigList.LevelsColorList[1].ActiveFillColor;
                        _deactiveFillColor = _configsProviderService.LevelConfigList.LevelsColorList[1].DeactiveFillColor;
                        _otherColor = _configsProviderService.LevelConfigList.LevelsColorList[1].OtherColor;
                        _gamePlayBackgroundColor = _configsProviderService.LevelConfigList.LevelsColorList[1].GamePlayBackgroundColor;

                        break;
                    }
                case <= 10:
                    {
                        _laserColor = _configsProviderService.LevelConfigList.LevelsColorList[2].LaserColor;
                        _contourColor = _configsProviderService.LevelConfigList.LevelsColorList[2].ContourColor;
                        _activeFillColor = _configsProviderService.LevelConfigList.LevelsColorList[2].ActiveFillColor;
                        _deactiveFillColor = _configsProviderService.LevelConfigList.LevelsColorList[2].DeactiveFillColor;
                        _otherColor = _configsProviderService.LevelConfigList.LevelsColorList[2].OtherColor;
                        _gamePlayBackgroundColor = _configsProviderService.LevelConfigList.LevelsColorList[2].GamePlayBackgroundColor;

                        break;
                    }

                default:
                    {
                        _laserColor = Color.blue;
                        _contourColor = Color.white;
                        _activeFillColor = Color.yellow;
                        _deactiveFillColor = new Color(1, 1, 1, 0.3f);
                        _otherColor = Color.white;
                        _gamePlayBackgroundColor = Color.black;

                        break;
                    }

            }
        }
    }
}
