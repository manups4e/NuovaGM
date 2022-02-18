using System;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori
{
    public class JobException : Exception
    {
        public JobException(GenericJob job, string message) : base($"[Lavoro] [{job.Label}] {message}")
        {
        }
    }
}
