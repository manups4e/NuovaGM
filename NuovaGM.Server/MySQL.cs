using System;
using Dapper;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using NuovaGM.Shared;

namespace NuovaGM.Server
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
					return await _conn.QueryAsync<dynamic>(def);
				}
			}
			catch (Exception ex)
			{
				Server.Printa(LogType.Fatal, ex.ToString());
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
					_conn.CloseAsync();
				}
			}
			catch (Exception ex)
			{
				Server.Printa(LogType.Fatal, ex.ToString());
			}
		}
	}
}