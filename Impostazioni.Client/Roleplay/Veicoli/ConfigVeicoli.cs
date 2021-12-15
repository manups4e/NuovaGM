using System.Collections.Generic;

namespace Impostazioni.Client.Configurazione.Veicoli
{
    public class ConfVeicoli
    {
        public float FuelCapacity { get; set; }
        public float FuelRpmImpact { get; set; }
        public float FuelAccelImpact { get; set; }
        public float FuelTractionImpact { get; set; }
        public float refuelCost { get; set; }
        public int maxtankerfuel { get; set; }
        public List<string> trucks { get; set; }
        public string tanker { get; set; }
        public int deformationMultiplier { get; set; }
        public float deformationExponent { get; set; }
        public float collisionDamageExponent { get; set; }
        public float damageFactorEngine { get; set; }
        public float damageFactorBody { get; set; }
        public float damageFactorPetrolTank { get; set; }
        public float engineDamageExponent { get; set; }
        public float weaponsDamageMultiplier { get; set; }
        public int degradingHealthSpeedFactor { get; set; }
        public float cascadingFailureSpeedFactor { get; set; }
        public float degradingFailureThreshold { get; set; }
        public float cascadingFailureThreshold { get; set; }
        public float engineSafeGuard { get; set; }
        public bool torqueMultiplierEnabled { get; set; }
        public bool limpMode { get; set; }
        public float limpModeMultiplier { get; set; }
        public bool preventVehicleFlip { get; set; }
        public bool sundayDriver { get; set; }
        public bool displayMessage { get; set; }
        public bool compatibilityMode { get; set; }
        public int randomTireBurstInterval { get; set; }

        public List<float> classDamageMultiplier { get; set; }
    }
}