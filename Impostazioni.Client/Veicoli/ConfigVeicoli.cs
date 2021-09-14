using System.Collections.Generic;

namespace Impostazioni.Client.Configurazione.Veicoli
{
    public class ConfVeicoli
    {
        public float FuelCapacity = 65f;
        public float FuelRpmImpact = 0.0045f;
        public float FuelAccelImpact = 0.002f;
        public float FuelTractionImpact = 0.001f;
        public float refuelCost = 0.85f;
        public int maxtankerfuel = 2100;
        public List<string> trucks = new() {"HAULER", "PACKER", "PHANTOM"};
        public string tanker = "TANKER";
        public int deformationMultiplier = -1;
        public float deformationExponent = 0.4f;
        public float collisionDamageExponent = 0.6f;
        public float damageFactorEngine = 10f;
        public float damageFactorBody = 10f;
        public float damageFactorPetrolTank = 64f;
        public float engineDamageExponent = 0.6f;
        public float weaponsDamageMultiplier = 0.01f;
        public int degradingHealthSpeedFactor = 8;
        public float cascadingFailureSpeedFactor = 5f;
        public float degradingFailureThreshold = 600f;
        public float cascadingFailureThreshold = 500f;
        public float engineSafeGuard = 80f;
        public bool torqueMultiplierEnabled = true;
        public bool limpMode = false;
        public float limpModeMultiplier = 0.15f;
        public bool preventVehicleFlip = true;
        public bool sundayDriver = false;
        public bool displayMessage = true;
        public bool compatibilityMode = false;
        public int randomTireBurstInterval = 60;

        public List<float> classDamageMultiplier = new()
        {
            1.0f,
            1.0f,
            1.0f,
            1.0f,
            1.0f,
            1.0f,
            1.0f,
            1.0f,
            0.25f,
            0.7f,
            0.25f,
            1.0f,
            1.0f,
            1.0f,
            0.5f,
            1.0f,
            1.0f,
            1.0f,
            0.75f,
            0.75f,
            1.0f,
            1.0f,
        };
    }
}