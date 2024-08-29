using Microsoft.Data.SqlClient;

Console.WriteLine("Exo ADO");
Console.WriteLine("*******");

const string connectionString = @"Data Source=TFNSDOT0500A;Initial Catalog=Exo_ADO;Integrated Security=True;Trust Server Certificate=True;";


Console.WriteLine("Afficher les student via la vue \"V_Student\"");
using(SqlConnection connection = new SqlConnection(connectionString))
{
    string query = "SELECT [Id], [FirstName], [LastName] FROM V_Student";

    using(SqlCommand command = connection.CreateCommand())
    {
        command.CommandType = System.Data.CommandType.Text;
        command.CommandText = query;

        connection.Open();
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read()) {
                int studentId = (int)reader["Id"];
                string firstname = (string)reader["FirstName"];
                string lastname = (string)reader["LastName"];

                Console.WriteLine($" - {studentId} {firstname} {lastname}");
            }
        }
        connection.Close();

    }
}
Console.WriteLine();



// Afficher la moyenne annuelle des etudiants
using (SqlConnection connection = new SqlConnection(connectionString))
{
    string query = "SELECT AVG(CAST([YearResult] AS REAL)) FROM V_Student";

    using (SqlCommand cmd = connection.CreateCommand())
    {
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.CommandText = query;

        connection.Open();
        double avgResult = (double)cmd.ExecuteScalar();
        connection.Close();

        Console.WriteLine($"La moyenne est de {avgResult}");
    }
}