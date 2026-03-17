using Assets.LazerPath2D.Scripts.CommonUI;
using Assets.LazerPath2D.Scripts.CommonUI.View;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.MainMenu.UI
{
    public class MainMenuUIRoot : MonoBehaviour, IView
    {
        [field: SerializeField] public Transform HUDLayer {  get; private set; }

        [field : SerializeField] public Transform VFXUnderPopup {  get; private set; }

        [field: SerializeField] public Transform PopupsLayer { get; private set; }

        [field: SerializeField] public Transform VFXOverPopup { get; private set; }
    }
}
