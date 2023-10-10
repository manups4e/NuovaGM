using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Core.status.Interfaces;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Core.Status
{
    public enum Skills
    {
        STAMINA,
        STRENGTH,
        FLYING_ABILITY,
        LUNG_CAPACITY,
        WHEELIE_ABILITY,
        DRUGS,
        FISHING,
        HUNTING
    }

    internal static class StatsNeeds
    {
        public static int StatusMax = 100;
        private static bool _hunger20 = false;
        private static bool _hunger60 = false;
        private static bool _hunger80 = false;
        private static bool _hunger100 = false;
        private static bool _thirst20 = false;
        private static bool _thirst60 = false;
        private static bool _thirst80 = false;
        private static bool _thirst100 = false;
        private static bool _tireness20 = false;
        private static bool _tireness40 = false;
        private static bool _tireness60 = false;
        //private static bool stanchezza80 = false;
        //private static bool stanchezza100 = false;
        private static int _faintTimer = 0;
        private static int _updTimer = 0;
        public static Dictionary<string, status.Interfaces.Needs> Needs = new Dictionary<string, status.Interfaces.Needs>();
        public static Dictionary<string, Stats> Statistics = new Dictionary<string, Stats>();

        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
            Client.Instance.AddEventHandler("lprp:skills:registraSkill", new Action<string, float>(RegisterStats));
            Needs.Add("Hunger", new status.Interfaces.Needs("Hunger", 0, 0.005f, new Action<Ped, Player, status.Interfaces.Needs>(Hunger)));
            Needs.Add("Thirst", new status.Interfaces.Needs("Thirst", 0, 0.006f, new Action<Ped, Player, status.Interfaces.Needs>(Thirst)));
            Needs.Add("Tireness", new status.Interfaces.Needs("Tireness", 0, 0.007f, new Action<Ped, Player, status.Interfaces.Needs>(Tireness)));
            Statistics.Add("STAMINA", new Stats("Stamina", "MP0_STAMINA", "PSF_STAMINA", new Action<Ped, Player, Stats>(Stamina)));
            Statistics.Add("STRENGTH", new Stats("Strenght", "MP0_STRENGTH", "PSF_STRENGTH", new Action<Ped, Player, Stats>(Strenght)));
            Statistics.Add("FLYING_ABILITY", new Stats("Flying_ability", "MP0_FLYING_ABILITY", "PSF_FLYING", new Action<Ped, Player, Stats>(Flying)));
            Statistics.Add("LUNG_CAPACITY", new Stats("Lung_capacity", "MP0_LUNG_CAPACITY", "PSF_LUNG", new Action<Ped, Player, Stats>(Lung)));
            Statistics.Add("WHEELIE_ABILITY", new Stats("Wheelie_ability", "MP0_WHEELIE_ABILITY", "PSF_DRIVING", new Action<Ped, Player, Stats>(Driving)));
            Statistics.Add("SHOOTING_ABILITY", new Stats("Mira", "SHOOTING_ABILITY", "PSF_SHOOTING", new Action<Ped, Player, Stats>(Shooting)));
            Statistics.Add("FISHING", new Stats("Fisherman", "Fisherman", "Fishing +", new Action<Ped, Player, Stats>(Fisherman)));
            Statistics.Add("HUNTING", new Stats("Hunter", "Hunter", "Hunting +", new Action<Ped, Player, Stats>(Hunter)));
            Statistics.Add("DRUGS", new Stats("Drugs", "Drugs", "Drug +", new Action<Ped, Player, Stats>(Drug)));
            //PSF_SHOOTING To add shooting skills?
        }
        public static void onPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveEventHandler("lprp:skills:registraSkill", new Action<string, float>(RegisterStats));
            Needs = null;
            Statistics = null;
            //PSF_SHOOTING To add shooting skills?
        }

        public static void Spawned(PlayerClient client)
        {
            User me = client.User;
            Needs["Hunger"].Val = me.CurrentChar.Needs.Hunger;
            Needs["Thirst"].Val = me.CurrentChar.Needs.Thirst;
            Needs["Tireness"].Val = me.CurrentChar.Needs.Tiredness;
            //nee.malattia = m.CurrentChar.needs.malattia;
            Statistics["STAMINA"].Val = me.CurrentChar.Statistics.STAMINA;
            Statistics["STRENGTH"].Val = me.CurrentChar.Statistics.STRENGTH;
            Statistics["FLYING_ABILITY"].Val = me.CurrentChar.Statistics.FLYING_ABILITY;
            Statistics["LUNG_CAPACITY"].Val = me.CurrentChar.Statistics.LUNG_CAPACITY;
            Statistics["WHEELIE_ABILITY"].Val = me.CurrentChar.Statistics.WHEELIE_ABILITY;
            Statistics["DRUGS"].Val = me.CurrentChar.Statistics.DRUGS;
            Statistics["FISHING"].Val = me.CurrentChar.Statistics.FISHING;
            Statistics["HUNTING"].Val = me.CurrentChar.Statistics.HUNTING;
            StatSetInt(Functions.HashUint("MP0_STAMINA"), (int)Statistics["STAMINA"].Val, true);
            StatSetInt(Functions.HashUint("MP0_STRENGTH"), (int)Statistics["STRENGTH"].Val, true);
            StatSetInt(Functions.HashUint("MP0_FLYING_ABILITY"), (int)Statistics["FLYING_ABILITY"].Val, true);
            StatSetInt(Functions.HashUint("MP0_LUNG_CAPACITY"), (int)Statistics["LUNG_CAPACITY"].Val, true);
            StatSetInt(Functions.HashUint("MP0_WHEELIE_ABILITY"), (int)Statistics["WHEELIE_ABILITY"].Val, true);
        }

        public static void RegisterStats(Skills cap, float val)
        {
            if (Statistics.ContainsKey(cap.ToString())) Statistics[cap.ToString()].Val += val;
        }

        public static void RegisterStats(string cap, float val)
        {
            if (Statistics.ContainsKey(cap)) Statistics[cap].Val += val;
        }

        private static async Task HungerThirst()
        {
            await BaseScript.Delay(1000);
            Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;

            switch (playerPed.Health > 0)
            {
                case true when _hunger100 || _thirst100:
                    {
                        if (playerPed.Health <= 50)
                            playerPed.Health -= 5;
                        else
                            playerPed.Health -= 1;

                        break;
                    }
                default:
                    Client.Instance.RemoveTick(HungerThirst);

                    break;
            }
        }

        private static async void Clacson()
        {
            Ped p = PlayerCache.MyPlayer.Ped;
            if (PlayerCache.MyPlayer.Status.PlayerStates.InVehicle) Client.Instance.AddTick(Horn);
            await BaseScript.Delay(30000);
            p.CancelRagdoll();
            if (PlayerCache.MyPlayer.Status.PlayerStates.InVehicle) Client.Instance.RemoveTick(Horn);
        }

        public static async Task Horn()
        {
            SoundVehicleHornThisFrame(GetVehiclePedIsUsing(PlayerPedId()));
            await Task.FromResult(0);
        }

        public static async Task Agg()
        {
            Shared.Needs nee = new Shared.Needs() { Hunger = Needs["Hunger"].Val, Thirst = Needs["Thirst"].Val, Tiredness = Needs["Tireness"].Val, Sickness = Cache.PlayerCache.MyPlayer.User.CurrentChar.Needs.Sickness };
            Statistiche skill = new Statistiche()
            {
                STAMINA = Statistics["STAMINA"].Val,
                STRENGTH = Statistics["STRENGTH"].Val,
                LUNG_CAPACITY = Statistics["LUNG_CAPACITY"].Val,
                SHOOTING_ABILITY = Statistics["SHOOTING_ABILITY"].Val,
                WHEELIE_ABILITY = Statistics["WHEELIE_ABILITY"].Val,
                FLYING_ABILITY = Statistics["FLYING_ABILITY"].Val,
                DRUGS = Statistics["DRUGS"].Val,
                FISHING = Statistics["FISHING"].Val,
                HUNTING = Statistics["HUNTING"].Val
            };
            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "needs", nee.ToJson());
            //BaseScript.TriggerServerEvent("lprp:updateCurChar", "skill", skill.ToJson());
            await Task.FromResult(0);
        }

        public static async Task StatsSkillHandler()
        {
            await BaseScript.Delay(1000);
            Ped p = Cache.PlayerCache.MyPlayer.Ped;
            Player m = Cache.PlayerCache.MyPlayer.Player;
            Needs.Values.ToList().ForEach(x => x.OnTick(p, m));
            Statistics.Values.ToList().ForEach(x => x.OnTick(p, m));

            /*
			if (Game.GameTime - _updTimer > 30000) //60000)
			{
				await Agg();
				_updTimer = Game.GameTime;
			}
			*/

            await Task.FromResult(0);
            //Client.Logger.Debug( $"{n.Name} = {n.Val} [{n.GetPercent()}, {n.ChangeVal}]");
        }

        #region Needs

        private static void Hunger(Ped playerPed, Player me, status.Interfaces.Needs fame)
        {
            if (!playerPed.IsAiming && (playerPed.IsRunning || playerPed.IsSwimming || playerPed.IsJumping))
                fame.ChangeVal = 0.025f;
            else if (!playerPed.IsAiming && (playerPed.IsSwimmingUnderWater || playerPed.IsSprinting))
                fame.ChangeVal = 0.040f;
            else if (playerPed.IsInMeleeCombat)
                fame.ChangeVal = 0.015f;
            else
                fame.ChangeVal = 0.005f;
            fame.Val += fame.ChangeVal;
            if (fame.Val >= 100)
                fame.Val = 100;

            if (fame.Val < 20.0f && (_hunger20 || _hunger60 || _hunger80 || _hunger100))
            {
                StatSetInt(Functions.HashUint("MP0_STAMINA"), (int)Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.STAMINA, true);
                _hunger20 = false;
                _hunger60 = false;
                _hunger80 = false;
                _hunger100 = false;
            }

            if (fame.Val >= 20f && !_hunger20)
            {
                HUD.ShowNotification("You're feeling a little peckish... You'd love to nibble on something.", ColoreNotifica.GreenDark, true);
                _hunger20 = true;
            }

            if (fame.Val >= 60 && !_hunger60)
            {
                HUD.ShowNotification("You're hungry! maybe you should eat something!", ColoreNotifica.Yellow, true);
                _hunger60 = true;
                int stam = 0;
                StatGetInt(Functions.HashUint("MP0_STAMINA"), ref stam, -1);
                StatSetInt(Functions.HashUint("MP0_STAMINA"), (int)(stam - stam / 10f), true);
            }

            if (fame.Val >= 80 && !_hunger80)
            {
                HUD.ShowNotification("You're starving! If you continue like this you risk dying!.", ColoreNotifica.Red, true);
                _hunger80 = true;
                playerPed.Health -= 5;
                playerPed.MovementAnimationSet = "move_injured_generic";
            }

            if (fame.Val == 100 && !_hunger100)
            {
                HUD.ShowNotification("You're starving!", ColoreNotifica.Red, true);
                _hunger100 = true;
                Client.Instance.AddTick(HungerThirst);
            }
        }

        private static void Thirst(Ped playerPed, Player me, status.Interfaces.Needs sete)
        {
            if (!playerPed.IsAiming && (playerPed.IsRunning || playerPed.IsSwimming || playerPed.IsJumping))
                sete.ChangeVal = 0.035f;
            else if (!playerPed.IsAiming && (playerPed.IsSwimmingUnderWater || playerPed.IsSprinting))
                sete.ChangeVal = 0.055f;
            else if (playerPed.IsInMeleeCombat)
                sete.ChangeVal = 0.033f;
            else
                sete.ChangeVal = 0.006f;
            sete.Val += sete.ChangeVal;
            if (sete.Val >= 100)
                sete.Val = 100;

            if (sete.Val < 20.0f && (_hunger20 || _hunger60 || _hunger80 || _hunger100))
            {
                StatSetInt(Functions.HashUint("MP0_STAMINA"), (int)Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.STAMINA, true);
                _hunger20 = false;
                _hunger60 = false;
                _hunger80 = false;
                _hunger100 = false;
            }

            if (sete.Val >= 20f && !_thirst20)
            {
                HUD.ShowNotification("Your throat is a little dry... you could use a drink.", ColoreNotifica.GreenDark, true);
                _thirst20 = true;
            }

            if (sete.Val >= 60 && !_thirst60)
            {
                HUD.ShowNotification("You're thirsty! maybe you should have a drink!", ColoreNotifica.Yellow, true);
                _thirst60 = true;
                int stam = 0;
                StatGetInt(Functions.HashUint("MP0_STAMINA"), ref stam, -1);
                StatSetInt(Functions.HashUint("MP0_STAMINA"), (int)(stam - stam / 10f), true);
            }

            if (sete.Val >= 80 && !_thirst80)
            {
                HUD.ShowNotification("You're dying of thirst! If you continue like this you risk dying!.", ColoreNotifica.Red, true);
                playerPed.Health -= 5;
                playerPed.MovementAnimationSet = "move_injured_generic";
                _thirst80 = true;
            }

            if (sete.Val == 100 && !_thirst100)
            {
                HUD.ShowNotification("You're dying of thirst!", ColoreNotifica.Red, true);
                _thirst100 = true;
                Client.Instance.AddTick(HungerThirst);
            }
        }

        private static void Tireness(Ped playerPed, Player me, status.Interfaces.Needs stanchezza)
        {
            if (!playerPed.IsAiming && (playerPed.IsRunning || playerPed.IsSwimming || playerPed.IsJumping))
                stanchezza.ChangeVal = 0.0225f;
            else if (!playerPed.IsAiming && (playerPed.IsSwimmingUnderWater || playerPed.IsSprinting))
                stanchezza.ChangeVal = 0.035f;
            else if (playerPed.IsInMeleeCombat)
                stanchezza.ChangeVal = 0.0285f;
            else
                stanchezza.ChangeVal = 0.0055f;

            stanchezza.Val += stanchezza.ChangeVal;
            if (stanchezza.Val >= 100)
                stanchezza.Val = 100;
            if (World.CurrentDayTime.Hours >= 18 || World.CurrentDayTime.Hours <= 6) stanchezza.Val += 0.03f;

            if (stanchezza.Val < 20.0f && (_tireness20 || _tireness40 || _tireness60))
            {
                StatSetInt(Functions.HashUint("MP0_STAMINA"), (int)Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.STAMINA, true);
                StatSetInt(Functions.HashUint("MP0_SHOOTING_ABILITY"), (int)Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.SHOOTING_ABILITY, true);
                _tireness20 = false;
                _tireness40 = false;
                _tireness60 = false;
                //stanchezza80 = false;
                //stanchezza100 = false;
            }

            if (stanchezza.Val >= 20f && !_tireness20)
            {
                int stam = 0;
                int shot = 0;
                StatGetInt(Functions.HashUint("MP0_STAMINA"), ref stam, -1);
                StatGetInt(Functions.HashUint("MP0_SHOOTING_ABILITY"), ref shot, -1);
                if (stam > 10) StatSetInt(Functions.HashUint("MP0_STAMINA"), (int)(stam - stam / 20f), true);
                if (shot > 10) StatSetInt(Functions.HashUint("MP0_SHOOTING_ABILITY"), (int)(shot - shot / 20f), true);
                _tireness20 = true;
            }

            if (stanchezza.Val >= 40.0f && !_tireness40)
            {
                StatSetInt(Functions.HashUint("MP0_STAMINA"), 1, true);
                StatSetInt(Functions.HashUint("MP0_SHOOTING_ABILITY"), 1, true);
                _tireness40 = true;
            }

            if (stanchezza.Val >= 60.0f && !_tireness60)
            {
                SetPlayerSprint(PlayerId(), false);
                _tireness60 = true;
            }

            if (stanchezza.Val >= 80.0f)
                if (Game.GameTime - _faintTimer > 600000)
                {
                    if (SharedMath.GetRandomInt(100) > 85)
                    {
                        if (playerPed.IsWalking)
                        {
                            playerPed.Ragdoll(30000, RagdollType.Normal);
                            HUD.ShowNotification("You fainted because you were too tired.. Find a place to rest!!");
                            Clacson();
                        }
                        else if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle || playerPed.IsInFlyingVehicle)
                        {
                            SetBlockingOfNonTemporaryEvents(PlayerPedId(), true);
                            HUD.ShowNotification("You fainted because you were too tired.. Find a place to rest!!");
                            playerPed.Task.PlayAnimation("rcmnigel2", "die_horn", 4f, -1, AnimationFlags.StayInEndFrame);
                            Clacson();
                        }
                    }

                    _faintTimer = Game.GameTime;
                }
        }

        #endregion

        #region skills

        public static void Stamina(Ped playerPed, Player me, Stats stam)
        {
            int baseStat = (int)Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.STAMINA;
            stam.ChangeVal = !Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle ? playerPed.IsSprinting || playerPed.IsSwimmingUnderWater ? 0.002f : playerPed.IsRunning || playerPed.IsSwimming ? 0.001f : 0f : playerPed.CurrentVehicle.Model.IsBicycle ? 0.003f : 0f;
            stam.Val += stam.ChangeVal;

            if (stam.Val - baseStat >= 1f)
            {
                Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.STAMINA = stam.Val;
                StatSetInt(Functions.HashUint("MP0_STAMINA"), (int)stam.Val, true);
                stam.ShowStatNotification();
            }
        }

        public static void Strenght(Ped playerPed, Player me, Stats strenght)
        {
            int baseStat = (int)Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.STRENGTH;
            strenght.ChangeVal = playerPed.IsInMeleeCombat ? 0.002f : 0f;
            strenght.Val += strenght.ChangeVal;

            if (strenght.Val - baseStat >= 1f)
            {
                Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.STRENGTH = strenght.Val;
                StatSetInt(Functions.HashUint("MP0_STRENGTH"), (int)strenght.Val, true);
                strenght.ShowStatNotification();
            }
        }

        public static void Flying(Ped playerPed, Player me, Stats flying)
        {
            int baseStat = (int)Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.FLYING_ABILITY;

            // solo se in aereo
            if (playerPed.IsInPlane || playerPed.IsInHeli)
            {
                flying.ChangeVal = playerPed.SeatIndex == VehicleSeat.Driver && playerPed.CurrentVehicle.HeightAboveGround >= 15 ? 0.002f : 0f;
                flying.Val += flying.ChangeVal;
            }

            if (flying.Val - baseStat >= 1f)
            {
                Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.FLYING_ABILITY = flying.Val;
                StatSetInt(Functions.HashUint("MP0_FLYING_ABILITY"), (int)flying.Val, true);
                flying.ShowStatNotification();
            }
        }

        public static void Lung(Ped playerPed, Player me, Stats lung)
        {
            int baseStat = (int)Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.LUNG_CAPACITY;

            // solo se è in acqua
            if (playerPed.IsInWater)
            {
                lung.ChangeVal = playerPed.IsSwimmingUnderWater ? 0.002f : 0f;
                lung.Val += lung.ChangeVal;
            }

            if (lung.Val - baseStat >= 1f)
            {
                Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.LUNG_CAPACITY = lung.Val;
                StatSetInt(Functions.HashUint("MP0_LUNG_CAPACITY"), (int)lung.Val, true);
                lung.ShowStatNotification();
            }
        }

        public static void Driving(Ped playerPed, Player me, Stats driving)
        {
            int baseStat = (int)Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.WHEELIE_ABILITY;

            if (Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle && playerPed.SeatIndex == VehicleSeat.Driver)
            {
                if (playerPed.CurrentVehicle.Model.IsVehicle || playerPed.CurrentVehicle.Model.IsBike || playerPed.CurrentVehicle.Model.IsBoat || playerPed.CurrentVehicle.Model.IsQuadbike)
                {
                    float maxVal = 0;
                    float midVal = 0;
                    float minVal = 0;

                    switch (playerPed.CurrentVehicle.ClassType)
                    {
                        case VehicleClass.Commercial:
                        case VehicleClass.Emergency:
                        case VehicleClass.Industrial:
                        case VehicleClass.Military:
                        case VehicleClass.Service:
                            maxVal = 0.004f;
                            midVal = 0.003f;
                            minVal = 0.002f;

                            break;
                        case VehicleClass.Compacts:
                        case VehicleClass.SUVs:
                        case VehicleClass.OffRoad:
                        case VehicleClass.Vans:
                        case VehicleClass.Utility:
                        case VehicleClass.Boats:
                            maxVal = 0.002f;
                            midVal = 0.0015f;
                            minVal = 0.0010f;

                            break;
                        case VehicleClass.Coupes:
                        case VehicleClass.Motorcycles:
                        case VehicleClass.Muscle:
                        case VehicleClass.Sedans:
                            maxVal = 0.0035f;
                            midVal = 0.0020f;
                            minVal = 0.0015f;

                            break;
                        case VehicleClass.SportsClassics:
                        case VehicleClass.Sports:
                            maxVal = 0.004f;
                            midVal = 0.0035f;
                            minVal = 0.003f;

                            break;
                        case VehicleClass.Super:
                            maxVal = 0.005f;
                            midVal = 0.0045f;
                            minVal = 0.003f;

                            break;
                    }

                    float speed = playerPed.CurrentVehicle.Speed * 3.6f;
                    // altavelocita? --> velocità media? --> velocità minima?
                    driving.ChangeVal = speed > 150 ? maxVal : speed > 90f && speed < 150f ? midVal : speed < 90f && speed > 30f ? minVal : 0f;
                }

                // la skill viene gestita solo se siamo in macchina
                driving.Val += driving.ChangeVal;
            }

            if (driving.Val - baseStat >= 1f)
            {
                Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.WHEELIE_ABILITY = driving.Val;
                StatSetInt(Functions.HashUint("MP0_WHEELIE_ABILITY"), (int)driving.Val, true);
                driving.ShowStatNotification();
            }
        }

        public static void Shooting(Ped playerPed, Player me, Stats shoot)
        {
            int baseStat = (int)Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.SHOOTING_ABILITY;

            // GESTIRE SPARATORIE

            if (shoot.Val - baseStat >= 1f)
            {
                Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.LUNG_CAPACITY = shoot.Val;
                StatSetInt(Functions.HashUint("MP0_LUNG_CAPACITY"), (int)shoot.Val, true);
                shoot.ShowStatNotification();
            }
        }

        private static void Fisherman(Ped playerPed, Player me, Stats pesca)
        {
            int baseStat = (int)Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.FISHING;
            /*
            if (Lavori.Generici.Pescatore.PescatoreClient.Pescando)
            {
                pesca.ChangeVal = 0.003f;
                pesca.Val += pesca.ChangeVal;
            }
            else
            {
                pesca.ChangeVal = 0;
            }*/

            if (pesca.Val - baseStat >= 1f)
            {
                Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.FISHING = pesca.Val;
                pesca.ShowStatNotification();
            }
        }

        private static void Hunter(Ped playerPed, Player me, Stats caccia)
        {
            int baseStat = (int)Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.HUNTING;

            if (caccia.Val - baseStat >= 1f)
            {
                Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.HUNTING = caccia.Val;
                caccia.ShowStatNotification();
            }
        }

        private static void Drug(Ped playerPed, Player me, Stats droga)
        {
            int baseStat = (int)Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.DRUGS;

            if (droga.Val - baseStat >= 1f)
            {
                Cache.PlayerCache.MyPlayer.User.CurrentChar.Statistics.DRUGS = droga.Val;
                droga.ShowStatNotification();
            }
        }

        #endregion
    }
}