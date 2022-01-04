using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.IPLs.dlc_cayo_perico
{
    public class Island
    {
        private bool enabled;

        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                SetIslandHopperEnabled("HeistIsland", value);
                SetToggleMinimapHeistIsland(value);
                SetAiGlobalPathNodesType(value ? 1 : 0);
                LoadGlobalWaterType(value ? 1 : 0);
                SetMaxWantedLevel(0);
                SetScenarioGroupEnabled("Heist_Island_Peds", true);
                SetAudioFlag("PlayerOnDLCHeist4Island", value);
                SetAmbientZoneListStatePersistent("AZL_DLC_Hei4_Island_Zones", true, value);
                SetAmbientZoneListStatePersistent("AZL_DLC_Hei4_Island_Disabled_Zones", false, value);
            }
        }
    }
}
