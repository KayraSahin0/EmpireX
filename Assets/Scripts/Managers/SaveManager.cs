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
        
        private bool _autoSaveEnabled = true;

        public SaveManager(IEventBus eventBus) : base(eventBus) { }

        public override void Initialize()
        {
            ISerializer serializer = new JsonSerializer();
            IStorage storage = new FileStorage();
            IEncryptor encryptor = new SimpleEncryptor();
            
            _saveService = new SaveService(serializer, storage, encryptor);
        }

        public void ManualSave(string slotId)
        {
            SaveGame(slotId);
        }

        public void AutoSave()
        {
            if (!_autoSaveEnabled) return;
            
            _eventBus.Publish(new AutoSaveStarted());
            SaveGame("AutoSaveSlot");
            _eventBus.Publish(new AutoSaveCompleted());
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
                    _eventBus.Publish(new LoadCompleted { SlotId = slotId, Data = data });
                }
                else
                {
                    // Yeni Oyun Durumu (Save bulunamadıysa)
                    CurrentData = new SaveData();
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
        
        public void SetCurrentData(SaveData data)
        {
            CurrentData = data;
        }

        public override void Dispose()
        {
            // AutoSave when disposing manager
            AutoSave();
        }
    }
}
