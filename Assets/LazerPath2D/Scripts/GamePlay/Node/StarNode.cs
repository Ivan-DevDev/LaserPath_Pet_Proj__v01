using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode;
using Assets.LazerPath2D.Scripts.GamePlay.Node.NodesView;
using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node
{
    public class StarNode : MonoBehaviour, INode, IDetectableNode
    {
        [SerializeField] private Collider _selfCollider;

        [SerializeField] private NodeView _nodeView;

        private Color _contourColor;
        private Color _activeFillColor;
        private Color _deactiveFillColor;
        private Color _otherColor;
        private Color _laserColor;


        [SerializeField] private NodesDetector _nodesDetector;

        [SerializeField] private bool _isActive;
        [SerializeField] private bool _isInintialized;

        private PlaySound _playSound;

        public void Initialize(
            NodesDetector nodesDetector,
            PlaySound playSound,
            float speedRotation,
            Color contourColor = default,
            Color activeFillColor = default,
            Color deactiveFillColor = default,
            Color otherColor = default,
            Color laserColor = default)
        {
            if (_nodeView == null)
                _nodeView = GetComponentInChildren<NodeView>();

            if (_nodeView == null)
                throw new ArgumentNullException(nameof(_nodeView), " no Component!!! ");

            if (_selfCollider == null)
                _selfCollider = GetComponent<Collider>();

            if (_selfCollider == null)
                throw new ArgumentNullException(nameof(_selfCollider), " no Component!!! ");

            _contourColor = contourColor;
            _activeFillColor = activeFillColor;
            _deactiveFillColor = deactiveFillColor;
            _otherColor = otherColor;
            _laserColor = laserColor;

            _isInintialized = true;
        }

        public bool IsActive => _isActive;
        public Collider Collider => _selfCollider;
        public bool IsInintialized => _isInintialized;

        public void ToUpdate(float deltaTime)
        {

        }

        public void OnClicked()
        {

        }

        public void ToActive()
        {
            if (IsActive)
                return;

            _isActive = true;

            _nodeView.SetActive(_otherColor, _otherColor, _activeFillColor);
        }

        public void ToDeActive()
        {
            if (IsActive == false)
                return;

            _isActive = false;

            _nodeView.SetDeactive(_contourColor, _contourColor, _deactiveFillColor);
        }
    }
}
