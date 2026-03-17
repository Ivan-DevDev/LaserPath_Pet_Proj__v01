using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

namespace Assets.LazerPath2D.Scripts.CommonServices.SceneManagment
{
    public class DefaultSceneLoader : ISceneLoader
    {
        public event Action<float> LoadingProgress;       

        public IEnumerator LoadAsync(SceneID sceneID, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            AsyncOperation waitLoading = SceneManager.LoadSceneAsync(sceneID.ToString(), loadSceneMode);
           

            while (waitLoading.isDone == false)
            {
                LoadingProgress?.Invoke(waitLoading.progress);

                yield return null;
            }
        }        
    }
}
