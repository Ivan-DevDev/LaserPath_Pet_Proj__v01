using Unity.Cinemachine;
using UnityEngine;
using System;
using Assets.LazerPath2D.Scripts.DI;
using Assets.LazerPath2D.Scripts.Configs.GamePlay.Camera;

namespace Assets.LazerPath2D.Scripts.GamePlay.Managers
{
    public class CameraManager : IInitializable, IDisposable
    {
        private CinemachineCamera _cinemachineCamera;

        private CinemachineImpulseSource _impulseSource;
        private CinemachineImpulseDefinition _impulseDefinition;
        private CinemachineImpulseListener _impulseListener;

        private float _defaultLensSize;

        private CameraConfig _cameraConfig;
        private Camera _cameraMain;

        public CameraManager(CinemachineCamera cinemachineCamera, CameraConfig cameraConfig)
        {
            _cinemachineCamera = cinemachineCamera;
            _cameraConfig = cameraConfig;

            if (_cinemachineCamera.GetComponent<CinemachineImpulseSource>() != null)
                _impulseSource = _cinemachineCamera.GetComponent<CinemachineImpulseSource>();
            else
                throw new ArgumentNullException($"No component {_impulseSource} !!!");

            if (_cinemachineCamera.GetComponent<CinemachineImpulseListener>() != null)
                _impulseListener = _cinemachineCamera.GetComponent<CinemachineImpulseListener>();
            else
                throw new ArgumentNullException($"No component {_impulseListener} !!!");

            _impulseDefinition = _impulseSource.ImpulseDefinition;

            _cameraMain = Camera.main;
        }

        public void Initialize()
        {
            _defaultLensSize = _cinemachineCamera.Lens.OrthographicSize;
        }

        public void Dispose()
        {

        }

        public float ImpulseDuration => _cameraConfig.ImpactTime;
        public float CameraLensSize => _cinemachineCamera.Lens.OrthographicSize;


        public void SetCameraOrthographicSize(float cameraOrthographicSize)
        {
            float defaultZoom = 7f;

            _cinemachineCamera.Lens.OrthographicSize = cameraOrthographicSize;

            if (cameraOrthographicSize < 1)
            {
                _cinemachineCamera.Lens.OrthographicSize = defaultZoom;
            }
        }

        public void SetCameraPerspectiveFieldOfView(float cameraPerspectiveFieldOfView)
        {
            float defaultZoom = 65f;

            _cinemachineCamera.Lens.FieldOfView = cameraPerspectiveFieldOfView;

            if (cameraPerspectiveFieldOfView < 1)
                _cinemachineCamera.Lens.FieldOfView = defaultZoom;
        }


        public void ToCameraShakeEffects(float _timeDelay = default)
        {
            SetConfigSettings(_cameraConfig);

            _impulseSource.GenerateImpulse(_cameraConfig.ImpactTime);
        }

        public void ToCameraZoomEffect(float value)
        {
            float multiplier = 12f;

            if (_cameraMain.orthographic)
                _cinemachineCamera.Lens.OrthographicSize += value;
            else
                _cinemachineCamera.Lens.FieldOfView += (value * multiplier);
        }

        private void SetConfigSettings(CameraConfig cameraConfig)
        {
            // impulse Listener settings
            _impulseListener.Gain = cameraConfig.ListenerGain;
            _impulseListener.ReactionSettings.AmplitudeGain = cameraConfig.ListenerAmplitude;
            _impulseListener.ReactionSettings.FrequencyGain = cameraConfig.ListenerFrequency;
            _impulseListener.ReactionSettings.Duration = cameraConfig.ListenerDuration;

            // impulse source setting
            _impulseDefinition.ImpulseDuration = cameraConfig.ImpactTime;
            _impulseDefinition.ImpulseType = cameraConfig.ImpulseTypes;
            _impulseDefinition.CustomImpulseShape = cameraConfig.ImpulseCurve;
            _impulseSource.DefaultVelocity = cameraConfig.DefaultVelocity;
        }
    }
}
