using System.Collections.Generic;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node.VFX
{
    public class ParticlesVFXView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _particleSystems;

        private const string EmissionColor = "_EmissionColor";

        public void Initialize(Color color)
        {
            if (_particleSystems.Length > 0)
            {
                float colorIntensity = 6f;

                foreach (ParticleSystem particle in _particleSystems)
                {
                    Renderer renderer = particle.GetComponent<Renderer>();
                    renderer.material.SetColor(EmissionColor, color * colorIntensity);
                }
            }
        }

        public IReadOnlyCollection<ParticleSystem> ParticleSystems => _particleSystems;
        public void Show() => this.gameObject.SetActive(true);
        public void Hide() => this.gameObject.SetActive(false);

        public void SetPlay()
        {
            if (_particleSystems.Length > 0)
            {
                foreach (ParticleSystem particle in _particleSystems)
                {
                    particle.Play();
                }
            }
        }

        public void SetStop()
        {
            if (_particleSystems.Length > 0)
            {
                foreach (ParticleSystem particle in _particleSystems)
                {
                    particle.Stop();
                }
            }
        }

    }
}
