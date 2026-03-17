using Assets.LazerPath2D.Scripts.CommonServices.SceneManagment;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LazerPath2D.Scripts.CommonServices.LoadingScreen
{
    public class StandartLoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        //model
        private const string Precentage = "_Precentage";

        private DefaultSceneLoader _defaultSceneLoader;

        //view
        [SerializeField] private Image _loadingViewImage;
        [SerializeField] private TMP_Text _textLoading;

        public void Initialize(ISceneLoader _sceneLoader)
        {
            if (_sceneLoader is DefaultSceneLoader defaultSceneLoader)
            {
                _defaultSceneLoader = defaultSceneLoader;
                _defaultSceneLoader.LoadingProgress += OnLoadingProgress;
            }
        }

        private void Awake()
        {
            Hide();
            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            if( _defaultSceneLoader != null )
                _defaultSceneLoader.LoadingProgress -= OnLoadingProgress;
        }

        public bool IsShow => this.gameObject.activeSelf;
        public void SetTextLoading(string text) => _textLoading.text = text;

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);

            //if (_defaultSceneLoader != null)
            //{
            //    if (_defaultSceneLoader.LoadingOperation != null)
            //    {
            //        //_loadingViewImage.fillAmount = _defaultSceneLoader.LoadingOperation.progress;


            //        //_loadingViewImage.material.SetFloat(Precentage, _defaultSceneLoader.LoadingOperation.progress);

            //        //string defaultText = _textLoading.text;

            //        //_textLoading.text = $" {defaultText} {(_defaultSceneLoader.LoadingOperation.progress * 100)}%";
            //    }
            //}
        }

        private void OnLoadingProgress(float value)
        {
            float progress = Mathf.Clamp01(value / 0.9f);

            _loadingViewImage.material.SetFloat(Precentage, progress);
        }
    }
}
