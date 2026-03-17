using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonServices.ConfigsManagment;
using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using Assets.LazerPath2D.Scripts.CommonServices.LoadingScreen;
using Assets.LazerPath2D.Scripts.CommonServices.SceneManagment;
using Assets.LazerPath2D.Scripts.DI;
using System.Collections;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.EntryPoint
{
    public class Bootstrap : MonoBehaviour
    {
        public IEnumerator Run(DIContainer container)
        {
            ILoadingCurtain loadingCurtain = container.Resolve<ILoadingCurtain>();

            if (loadingCurtain is StandartLoadingCurtain standartLoadingCurtain)
                standartLoadingCurtain.Initialize(container.Resolve<ISceneLoader>());

            loadingCurtain.Show();

            PlaySound playSound = container.Resolve<PlaySound>();
            playSound.Initialize(container.Resolve<AudioHandler>());

            SceneSwitcher sceneSwitcher = container.Resolve<SceneSwitcher>();

            container.Resolve<ConfigsProviderService>().LoadAll();
            container.Resolve<PlayerDataProvider>().Load();
            container.Resolve<GameSettingsDataProvider>().Load();

            yield return null;

            loadingCurtain.Hide();

            // load main menu
            sceneSwitcher.ProcessSwitchSceneFor(new OutputBootstrapArgs(new MainMenuInputArgs()));
        }
    }
}
