using System;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Jobs
{
    public class JobException : Exception
    {
        public JobException(GenericJob job, string message) : base($"[Job] [{job.Label}] {message}")
        {
        }
    }
}
