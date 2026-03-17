using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.GamePlay.Infrastructure;
using Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node
{
    public interface INode : IUpdatable 
    {
        Transform transform { get; }
        Collider Collider { get; }
        
        bool IsInintialized { get; }    

        void Initialize(
            NodesDetector nodesDetector, 
            PlaySound playSound, 
            float speedRotation = 100,
            Color contourColor = default,
            Color activeFillColor = default,
            Color deactiveFillColor = default,
             Color OtherColor = default,
             Color _laserColor = default);      

        void OnClicked();

        void ToActive(); 

        void ToDeActive();

    }
}
