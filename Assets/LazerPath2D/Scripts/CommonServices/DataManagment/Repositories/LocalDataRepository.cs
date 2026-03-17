using System.IO;
using UnityEngine;


namespace Assets.LazerPath2D.Scripts.CommonServices.DataManagment.Repositories
{
    public class LocalDataRepository : IDataRepository // хранилище для мобилок и ПК, для Web сделать отдельно
    {
        private const string SaveFileExtention = "Json";// тип расширения файла для сохранения

        private string FolderPath => Application.persistentDataPath;

        public bool Exists(string key)
        {
           return File.Exists(FullPathFor(key));// проверяем содержится ли файл по заданному пути, если да то возвращаем файл
        }

        public string Read(string key)
        {
            return File.ReadAllText(FullPathFor(key));
        }

        public void Remove(string key)
        {
            File.Delete(FullPathFor(key));
        }

        public void Write(string key, string serializedData)
        {
            File.WriteAllText(FullPathFor(key), serializedData);
        }

        private string FullPathFor(string key) => Path.Combine(FolderPath, key) + "." + SaveFileExtention;// путь сохранения файла + расшерение(SaveFileExtention)
    }
}
