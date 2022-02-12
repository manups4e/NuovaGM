namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori
{
    public abstract class JobProfile
    {
        public Client LPRP => Client.Instance;
        public LavoroBase Job { get; set; }
        public abstract JobProfile[] Dependencies { get; set; }
        public abstract void Begin(LavoroBase job);
    }
}
