﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.FREEROAM.Spawner;


namespace TheLastPlanet.Client.GameMode.FREEROAM.Managers
{

    public enum GamerTagType
    {
        GAMER_NAME = 0,
        CREW_TAG = 1,
        healthArmour = 2,
        BIG_TEXT = 3,
        AUDIO_ICON = 4,
        MP_USING_MENU = 5,
        MP_PASSIVE_MODE = 6,
        WANTED_STARS = 7,
        MP_DRIVER = 8,
        MP_CO_DRIVER = 9,
        MP_TAGGED = 10,
        GAMER_NAME_NEARBY = 11,
        ARROW = 12,
        MP_PACKAGES = 13,
        INV_IF_PED_FOLLOWING = 14,
        RANK_TEXT = 15,
        MP_TYPING = 16
    }

    static class PlayerTags
    {
        public static Dictionary<int, GamerTag> GamerTags = new Dictionary<int, GamerTag>();
        public static void Init()
        {
            AccessingEvents.OnFreeRoamSpawn += (client) => Client.Instance.AddTick(GamerTagsHandler);
            AccessingEvents.OnFreeRoamLeave += OnPlayerLeft;
        }

        public static void OnPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveTick(GamerTagsHandler);
            foreach (var player in Client.Instance.GetPlayers)
            {
                if (GamerTags.ContainsKey(player.ServerId))
                {
                    RemoveMpGamerTag(GamerTags[player.ServerId].Tag);
                    GamerTags.Remove(player.ServerId);
                }
            }
        }

        public static async Task GamerTagsHandler()
        {
            foreach (var player in Client.Instance.GetPlayers)
            {
                if (NetworkIsPlayerActive(player.Handle) && player.Handle != PlayerCache.MyPlayer.Player.Handle)
                {
                    Ped ped = player.Character;

                    // Se il player non ha una gamertag o se il player ha cambiato ped o la gamertag non viene mostrata per qualche motivo
                    if (!GamerTags.ContainsKey(player.ServerId) || GamerTags[player.ServerId].Ped.Handle != ped.Handle || !IsMpGamerTagActive(GamerTags[player.ServerId].Tag))
                    {
                        if (GamerTags.ContainsKey(player.ServerId))
                            RemoveMpGamerTag(GamerTags[player.ServerId].Tag);

                        GamerTags[player.ServerId] = new GamerTag()
                        {
                            Tag = CreateMpGamerTag(ped.Handle, player.Name, false, false, "", 0),
                            Ped = player.Character
                        };
                    }

                    var tag = GamerTags[player.ServerId].Tag;

                    //TODO: aggiungere gestione giocatori in base al multiplayer in se.. (uccisioni.. contratti..)

                    /* TENGO IN CONSIDERATION
					        -- should the player be renamed? this is set by events
						if mpGamerTagSettings[i].rename then
							SetMpGamerTagName(tag, formatPlayerNameTag(i, templateStr))
							mpGamerTagSettings[i].rename = nil
						end
					*/

                    if (PlayerCache.MyPlayer.Position.Distance(ped.Position) < 250f && HasEntityClearLosToEntity(PlayerCache.MyPlayer.Ped.Handle, ped.Handle, 17))
                    {
                        SetMpGamerTagVisibility(tag, (int)GamerTagType.GAMER_NAME, true);
                        SetMpGamerTagVisibility(tag, (int)GamerTagType.healthArmour, PlayerCache.MyPlayer.Player.IsTargetting(ped));
                        SetMpGamerTagVisibility(tag, (int)GamerTagType.AUDIO_ICON, NetworkIsPlayerTalking(player.Handle));
                        SetMpGamerTagVisibility(tag, (int)GamerTagType.WANTED_STARS, player.WantedLevel > 0);

                        SetMpGamerTagWantedLevel(tag, player.WantedLevel);
                        SetMpGamerTagAlpha(tag, (int)GamerTagType.AUDIO_ICON, 255);
                        SetMpGamerTagAlpha(tag, (int)GamerTagType.healthArmour, 255);


                        /* DA CONSIDERARE
						-- override settings 
						local settings = mpGamerTagSettings[i]

						for k, v in pairs(settings.toggles) do
							SetMpGamerTagVisibility(tag, gtComponent[k], v)
						end

						for k, v in pairs(settings.alphas) do
							SetMpGamerTagAlpha(tag, gtComponent[k], v)
						end

						for k, v in pairs(settings.colors) do
							SetMpGamerTagColour(tag, gtComponent[k], v)
						end

						if settings.wantedLevel then
							SetMpGamerTagWantedLevel(tag, settings.wantedLevel)
						end

						if settings.healthColor then
							SetMpGamerTagHealthBarColour(tag, settings.healthColor)
						end
						*/
                    }
                    else
                    {
                        SetMpGamerTagVisibility(tag, (int)GamerTagType.GAMER_NAME, false);
                        SetMpGamerTagVisibility(tag, (int)GamerTagType.healthArmour, false);
                        SetMpGamerTagVisibility(tag, (int)GamerTagType.AUDIO_ICON, false);
                        SetMpGamerTagVisibility(tag, (int)GamerTagType.WANTED_STARS, false);
                    }
                }
                else
                {
                    if (GamerTags.ContainsKey(player.ServerId))
                    {
                        RemoveMpGamerTag(GamerTags[player.ServerId].Tag);
                        GamerTags.Remove(player.ServerId);
                    }
                }
                var daRim = GamerTags.Where(x => Client.Instance.GetPlayers.ToList().All(y => y.ServerId != x.Key) || !NetworkIsPlayerActive(GetPlayerFromServerId(x.Key))).ToList();
                if (daRim.Count > 0)
                {
                    foreach (var aa in daRim)
                    {
                        RemoveMpGamerTag(aa.Value.Tag);
                        GamerTags.Remove(aa.Key);
                    }
                }
            }
        }
    }

    public class GamerTag
    {
        public int Tag;
        public Ped Ped;
    }
}
