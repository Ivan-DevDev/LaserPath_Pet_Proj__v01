using Assets.LazerPath2D.Scripts.GamePlay.Node;
using Assets.LazerPath2D.Scripts.GamePlay.Node.Mirrors;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.LazerPath2D.Scripts.Configs.GamePlay.Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private float _cameraOrthographicSize;
        [SerializeField] private float _cameraPerspectiveFieldOfView;

        [SerializeField] private List<EmiterRay> _laserEmiters = new();
        [SerializeField] private List<Mirror> _mirrors = new();
        [SerializeField] private List<StarNode> _starNodes = new();
        [SerializeField] private List<ReceiverRay> _laserReceivers = new();


        public float CameraOrthographicSize => _cameraOrthographicSize;
        public float CameraPerspectiveFieldOfView => _cameraPerspectiveFieldOfView;

        public IReadOnlyList<EmiterRay> LaserEmiters => _laserEmiters;
        public IReadOnlyList<Mirror> Mirrors => _mirrors;

        public IReadOnlyList<StarNode> StarNodes => _starNodes;
        public IReadOnlyList<ReceiverRay> LaserReceivers => _laserReceivers;


        public void ToDistributeNodesInLists()
        {
            EmiterRay[] laserEmiters = GetComponentsInChildren<EmiterRay>();

            if (laserEmiters.Length > 0)
            {
                if (_laserEmiters.Count > 0)
                    _laserEmiters.Clear();

                _laserEmiters.AddRange(laserEmiters);
            }
            else
            {
                throw new ArgumentException(nameof(laserEmiters), " Внимание! Отсутствуют излучатели !!! ");
            }

            Mirror[] mirrors = GetComponentsInChildren<Mirror>();

            if (mirrors.Length > 0)
            {
                if (_mirrors.Count > 0)
                    _mirrors.Clear();

                _mirrors.AddRange(mirrors);
            }

            StarNode[] starNodes = GetComponentsInChildren<StarNode>();

            if (starNodes.Length > 0)
            {
                if (_starNodes.Count > 0)
                    _starNodes.Clear();

                _starNodes.AddRange(starNodes);
            }
            else
            {
                throw new ArgumentException(nameof(starNodes), "   Внимание! Отсутствуют звёзы !!! ");
            }

            ReceiverRay[] laserReceivers = GetComponentsInChildren<ReceiverRay>();

            if (laserReceivers.Length > 0)
            {
                if (_laserReceivers.Count > 0)
                    _laserReceivers.Clear();

                _laserReceivers.AddRange(laserReceivers);
            }
            else
            {
                throw new ArgumentException(nameof(laserEmiters), "   Внимание! Отсутствуют приёмники !!! ");
            }
        }
    }
}
