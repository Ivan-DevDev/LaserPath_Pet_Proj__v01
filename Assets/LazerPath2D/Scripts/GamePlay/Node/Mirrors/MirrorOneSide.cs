using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode;
using Assets.LazerPath2D.Scripts.GamePlay.Node.NodesView;
using Assets.LazerPath2D.Scripts.GamePlay.Node.Rotate;
using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node.Mirrors
{
    public class MirrorOneSide : Mirror
    {
        [Header("Config")]
        [SerializeField] private float _eulerAngleRotation = -90;
        private float _speedRotation;

        public override void Initialize(
            NodesDetector nodesDetector, 
            PlaySound playSound, 
            float speedRotation,
            Color contourColor, 
            Color activeFillColort, 
            Color defaultFillColor,
            Color otherColor,
            Color laserColor)
        {
            if (NodeView == null)
                NodeView = GetComponentInChildren<NodeView>();

            if (NodeView == null)
                throw new ArgumentNullException(" no Component", nameof(NodeView));

            if (SelfCollider == null)
                SelfCollider = GetComponent<Collider>();

            if (SelfCollider == null)
                throw new ArgumentNullException(" no Component", nameof(SelfCollider));

            NodesDetector = nodesDetector;

            if (NodesDetector == null)
                throw new ArgumentNullException(" Component = null ", nameof(NodesDetector));


            _speedRotation = speedRotation;

            Rotator = new Rotator(_speedRotation, _eulerAngleRotation);


            ContourColor = contourColor;
            ActiveFillColor = activeFillColort;
            DeactiveFillColor = defaultFillColor;
            OtherColor = otherColor;
            LaserColor = laserColor;


            Rotator.RotatingCompleted += OnRotatingCompleted;

            _playSound = playSound;

            NodeView.SetDeactive(contourColor, contourColor, defaultFillColor);

            IsInintialized = true;
        }
    }
}
