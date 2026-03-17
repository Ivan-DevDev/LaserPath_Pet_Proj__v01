namespace Assets.LazerPath2D.Scripts.CommonServices.SceneManagment
{
    public interface IInputSceneArgs
    {
    }

    public class GamePlayInputArgs: IInputSceneArgs
    {
        public GamePlayInputArgs(int levelNumber)
        {
            LevelNumber = levelNumber;
        }

        public int LevelNumber { get; } // временно (на удаление), потом брать из конфига
    }

    public class MainMenuInputArgs : IInputSceneArgs
    {
    }


}
