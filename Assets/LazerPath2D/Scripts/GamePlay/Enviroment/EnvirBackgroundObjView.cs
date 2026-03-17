using Assets.LazerPath2D.Scripts.CommonUI.View;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Enviroment
{
    public class EnvirBackgroundObjView : MonoBehaviour, IView
    {
        [SerializeField] private SpriteRenderer _sunSprite;
        [SerializeField] private SpriteRenderer _mountainsSprite;
        [SerializeField] private SpriteRenderer _groundSprite;


        public SpriteRenderer SunSprite => _sunSprite;
        public SpriteRenderer MountainsSprite => _mountainsSprite;
        public SpriteRenderer GroundSprite => _groundSprite;

        public void Show() => this.gameObject.SetActive(true);
        public void Hide() => this.gameObject.SetActive(false);
        
    }
}
