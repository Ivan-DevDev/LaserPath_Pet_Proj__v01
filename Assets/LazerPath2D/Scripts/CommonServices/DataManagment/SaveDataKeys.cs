using Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders;
using System;
using System.Collections.Generic;

namespace Assets.LazerPath2D.Scripts.CommonServices.DataManagment
{
    public static class SaveDataKeys
    {
        // словарь для хранения ключей(ключ это тип данных)
        private static Dictionary<Type, string> Keys = new Dictionary<Type, string>()
        {
            // добавляем новые типы данных
            {typeof(PlayerData), "PlayerData" },
            {typeof(GameSettingsData), "GameSettingsData" },

        };

        // достаём из словаря по ключу данные
        public static string GetKeyFor<TData>() where TData : ISaveData
        {
            return Keys[typeof(TData)];
        }
    }
}
