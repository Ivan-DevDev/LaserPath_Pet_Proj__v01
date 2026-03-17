using UnityEngine;

namespace Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders
{
    public class GameSettingsDataProvider : DataProvider<GameSettingsData>
    {
        public GameSettingsDataProvider(ISaveLoadService saveLoadService) : base(saveLoadService)
        {
        }

        protected override GameSettingsData GetOriginData()
        {
            // временно потом загружать из конфигов
            GameSettingsData gameSettingsData = new GameSettingsData()
            {
                IsMusic = true,
                IsSoundFX = true,

                //SoundFXVolume = 1,
                //MusicVolume = 1,

            };

            return gameSettingsData;
        }

        
    }
}
