using System;
using System.Collections.Generic;
using TheLastPlanet.Client.Core.Utility.HUD;

namespace TheLastPlanet.Client.CodaControl
{
    internal enum SessionState
    {
        Coda,
        Grazia,
        Caricamento,
        Attivo
    }

    internal enum Reserved
    {
        Public
    }

    internal static class CodaAdminPanel
    {
        private static string resourceName = GetCurrentResourceName();
        public static bool pannelloCodaAperto = false;

        public static void Init()
        {
            Client.Instance.AddEventHandler("onResourceStop", new Action<string>(OnResourceStop));
            Client.Instance.AddEventHandler("lprp:coda: sessionResponse", new Action<string>(SessionResponse));
            Client.Instance.NuiManager.RegisterCallback("ClosePanel", new Action<IDictionary<string, object>>(ClosePanel));
            Client.Instance.NuiManager.RegisterCallback("RefreshPanel", new Action<IDictionary<string, object>>(RefreshPanel));
            Client.Instance.NuiManager.RegisterCallback("KickUser", new Action<IDictionary<string, object>>(KickUser));
            Client.Instance.NuiManager.RegisterCallback("ChangePriority", new Action<IDictionary<string, object>>(ChangePriority));
        }

        private static void OnResourceStop(string name)
        {
            try
            {
                if (name == resourceName)
                {
                    Client.Instance.NuiManager.SetFocus(false, false);
                    pannelloCodaAperto = false;
                }
            }
            catch (Exception)
            {
                Client.Logger.Error($"[{resourceName} - Admin_Panel] - OnResourceStop()");
            }
        }

        private static void SessionResponse(string session)
        {
            try
            {
                if (!pannelloCodaAperto)
                {
                    List<dynamic> sessionAccounts = session.FromJson<List<dynamic>>();
                    string text = "";
                    sessionAccounts.ForEach(k =>
                    {
                        text = $"{text}<tr>" + $"<td>{k["Handle"]}</td>" + $"<td>{k["License"]}</td>" + $"<td>{k["Discord"]}</td>" + $"<td>{k["Steam"]}</td>" + $"<td>{k["Name"]}</td>" + $"<td>{k["Priority"]}</td>" + $"<td>{Enum.GetName(typeof(SessionState), (int)k["State"])}</td>" + $"<td><button class=button onclick=Change('{k["License"]}')>Modifica</button></td>" + $"</tr>";
                    });
                    Client.Instance.NuiManager.SendMessage(new { sessionlist = text });
                    Client.Instance.NuiManager.SetFocus(true, true);
                    pannelloCodaAperto = true;
                }
                else
                {
                    HUD.ShowNotification("Pannello coda già aperto!!", ColoreNotifica.Red, true);
                }
            }
            catch (Exception)
            {
                Client.Logger.Error($"[{resourceName} - Admin_Panel] - SessionResponse()");
            }
        }

        private static void ClosePanel(IDictionary<string, object> data)
        {
            try
            {
                Client.Instance.NuiManager.SetFocus(false, false);
                Client.Instance.NuiManager.SendMessage(new { panel = "close" });
                pannelloCodaAperto = false;
            }
            catch (Exception)
            {
                Client.Logger.Error($"[{resourceName} - Admin_Panel] - ClosePanel()");
            }
        }

        private static void RefreshPanel(IDictionary<string, object> data)
        {
            try
            {
                Client.Instance.NuiManager.SetFocus(false, false);
                Client.Instance.NuiManager.SendMessage(new { panel = "close" });
                pannelloCodaAperto = false;
                ExecuteCommand("sessione");
            }
            catch (Exception)
            {
                Client.Logger.Error($"[{resourceName} - Admin_Panel] - RefreshPanel()");
            }
        }

        private static void KickUser(IDictionary<string, object> data)
        {
            try
            {
                ExecuteCommand($"ckick {data["License"]}");
            }
            catch (Exception)
            {
                Client.Logger.Error($"[{resourceName} - Admin_Panel] - KickUser()");
            }
        }

        private static void ChangePriority(IDictionary<string, object> data)
        {
            try
            {
                if (data["Value"] as string == "False")
                {
                    ExecuteCommand($"rimuovipriorita {data["License"]}");

                    return;
                }

                ExecuteCommand($"daipriorita {data["License"]} {int.Parse(data["Value"].ToString())}");
            }
            catch (Exception)
            {
                Client.Logger.Error($"[{resourceName} - Admin_Panel] - ChangePriority()");
            }
        }
    }
}