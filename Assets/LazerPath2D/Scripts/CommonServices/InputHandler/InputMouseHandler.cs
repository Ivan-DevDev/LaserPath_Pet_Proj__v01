using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.CommonServices.InputHandler
{
    public class InputMouseHandler : IInputClickHandler
    {
        public event Action<Vector3> ClickedDown;
        

        private int _clickLeftMouseButton = 0;

        public bool IsClickTouch { get; private set; }

        public Vector3 ClickTouchPosition { get; private set; }
        

        public void Update()

        {
            IsClickTouch = Input.GetMouseButtonDown(_clickLeftMouseButton);

            if (IsClickTouch)
            {
                ClickTouchPosition = Input.mousePosition;

                ClickedDown?.Invoke(ClickTouchPosition);                
            }
        }
    }
}
