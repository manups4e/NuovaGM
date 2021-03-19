using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Lavori
{
    public class JobException : Exception
    {
        public JobException(LavoroBase job, string message) : base($"[Job] [{job.Lavoro}] {message}")
        {
        }
    }
}
