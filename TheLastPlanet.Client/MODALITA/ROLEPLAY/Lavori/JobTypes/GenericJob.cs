using TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori.Whitelistati.Lavoro;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori
{
    public delegate void OnJobSet();
    public delegate void OnJobFired();
    public abstract class GenericJob
    {
        public Employment Lavoro { get; set; }
        public string Label { get; set; }
        //public int Stipendio { get; set; }
        public int Grade { get; set; }
        public event OnJobSet OnJobSet;
        public event OnJobFired OnJobFired;

        public GenericJob(string job, Employment lavoro)
        {
            Label = job;
            Lavoro = lavoro;
        }
    }
}
