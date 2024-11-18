using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using Task.Connector;
using Task.Integration.Data.Models.Models;

string connection = "Server=(localdb)\\mssqllocaldb;Database=ConnectorDb;User ID=KMPO\\RSagdeev;Trusted_Connection=True;MultipleActiveResultSets=true";


string mssqlConnectionString = "Server=(locald)\\mssqllocaldb;Database=ConnectorDb;User ID=KMPO\\RSagdeev;Trusted_Connection=True;MultipleActiveResultSets=true";
string postgreConnectionString = "";

Dictionary<string, string> connectorsCS = new Dictionary<string, string>
		{
			{ "MSSQL",$"ConnectionString='{mssqlConnectionString}';Provider='SqlServer.2019';SchemaName='AvanpostIntegrationTestTaskSchema';"},
			{ "POSTGRE", $"ConnectionString='{postgreConnectionString}';Provider='PostgreSQL.9.5';SchemaName='AvanpostIntegrationTestTaskSchema';"}
		};

var re = Regex.Match(connectorsCS["MSSQL"], "ConnectionString='.*?;'");
Console.WriteLine(connectorsCS["MSSQL"].Split("'").Where(x => x.ToUpper().Contains("SERVER=")).FirstOrDefault());

using(var con = new SqlConnection(mssqlConnectionString))
{
	con.Open();
}

Console.WriteLine("done");
Console.ReadKey();