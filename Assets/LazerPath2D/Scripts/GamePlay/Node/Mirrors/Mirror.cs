using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode;
using Assets.LazerPath2D.Scripts.GamePlay.Node.NodesView;
using Assets.LazerPath2D.Scripts.GamePlay.Node.Rotate;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node.Mirrors
{
    public abstract class Mirror : MonoBehaviour, INode, IRotatable, IDetectableNode, IReflectableNode, IDetectingNode
    {
        public Collider SelfCollider { get; protected set; }

        [SerializeField] protected NodesDetector NodesDetector;
        [Space]
        [SerializeField] private bool _isActive;
        [SerializeField] private bool _isRotating;

        [SerializeField] protected NodeView NodeView;

        protected Color ContourColor;
        protected Color ActiveFillColor;
        protected Color DeactiveFillColor;
        protected Color OtherColor;
        protected Color LaserColor;

        [field: SerializeField] public bool IsInintialized { get; protected set; }

        protected Rotator Rotator;
        protected PlaySound _playSound;

        public abstract void Initialize(
            NodesDetector nodesDetector,
            PlaySound playSound,
            float speedRotation,
             Color contourColor = default,
             Color activeFillColor = default,
             Color defaultFillColor = default,
             Color otherColor = default,
             Color laserColor = default);

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            if (Rotator != null)
                Rotator.RotatingCompleted -= OnRotatingCompleted;
        }

        public Collider Collider => SelfCollider;


        public void StartUpdate()
        {
            UpdateDetector();
        }

        public void ToUpdate(float deltaTime)
        {
            if (_isRotating)
                Rotator.Update(deltaTime);
        }

        public void UpdateDetector()
        {
            NodesDetector.ToDecectNode();
        }

        public void OnClicked()
        {
            ToRotate();
        }

        public void ToActive()
        {
            _isActive = true;

            if (NodeView != null)
            {
                NodeView.SetActive(ContourColor, ContourColor, ActiveFillColor);

                if (NodeView is MirrorView mirrorView)
                    mirrorView.ToLightingMirrorContourOn(ActiveFillColor);
            }
        }

        public void ToDeActive()
        {
            _isActive = false;

            if (NodeView != null)
            {
                NodeView.SetDeactive(ContourColor, ContourColor, DeactiveFillColor);

                if (NodeView is MirrorView mirrorView)
                    mirrorView.ToLightingMirrorContourOff();
            }
        }

        public void ToRotate()
        {
            if (_isRotating)
                return;

            _isRotating = true;
                  
            NodesDetector.ToTryRemoveNodesInDetectingListAfterThis(this);

            NodesDetector.UpdateActiveNodesList();
            NodesDetector.SetActiveOrDeactiveDetectingNodes();
           
            foreach (NodesDetectorData nodesDetectorData in NodesDetector.NodesDetectorDates)
            {
                if (nodesDetectorData.DetectingNodes.Contains(this))
                {
                    NodesDetector.ToUpdateDrawLaserView(nodesDetectorData, true, default);
                }
            }

            Rotator.StartRotateSmoothlyZAxis(transform);

            _playSound.OnRotateMirrorNodeClip();
        }

        protected void OnRotatingCompleted()
        {
            _isRotating = false;

            UpdateDetector();
        }
    }
}
