using Assets.LazerPath2D.Scripts.CommonServices.AssetsManagment;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.LazerPath2D.Scripts.CommonUI.View
{
    public class ViewsFactory
    {
        private readonly ResourcesAssetLoader _resourcesAssetLoader;

        private readonly Dictionary<string, string> _viewIDToResourcesPath = new()
        {            
            {ViewIDs.MainMenuScreenView,"MainMenu/UI/MainMenuScreenView" },
            {ViewIDs.EnvirMainMenuObjView,"MainMenu/Environment/Envir_MainMenuObjView" },

            {ViewIDs.OptionsMenuPopupView,"MainMenu/UI/OptionsMenuPopup/OptionsMenuPopupView"},
            {ViewIDs.LevelMenuPoupView, "MainMenu/UI/LevelsMenuPopup/LevelsMenuPopupView"},

            {ViewIDs.LevelsMenuPopupView, "MainMenu/UI/LevelsMenuPopup/LevelsMenuPopupView" },
            //{ViewIDs.LevelTileListView, "MainMenu/UI/LevelsMenuPopup/LevelTileListView" },
            {ViewIDs.LevelTileView, "MainMenu/UI/LevelsMenuPopup/LevelTileView" },

            {ViewIDs.StarLevelView,"CommonUI/Stars/StarLevelView"},
            {ViewIDs.StarGamePlayView, "CommonUI/Stars/StarGamePlayView" },

            {ViewIDs.GamePlayMenuScreenView, "GamePlay/UI/GamePlayMenuScreenView" },
            {ViewIDs.PauseMenuPopupView,"GamePlay/UI/PauseMenuPopup/PauseMenuPopupView" },
            {ViewIDs.GameOverPopupView, "GamePlay/UI/GameOverPopup/GameOverPopupView" },
            {ViewIDs.GamePlayBackgroundView,"GamePlay/UI/Background/BackGroundCanvas" },
            {ViewIDs.EnvirBackgroundObjView,"GamePlay/ObjectsInScene/Environment/EnvirBackgroundObjView"}
        };

        public ViewsFactory(ResourcesAssetLoader resourcesAssetLoader)
        {
            _resourcesAssetLoader = resourcesAssetLoader;
        }

        public TView Create<TView>(string viewID, Transform parent = null) where TView : MonoBehaviour, IView
        {
            if (_viewIDToResourcesPath.TryGetValue(viewID, out string resourcePath) == false)
                throw new ArgumentException($" Path to view is faild {typeof(TView)}, searched id: {viewID} !!!");


            GameObject prefb = _resourcesAssetLoader.LoadResource<GameObject>(resourcePath);

            GameObject instance = Object.Instantiate(prefb, parent);

            TView view = instance.GetComponent<TView>();

            if (view == null)
                throw new InvalidOperationException($" Not found {typeof(TView)} component on view instance !!! ");

            return view;
        }

        public void Release<TView>(TView view) where TView : MonoBehaviour, IView
        {
            Object.Destroy(view.gameObject);
        }
    }
}
