using System.IO;
using UnityEngine;

namespace EmpireX.Save
{
    public class FileStorage : IStorage
    {
        private string GetPath(string key) => Path.Combine(Application.persistentDataPath, $"{key}.sav");

        public void Write(string key, string data)
        {
            File.WriteAllText(GetPath(key), data);
        }

        public string Read(string key)
        {
            return File.ReadAllText(GetPath(key));
        }

        public bool Exists(string key)
        {
            return File.Exists(GetPath(key));
        }

        public void Delete(string key)
        {
            if (Exists(key)) File.Delete(GetPath(key));
        }
    }
}
