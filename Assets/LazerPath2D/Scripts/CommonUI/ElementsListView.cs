using Assets.LazerPath2D.Scripts.CommonUI.View;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.LazerPath2D.Scripts.CommonUI
{
    public class ElementsListView<TElement> : MonoBehaviour, IView where TElement : MonoBehaviour, IView
    {
        [SerializeField] private Transform _parent;

        private List<TElement> _elements = new();

        public IReadOnlyList<TElement> Elements => _elements;

        public void Add(TElement element)
        {
            element.transform.SetParent(_parent);

            _elements.Add(element);
        }

        public void Remove(TElement element)
        {
            element.transform.SetParent(null);
            _elements.Remove(element);
            //Object.Destroy(element.gameObject);
        }
    }
}
