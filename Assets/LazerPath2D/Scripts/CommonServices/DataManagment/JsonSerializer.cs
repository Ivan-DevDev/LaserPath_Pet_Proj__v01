using Newtonsoft.Json;

namespace Assets.LazerPath2D.Scripts.CommonServices.DataManagment
{
    public class JsonSerializer : IDataSerializer
    {
        public TData Deserialize<TData>(string serializedData)
        {
          return  JsonConvert.DeserializeObject<TData>(serializedData, new JsonSerializerSettings
          {
              TypeNameHandling = TypeNameHandling.Auto,
              TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
          });
        }

        public string Serialize<TData>(TData data)
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            });
        }
    }
}
