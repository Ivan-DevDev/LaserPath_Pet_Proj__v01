using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.CommonServices.InputHandler
{
    public interface IInputClickHandler
    {
        event Action<Vector3> ClickedDown;

        Vector3 ClickTouchPosition { get; }

        bool IsClickTouch { get; }

        void Update();
    }
}
