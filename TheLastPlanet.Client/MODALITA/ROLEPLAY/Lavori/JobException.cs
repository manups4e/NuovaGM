using System;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori
{
    public class JobException : Exception
    {
        public JobException(LavoroBase job, string message) : base($"[Job] [{job.Lavoro}] {message}")
        {
        }
    }
}
