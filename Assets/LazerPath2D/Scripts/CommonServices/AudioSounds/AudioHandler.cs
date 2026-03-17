using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.LazerPath2D.Scripts.CommonServices.AudioSounds
{
    public class AudioHandler : IDataReader<GameSettingsData>, IDataWriter<GameSettingsData>
    {
        private const float OffVolumeValue = -80f;
        private const float OnVolumeValue = 0;

        private const string MusicKey = "MusicVolume";
        private const string SoundsFXKey = "SoundFXVolume";

        private AudioMixer _audioMixer;
        GameSettingsDataProvider _gameSettingsDataProvider;

        //public bool IsMusic { get; private set; }
        //public bool IsSoundFX { get; private set; }

        public AudioHandler(AudioMixer audioMixer, GameSettingsDataProvider gameSettingsDataProvider)
        {
            _audioMixer = audioMixer;

            gameSettingsDataProvider.RegisterReader(this);
            gameSettingsDataProvider.RegisterWriter(this);

            _gameSettingsDataProvider = gameSettingsDataProvider;
        }


        public bool IsMusicOn() => IsVolumeOn(MusicKey);
        public bool IsSoundFXOn() => IsVolumeOn(SoundsFXKey);

        public void SetSettingsFromData()
        {
            if(_gameSettingsDataProvider.Data.IsMusic)
                OnMusic();
            else
                OffMusic();

            if(_gameSettingsDataProvider.Data.IsSoundFX)
                OnSoundFX();
            else
                OffSoundFX();
        }

        public void OnMusic()
        {
            OnVolume(MusicKey);
            //IsMusic = true;
        }

        public void OffMusic()
        {
            OffVolume(MusicKey);

            //IsMusic = false;
        }

        public void OnSoundFX()
        {
            OnVolume(SoundsFXKey);
            //IsSoundFX = true;
        }

        public void OffSoundFX()
        {
            OffVolume(SoundsFXKey);
            //IsSoundFX = false;
        }


        private void OnVolume(string Key) => _audioMixer.SetFloat(Key, OnVolumeValue);

        private void OffVolume(string Key) => _audioMixer.SetFloat(Key, OffVolumeValue);



        private bool IsVolumeOn(string Key)
        {
            return _audioMixer.GetFloat(Key, out var volumeValue) &&
                  Mathf.Abs(volumeValue - OnVolumeValue) <= 0.01f;
        }



        public void ReadFrom(GameSettingsData data)
        {
            if (data.IsMusic)
                OnMusic();
            else
                OffMusic();


            if (data.IsSoundFX)
                OnSoundFX();
            else
                OffSoundFX();


            _audioMixer.GetFloat(MusicKey, out var volumeValue);
        }

        public void WriteTo(GameSettingsData data)
        {
            data.IsMusic = IsMusicOn();
            data.IsSoundFX = IsSoundFXOn();

            //Debug.Log($" Сохранили настройки звука ");
        }
    }
}
