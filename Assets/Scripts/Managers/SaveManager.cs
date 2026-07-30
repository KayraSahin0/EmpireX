using System;
using EmpireX.Events;
using EmpireX.Save;
using EmpireX.Data;

namespace EmpireX.Core
{
    /// <summary>
    /// SaveManager sınıfı.
    /// </summary>
    public class SaveManager : BaseManager
    {
        private ISaveService _saveService;
        public SaveData CurrentData { get; private set; }
        public string CurrentSlotId { get; private set; }
        
        private bool _autoSaveEnabled = false;
        
        public bool IsAutoSaveEnabled => _autoSaveEnabled;
        
        public void SetAutoSave(bool state)
        {
            _autoSaveEnabled = state;
            UnityEngine.PlayerPrefs.SetInt("AutoSaveSetting", state ? 1 : 0);
            UnityEngine.PlayerPrefs.Save();
        }

        public SaveManager(IEventBus eventBus) : base(eventBus) { }

        public override void Initialize()
        {
            ISerializer serializer = new JsonSerializer();
            IStorage storage = new FileStorage();
            IEncryptor encryptor = new SimpleEncryptor();
            
            _saveService = new SaveService(serializer, storage, encryptor);
            
            // Kullanıcının AutoSave tercihini belleğe yükle
            int pref = UnityEngine.PlayerPrefs.GetInt("AutoSaveSetting", -1);
            if (pref == -1)
            {
                // Eğer oyuncu kendi eliyle ayarı hiç değiştirmediyse ve mevcut kaydı varsa aç
                _autoSaveEnabled = GetAllSaves().Count > 0;
            }
            else
            {
                _autoSaveEnabled = pref == 1;
            }
        }

        public void ManualSave(string slotId)
        {
            SaveGame(slotId);
        }

        public void AutoSave()
        {
            if (!_autoSaveEnabled) return;
            if (CurrentData == null || string.IsNullOrEmpty(CurrentSlotId)) return; 
            
            _eventBus.Publish(new AutoSaveStarted());
            SaveGame(CurrentSlotId);
            _eventBus.Publish(new AutoSaveCompleted());
        }

        public void SaveOnExit()
        {
            if (CurrentData == null || string.IsNullOrEmpty(CurrentSlotId)) return; 
            
            SaveGame(CurrentSlotId);
        }

        private void SaveGame(string slotId)
        {
            try
            {
                _eventBus.Publish(new SaveStarted { SlotId = slotId });
                
                if (CurrentData == null)
                {
                    CurrentData = new SaveData();
                    CurrentData.ValidateAndInitializeMissing();
                }

                _saveService.Save(slotId, CurrentData);
                
                _eventBus.Publish(new SaveCompleted { SlotId = slotId });
            }
            catch (Exception ex)
            {
                _eventBus.Publish(new SaveFailed { SlotId = slotId, Error = ex.Message });
            }
        }

        public void LoadGame(string slotId)
        {
            try
            {
                _eventBus.Publish(new LoadStarted { SlotId = slotId });
                
                var data = _saveService.Load(slotId);
                if (data != null)
                {
                    CurrentData = data;
                    CurrentSlotId = slotId;
                    _eventBus.Publish(new LoadCompleted { SlotId = slotId, Data = data });
                }
                else
                {
                    // Yeni Oyun Durumu (Save bulunamadıysa)
                    CurrentData = new SaveData();
                    CurrentSlotId = slotId;
                    CurrentData.ValidateAndInitializeMissing();
                    _eventBus.Publish(new LoadCompleted { SlotId = slotId, Data = CurrentData });
                }
            }
            catch (Exception ex)
            {
                _eventBus.Publish(new LoadFailed { SlotId = slotId, Error = ex.Message });
            }
        }
        
        public bool HasSave(string slotId) => _saveService.HasSave(slotId);
        
        public void DeleteSave(string slotId)
        {
            _saveService.Delete(slotId);
        }
        
        public System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, SaveData>> GetAllSaves()
        {
            var list = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, SaveData>>();
            var dir = UnityEngine.Application.persistentDataPath;
            var files = System.IO.Directory.GetFiles(dir, "*.sav");
            
            foreach (var file in files)
            {
                string slotId = System.IO.Path.GetFileNameWithoutExtension(file);
                // Backup dosyalarını atla
                if (slotId.EndsWith("_backup")) continue;
                
                try 
                {
                    var data = _saveService.Load(slotId);
                    if (data != null)
                    {
                        list.Add(new System.Collections.Generic.KeyValuePair<string, SaveData>(slotId, data));
                    }
                }
                catch { } // Bozuk save dosyalarını atla
            }
            return list;
        }
        
        public void SetCurrentData(SaveData data, string slotId)
        {
            CurrentData = data;
            CurrentSlotId = slotId;
            
            // Veri dışarıdan set edildiğinde (Yeni Oyun) diğer manager'ların 
            // referanslarını alabilmesi için LoadCompleted simüle et.
            if (data != null)
            {
                _eventBus.Publish(new LoadCompleted { SlotId = slotId, Data = data });
            }
        }

        public override void Dispose()
        {
            // AutoSave when disposing manager
            SaveOnExit();
        }
    }
}
