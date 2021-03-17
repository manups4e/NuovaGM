namespace TheLastPlanet.Shared.SistemaEventi
{
    // TODO: Tokens to increase security.
    public class EventMetadata : Metadata
    {
	    public string Inherit { get; set; }
	    public int Sender { get; set; }

	    public EventMetadata()
	    {
	    }

	    public EventMetadata(Event parent)
	    {
		    Inherit = parent.Seed;
		    Sender = parent.Sender;
	    }
    }
}