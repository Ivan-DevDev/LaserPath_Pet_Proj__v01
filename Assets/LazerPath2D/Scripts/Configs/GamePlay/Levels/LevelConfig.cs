using System;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.Configs.GamePlay.Levels
{
    [CreateAssetMenu(menuName = "Configs/GamePlay/LevelConfig", fileName = "LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public string LevelName { get; private set; }

        [field: SerializeField] public int AmountStarsNodes { get; private set ; }
    }
}
