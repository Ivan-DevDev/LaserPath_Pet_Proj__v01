using Assets.LazerPath2D.Scripts.CommonServices.AudioSounds;
using Assets.LazerPath2D.Scripts.CommonServices.CoroutinePerformer;
using Assets.LazerPath2D.Scripts.CommonUI.Presenter;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.CommonUI.Popup
{
    public abstract class PopupPresenterBase : IPresenter
    {
        // вариант на закрытие попапа по запросу вьюхи
        public event Action<PopupPresenterBase> CloseRequest;

        private readonly ICoroutinePerformer _coroutinePerformer;
        private readonly PlaySound _playSound;

        private Coroutine _process;

        protected PopupPresenterBase(ICoroutinePerformer coroutinePerformer, PlaySound playSound)
        {
            _coroutinePerformer = coroutinePerformer;
            _playSound = playSound;
        }

        protected abstract PopupViewBase PopupView { get; }

        public virtual void Initialize()
        {           
        }

        public virtual void Dispose()
        {
            KillProssec();
            PopupView.CloseRequest -= OnCloseRequest;
        }

        public void Show()
        {
            KillProssec();

            _process = _coroutinePerformer.StartPerform(ProssecShow());
        }

        public void Hide(Action callback = null)
        {
            KillProssec();

            _process = _coroutinePerformer.StartPerform(ProssecHide(callback));
        }

        protected virtual void OnPreShow() 
        {
            // вариант вызова на закрытие попапа по запросу вьюхи
            PopupView.CloseRequest += OnCloseRequest;
        }

        protected virtual void OnPostShow() 
        {
            
        }

        protected virtual void OnPostHide() { }
       
        protected virtual void OnPreHide() 
        {           
            PopupView.CloseRequest -= OnCloseRequest;
        } 
        

        protected void OnCloseRequest()
        {
            // выкидываем событие что вьюху закрыли
           CloseRequest?.Invoke(this);
        }

        private IEnumerator ProssecShow()
        {
            OnPreShow();

           yield return PopupView.Show().WaitForCompletion();// дожедаемся завершения анимации открытия

            OnPostShow();

            yield return null;
        }

        private IEnumerator ProssecHide(Action callback)
        {
            OnPreHide();

           yield return PopupView.Hide().WaitForCompletion();// дожедаемся завершения анимации закрытия

            OnPostHide();

            callback?.Invoke();// коллбэк для какого-то действия после закрытия попапа            
        }

        private void KillProssec()
        {
            if(_process != null)
            _coroutinePerformer.StopPerform(_process);
        }
    }
}
