using Assets.LazerPath2D.Scripts.GamePlay.Node;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Utility
{
    public interface IReflectionNodeCalculator
    {
        Vector3 ToСalculateReflection(IReflectableNode reflectablenode, Vector3 receivedDirection);
    }
}
