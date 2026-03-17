using System.Linq;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode
{
    public class RayTracerToDetectNode : IDetectorNode
    {
        public INode ToDetectNodeByRayCast(Vector3 selfPosition, Collider selfCollider, Vector3 direction, float maxRayLength, LayerMask receivingNodeLayer)
        {
            RaycastHit[] raycastHits = Physics.RaycastAll(selfPosition, direction, maxRayLength, receivingNodeLayer);

            RaycastHit raycastHit = raycastHits.Where(hit => hit.collider != selfCollider).
                OrderBy(hit => hit.distance).FirstOrDefault();

            if (raycastHit.collider == null)
                return null;

            if (raycastHit.collider.TryGetComponent(out INode node))
            {
                return node;
            }

            return null;
        }
    }
}
