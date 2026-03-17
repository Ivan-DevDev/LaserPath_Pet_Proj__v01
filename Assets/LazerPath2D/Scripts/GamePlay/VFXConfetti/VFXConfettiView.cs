using System.Collections.Generic;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.VFXConfetti
{
    public class VFXConfettiView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _particleVFXPrefabs;

        public IReadOnlyCollection<ParticleSystem> ParticleVFXArray => _particleVFXPrefabs;

        public void PlayParticleVFX()
        {
            foreach(ParticleSystem particleSystem in _particleVFXPrefabs) 
                particleSystem.Play();
        }

        public void StopParticleVFX()
        {
            foreach (ParticleSystem particleSystem in _particleVFXPrefabs)
                particleSystem.Stop();
        }
    }
}
