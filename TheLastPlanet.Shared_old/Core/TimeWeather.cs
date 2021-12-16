using System;
using System.Collections.Generic;
using System.Text;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace Impostazioni.Shared
{
    [Serialization]
    public partial class Time
    {
        public DateTime Date { get; set; }
        public int SecondOfDay { get; set; }
        public bool Frozen { get; set; }
    }

    public class Weather
    {

    }
}
