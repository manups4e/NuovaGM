using System.Collections.Generic;

namespace Settings.Client.Configurazione.Vehicles
{
    public class ConfigVehicles
    {
        public float FuelCapacity { get; set; }
        public float FuelRpmImpact { get; set; }
        public float FuelAccelImpact { get; set; }
        public float FuelTractionImpact { get; set; }
        public float RefuelCost { get; set; }
        public int Maxtankerfuel { get; set; }
        public List<string> Trucks { get; set; }
        public string Tanker { get; set; }
        public int DeformationMultiplier { get; set; }
        public float DeformationExponent { get; set; }
        public float CollisionDamageExponent { get; set; }
        public float DamageFactorEngine { get; set; }
        public float DamageFactorBody { get; set; }
        public float DamageFactorPetrolTank { get; set; }
        public float EngineDamageExponent { get; set; }
        public float WeaponsDamageMultiplier { get; set; }
        public int DegradingHealthSpeedFactor { get; set; }
        public float CascadingFailureSpeedFactor { get; set; }
        public float DegradingFailureThreshold { get; set; }
        public float CascadingFailureThreshold { get; set; }
        public float EngineSafeGuard { get; set; }
        public bool TorqueMultiplierEnabled { get; set; }
        public bool LimpMode { get; set; }
        public float LimpModeMultiplier { get; set; }
        public bool PreventVehicleFlip { get; set; }
        public bool SundayDriver { get; set; }
        public bool DisplayMessage { get; set; }
        public bool CompatibilityMode { get; set; }
        public int RandomTireBurstInterval { get; set; }

        public List<float> ClassDamageMultiplier { get; set; }
    }
}