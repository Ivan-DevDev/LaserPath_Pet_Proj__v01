using System;
using System.Collections.Generic;

namespace Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders
{
    [Serializable]
    public class PlayerData : ISaveData // данные игрока(пройденные уровни, кошелёк и т.д)
    {
        public List<int> CompletedLevels;

        public Dictionary<int, int> ActiveStarsInLevels;        

    }
}
