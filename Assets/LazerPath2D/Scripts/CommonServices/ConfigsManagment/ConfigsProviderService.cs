using Assets.LazerPath2D.Scripts.CommonServices.AssetsManagment;
using Assets.LazerPath2D.Scripts.Configs.GamePlay.Camera;
using Assets.LazerPath2D.Scripts.Configs.GamePlay.Levels;
using System;
using UnityEngine;


namespace Assets.LazerPath2D.Scripts.CommonServices.ConfigsManagment
{
    public class ConfigsProviderService
    {
        private ResourcesAssetLoader _resourcesAssetLoader;

        public ConfigsProviderService(ResourcesAssetLoader resourcesAssetLoader)
        {
            _resourcesAssetLoader = resourcesAssetLoader;
        }

        public LevelConfigList LevelConfigList { get; private set; }
        public CameraConfig CameraConfig { get; private set; }


        public void LoadAll()
        {
            // подгружаем конфиги из ресурсов
            LoadLevelConfigList();
            LoadCameraConfig();
        }

        private void LoadCameraConfig()
        {
            CameraConfig = _resourcesAssetLoader.LoadResource<CameraConfig>("GamePlay/Configs/Camera/CameraConfig");
        }

        private void LoadLevelConfigList()
        {
            LevelConfigList = _resourcesAssetLoader.LoadResource<LevelConfigList>("GamePlay/Configs/Levels/LevelConfigList");            
        }
    }
}
