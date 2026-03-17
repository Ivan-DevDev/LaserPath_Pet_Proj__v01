using Assets.LazerPath2D.Scripts.GamePlay.Managers;
using Assets.LazerPath2D.Scripts.GamePlay.ScreenClick;
using Assets.LazerPath2D.Scripts.GamePlay.UI;
using Assets.LazerPath2D.Scripts.GamePlay.VFXConfetti;
using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.GameState.States
{
    public class GameOverState : IGameState, IDisposable
    {
        protected readonly IGameStateSwitcher StateSwitcher;

        private ScreenClickHandler _screenClickHandler;
        private GamePlayMenuPopupService _gamePlayMenuPopupService;
        private CameraManager _cameraManager;

        private float _timeDelay = 3.5f;
        private float _maxTimeDelay;
        private float _halfTimeDelay;

        private bool _isStartTimer;

        public GameOverState(
            IGameStateSwitcher stateSwitcher,
            ScreenClickHandler screenClickHandler,
            GamePlayMenuPopupService gamePlayMenuPopupService,
            CameraManager cameraManager)
        {
            StateSwitcher = stateSwitcher;
            _screenClickHandler = screenClickHandler;

            _gamePlayMenuPopupService = gamePlayMenuPopupService;
            _cameraManager = cameraManager;

            _maxTimeDelay = _timeDelay;
            _halfTimeDelay = _maxTimeDelay / 2;
        }

        public void Dispose()
        {

        }

        public void Enter()
        {
            _screenClickHandler.SetDisable();

            _isStartTimer = true;           
        }

        public void Exit()
        {
            _screenClickHandler.SetEnable();
        }

        public void ToUpdate(float deltaTime)
        {
            if (_isStartTimer == false)
                return;

            StartTimer(deltaTime);
        }

        private void StartTimer(float deltaTime)
        {
            _timeDelay -= deltaTime;
            _halfTimeDelay -= deltaTime;

            _cameraManager.ToCameraShakeEffects(_timeDelay);


            float multiplier = deltaTime * 0.4f;

            if (_halfTimeDelay > 0)
            {
                _cameraManager.ToCameraZoomEffect(-multiplier); 
            }
            else
            {
                _cameraManager.ToCameraZoomEffect(multiplier);               
            }

            if(MathF.Abs(_timeDelay - _maxTimeDelay/2) < 0.01f)
                ToSpawnVFXConfettiView();

            if (_timeDelay < 0)
            {
                _timeDelay = _maxTimeDelay;
                _halfTimeDelay = _maxTimeDelay / 2;

                _isStartTimer = false;

                _gamePlayMenuPopupService.OpenedGameOverPopupPresenter();
            }
        }

        private void ToSpawnVFXConfettiView()
        {
            float randomOffsetAxis = UnityEngine.Random.Range(-1f, 1f);
            Vector3 position = new Vector3(randomOffsetAxis, randomOffsetAxis, 0);

            VFXConfettiView vFXConfettiViewPrefab = Resources.Load<VFXConfettiView>("GamePlay/VFX/Salut_v02/ButterflyConfetti");

            VFXConfettiView vFXConfettiView = UnityEngine.Object.Instantiate(vFXConfettiViewPrefab, position, Quaternion.identity);
            

            UnityEngine.Object.Destroy(vFXConfettiView.gameObject, 3f);
        }
    }
}
