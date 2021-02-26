using System;
using System.Collections.Generic;
using Dapper;
using System.Threading.Tasks;
using CitizenFX.Core.Native;

using CitizenFX.Core;
using Logger;
using MySqlConnector;

namespace TheLastPlanet.Server
{
	public static class MySQL
	{
		private static string _connectionString = API.GetConvar("mysql_connection_string", "");

		public static async Task<dynamic> QueryAsync(string query, object parameters = null)
		{
			try
			{
				using (MySqlConnection _conn = new MySqlConnection(_connectionString))
				{
					CommandDefinition def = new CommandDefinition(query, parameters);
					IEnumerable<dynamic> result = await _conn.QueryAsync<dynamic>(def);
					await _conn.CloseAsync();
					await BaseScript.Delay(0);
					return result;
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Fatal, ex.ToString());
				return null;
			}
		}

		public static async Task ExecuteAsync(string query, object parameters)
		{
			try
			{
				using (MySqlConnection _conn = new MySqlConnection(_connectionString))
				{
					CommandDefinition def = new CommandDefinition(query, parameters);
					await _conn.ExecuteAsync(def);
					await BaseScript.Delay(0);
					await _conn.CloseAsync();
					await BaseScript.Delay(0);
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Fatal, ex.ToString());
			}
		}
	}
}