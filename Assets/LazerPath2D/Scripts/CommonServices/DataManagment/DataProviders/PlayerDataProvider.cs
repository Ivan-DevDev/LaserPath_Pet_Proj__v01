using Assets.LazerPath2D.Scripts.Configs.GamePlay.Levels;
using System.Collections.Generic;

namespace Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders
{
    public class PlayerDataProvider : DataProvider<PlayerData> // провайдер для "PlayerData"
    {
        public PlayerDataProvider(ISaveLoadService saveLoadService) : base(saveLoadService)
        {
        }

        public IReadOnlyList<int> CompletedLevels => CompletedLevels;

        protected override PlayerData GetOriginData()
        {
            // создаём PlayerData по умолчанию
            PlayerData originPlayerData = new PlayerData()//временно, потом брать из конфига !!!
            {
                CompletedLevels = InitCompletedLevels(),
                ActiveStarsInLevels = InitActiveStarsInLevels(),                
            };

            return originPlayerData;
        }

        private List<int> InitCompletedLevels()
        {
            List<int> completedLevels = new();

            return completedLevels;
        }

        private Dictionary<int, int> InitActiveStarsInLevels()
        {

            Dictionary<int, int> activeStarsInLevels = new();

            return activeStarsInLevels;
        }
    }
}
