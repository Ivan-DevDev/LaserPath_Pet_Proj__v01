using Assets.LazerPath2D.Scripts.Configs.Common.Sound;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.LazerPath2D.Scripts.CommonServices.AudioSounds
{
    public class PlaySound : MonoBehaviour
    {
        [Header("Links")]

        [SerializeField] private AudioSource _musicSource;

        [SerializeField] private AudioSource _soundFXSource;

        [SerializeField] private AudioSource _soundFXSpecial;

        [Header("Sound Clips")]
        [SerializeField] private SoundPrefabsConfigs _soundPrefabsConfigs;

        private float _pitchDefault = 1f;
        private float _volumeSoundFXDefault;

        private AudioHandler _audioHandler;
        private Coroutine _playAudoiClipsInOderCoroutine;

        public void Initialize(AudioHandler audioHandler)
        {
            _audioHandler = audioHandler;
            _volumeSoundFXDefault = _soundFXSource.volume;

            DontDestroyOnLoad(this);
        }

        public bool IsMusicPlay => _audioHandler.IsMusicOn();

        public bool IsSoundFXPlay => _audioHandler.IsSoundFXOn();

        public AudioSource MusicSource => _musicSource;


        public void ToOnPlayMusic() => _audioHandler.OnMusic();

        public void ToOffPlayMusic() => _audioHandler.OffMusic();

        public void ToOnPlaySounsFX() => _audioHandler.OnSoundFX();

        public void ToOffPlaySoundsFX() => _audioHandler.OffSoundFX();


        public void ToPlayMusicClip()
        {
            if (_soundPrefabsConfigs.MusicPlayClips.Length > 0)
                _musicSource.clip = SetRandomClip(_soundPrefabsConfigs.MusicPlayClips);
            else
                throw new IndexOutOfRangeException(" No prefabs music" + (nameof(_musicSource.clip)));

            float musicVolume = 0.40f;
            _musicSource.volume = musicVolume;

            _musicSource.Play();
        }

        public void ToPlayAudoiClipsInOder()
        {
            if (_soundPrefabsConfigs.MusicPlayClips.Length == 0)
                throw new IndexOutOfRangeException(" No prefabs music" + (nameof(_musicSource.clip)));

            float musicVolume = 0.40f;
            _musicSource.volume = musicVolume;

            if (_playAudoiClipsInOderCoroutine != null)
            {
                this.StopCoroutine(_playAudoiClipsInOderCoroutine);

                _playAudoiClipsInOderCoroutine = null;
            }

            _playAudoiClipsInOderCoroutine = StartCoroutine(StartPlayInOder(_soundPrefabsConfigs.MusicPlayClips));
        }

        public void OnRotateEmiterNodeClip()
        {
            _soundFXSource.volume = 0.4f;
            _soundFXSource.pitch = Random.Range(1f, 1.5f); ;
            _soundFXSource.clip = SetRandomClip(_soundPrefabsConfigs.RotateEmiterNodeClips);
            _soundFXSource.Play();
        }

        public void OnRotateMirrorNodeClip()
        {
            _soundFXSource.volume = 0.5f;
            _soundFXSource.pitch = _pitchDefault;
            _soundFXSource.clip = SetRandomClip(_soundPrefabsConfigs.RotateMirrorNodeClips);
            _soundFXSource.PlayOneShot(_soundFXSource.clip);
        }

        public void OnNodeReceiverCompletedClip()
        {
            _soundFXSource.volume = _volumeSoundFXDefault;
            _soundFXSource.pitch = Random.Range(0.8f, 1.5f);
            _soundFXSource.clip = SetRandomClip(_soundPrefabsConfigs.NodeReceiverCompletedClips);
            _soundFXSource.PlayOneShot(_soundFXSource.clip);
        }

        public void OnGameOverCompletedClip()
        {
            _soundFXSource.volume = _volumeSoundFXDefault;
            _soundFXSource.pitch = Random.Range(0.8f, 1.2f);
            _soundFXSource.clip = SetRandomClip(_soundPrefabsConfigs.GameOverCompletedClips);
            _soundFXSource.PlayOneShot(_soundFXSource.clip);
        }


        public void OnUIAnimScalingClip()
        {
            _soundFXSource.volume = _volumeSoundFXDefault;
            _soundFXSource.pitch = Random.Range(0.8f, 2.2f);
            _soundFXSource.clip = SetRandomClip(_soundPrefabsConfigs.UIAnimScalingClips);
            _soundFXSource.PlayOneShot(_soundFXSource.clip);
        }

        public void OnUIAnimVibratoClip()
        {
            _soundFXSource.volume = _volumeSoundFXDefault;
            _soundFXSource.pitch = Random.Range(0.8f, 1.5f);
            _soundFXSource.clip = SetRandomClip(_soundPrefabsConfigs.UIAnimVibratoClips);
            _soundFXSource.PlayOneShot(_soundFXSource.clip);
        }

        public void OnUIAnimAttentionClips()
        {
            _soundFXSource.volume = _volumeSoundFXDefault;
            _soundFXSource.pitch = Random.Range(0.8f, 1.5f);
            _soundFXSource.clip = SetRandomClip(_soundPrefabsConfigs.UIAnimAttentionClips);
            _soundFXSource.PlayOneShot(_soundFXSource.clip);
        }

        public void OnUIClikedButtonClips()
        {
            _soundFXSource.volume = _volumeSoundFXDefault;
            _soundFXSource.pitch = Random.Range(0.8f, 1.5f);
            _soundFXSource.clip = SetRandomClip(_soundPrefabsConfigs.UIClikedButtonClips);
            _soundFXSource.PlayOneShot(_soundFXSource.clip);
        }


        private AudioClip SetRandomClip(AudioClip[] audioClips)
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }

        private IEnumerator StartPlayInOder(AudioClip[] audioClips)
        {
            AudioClip audioClip = SetRandomClip(audioClips);

            _musicSource.clip = audioClip;
            _musicSource.Play();

            while (_musicSource.isPlaying)
            {
                yield return null;
            }

            _playAudoiClipsInOderCoroutine = StartCoroutine(StartPlayInOder(audioClips));
        }
    }
}
