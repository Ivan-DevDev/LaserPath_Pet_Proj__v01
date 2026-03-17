using System.Collections.Generic;
using System;

namespace Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders
{
    public abstract class DataProvider<TData> where TData : ISaveData // класс будет управлять всеми датами
                                                                      // опрашивать все даты и сохранять или загружать их
    {
        private readonly ISaveLoadService _saveLoadService;

        private List<IDataWriter<TData>> _writers = new();
        private List<IDataReader<TData>> _readers = new();
        protected DataProvider(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public TData Data { get; private set; }// поле для данных

        public void RegisterWriter(IDataWriter<TData> writer)
        {
            if (_writers.Contains(writer))
                throw new ArgumentException(nameof(writer));

            _writers.Add(writer);
        }

        public void RegisterReader(IDataReader<TData> reader)
        {
            if (_readers.Contains(reader))
                throw new ArgumentException(nameof(reader));

            _readers.Add(reader);
        }

        public void Load()
        {
            // пробуем подгрузить данные
            if (_saveLoadService.TryLoad(out TData data))
                Data = data;
            else
                Reset();

            //считываем данные у всех Data
            foreach (IDataReader<TData> reader in _readers)
                reader.ReadFrom(Data);
        }

        public void Save()
        {
            //проходимся по сервисам, записываем новую Data
            foreach (IDataWriter<TData> writer in _writers)
                writer.WriteTo(Data);

            // и сохраняем
            _saveLoadService.Save(Data);
        }

        public void Reset()
        {
            // формируем начальные данные
            Data = GetOriginData();
            Save();
        }

        protected abstract TData GetOriginData();// каждая data будет переопределять метод и формировать данные для себя       
    }
}
