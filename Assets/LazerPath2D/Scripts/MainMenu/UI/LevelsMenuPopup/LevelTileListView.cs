using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LazerPath2D.Scripts.MainMenu.UI.LevelsMenuPopup
{
    public class LevelTileListView : MonoBehaviour 
    {
        [SerializeField] private LevelTileView levelTileViewPrefab;
        [SerializeField] private Transform _parent;

        [SerializeField] private Image _contur;
        [SerializeField] private Image _conturLight;

        private List<LevelTileView> _elements = new();

        public IReadOnlyList<LevelTileView> Elements => _elements;
        public Transform Parent => _parent;

        public Image Contur => _contur;
        public Image ConturLight => _conturLight;

        public LevelTileView AddElement(LevelTileView levelTileView)
        {
            _elements.Add(levelTileView);

            return levelTileView;
        }

        public void RemoveElement(LevelTileView levelTileView)
        {
            _elements.Remove(levelTileView);
        }
    }
}
