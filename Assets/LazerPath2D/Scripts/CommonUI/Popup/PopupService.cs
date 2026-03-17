using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.CommonUI.Popup
{
    public abstract class PopupService : IDisposable
    {
        protected readonly ViewsFactory ViewsFactory;
        protected readonly ProjectPresentersFactory _presentersFactory;
        protected readonly PlaySound _playSound;

        // словарь для хранения попапов и их вьюх
        private Dictionary<PopupPresenterBase, PopupInfo> _presentersToInfo = new();

        protected PopupService(
            ViewsFactory viewsFactory,
            ProjectPresentersFactory presentersFactory,
            PlaySound playSound)
        {
            ViewsFactory = viewsFactory;
            _presentersFactory = presentersFactory;
            _playSound = playSound;
        }

        protected abstract Transform PopupLayer { get; }

        public void Dispose()
        {
            foreach (PopupPresenterBase popupPresenter in _presentersToInfo.Keys)
            {
                popupPresenter.CloseRequest -= OnClosePopup;
                DisposeFor(popupPresenter);
            }

            _presentersToInfo.Clear();
        }

        // открытие попапа
        protected void OnPopupCreated(
            PopupPresenterBase popupPresenter,
            PopupViewBase view,
            Action closedCallback = null)
        {
            PopupInfo popupInfo = new PopupInfo(view, closedCallback);

            _presentersToInfo.Add(popupPresenter, popupInfo);

            popupPresenter.Initialize();
            popupPresenter.Show();

            _playSound.OnUIAnimScalingClip();

            popupPresenter.CloseRequest += OnClosePopup;
        }


        // закрытие попапа
        public void OnClosePopup(PopupPresenterBase popupPresenter)
        {
            popupPresenter.CloseRequest -= OnClosePopup;

            _playSound.OnUIAnimScalingClip();

            popupPresenter.Hide(() =>
            {
                _presentersToInfo[popupPresenter].ClosedCallback?.Invoke();


                DisposeFor(popupPresenter);

                _presentersToInfo.Remove(popupPresenter);// удаляем запись попапа в словаре
            });

        }


        private void DisposeFor(PopupPresenterBase popupPresenter)
        {
            popupPresenter.Dispose();
            ViewsFactory.Release(_presentersToInfo[popupPresenter].View);// удаляем вьюху
        }

        private class PopupInfo // для хранения callback, если нужно что-то сделать при закрытии попапа
        {
            public PopupInfo(PopupViewBase view, Action closedCallback)
            {
                View = view;
                ClosedCallback = closedCallback;
            }

            public PopupViewBase View { get; }

            public Action ClosedCallback { get; }
        }
    }
}
