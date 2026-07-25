using EmpireX.Data;
using System;

namespace EmpireX.Save
{
    public class SaveService : ISaveService
    {
        private readonly ISerializer _serializer;
        private readonly IStorage _storage;
        private readonly IEncryptor _encryptor;

        private const string CURRENT_VERSION = "1.0.0";

        public SaveService(ISerializer serializer, IStorage storage, IEncryptor encryptor)
        {
            _serializer = serializer;
            _storage = storage;
            _encryptor = encryptor;
        }

        public void Save(string slotId, SaveData data)
        {
            data.SaveVersion = CURRENT_VERSION;
            data.SaveDate = DateTime.UtcNow.Ticks;

            string json = _serializer.Serialize(data);
            string encrypted = _encryptor.Encrypt(json);
            
            // Backup existing
            if (_storage.Exists(slotId))
            {
                string existingData = _storage.Read(slotId);
                _storage.Write(slotId + "_backup", existingData);
            }

            _storage.Write(slotId, encrypted);
        }

        public SaveData Load(string slotId)
        {
            if (!_storage.Exists(slotId)) return null;

            string encrypted = _storage.Read(slotId);
            string json = _encryptor.Decrypt(encrypted);
            
            var data = _serializer.Deserialize<SaveData>(json);
            
            // Versioning / Migration
            if (data != null && data.SaveVersion != CURRENT_VERSION)
            {
                Migrate(data);
            }

            data?.ValidateAndInitializeMissing();
            return data;
        }

        public bool HasSave(string slotId) => _storage.Exists(slotId);
        
        public void Delete(string slotId) => _storage.Delete(slotId);
        
        private void Migrate(SaveData data)
        {
            data.SaveVersion = CURRENT_VERSION;
        }
    }
}
