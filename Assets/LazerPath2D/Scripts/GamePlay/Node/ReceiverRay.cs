using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode;
using Assets.LazerPath2D.Scripts.GamePlay.Node.NodesView;
using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node
{
    public class ReceiverRay : MonoBehaviour, INode, IDetectableNode
    {
        [SerializeField] private Collider _selfCollider;
        [Space]
        [SerializeField] private NodeView _nodeView;
        [Space]
        [SerializeField] private bool _isActive;
        [SerializeField] private bool _isInintialized;

        private Color _contourColor;
        private Color _activeFillColor;
        private Color _deactiveFillColor;
        private Color _otherColor;
        private Color _laserColor;

        private PlaySound _playSound;

        public void Initialize(
            NodesDetector nodesDetector,
            PlaySound playSound,
            float speedRotation,
            Color contourColor,
            Color activeFillColor,
            Color deactiveFillColor,
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

            _contourColor = contourColor;
            _activeFillColor = activeFillColor;
            _deactiveFillColor = deactiveFillColor;
            _otherColor = otherColor;
            _laserColor = laserColor;

            _nodeView.InitializeVFX(laserColor);

            _nodeView.VFXView.Hide();

            _playSound = playSound;

            _nodeView.SetDeactive(contourColor, contourColor, deactiveFillColor);
            _nodeView.SetColorOtherSprites(otherColor);

            _isInintialized = true;
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
           
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

            _nodeView.VFXView.Show();
            _nodeView.VFXView.SetPlay();

            if (_nodeView != null)
            {
                _nodeView.SetActive(_contourColor, _contourColor, _activeFillColor);

                if (_nodeView is ReceiverRayView receiverRayView)
                {
                    receiverRayView.ToStartDissolve();
                    receiverRayView.ToLightingMirrorContourOn(_activeFillColor);
                }
            }

            _playSound.OnNodeReceiverCompletedClip();
        }

        public void ToDeActive()
        {
            if (IsActive == false)
                return;


            _isActive = false;

            _nodeView.VFXView.Hide();
            _nodeView.VFXView.SetStop();


            if (_nodeView != null)
            {
                _nodeView.SetDeactive(_contourColor, _contourColor, _deactiveFillColor);

                if (_nodeView is ReceiverRayView receiverRayView)
                {
                    receiverRayView.ToLightingMirrorContourOff();
                }
            }
        }
    }
}
