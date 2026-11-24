using System;
using Microsoft.Data.SqlClient;

namespace CnANCA_Northwind
{
	class Program
	{
		static string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Munka\\Cn-ADONET-Northwind\\northwind.mdf;Integrated Security=True;Connect Timeout=30";
		static void Main(string[] args)
		{
			ReadData();
			InsertData();
			ReadData();
			DeleteData();
			ReadData();
			DetachDb();
		}

		private static void DetachDb()
		{
			SqlConnection sc = new SqlConnection(ConnectionString);
			try
			{
				sc.Open();
				string[] commands =
				{ "USE master",
				$"ALTER DATABASE [{sc.Database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE",
				$"ALTER DATABASE [{sc.Database}] SET OFFLINE WITH ROLLBACK IMMEDIATE",
				$"EXEC sp_detach_db '{sc.Database}'"
			};
				using (var sqlCommand = new SqlCommand())
				{
					sqlCommand.Connection = sc;
					foreach (string command in commands)
					{
						sqlCommand.CommandText = command;
						sqlCommand.ExecuteNonQuery();
					}
				}
				sc.Close();
			}
			catch (Exception exc)
			{
				Console.WriteLine("Database error: " + exc.Message);
			}
		}

		private static void DeleteData()
		{
			Console.WriteLine("Connected data access - delete data");
			SqlConnection sc = new SqlConnection(ConnectionString);
			try
			{
				sc.Open();
				SqlCommand sco = new SqlCommand("delete from categories where CategoryName LIKE 'Books' and Description LIKE 'Programming, DOE'", sc);
				sco.ExecuteNonQuery();
				sc.Close();
			}
			catch (Exception exc)
			{
				Console.WriteLine("Database error: " + exc.Message);
			}
		}

		static void InsertData()
		{	Console.WriteLine("Connected data access - insert data");
			SqlConnection sc = new SqlConnection(ConnectionString);
			try
			{	sc.Open();
				SqlCommand sco = new SqlCommand("insert into categories(CategoryName,Description) values ('Books','Programming, DOE')", sc);
				sco.ExecuteNonQuery();
				sc.Close();
			}
			catch (Exception exc)
			{
				Console.WriteLine("Database error: " + exc.Message);
			}
		}

		static void ReadData()
		{
			Console.WriteLine("Connected data access - read data");
			SqlConnection sc = new SqlConnection(ConnectionString);
			try
			{	sc.Open();
				SqlCommand sco = new SqlCommand("select CategoryName,Description from categories", sc);
				SqlDataReader sdr = sco.ExecuteReader();
				if (sdr != null)
				{	Console.WriteLine("{0,-20}{1,-40}\n", "Categories", "Description");
					while (sdr.Read())
					{	Console.WriteLine("{0,-20}{1,-40}", sdr.GetString(0), sdr.GetString(1));
					}
					sdr.Close();
				}
				sc.Close();
			}
			catch (Exception exc)
			{	Console.WriteLine("Database error: " + exc.Message);
			}
		}
	}


}

