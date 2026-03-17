using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using Assets.LazerPath2D.Scripts.CommonServices.LevelsService;
using Assets.LazerPath2D.Scripts.GamePlay.Managers;
using Assets.LazerPath2D.Scripts.GamePlay.Node;
using Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode;
using Assets.LazerPath2D.Scripts.GamePlay.ScreenClick;
using Assets.LazerPath2D.Scripts.GamePlay.UI;
using Assets.LazerPath2D.Scripts.GamePlay.UI.GameOverMenu;
using Assets.LazerPath2D.Scripts.GamePlay.UI.PauseMenu;
using Assets.LazerPath2D.Scripts.GamePlay.Utility.Reactive;
using System.Collections.Generic;
using Unity.Cinemachine;

namespace Assets.LazerPath2D.Scripts.GamePlay.GameState
{
    public class GameStateMachineFactory
    {
        private ScreenClickHandler _screenClickHandler;

        public GameStateMachineFactory(ScreenClickHandler screenClickHandler)
        {
            _screenClickHandler = screenClickHandler;
        }

        public GameStateMachine CreateGameStateMachine(
            IReadOnlyList<INode> nodes,
            GamePlayMenuPopupService gamePlayMenuPopupService, 
            NodesDetector nodesDetector,
            ReactiveVariable<int> amountActiveStars,
            IReadOnlyVariable<int> currentLevelNumber, 
            CompletedLevelsService completedLevelsService,
             PlayerDataProvider playerDataProvider,
             CameraManager cameraManager)
           
        {
            GameStateMachine gameStateMachine = new GameStateMachine(
                nodes,_screenClickHandler,
                gamePlayMenuPopupService,
                nodesDetector,
                amountActiveStars,
                currentLevelNumber,
                completedLevelsService,
                playerDataProvider,
                cameraManager);

            return gameStateMachine;
        }        
    }
}
