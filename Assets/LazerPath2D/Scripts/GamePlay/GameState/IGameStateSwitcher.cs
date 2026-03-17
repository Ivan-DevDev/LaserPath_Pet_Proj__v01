namespace Assets.LazerPath2D.Scripts.GamePlay.GameState
{
    public interface IGameStateSwitcher
    {
        void SwitchState<T>() where T : IGameState;
    }
}
