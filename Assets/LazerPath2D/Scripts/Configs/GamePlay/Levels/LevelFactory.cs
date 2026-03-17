using Assets.LazerPath2D.Scripts.CommonServices.ConfigsManagment;
using Assets.LazerPath2D.Scripts.DI;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.Configs.GamePlay.Levels
{
    public class LevelFactory
    {
        private const string _levelPathPrefabs = "GamePlay/Levels/";

        //private ConfigsProviderService _configsProviderService;

        private LevelConfigList _levelConfigList;
        private int _maxLevelNumber;

        public LevelFactory(DIContainer container)
        {
            _levelConfigList = container.Resolve<ConfigsProviderService>().LevelConfigList;

            _maxLevelNumber = _levelConfigList.LevelsConfigList.Count;
        }

        public int MaxLevelNumber => _maxLevelNumber;

        public Level CreateLevel(int numberLevel)
        {
            int indexLevel = numberLevel - 1;

            string levelPrefabName = _levelConfigList.LevelsConfigList[indexLevel].LevelName;

            Level levelPrefab = Resources.Load<Level>(_levelPathPrefabs + levelPrefabName);
            
            Level level = UnityEngine.Object.Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);

            return level;
        }
    }
}
