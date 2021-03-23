

namespace TheLastPlanet.Shared.Internal.Events
{
    
    public interface ISerializable
    {
        string Signature { get; set; }
        string Serialize();
    }
}