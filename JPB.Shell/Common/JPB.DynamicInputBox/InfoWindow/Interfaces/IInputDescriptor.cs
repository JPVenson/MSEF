#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 14:56

#endregion

namespace JPB.DynamicInputBox.InfoWindow.Interfaces
{
    public interface IInputDescriptor
    {
        int Index { get; set; }
        string Text { get; set; }
        string Descriptor { get; set; }
    }
}