﻿using System.Threading.Tasks;

namespace TheLastPlanet.Client
{
    internal static class DecorationClass
    {
        public static async void DichiaraDecor()
        {
            /* DECOR GENERICI */
            EntityDecoration.RegisterProperty("NuovaGM2019fighissimo!yeah!", DecorationType.Int);
            EntityDecoration.RegisterProperty("Testdecor", DecorationType.Int);
            /* DECOR VEICOLI */
            EntityDecoration.RegisterProperty("lprp_fuel", DecorationType.Float);
            EntityDecoration.RegisterProperty("VehiclePoliceizia", DecorationType.Int);
            EntityDecoration.RegisterProperty("VeicoloMedici", DecorationType.Int);
            EntityDecoration.RegisterProperty("VeicoloRimozione", DecorationType.Int);
            EntityDecoration.RegisterProperty("VeicoloPersonale", DecorationType.Int);
            /* DECOR PICKUP */
            EntityDecoration.RegisterProperty("PickupOggetto", DecorationType.Int);
            EntityDecoration.RegisterProperty("PickupAccount", DecorationType.Int);
            EntityDecoration.RegisterProperty("PickupArma", DecorationType.Int);
            /* DECOR LOCK */
            EntityDecoration.LockProperties();

            await Cache.PlayerCache.Loaded();
            await Task.FromResult(0);
        }

        public static void Stop()
        {
        }
    }
}