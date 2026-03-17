using Unity.Cinemachine;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.Configs.GamePlay.Camera
{
    [CreateAssetMenu(menuName = "Configs/GamePlay/CameraConfig", fileName = "CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [Header("Impulse Source Settings")]
        [field: SerializeField, Range(0, 5f)] public float ImpactTime = 0.15f;
        [field: SerializeField, Range(0, 5f)] public float ImpactForce = 0.3f;

        public CinemachineImpulseDefinition.ImpulseTypes ImpulseTypes;

        public Vector3 DefaultVelocity = new Vector3(0.05f, -0.7f, 0);
        public AnimationCurve ImpulseCurve;

        [Header("Impulse Listener Settings")]

        public float ListenerGain = 0.2f;
        public float ListenerAmplitude = 0.2f;
        public float ListenerFrequency = 0.1f;
        public float ListenerDuration = 0.1f;
    }
}
