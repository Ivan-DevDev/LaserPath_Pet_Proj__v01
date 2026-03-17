using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using Assets.LazerPath2D.Scripts.CommonServices.LevelsService;
using Assets.LazerPath2D.Scripts.GamePlay.Node;
using Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode;
using Assets.LazerPath2D.Scripts.GamePlay.UI;
using Assets.LazerPath2D.Scripts.GamePlay.UI.PauseMenu;
using Assets.LazerPath2D.Scripts.GamePlay.Utility.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.LazerPath2D.Scripts.GamePlay.GameState.States
{
    public class PlayState : IGameState, IDisposable
    {
        private readonly IGameStateSwitcher StateSwitcher;

        private IReadOnlyList<INode> _nodes;
        private List<ReceiverRay> _receiverRays = new();        

        private GamePlayMenuPopupService _gamePlayMenuPopupService;
        private NodesDetector _nodesDetector;

        private ReactiveVariable<int> _amountActiveStars;
        private IReadOnlyVariable<int> CurrentLevelNumber;
        private CompletedLevelsService _completedLevelsService;
        private PlayerDataProvider _playerDataProvider;


        private bool _isGameOver;

        public PlayState(
            IGameStateSwitcher stateSwitcher, 
            IReadOnlyList<INode> nodes,
            GamePlayMenuPopupService gamePlayMenuPopupService,
            NodesDetector nodesDetector,
            ReactiveVariable<int> amountActiveStars,
            IReadOnlyVariable<int> currentLevelNumber,
            CompletedLevelsService completedLevelsService,
            PlayerDataProvider playerDataProvider)
        {
            StateSwitcher = stateSwitcher;

            _nodes = nodes;

            foreach (INode node in _nodes)
            {
                if (node is ReceiverRay receiver)
                    _receiverRays.Add(receiver);
            }

            if (_receiverRays.Count == 0)
                throw new ArgumentOutOfRangeException($" Список приёмников {_receiverRays} пустой !!!!");

            _gamePlayMenuPopupService = gamePlayMenuPopupService;
            _nodesDetector = nodesDetector;
            _amountActiveStars = amountActiveStars;

            CurrentLevelNumber = currentLevelNumber;
            _completedLevelsService = completedLevelsService;
            _playerDataProvider = playerDataProvider;

            _nodesDetector.UpdatedDetoctor += OnUpdatedDetoctor;

            _gamePlayMenuPopupService.OpenedPauseMenu += OnOpenedPauseMenu;
        }


        public void Dispose()
        {
            _gamePlayMenuPopupService.OpenedPauseMenu -= OnOpenedPauseMenu;

            if(_nodesDetector != null)
            _nodesDetector.UpdatedDetoctor -= OnUpdatedDetoctor;
        }


        public void Enter()
        {
            _isGameOver = false;           
        }

        public void Exit()
        {
           
        }

        public void ToUpdate(float deltaTime)
        {
            if (_isGameOver)
                return;

            foreach (INode node in _nodes)
                node.ToUpdate(deltaTime);
         
        }

        private void OnUpdatedDetoctor()
        {
            int activeStars = 0;

            foreach (INode node in _nodes)
            {
                if (node is StarNode starNode)
                {
                    if (starNode.IsActive)
                        activeStars++;
                }
            }

            if (_receiverRays.All(receiverRay => receiverRay.IsActive))
            {
                _isGameOver = true;

                _completedLevelsService.UpdateCompletedLevel(CurrentLevelNumber.Value, activeStars);
                _playerDataProvider.Save();
               
                StateSwitcher.SwitchState<GameOverState>();
            } 

            _amountActiveStars.Value = activeStars;            
        }

        private void OnOpenedPauseMenu(PauseMenuPopupPresenter presenter)
        {
            StateSwitcher.SwitchState<PauseState>();
        } 
    }
}
