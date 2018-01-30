using Dapper;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace CoinPrice
{
	/// <summary>
	/// This class is for creating, accessing and modifying SQLite database.
	/// </summary>
	public class SQLiteDatabase : IDisposable
	{
		//========================================================
		//	Fields
		//========================================================

		private readonly SQLiteConnection connection;

		private bool disposed = false;

		private const string creationQuery = @"CREATE TABLE IF NOT EXISTS [Coins] (
					[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
					[BoughtPriceInEur] FLOAT,
					[BoughtPriceInUSD] FLOAT,
					[CoinName] NVARCHAR(64) NOT NULL,
					[CoinUrlName] NVARCHAR(64) NOT NULL,
					[CurrentAmount] FLOAT
				)";
		private const string insertQuery = @"INSERT INTO Coins (BoughtPriceInEur, BoughtPriceInUSD, CoinName, CoinUrlName, CurrentAmount)
					VALUES (@BoughtPriceInEur, @BoughtPriceInUSD, @CoinName, @CoinUrlName, @CurrentAmount)";
		private const string deleteQuery = @"DELETE FROM Coins 
					WHERE Id = @Id";
		private const string updateQuery = @"UPDATE Coins 
					SET BoughtPriceInEur = @BoughtPriceInEur, BoughtPriceInUSD = @BoughtPriceInUSD, CoinName = @CoinName, CoinUrlName = @CoinUrlName, CurrentAmount = @CurrentAmount
					WHERE Id = @Id";
		private const string selectQuery = @"SELECT * FROM Coins";

		//========================================================
		//	Constructors
		//========================================================

		/// <summary>
		/// Creates SQLite database file with default path.
		/// </summary>
		public SQLiteDatabase() : this(Path.GetTempPath() + @"coins.db") { }

		/// <summary>
		/// Creates SQLite database file using given path.
		/// </summary>
		/// <param name="databasePath">Path to database file.</param>
		public SQLiteDatabase(string databasePath)
		{
			if (databasePath.IsNullOrEmpty())
			{
				throw new ArgumentException("Database file path is either null or empty.", "databasePath");
			}

			FilePath = databasePath;
			if (!File.Exists(FilePath))
			{
				SQLiteConnection.CreateFile(FilePath);
			}
			connection = new SQLiteConnection("Data Source=" + FilePath + ";Version=3;");
			connection.Open();
			SeedDatabase();
			ValidateTables();
		}

		//========================================================
		//	Destructors
		//========================================================

		~SQLiteDatabase()
		{
			Dispose(false);
		}

		//========================================================
		//	Properties
		//========================================================

		/// <summary>
		/// SQLite database file path.
		/// </summary>
		public string FilePath { get; private set; }

		//========================================================
		//	Methods
		//========================================================
		//--------------------------------------------------------
		//	Public
		//--------------------------------------------------------

		public bool AddCoin(UserCoinData coin)
		{
			return ExecuteNonQuery(insertQuery, coin) > 0;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public List<UserCoinData> GetAllCoins()
		{
			return connection.Query<UserCoinData>(selectQuery).ToList();
		}

		public bool RemoveCoin(UserCoinData coin)
		{
			return ExecuteNonQuery(deleteQuery, coin) > 0;
		}

		public bool UpdateCoin(UserCoinData coin)
		{
			return ExecuteNonQuery(updateQuery, coin) > 0;
		}

		//--------------------------------------------------------
		//	Protected
		//--------------------------------------------------------

		protected int ExecuteNonQuery(string commandText, object param = null)
		{
			if (connection.State != ConnectionState.Open)
			{
				connection.Open();
			}
			// TODO: Handle "System.Data.SQLite.SQLiteException: attempt to write a readonly database" exception. 
			// It can be thrown when application doesn't have full rights to SQLiteDatabase file.
			return connection.Execute(commandText, param);
		}

		/// <summary>
		/// Creates all tables if they do not exist.
		/// </summary>
		protected void SeedDatabase()
		{
			ExecuteNonQuery(creationQuery);
		}

		/// <summary>
		/// Checks if created tables' schemes are correct(valid).
		/// </summary>
		/// <exception cref="InvalidDataException">Thrown when at least one table has incorrect scheme.</exception>
		protected void ValidateTables()
		{
			// Get all tables' creation scripts.
			var results = connection.Query(@"SELECT sql FROM sqlite_master ORDER BY tbl_name, type DESC, name");

			var tmpSchema = creationQuery.Replace("IF NOT EXISTS ", ""); // This part is not returned from database, so it has to be removed in order to successfully compare.

			foreach (var result in results)
			{
				var resultStr = ((IDictionary<string, object>)result)["sql"] as string;

				if (tmpSchema.Equals(resultStr, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
			throw new InvalidDataException("Coins table is invalid.");
		}

		//--------------------------------------------------------
		//	Private
		//--------------------------------------------------------

		private void Dispose(bool disposing)
		{
			if (disposed) return;

			if (disposing)
			{
				connection.Dispose();
			}

			disposed = true;
		}
	}
}
