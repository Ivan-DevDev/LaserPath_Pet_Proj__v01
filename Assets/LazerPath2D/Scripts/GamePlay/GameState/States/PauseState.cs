using Assets.LazerPath2D.Scripts.GamePlay.Node;
using Assets.LazerPath2D.Scripts.GamePlay.ScreenClick;
using Assets.LazerPath2D.Scripts.GamePlay.UI;
using Assets.LazerPath2D.Scripts.GamePlay.UI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.LazerPath2D.Scripts.GamePlay.GameState.States
{
    public class PauseState : IGameState, IDisposable
    {
        protected readonly IGameStateSwitcher StateSwitcher;

        private ScreenClickHandler _screenClickHandler;

        private List<IRotatable> _rotatableNodes = new List<IRotatable>();
        
        private readonly GamePlayMenuPopupService _gamePlayMenuPopupService;

        public PauseState(IGameStateSwitcher stateSwitcher, 
            ScreenClickHandler screenClickHandler, 
            IReadOnlyList<INode> nodes,
            GamePlayMenuPopupService gamePlayMenuPopupService)
        {
            StateSwitcher = stateSwitcher;
            _screenClickHandler = screenClickHandler;

            _rotatableNodes = nodes.Where(node => node is IRotatable).Cast<IRotatable>().ToList();

            _gamePlayMenuPopupService = gamePlayMenuPopupService;

            _gamePlayMenuPopupService.ClosedPauseMenu += OnClosedPauseMenu; 
        }

        public void Dispose()
        {
            _gamePlayMenuPopupService.ClosedPauseMenu -= OnClosedPauseMenu;
        }


        public void Enter()
        {
            _screenClickHandler.SetDisable();              
        }

        public void Exit()
        {
            _screenClickHandler.SetEnable();            
        }

        public void ToUpdate(float deltaTime)
        {
           

        }

        private void OnClosedPauseMenu(PauseMenuPopupPresenter presenter)
        {
            StateSwitcher.SwitchState<PlayState>();
        }

    }
}
