namespace Assets.LazerPath2D.Scripts.CommonServices.DataManagment.DataProviders
{
    public interface IDataReader<TData> where TData : ISaveData // маркеруем этим интерфейсом данные для считывания
    {
        void ReadFrom(TData data);
    }
}
