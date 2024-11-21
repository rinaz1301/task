using Microsoft.Data.SqlClient;

string con = "Server=(localdb)\\mssqllocaldb;User Id=rinsa; Password=1301;Database=Test;Trusted_Connection=True;";

using (var c = new SqlConnection(con))
{
    c.Open();
    string q = "SELECT * FROM CARS";
    using(var com = new SqlCommand(q, c))
    {
        using (var r = com.ExecuteReader())
        {
            while (r.Read())
            {
                Console.WriteLine(r.GetInt32(0));
            }
        }
    }
    
}
Console.WriteLine("df");

Console.ReadKey();
