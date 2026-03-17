using Assets.LazerPath2D.Scripts.GamePlay.Node;
using Assets.LazerPath2D.Scripts.GamePlay.Node.Mirrors;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Utility
{
    public class ReflectionDirectionNodeCalculator : IReflectionNodeCalculator
    {
        public Vector3 ToСalculateReflection(IReflectableNode reflectableNode, Vector3 receivedDirection)
        {            
            Vector3 directionNormal = reflectableNode.transform.up;
            receivedDirection = receivedDirection.normalized;
            Vector3 reflection = Vector3.zero;

            switch (reflectableNode)
            {
                case MirrorUniversal mirrorUniversal:
                    { 
                        reflection = Vector3.Reflect(receivedDirection, directionNormal);                       

                        return reflection;
                    }

                case MirrorOneSide mirrorOneSide:
                    {
                        float dot = Vector3.Dot(directionNormal, receivedDirection);

                        if (dot < 0)
                        {
                            reflection = Vector3.Reflect(receivedDirection, directionNormal);
                        }                        

                        return reflection;
                    }

                default:
                    {
                        return reflection;                       
                    }
            }
        }
    }
}
