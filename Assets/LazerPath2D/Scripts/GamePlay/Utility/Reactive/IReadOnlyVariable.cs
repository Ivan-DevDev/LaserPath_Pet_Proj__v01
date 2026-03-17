using System;

namespace Assets.LazerPath2D.Scripts.GamePlay.Utility.Reactive
{
    public interface IReadOnlyVariable <T>
    {
        event Action<T, T> Changed;
        T Value { get; }
    }
}
