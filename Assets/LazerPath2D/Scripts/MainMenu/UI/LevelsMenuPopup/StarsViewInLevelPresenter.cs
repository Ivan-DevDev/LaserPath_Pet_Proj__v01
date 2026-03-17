using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using Assets.LazerPath2D.Scripts.CommonUI.Stars;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using Assets.LazerPath2D.Scripts.MainMenu.Enviroment;
using System;
using System.Collections.Generic;

namespace Assets.LazerPath2D.Scripts.MainMenu.UI.LevelsMenuPopup
{
    public class StarsViewInLevelPresenter : IPresenter
    {
       
        private readonly ViewsFactory _viewsFactory;        

        public StarsViewInLevelPresenter(
            ViewsFactory viewsFactory)           
        {
            _viewsFactory = viewsFactory;           
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

        public List<StarView> GetSpawnStarView(LevelTileView levelTileViewPrefab, int amountStarsInLevel)
        {
            List<StarView> starViewList = new();

            if (amountStarsInLevel > 0)
            {
                for (int i = 0; i < amountStarsInLevel; i++)
                {
                    StarView starView = _viewsFactory.Create<StarView>(ViewIDs.StarLevelView, levelTileViewPrefab.ContainerStars);

                    starView.SetDeActive();

                    starViewList.Add(starView);
                }
            }
          
            return starViewList;
        }

        public void SetActiveStarsInLevel(List<StarView> starViewList, int activeStarsInLevel)
        {
            if (activeStarsInLevel == 0)
                return;

            if (activeStarsInLevel <= starViewList.Count)
            {
                for (int i = 0; i < activeStarsInLevel; i++)
                {
                    starViewList[i].SetActive();
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(activeStarsInLevel));
            }
        }       
    }
}
