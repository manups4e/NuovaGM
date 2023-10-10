using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TheLastPlanet.Server.FreeRoam.Scripts.FreeroamEvents
{
    internal class FreeRoamEvents
    {
        public static void Init()
        {
            EventDispatcher.Mount("tlg:freeroam:finishCharServer", new Action<PlayerClient, FreeRoamChar>(FinishChar));
            EventDispatcher.Mount("tlg:freeroam:salvapersonaggio", new Action<PlayerClient>(SalvaPersonaggio));
            EventDispatcher.Mount("tlg:casino:getVehModel", new Func<PlayerClient, Task<string>>(ReturnCasinoPriceModelForPlayer));
        }

        public static void SalvaPersonaggio([FromSource] PlayerClient client)
        {
            client.User.FreeRoamChar.Position = client.Ped.Position.ToPosition();
            API.SetResourceKvpNoSync($"freeroam:player_{client.User.Identifiers.Discord}:char_model", BitConverter.ToString(client.User.FreeRoamChar.ToBytes()));
        }

        public static void FinishChar([FromSource] PlayerClient client, FreeRoamChar data)
        {
            try
            {
                FreeRoamChar Char = data;
                client.User.FreeRoamChar = Char;
                API.SetResourceKvpNoSync($"freeroam:player_{client.User.Identifiers.Discord}:char_model", BitConverter.ToString(Char.ToBytes()));
                //API.DeleteResourceKvpNoSync($"freeroam:player_{client.User.Identifiers.Discord}:char_model");
            }
            catch (Exception e)
            {
                Server.Logger.Error($"{e.Message}");
            }
        }
        public static async Task<string> ReturnCasinoPriceModelForPlayer([FromSource] PlayerClient client)
        {
            // TODO: FOR NOW WE USE A CAR THEN WE'LL SEE
            return "zentorno";
        }

        public static async void SpawnEventVehicles(Dictionary<Vector4, uint> vehicles)
        {

        }

    }
}
