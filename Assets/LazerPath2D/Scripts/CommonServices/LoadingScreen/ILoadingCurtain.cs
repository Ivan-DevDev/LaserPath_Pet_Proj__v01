using UnityEngine;

namespace Assets.LazerPath2D.Scripts.CommonServices.LoadingScreen
{
    public interface ILoadingCurtain
    {
        bool IsShow { get; }

        void Show();
        void Hide();
    }
}
