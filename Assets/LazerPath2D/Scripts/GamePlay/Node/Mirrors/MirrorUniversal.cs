using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode;
using Assets.LazerPath2D.Scripts.GamePlay.Node.NodesView;
using Assets.LazerPath2D.Scripts.GamePlay.Node.Rotate;
using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node.Mirrors
{
    public class MirrorUniversal : Mirror
    {
        [Header("Config")]
        [SerializeField] private float _eulerAngleRotation = -45;
        private float _speedRotation;

        public override void Initialize(
            NodesDetector nodesDetector,
            PlaySound playSound, 
            float speedRotation,
            Color contourColor , 
            Color activeFillColor, 
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
            else
                throw new ArgumentNullException(" no Component", nameof(SelfCollider));

            NodesDetector = nodesDetector;

            if (NodesDetector == null)
                throw new ArgumentNullException(" Component = null ", nameof(NodesDetector));

            _speedRotation = speedRotation;

            Rotator = new Rotator (_speedRotation, _eulerAngleRotation);

            ContourColor = contourColor;
            ActiveFillColor = activeFillColor;
            DeactiveFillColor = defaultFillColor;
            OtherColor = otherColor;
            LaserColor = laserColor;

            Rotator.RotatingCompleted += OnRotatingCompleted;
            _playSound = playSound;

            IsInintialized = true;
        }       
    }
}
