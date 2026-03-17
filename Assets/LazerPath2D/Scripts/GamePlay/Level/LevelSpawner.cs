using Assets.LazerPath2D.Scripts.Configs.GamePlay.Levels;
using Assets.LazerPath2D.Scripts.GamePlay.Node;
using System.Collections.Generic;
using System;


namespace Assets.LazerPath2D.Scripts.GamePlay.Level
{
    public class LevelSpawner
    {
        private LevelFactory _levelFactory;        

        private Configs.GamePlay.Levels.Level _currentLevel;

        private List<INode> _nodes;

        private int _maxLevelNumber;

        public LevelSpawner(LevelFactory levelFactory)
        {
            _levelFactory = levelFactory;
            _maxLevelNumber = _levelFactory.MaxLevelNumber;
        }

        public Configs.GamePlay.Levels.Level CurrentLevel => _currentLevel;
        public IReadOnlyList<INode> Nodes => _nodes;
        public int MaxLevelNumber => _maxLevelNumber;

        public void SpawnLevel(int numberLevel)
        {
            if(numberLevel > _maxLevelNumber) 
                throw new ArgumentOutOfRangeException(nameof(numberLevel));

            Configs.GamePlay.Levels.Level level = _levelFactory.CreateLevel(numberLevel);

            _currentLevel = level;

            List<INode> nodes = new();

            nodes.AddRange(_currentLevel.LaserReceivers);
            nodes.AddRange(_currentLevel.Mirrors);
            nodes.AddRange(_currentLevel.StarNodes);
            nodes.AddRange(_currentLevel.LaserEmiters);

            _nodes = nodes;
        }

        public void RemoveSpawnedLevel()
        {
            if (_nodes.Count > 0)
            {
                _nodes.Clear();
            }

            if(_currentLevel != null)
            {
                UnityEngine.Object.Destroy(_currentLevel.gameObject);
            }             
        }
    }
}
