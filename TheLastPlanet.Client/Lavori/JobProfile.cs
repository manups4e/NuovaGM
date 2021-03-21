﻿using System;
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
    public abstract class JobProfile
    {
        public ClientSession LPRP => ClientSession.Instance;
        public LavoroBase Job { get; set; }
        public abstract JobProfile[] Dependencies { get; set; }
        public abstract void Begin(LavoroBase job);
    }
}