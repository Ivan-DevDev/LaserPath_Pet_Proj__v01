using UnityEngine;

namespace Assets.LazerPath2D.Scripts.CommonServices.AssetsManagment
{
    public class ResourcesAssetLoader
    {
        public T LoadResource<T>(string resourcePath) where T : UnityEngine.Object
        {
            return Resources.Load<T>(resourcePath);
        }       
    }
}
