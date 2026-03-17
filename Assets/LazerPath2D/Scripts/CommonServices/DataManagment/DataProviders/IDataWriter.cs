namespace Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders
{
    public interface IDataWriter<TData> where TData : ISaveData // маркеруем этим интерфейсом данные для записи
    {
        void WriteTo(TData data);
    }
}
