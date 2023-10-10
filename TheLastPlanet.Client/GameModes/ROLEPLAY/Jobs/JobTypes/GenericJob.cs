using TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs.Whitelisted.Lavoro;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs
{
    public delegate void OnJobSet();
    public delegate void OnJobFired();
    public abstract class GenericJob
    {
        public Employment Job { get; set; }
        public string Label { get; set; }
        //public int Stipendio { get; set; }
        public int Grade { get; set; }
        public event OnJobSet OnJobSet;
        public event OnJobFired OnJobFired;

        public GenericJob(string job, Employment lavoro)
        {
            Label = job;
            Job = lavoro;
        }
    }
}
