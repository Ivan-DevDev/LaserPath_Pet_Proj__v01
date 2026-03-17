namespace Assets.LazerPath2D.Scripts.CommonServices.DataManagment
{
    public interface ISaveLoadService
    {
        bool TryLoad<TData>(out TData data) where TData : ISaveData;

        void Save<TData>(TData data) where TData : ISaveData;
    }
}
