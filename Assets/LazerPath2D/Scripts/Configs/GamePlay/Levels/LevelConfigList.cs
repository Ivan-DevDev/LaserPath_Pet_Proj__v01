using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.LazerPath2D.Scripts.Configs.GamePlay.Levels
{
    [CreateAssetMenu(menuName = "Configs/GamePlay/LevelConfigList ", fileName = "LevelConfigList")]
    public class LevelConfigList : ScriptableObject
    {
        [SerializeField] private List<LevelConfig> _levelsConfigList;
        [SerializeField] private List<LevelColor> _levelsColorList;

        public IReadOnlyList<LevelConfig> LevelsConfigList => _levelsConfigList;
        public IReadOnlyList<LevelColor> LevelsColorList => _levelsColorList;
        public int MaxLevelNumber => _levelsConfigList.Count;

       
    }

    [Serializable]
    public class LevelColor
    {
        [field: SerializeField] public Color LaserColor { get; private set; }

        [field: SerializeField] public Color ContourColor { get; private set; }
        [field: SerializeField] public Color ActiveFillColor { get; private set; }
        [field: SerializeField] public Color DeactiveFillColor { get; private set; }

        [field: SerializeField] public Color OtherColor { get; private set; }

        [field: SerializeField] public Color GamePlayBackgroundColor { get; private set; }
    }
}
