using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.LaserView
{
    public class LaserVisualizer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        private const string ColorBase = "_ColorBase";
        private int _colorID = default;

        private Material _material;

        public void Initialize()
        {
            if (_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();

            if (_lineRenderer == null)
                throw new ArgumentNullException(" no Component", nameof(LineRenderer));

            _colorID = Shader.PropertyToID("_ColorBase");

            _material = _lineRenderer.material;           
        }        

        public void ToDeactive() => _lineRenderer.enabled = false;
        public void ToActive() => _lineRenderer.enabled = true;

        public bool IsActive() => _lineRenderer.enabled;        

        public void SetColorLaser(Color colorlaser)
        {
            float intensity = 8f;

            _material.SetColor(_colorID, colorlaser * intensity);
        }

        public void UpdateLaserView(List<Vector3> laserViewPoints)
        {
            if (_lineRenderer == null)
                return;         


            _lineRenderer.positionCount = laserViewPoints.Count;

            _lineRenderer.SetPositions(laserViewPoints.ToArray());
        }
    }
}
