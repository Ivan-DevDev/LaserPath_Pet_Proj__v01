using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode
{
    public interface IDetectorNode
    {
        public INode ToDetectNodeByRayCast(Vector3 selfPosition, Collider selfCollider, Vector3 direction, float maxRayLength, LayerMask receivingNodeLayer);
    }
}
