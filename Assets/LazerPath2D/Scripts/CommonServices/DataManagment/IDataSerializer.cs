namespace Assets.LazerPath2D.Scripts.CommonServices.DataManagment
{
    public interface IDataSerializer
    {
        // данные для преобразования в строку
        string Serialize<TData>(TData data);

        // данные преобразованные из строки в нужный тип данных
        TData Deserialize<TData>(string serializedData);
    }
}
