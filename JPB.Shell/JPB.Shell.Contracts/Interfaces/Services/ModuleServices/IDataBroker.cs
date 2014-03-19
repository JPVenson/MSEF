#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 13:16

#endregion

namespace JPB.Shell.Contracts.Interfaces.Services.ModuleServices
{
    public interface IDataBroker : IService
    {
        object this[string index] { get; set; }
        string Filename { get; set; }

        void ChangeFileWithCopy(string filename);
        void LoadFromFile(string file);
        void SaveToFile();
        void SetData<T>(string key, T value) where T : class;
        void OverrideData<T>(string key, T value) where T : class;
        T GetData<T>(string key) where T : class;
        bool RemoveData<T>(string key) where T : class;
    }
}