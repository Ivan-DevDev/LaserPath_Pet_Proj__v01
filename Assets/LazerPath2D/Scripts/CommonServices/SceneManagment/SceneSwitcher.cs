using Assets.LazerPath2D.Scripts.CommonServices.CoroutinePerformer;
using Assets.LazerPath2D.Scripts.CommonServices.LoadingScreen;
using Assets.LazerPath2D.Scripts.DI;
using Assets.LazerPath2D.Scripts.GamePlay.Infrastructure;
using Assets.LazerPath2D.Scripts.MainMenu.Infrastructure;
using System;
using System.Collections;
using Object = UnityEngine.Object;

namespace Assets.LazerPath2D.Scripts.CommonServices.SceneManagment
{
    public class SceneSwitcher
    {
        private const string ErrorSceneTransitionMessage = " Данный переход невозможен !!!";

        private readonly DIContainer _projectContainer;
        private readonly ICoroutinePerformer _coroutinePerformer;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly ISceneLoader _sceneLoader;

        private DIContainer _currentSceneContainer;

        public SceneSwitcher(
            ICoroutinePerformer coroutinePerformer,
            ILoadingCurtain loadingCurtain,
            ISceneLoader sceneLoader,
            DIContainer projectContainer)
        {
            _coroutinePerformer = coroutinePerformer;
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
            _projectContainer = projectContainer;
        }

        public void ProcessSwitchSceneFor(IOutputSceneArgs outputSceneArgs) // по выходным аргументам, переходим в нужную сцену
        {
            switch (outputSceneArgs)
            {
                case OutputBootstrapArgs outputBootstrapArgs:
                    {
                        _coroutinePerformer.StartPerform(ProcessSwitchFromBootStrapScene(outputBootstrapArgs));
                        break;
                    }

                case OutputMainMenuArgs outputMainMenuArgs:
                    {
                        _coroutinePerformer.StartPerform(ProcessSwitchFromMainMenuScene(outputMainMenuArgs));
                        break;
                    }

                case OutputGamePlayArgs outputGamePlayArgs:
                    {
                        _coroutinePerformer.StartPerform(ProcessSwitchFromGamePlayScene(outputGamePlayArgs));
                        break;
                    }


                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(outputSceneArgs));
                    }
            }
        }

        private IEnumerator ProcessSwitchFromBootStrapScene(OutputBootstrapArgs outputBootstrapArgs)
        {
            switch (outputBootstrapArgs.NextSeneInputArgs)
            {
                case MainMenuInputArgs mainMenuInputArgs:
                    {
                        yield return ProccesSwitchMainMenuScene(mainMenuInputArgs);
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException(ErrorSceneTransitionMessage);
            }
        }

        private IEnumerator ProcessSwitchFromMainMenuScene(OutputMainMenuArgs outputMainMenuArgs)
        {

            switch (outputMainMenuArgs.NextSeneInputArgs)
            {
                case GamePlayInputArgs gamePlayInputArgs:
                    {
                        yield return ProcessSwitchGamePlayScene(gamePlayInputArgs);
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException(ErrorSceneTransitionMessage);
            }

        }

        private IEnumerator ProcessSwitchFromGamePlayScene(OutputGamePlayArgs outputGamePlayArgs)
        {
            switch (outputGamePlayArgs.NextSeneInputArgs)
            {
                case MainMenuInputArgs mainMenuInputArgs:
                    {
                        yield return ProccesSwitchMainMenuScene(mainMenuInputArgs);
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException(ErrorSceneTransitionMessage);
            }
        }

        private IEnumerator ProccesSwitchMainMenuScene(MainMenuInputArgs mainMenuInputArgs)
        {
            _loadingCurtain.Show();

            _currentSceneContainer?.Dispose();

            yield return _sceneLoader.LoadAsync(SceneID.Empty);
            yield return _sceneLoader.LoadAsync(SceneID.MainMenu);

            MainMenuBootstrap mainMenuBootstrap = Object.FindAnyObjectByType<MainMenuBootstrap>();

            if (mainMenuBootstrap == null)
                throw new NullReferenceException(nameof(mainMenuBootstrap));

            _currentSceneContainer = new DIContainer(_projectContainer);// создаём контэйнер для сцены "MainMenu"

            yield return mainMenuBootstrap.Run(_currentSceneContainer, mainMenuInputArgs);

            _loadingCurtain.Hide();
        }

        private IEnumerator ProcessSwitchGamePlayScene(GamePlayInputArgs gamePlayInputArgs)
        {
            _loadingCurtain.Show();

            _currentSceneContainer?.Dispose();

            yield return _sceneLoader.LoadAsync(SceneID.Empty);
            yield return _sceneLoader.LoadAsync(SceneID.GamePlay);

            GamePlayBootstrap gamePlayBootstrap = Object.FindAnyObjectByType<GamePlayBootstrap>();

            if (gamePlayBootstrap == null)
                throw new NullReferenceException(nameof(gamePlayBootstrap));

            _currentSceneContainer = new DIContainer(_projectContainer);// создаём контэйнер для сцены "GamePlay"

            yield return gamePlayBootstrap.Run(_currentSceneContainer, gamePlayInputArgs);

            _loadingCurtain.Hide();
        }
    }
}
