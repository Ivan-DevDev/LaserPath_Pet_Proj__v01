using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node.Rotate
{
    [Serializable]
    public class Rotator
    {
        public event Action RotatingCompleted;
        
        private float _speedRotation;
        private float _eulerAngleRotation;

        private Transform _transformObjForRotate;

        private Quaternion _targetRotation;

        [field: SerializeField] public bool IsRotating { get; private set; }

        public Rotator(float speedRotation = 0.5f, float eulerAngleRotation = -90f)
        {
            _speedRotation = speedRotation;
            _eulerAngleRotation = eulerAngleRotation;
        }

        public void Update(float deltaTime)
        {
            if (IsRotating == false)
                return;

            ToRotateSmoothly(deltaTime);
        }

        public void ToEnable() => IsRotating = true;
        public void ToDisable() => IsRotating = false;

        public float EulerAngleRotation => _eulerAngleRotation;

        public void StartRotateSmoothlyZAxis(Transform transformObjForRotate)
        {
            _transformObjForRotate = transformObjForRotate;

            _targetRotation = Quaternion.Euler(0, 0, _eulerAngleRotation) * _transformObjForRotate.rotation;

            IsRotating = true;
        }

        private void ToRotateSmoothly(float deltaTime)
        {
            Quaternion startRotation = _transformObjForRotate.rotation;

            float step = _speedRotation * deltaTime;

            _transformObjForRotate.rotation = Quaternion.RotateTowards(startRotation, _targetRotation, step);

            if (Mathf.Abs(Quaternion.Angle(startRotation, _targetRotation)) < 0.1f)
            {
                _transformObjForRotate.rotation = _targetRotation;

                IsRotating = false;

                RotatingCompleted?.Invoke();
            }
        }

    }
}
