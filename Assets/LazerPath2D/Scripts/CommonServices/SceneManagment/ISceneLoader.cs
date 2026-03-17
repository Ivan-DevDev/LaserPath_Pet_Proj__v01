using System.Collections;
using UnityEngine.SceneManagement;

namespace Assets.LazerPath2D.Scripts.CommonServices.SceneManagment
{
    public interface ISceneLoader
    {
        IEnumerator LoadAsync(SceneID sceneID, LoadSceneMode loadSceneMode = LoadSceneMode.Single);//LoadSceneMode.Single: полностью сцену                                                                                                     
        
    }
}
