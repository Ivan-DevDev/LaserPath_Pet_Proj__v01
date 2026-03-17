using System.Collections;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node.NodesView
{
    public class MirrorView : NodeView
    {
        private const string OpacityValue = "_OpacityValue";
        private const string ColorBase = "_ColorBase";
        private const string ColorMixer = "_ColorMixer";

        private float _maxTime = 0.7f;
       
        private float _colorIntensity = 5.5f;

        private Coroutine _coroutineEffect;

        private Material _materialLightContour;

        private void OnEnable()
        {
            _materialLightContour = LightSprite.material;

            float invisible = 0f;
            float visible = 1f;

            _materialLightContour.SetFloat(OpacityValue, invisible);
            _materialLightContour.SetFloat(ColorMixer, visible);            
        }

        private void OnDisable()
        {
            if (_coroutineEffect != null)
                this.StopCoroutine(_coroutineEffect);
        }       

        public void ToLightingMirrorContourOn(Color color)
        {

            Color colorLight = color;
            color.a = 1;

            _materialLightContour.SetColor(ColorBase, colorLight * _colorIntensity);

            if(_coroutineEffect != null)
                this.StopCoroutine(_coroutineEffect);

            _coroutineEffect = this.StartCoroutine(StartDelayLightingOn(_maxTime));
        }


        public void ToLightingMirrorContourOff()
        {
            if (_coroutineEffect != null)
                this.StopCoroutine(_coroutineEffect);

            float invisible = 0f;

            _materialLightContour.SetFloat(OpacityValue, invisible);
        }


        private IEnumerator StartDelayLightingOn(float maxTime)
        {
            float timeDelay = default;
            float stepOpacity = default;

            while (timeDelay < maxTime)
            {
                timeDelay += Time.deltaTime;

                stepOpacity = Mathf.Lerp(0, 1f, timeDelay/maxTime);

                _materialLightContour.SetFloat(OpacityValue, stepOpacity);

                yield return null;
            }

            _coroutineEffect = null;
        }
    }
}
