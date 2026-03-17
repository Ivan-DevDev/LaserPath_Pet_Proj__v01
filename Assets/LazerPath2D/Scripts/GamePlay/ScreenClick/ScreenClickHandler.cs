using Assets.LazerPath2D.Scripts.CommonServices.InputHandler;
using Assets.LazerPath2D.Scripts.GamePlay.Node;
using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.ScreenClick
{
    public class ScreenClickHandler : IDisposable
    {
        public event Action<INode> ClickedOnNode;

        private IInputClickHandler _inputClickHandler;
        private LayerMask _clickableLayerMask;
        private float _rayDistance = 500f;
        private Camera _mainCamera;

        public bool IsTurnOff { get; private set; }

        public ScreenClickHandler(IInputClickHandler inputClickHandler, LayerMask clickableLayerMask)
        {
            _inputClickHandler = inputClickHandler;
            _clickableLayerMask = clickableLayerMask;
            _mainCamera = Camera.main;

            _inputClickHandler.ClickedDown += OnClickedDown;
        }

        public void Dispose()
        {
            _inputClickHandler.ClickedDown -= OnClickedDown;
        }

        public void SetEnable() => IsTurnOff = false;
        public void SetDisable() => IsTurnOff = true;

        private void OnClickedDown(Vector3 clickTouchPosition)
        {
            if (IsTurnOff)
                return;

            Ray ray = _mainCamera.ScreenPointToRay(clickTouchPosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, _rayDistance, _clickableLayerMask))
            {

                if (hitInfo.collider.TryGetComponent(out INode node))
                {
                    ClickedOnNode?.Invoke(node);                   
                }
            }
        }
    }
}
