using Assets.LazerPath2D.Scripts.CommonServices.ConfigsManagment;
using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using Assets.LazerPath2D.Scripts.GamePlay.Utility.Reactive;
using System;
using System.Collections.Generic;

namespace Assets.LazerPath2D.Scripts.CommonServices.LevelsService
{
    public class CompletedLevelsService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private List<int> _completedLevels = new();
        private Dictionary<int, ReactiveVariable<int>> _activeStarsInLevels = new();

        private ConfigsProviderService _configsProviderService;

        public CompletedLevelsService(
            PlayerDataProvider playerDataProvider, 
            ConfigsProviderService configsProviderService)
        {
            playerDataProvider.RegisterWriter(this);
            playerDataProvider.RegisterReader(this);

            _configsProviderService = configsProviderService;
        }

        
        public bool IsLevelNumberExists(int levelNumber) => _configsProviderService.LevelConfigList.LevelsConfigList.Count >= levelNumber;
        public bool IsLevelCompleted(int levelNumber) => _completedLevels.Contains(levelNumber);
        public IReadOnlyList<int> CompletedLevels => _completedLevels;

        public IReadOnlyVariable<int> GetActiveStarsInLevel(int numberLevel)
        {
            if (IsLevelNumberExists(numberLevel))
                return _activeStarsInLevels[numberLevel];
            else
                throw new ArgumentOutOfRangeException(nameof(numberLevel), " Запрашиваемого уровня не существует !!!");

        }

        public int GetStarsNodesInLevel(int numberLevel)
        {
            if (IsLevelNumberExists(numberLevel))
            {
                int indexlevel = numberLevel - 1;
                int starsNodesInLevel = _configsProviderService.LevelConfigList.LevelsConfigList[indexlevel].AmountStarsNodes;

                return starsNodesInLevel;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(numberLevel), " Запрашиваемого уровня не существует !!!");
            }
        }

        public void ReadFrom(PlayerData data)
        {
            _completedLevels.Clear();
            _completedLevels.AddRange(data.CompletedLevels);

            foreach (KeyValuePair<int, int> activeStars in data.ActiveStarsInLevels)
            {
                // если в словаре есть такой уровень(ключ), то обновляем кол-во активных звёзд,
                // иначе добавляем элемент в словарь "_activeStarsInLevel" этого сервиса и его значение

                if (_activeStarsInLevels.ContainsKey(activeStars.Key))
                    _activeStarsInLevels[activeStars.Key].Value = activeStars.Value;
                else
                    _activeStarsInLevels.Add(activeStars.Key, new ReactiveVariable<int>(activeStars.Value));
            }
        }

        public void WriteTo(PlayerData data)
        {
            data.CompletedLevels.Clear();
            data.CompletedLevels.AddRange(_completedLevels);


            foreach (KeyValuePair<int, ReactiveVariable<int>> activeStars in _activeStarsInLevels)
            {
                // если в словаре есть такой уровень(ключ), то обновляем кол-во активных звёзд,
                // иначе добавляем элемент в словарь даты"data.ActiveStarsInLevel" и его значение
                if (data.ActiveStarsInLevels.ContainsKey(activeStars.Key))
                    data.ActiveStarsInLevels[activeStars.Key] = activeStars.Value.Value;
                else
                    data.ActiveStarsInLevels.Add(activeStars.Key, activeStars.Value.Value);
            }
        }

        public void UpdateCompletedLevel(int LevelNumber, int amountActivStars)
        {
            int maxLevelNumber = _configsProviderService.LevelConfigList.MaxLevelNumber;

            if (LevelNumber > maxLevelNumber)
                throw new ArgumentOutOfRangeException(nameof(LevelNumber), " Нет такого номера уровня, в доступных уровнях !!!");

            // проверка что уровень пройден в порядке возрастания
            //if (LevelNumber > _completedLevels.Count + 2)
            //    throw new ArgumentException(nameof(LevelNumber),
            //        " Номер нового пройденного уровня превышает допустимое значение прохождения уровней в порядке возрастания");

            //проверка что уровень уже проходили
            if (IsLevelCompleted(LevelNumber) == false)
                _completedLevels.Add(LevelNumber);


            // обновление звёзд за прохождение уровня
            if (_activeStarsInLevels.ContainsKey(LevelNumber))
                _activeStarsInLevels[LevelNumber].Value = amountActivStars;
            else
                _activeStarsInLevels.Add(LevelNumber, new ReactiveVariable<int>(amountActivStars));

        }        
    }
}
