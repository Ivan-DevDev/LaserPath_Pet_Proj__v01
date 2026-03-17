using System;

namespace Assets.LazerPath2D.Scripts.GamePlay.Utility.Reactive
{
    public class ReactiveVariable<T> : IReadOnlyVariable<T> where T : IEquatable<T>
    {
        public event Action<T, T> Changed;

        private T _value;

        public ReactiveVariable() => _value = default(T);

        public ReactiveVariable(T value) => _value = value; 

        public T Value
        {
            get => _value;

            set
            {
                T oldValue = _value;

                _value = value;

                if (_value.Equals(oldValue) == false)
                    Changed?.Invoke(oldValue, value);
            }
        }
    }
}
