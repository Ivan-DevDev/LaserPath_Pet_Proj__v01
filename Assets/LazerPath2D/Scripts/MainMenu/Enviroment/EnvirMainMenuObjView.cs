using Assets.LazerPath2D.Scripts.CommonUI.View;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.MainMenu.Enviroment
{
    public class EnvirMainMenuObjView: MonoBehaviour, IView
    {
        [SerializeField] private SpriteRenderer _groundSprite;

      
        public SpriteRenderer GroundSprite => _groundSprite;

        public void Show() => this.gameObject.SetActive(true);
        public void Hide() => this.gameObject.SetActive(false);
    }
}
