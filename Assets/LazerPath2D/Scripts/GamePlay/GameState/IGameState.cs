using Assets.LazerPath2D.Scripts.GamePlay.Infrastructure;

namespace Assets.LazerPath2D.Scripts.GamePlay.GameState
{
    public interface IGameState: IUpdatable
    {
        void Enter();
        void Exit();         
    }
}
