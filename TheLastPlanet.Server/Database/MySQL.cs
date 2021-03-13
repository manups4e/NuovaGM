using System;
using System.Collections.Generic;
using Dapper;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using Logger;
using MySqlConnector;

namespace TheLastPlanet.Server
{
	public static class MySQL
	{
		private static string _connectionString = API.GetConvar("mysql_connection_string", "");

		#region QuerySingle

		public static async Task<T> QuerySingleAsync<T>(string query, object parameters = null)
		{
			try
			{
				using (MySqlConnection conn = new(_connectionString))
				{
					CommandDefinition def = new(query, parameters);
					T result = await conn.QuerySingleAsync<T>(def);
					conn.Close();

					return result;
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Fatal, ex.ToString());

				return default;
			}
		}

		public static async Task<dynamic> QuerySingleAsync(string query, object parameters = null)
		{
			try
			{
				using (MySqlConnection conn = new(_connectionString))
				{
					CommandDefinition def = new(query, parameters);
					dynamic result = await conn.QuerySingleAsync<dynamic>(def);
					conn.Close();

					return result;
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Fatal, ex.ToString());

				return default;
			}
		}

		#endregion

		#region QueryFirst

		public static async Task<T> QueryFirstAsync<T>(string query, object parameters = null)
		{
			try
			{
				using (MySqlConnection conn = new MySqlConnection(_connectionString))
				{
					CommandDefinition def = new CommandDefinition(query, parameters);
					T result = await conn.QueryFirstAsync<T>(def);
					conn.Close();

					return result;
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Fatal, ex.ToString());

				return default;
			}
		}

		public static async Task<dynamic> QueryFirstAsync(string query, object parameters = null)
		{
			try
			{
				using (MySqlConnection conn = new MySqlConnection(_connectionString))
				{
					CommandDefinition def = new CommandDefinition(query, parameters);
					dynamic result = await conn.QueryFirstAsync<dynamic>(def);
					conn.Close();

					return result;
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Fatal, ex.ToString());

				return default;
			}
		}

		#endregion

		#region QueryList

		public static async Task<dynamic> QueryListAsync(string query, object parameters = null)
		{
			try
			{
				using (MySqlConnection conn = new MySqlConnection(_connectionString))
				{
					CommandDefinition def = new CommandDefinition(query, parameters);
					IEnumerable<dynamic> result = await conn.QueryAsync<dynamic>(def);
					conn.Close();

					return result;
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Fatal, ex.ToString());

				return null;
			}
		}

		public static async Task<IEnumerable<T>> QueryListAsync<T>(string query, object parameters = null)
		{
			try
			{
				using (MySqlConnection conn = new MySqlConnection(_connectionString))
				{
					CommandDefinition def = new CommandDefinition(query, parameters);
					IEnumerable<T> result = await conn.QueryAsync<T>(def);
					conn.Close();

					return result;
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Fatal, ex.ToString());

				return default;
			}
		}

		#endregion

		#region Execute

		public static async Task<int> ExecuteAsync(string query, object parameters)
		{
			try
			{
				using (MySqlConnection conn = new MySqlConnection(_connectionString))
				{
					CommandDefinition def = new CommandDefinition(query, parameters);
					int res = await conn.ExecuteAsync(def);
					conn.Close();

					return res;
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Fatal, ex.ToString());

				return 0;
			}
		}

		#endregion
	}
}