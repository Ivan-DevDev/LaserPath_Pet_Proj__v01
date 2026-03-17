using Assets.LazerPath2D.Scripts.GamePlay.Node.VFX;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node.NodesView
{
    public abstract class NodeView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _backgroundSprite;
        [SerializeField] private SpriteRenderer _contourSprite;
        [SerializeField] private SpriteRenderer _lightSprite;

        [SerializeField] private List<SpriteRenderer> _otherSprites;

        [SerializeField] protected ParticlesVFXView ParticlesVFXView;


        public SpriteRenderer BackgroundSprite => _backgroundSprite;
        public  SpriteRenderer ContourSprite => _contourSprite;
        public  SpriteRenderer LightSprite=> _lightSprite;

        public ParticlesVFXView VFXView => ParticlesVFXView;

        public  void InitializeVFX(Color ColorVFX)
        {
            if (ParticlesVFXView != null)
                ParticlesVFXView.Initialize(ColorVFX);
        }

        public void SetColorBackground(Color color) => _backgroundSprite.color = color;
        public void SetColorConturSprite(Color color) => _contourSprite.color = color;
        public void SetColorLightSprite(Color color) => _lightSprite.color = color;

        public void SetColorOtherSprites(Color color)
        {
            if (_otherSprites.Count > 0)
            {
                foreach (SpriteRenderer spriteRenderer in _otherSprites)
                    spriteRenderer.color = color;
            }
        }


        public void SetActive(Color light, Color contour, Color background)
        {
            _backgroundSprite.color = background;
            _contourSprite.color = contour;
            _lightSprite.color = light;           
        }

        public void SetDeactive(Color light, Color contour, Color background)
        {
            _backgroundSprite.color = background;
            _contourSprite.color = contour;
            _lightSprite.color = light;
        }
    }
}
