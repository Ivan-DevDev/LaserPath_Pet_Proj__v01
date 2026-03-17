namespace Assets.LazerPath2D.Scripts.CommonServices.SceneManagment
{
    public interface IOutputSceneArgs
    {
        // выходные параметры любой сцена, будут служить входными для сцены в которую переходим
        IInputSceneArgs NextSeneInputArgs { get; }
    }

    public abstract class OutputSceneArgs : IOutputSceneArgs
    {
        protected OutputSceneArgs(IInputSceneArgs nextSeneInputArgs)
        {
            NextSeneInputArgs = nextSeneInputArgs;
        }

        public IInputSceneArgs NextSeneInputArgs { get; }
    }



    public class OutputGamePlayArgs : OutputSceneArgs
    {
        public OutputGamePlayArgs(IInputSceneArgs nextSeneInputArgs) : base(nextSeneInputArgs)
        {
        }
    }

    public class OutputMainMenuArgs : OutputSceneArgs
    {
        public OutputMainMenuArgs(IInputSceneArgs nextSeneInputArgs) : base(nextSeneInputArgs)
        {
        }
    }

    public class OutputBootstrapArgs : OutputSceneArgs
    {
        public OutputBootstrapArgs(IInputSceneArgs nextSeneInputArgs) : base(nextSeneInputArgs)
        {
        }
    }
}
