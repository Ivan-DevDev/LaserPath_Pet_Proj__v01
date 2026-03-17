using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.CommonUI.PlaySounds
{
    public class PlaySoundViewPresenter : IPresenter
    {
        // model
        private PlaySound _playSound;

        // view
        private PlaySoundView _view;

        public PlaySoundViewPresenter(PlaySound playSound, PlaySoundView view)
        {
            _playSound = playSound;
            _view = view;
        }

        public void Initialize()
        {
            _view.MusicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
            _view.SoundSlider.onValueChanged.AddListener(OnSoundSliderChanged);

            ToSetMusicSliderView();
            ToSetSoundSliderView();
        }

        public void Dispose()
        {
            _view.MusicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
            _view.SoundSlider.onValueChanged.RemoveListener(OnSoundSliderChanged);
        }


        private void OnMusicSliderChanged(float value)
        {
            if (value == 1)
                _playSound.ToOnPlayMusic();
            else 
                _playSound.ToOffPlayMusic();
        }

        private void OnSoundSliderChanged(float value)
        {
            if (value == 1)
                _playSound.ToOnPlaySounsFX();
            else 
                _playSound.ToOffPlaySoundsFX();
        }

        private void ToSetMusicSliderView()
        {
            if (_playSound.IsMusicPlay)
            {
                _view.SetMusicSlider(1);

            }
            else
            {
                _view.SetMusicSlider(0);

            }           
        }

        private void ToSetSoundSliderView()
        {
            if (_playSound.IsSoundFXPlay)
            {
                _view.SetSoundFXSlider(1);

            }
            else
            {

                _view.SetSoundFXSlider(0);
            }           
        }
    }
}
