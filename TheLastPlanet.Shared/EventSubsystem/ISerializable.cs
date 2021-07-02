using TheLastPlanet.Shared.Snowflakes;

namespace TheLastPlanet.Shared.Internal.Events
{

    public interface IMessage
    {
        Snowflake Id { get; set; }
        string Endpoint { get; set; }
        string Signature { get; set; }
    }
}