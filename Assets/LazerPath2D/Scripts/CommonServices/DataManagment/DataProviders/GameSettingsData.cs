using System;

namespace Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders
{
    [Serializable]
    public class GameSettingsData : ISaveData
    {
        public bool IsMusic;
        public bool IsSoundFX;

        //public int SoundFXVolume;
        //public int MusicVolume;
    }
}
