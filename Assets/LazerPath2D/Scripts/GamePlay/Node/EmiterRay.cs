using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode;
using Assets.LazerPath2D.Scripts.GamePlay.Node.NodesView;
using Assets.LazerPath2D.Scripts.GamePlay.Node.Rotate;
using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node
{
    public class EmiterRay : MonoBehaviour, INode, IRotatable, IDetectableNode, IDetectingNode
    {
        [SerializeField] private float _eulerAngleRotation;
        [Space]
        [Space]
        [SerializeField] private Collider _selfCollider;
        [SerializeField] private LineRenderer _lineRenderer;
        [Space]
        [SerializeField] private NodesDetector _nodesDetector;
        [Space]
        [SerializeField] private bool _isRotating;
        [SerializeField] private bool _isInintialized;
        [Space]
        [SerializeField] private NodeView _nodeView;

        private Color _contourColor;
        private Color _activeFillColor;
        private Color _deactiveFillColor;
        private Color _otherColor;
        private Color _laserColor;

        private float _speedRotation;
        private PlaySound _playSound;


        private Rotator _rotator;

        public void Initialize(
            NodesDetector nodesDetector,
            PlaySound playSound,
            float speedRotation,
            Color contourColor,
            Color activeFillColor,
            Color defaultFillColor,
            Color otherColor, 
            Color laserColor)
        {
            if (_nodeView == null)
                _nodeView = GetComponentInChildren<NodeView>();

            if (_nodeView == null)
                throw new ArgumentNullException(" no Component", nameof(_nodeView));

            if (_selfCollider == null)
                _selfCollider = GetComponent<Collider>();

            if (_selfCollider == null)
                throw new ArgumentNullException(" no Component", nameof(_selfCollider));

            _nodesDetector = nodesDetector;

            if (_nodesDetector == null)
                throw new ArgumentNullException(" Component = null ", nameof(_nodesDetector));


            _speedRotation = speedRotation;
            _playSound = playSound;

            _rotator = new Rotator(_speedRotation, _eulerAngleRotation);


            _contourColor = contourColor;
            _activeFillColor = activeFillColor;
            _deactiveFillColor = defaultFillColor;
            _otherColor = otherColor;
            _laserColor = laserColor;

            _nodeView.SetActive(contourColor, contourColor, activeFillColor);
            _nodeView.SetColorOtherSprites(otherColor);
            _nodeView.InitializeVFX(laserColor);

            _rotator.RotatingCompleted += OnRotatingCompleted;

            _isInintialized = true;
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            if (_rotator != null)
                _rotator.RotatingCompleted -= OnRotatingCompleted;

        }

        public void StartUpdate()
        {
            UpdateDetector();
        }

        public Collider Collider => _selfCollider;
        public bool IsInintialized => _isInintialized;

        public void ToUpdate(float deltaTime)
        {
            if (_isRotating)
                _rotator.Update(deltaTime);
        }

        public void UpdateDetector()
        {
            _nodesDetector.ToDecectNode();
        }

        public void OnClicked()
        {
            ToRotate();
        }

        public void ToActive()
        {
           
        }

        public void ToDeActive()
        {
            
        }

        public void ToRotate()
        {
            if (_isRotating)
                return;

            _isRotating = true;

            _nodesDetector.ToClearDetectingNodesInData(this);

            _nodesDetector.UpdateActiveNodesList();
            _nodesDetector.SetActiveOrDeactiveDetectingNodes();
           
            foreach (NodesDetectorData nodesDetectorData in _nodesDetector.NodesDetectorDates)
            {
                if (nodesDetectorData.EmiterRay == this)
                {
                    nodesDetectorData.LaserVisualizer.ToDeactive();
                }
            }

            _rotator.StartRotateSmoothlyZAxis(this.transform);

            _nodeView.VFXView.Hide();

            _playSound.OnRotateEmiterNodeClip();
        }

        private void OnRotatingCompleted()
        {
            _isRotating = false;

            foreach (NodesDetectorData nodesDetectorData in _nodesDetector.NodesDetectorDates)
            {
                if (nodesDetectorData.EmiterRay == this)
                {
                    nodesDetectorData.LaserVisualizer.ToActive();
                }
            }

            _nodeView.VFXView.Show();

            UpdateDetector();
        }
    }
}
