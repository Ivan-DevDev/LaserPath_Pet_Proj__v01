using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using Assets.LazerPath2D.Scripts.CommonServices.LevelsService;
using Assets.LazerPath2D.Scripts.GamePlay.GameState.States;
using Assets.LazerPath2D.Scripts.GamePlay.Infrastructure;
using Assets.LazerPath2D.Scripts.GamePlay.Managers;
using Assets.LazerPath2D.Scripts.GamePlay.Node;
using Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode;
using Assets.LazerPath2D.Scripts.GamePlay.ScreenClick;
using Assets.LazerPath2D.Scripts.GamePlay.UI;
using Assets.LazerPath2D.Scripts.GamePlay.Utility.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.LazerPath2D.Scripts.GamePlay.GameState
{
    public class GameStateMachine : IGameStateSwitcher, IUpdatable, IDisposable
    {
        private List<IGameState> _gameStates;

        private IGameState _currentState;

        public GameStateMachine(
            IReadOnlyList<INode> nodes,
            ScreenClickHandler screenClickHandler,
            GamePlayMenuPopupService gamePlayMenuPopupService,
            NodesDetector nodesDetector,
             ReactiveVariable<int> amountActiveStars,
             IReadOnlyVariable<int> currentLevelNumber,
             CompletedLevelsService completedLevelsService,
             PlayerDataProvider playerDataProvider,
            CameraManager cameraManager)
        {
            IGameState playState = new PlayState(
                this,
                nodes,
                gamePlayMenuPopupService,
                nodesDetector,
                amountActiveStars,
                currentLevelNumber,
                completedLevelsService,
                playerDataProvider);

            IGameState pauseState = new PauseState(this, screenClickHandler, nodes, gamePlayMenuPopupService);
            IGameState gameOverState = new GameOverState(this, screenClickHandler, gamePlayMenuPopupService, cameraManager);

            _gameStates = new List<IGameState>()
            {
                playState,
                pauseState,
                gameOverState,
            };

            _currentState = _gameStates.FirstOrDefault(state => state is PlayState);
            _currentState.Enter();
        }
        public IGameState CurrentState => _currentState;

        public void Dispose()
        {
            _currentState?.Exit();

            foreach (IDisposable disposable in _gameStates)
                disposable.Dispose();
        }

        public void SwitchState<T>() where T : IGameState
        {
            IGameState gameState = _gameStates.FirstOrDefault(gameState => gameState is T);

            _currentState?.Exit();
            _currentState = gameState;
            _currentState.Enter();
        }

        public void ToUpdate(float deltaTime)
        {
            _currentState.ToUpdate(deltaTime);
        }
    }
}
