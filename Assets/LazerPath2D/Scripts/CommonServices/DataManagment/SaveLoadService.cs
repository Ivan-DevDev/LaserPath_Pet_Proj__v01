namespace Assets.LazerPath2D.Scripts.CommonServices.DataManagment
{
    public class SaveLoadService : ISaveLoadService
    {
        //какой-то сериалайзер данных
        private readonly IDataSerializer _serializer;
        // какой-то репозиторий данных
        private readonly IDataRepository _repository;// для Web поменять на "PlayerPrefs"


        public SaveLoadService(IDataSerializer serializer, IDataRepository repository)
        {
            _serializer = serializer;
            _repository = repository;
        }

        public void Save<TData>(TData data) where TData : ISaveData
        {
            string serializeData = _serializer.Serialize(data);
            _repository.Write(SaveDataKeys.GetKeyFor<TData>(), serializeData);// получаем ключ от даты и по этому ключу записываем
        }

        public bool TryLoad<TData>(out TData data) where TData : ISaveData
        {
            string key = SaveDataKeys.GetKeyFor<TData>();// получаю ключ

            if(_repository.Exists(key) == false) // запрашиваю у репозитория, есть ли данные по такому ключу
            {
                data = default(TData);
                return false;
            } 
            
            string serializedData = _repository.Read(key); // считываем дату по ключу
            data = _serializer.Deserialize<TData>(serializedData);// приводим данные к нужному типу
            return true;
        }
    }
}
