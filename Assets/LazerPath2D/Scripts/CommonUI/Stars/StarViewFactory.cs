using Assets.LazerPath2D.Scripts.CommonUI.Stars;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.CommonServices.UI.Stars
{
    public class StarViewFactory
    {
        public StarView CreateStarGamePlayView(Transform parent)
        {
            StarView starViewPrefab = Resources.Load<StarView>("CommonUI/Stars/StarGamePlayView");

            StarView starView = UnityEngine.Object.Instantiate(starViewPrefab, parent);

            return starView;
        }

        public StarView CreateStarLevelView(Transform parent)
        {
            StarView starViewPrefab = Resources.Load<StarView>("CommonUI/Stars/StarLevelView");

            StarView starView = UnityEngine.Object.Instantiate(starViewPrefab, parent);

            return starView;
        }
    }
}
