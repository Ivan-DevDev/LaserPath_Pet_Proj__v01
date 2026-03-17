using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LazerPath2D.Scripts.CommonUI.PlaySounds
{
    public class PlaySoundView : MonoBehaviour
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;

        [SerializeField] private TMP_Text _musicOnText;
        [SerializeField] private TMP_Text _musicOffText;
        [SerializeField] private TMP_Text _soundFXOnText;
        [SerializeField] private TMP_Text _soundFXOffText;

       
        public Slider MusicSlider => _musicSlider;
        public Slider SoundSlider => _soundSlider;

        public void SetMusicSlider(float value) => _musicSlider.value = value;
        public void SetSoundFXSlider(float value) => _soundSlider.value = value;

        public void SetMusicOnText(string text) => _musicOnText.text = text;
        public void SetMusicOffText(string text) => _musicOffText.text = text;

        public void SetSoundFXOnText(string text) => _soundFXOnText.text = text;
        public void SetSoundFXOffText(string text) => _soundFXOffText.text = text;
    }
}
