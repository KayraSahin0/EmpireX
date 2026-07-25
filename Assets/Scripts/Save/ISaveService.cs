using EmpireX.Data;

namespace EmpireX.Save
{
    public interface ISaveService
    {
        void Save(string slotId, SaveData data);
        SaveData Load(string slotId);
        bool HasSave(string slotId);
        void Delete(string slotId);
    }
}
