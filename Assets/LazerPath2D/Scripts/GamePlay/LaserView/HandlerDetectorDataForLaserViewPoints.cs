using Assets.LazerPath2D.Scripts.GamePlay.Node;
using Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.LaserView
{
    public class HandlerDetectorDataForLaserViewPoints
    {
        public void ToHandleData(NodesDetectorData nodesDetectorData, bool nodeIsRotating = false, bool isRereflection = false)
        {
            float laserlength = 1.1f;

            List<Vector3> laserViewPoints = new();

            Vector3 firstPoint = nodesDetectorData.EmiterRay.transform.position;

            laserViewPoints.Add(firstPoint);

           
            if (nodesDetectorData.DetectingNodes.Count == 0)
            {
                if (!nodeIsRotating)
                {
                    Vector3 directionLength = nodesDetectorData.EmiterRay.transform.up * laserlength;

                    Vector3 lastPoint = firstPoint + directionLength;

                    laserViewPoints.Add(lastPoint);
                }
            }

            if (nodesDetectorData.DetectingNodes.Count > 0)
            {
                Vector3[] points = nodesDetectorData.DetectingNodes.Select(node => node.transform.position).ToArray();
                laserViewPoints.AddRange(points);

              
                if (nodesDetectorData.DetectingNodes[nodesDetectorData.DetectingNodes.Count - 1] is IReflectableNode 
                    && !nodeIsRotating && isRereflection == false)
                {
                    Vector3 directionLength = nodesDetectorData.DirectionReflection * laserlength;
                    Vector3 nodePosition = nodesDetectorData.DetectingNodes[nodesDetectorData.DetectingNodes.Count - 1].transform.position;

                    Vector3 lastPoint = nodePosition + directionLength;

                    laserViewPoints.Add(lastPoint);
                }
            }

            nodesDetectorData.LaserViewPoints = laserViewPoints;

            nodesDetectorData.LaserVisualizer.UpdateLaserView(laserViewPoints);
        }
    }
}
