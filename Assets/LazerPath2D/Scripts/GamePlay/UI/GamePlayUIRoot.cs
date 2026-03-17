using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.UI
{
    public class GamePlayUIRoot : MonoBehaviour
    {
        [field: SerializeField] public Transform HUDLayer {  get; private set; }

        [field: SerializeField] public Transform VFXUnderPopup { get; private set; }

        [field: SerializeField] public Transform PopupsLayer { get; private set; }

        [field: SerializeField]public Transform VFXOverPopup { get; private set; }        
    }
}
