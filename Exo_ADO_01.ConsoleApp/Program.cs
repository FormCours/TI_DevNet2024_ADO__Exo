using Exo_ADO_01.ConsoleApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

Console.WriteLine("Exo ADO");
Console.WriteLine("*******");

const string connectionString = @"Data Source=TFNSDOT0500A;Initial Catalog=Exo_ADO;Integrated Security=True;Trust Server Certificate=True;";


Console.WriteLine("Afficher les student via la vue \"V_Student\"");
using(SqlConnection connection = new SqlConnection(connectionString))
{
    string query = "SELECT [Id], [FirstName], [LastName] FROM V_Student";

    using(SqlCommand command = connection.CreateCommand())
    {
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


Console.WriteLine("Afficher les sections en mode déconnectée");
DataTable sections = new DataTable();

using (SqlConnection connection = new SqlConnection(connectionString))
{
    SqlCommand cmdSelect = connection.CreateCommand();
    cmdSelect.CommandText = "SELECT * FROM Section";

    SqlCommand cmdInsert = connection.CreateCommand();
    cmdInsert.CommandText = "INSERT INTO Section VALUES(@Id, @SectionName)";

    SqlDataAdapter adapter = new SqlDataAdapter();
    adapter.SelectCommand = cmdSelect;
    adapter.InsertCommand = cmdInsert;

    adapter.Fill(sections);
}

foreach(DataRow section in sections.Rows)
{
    Console.WriteLine($" - {section["Id"]} {section["SectionName"]}");
}
Console.WriteLine();


Console.WriteLine("Afficher la moyenne annuelle des etudiants");
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
Console.WriteLine();

Console.WriteLine("Ajouter un etudiant (Requete)");
Student e1 = new Student("Della", "Duck", new DateTime(1988, 11, 11), 19, 1020);

using(SqlConnection connection = new SqlConnection( connectionString ))
{
    string query = "INSERT INTO [Student] (Firstname, Lastname, BirthDate, YearResult, SectionId)" +
                   " VALUES (@Firstname, @Lastname, @BirthDate, @YearResult, @SectionId)";

    using(SqlCommand cmd = connection.CreateCommand()) {
        cmd.CommandText = query;
        cmd.Parameters.AddWithValue("Firstname", e1.Firstname);
        cmd.Parameters.AddWithValue("SectionId", e1.SectionId);
        cmd.Parameters.AddWithValue("YearResult", e1.YearResult);
        cmd.Parameters.AddWithValue("Lastname", e1.Lastname);
        cmd.Parameters.AddWithValue("BirthDate", e1.BirthDate);

        connection.Open();
        int nbRowAdd = cmd.ExecuteNonQuery();
        connection.Close();

        if(nbRowAdd == 1)
        {
            Console.WriteLine($"L'étudiant·e {e1.Firstname} {e1.Lastname} a été ajouté·e !");
        }
        else
        {
            Console.WriteLine("Oups, Quelque chose s'est mal passé :o");
        }
    }
}
Console.WriteLine();


Console.WriteLine("Ajouter un etudiant (StoredProcedure - Result \"DRL\")");
Student e2 = new Student("Gontran", "Bonheur", new DateTime(1987, 01, 01), 9, 1010);

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = connection.CreateCommand())
    {
        cmd.CommandText = "AddStudent";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("Firstname", e2.Firstname);
        cmd.Parameters.AddWithValue("SectionId", e2.SectionId);
        cmd.Parameters.AddWithValue("YearResult", e2.YearResult);
        cmd.Parameters.AddWithValue("Lastname", e2.Lastname);
        cmd.Parameters.AddWithValue("BirthDate", e2.BirthDate);

        connection.Open();
        e2.Id = (int)cmd.ExecuteScalar();
        connection.Close();
        
        Console.WriteLine($"L'étudiant·e {e2.Firstname} {e2.Lastname} a été ajouté·e avec l'id {e2.Id}!");
    }
}
Console.WriteLine();

Console.WriteLine("Ajouter un etudiant (StoredProcedure - Result \"Param OUTPUT\")");
Student e3 = new Student("Miss Tick", "De Magica", new DateTime(1980, 02, 03), 9, 1010);

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = connection.CreateCommand())
    {
        cmd.CommandText = "AddStudent2";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("Firstname", e3.Firstname);
        cmd.Parameters.AddWithValue("SectionId", e3.SectionId);
        cmd.Parameters.AddWithValue("YearResult", e3.YearResult);
        cmd.Parameters.AddWithValue("Lastname", e3.Lastname);
        cmd.Parameters.AddWithValue("BirthDate", e3.BirthDate);

        cmd.Parameters.Add(new SqlParameter()
        {
            ParameterName = "StudentId",
            Value = 0,
            Direction = ParameterDirection.Output
        });

        connection.Open();
        int nbRowAdd = cmd.ExecuteNonQuery();
        e3.Id = (int)cmd.Parameters["StudentId"].Value;
        connection.Close();

        Console.WriteLine($"L'étudiant·e {e3.Firstname} {e3.Lastname} a été ajouté·e avec l'id {e3.Id}!");
    }
}
Console.WriteLine();

Console.WriteLine("Changer un etudiant de section");
Console.Write(" - StudentId : ");
int targetId = int.Parse(Console.ReadLine()!);
Console.Write(" - Nouvelle Section : ");
int sectionId = int.Parse(Console.ReadLine()!);

using(SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    int result;
    using (SqlCommand cmd1 = connection.CreateCommand())
    {
        cmd1.CommandText = "SELECT YearResult FROM student WHERE [Id] = @StudentId";
        cmd1.Parameters.AddWithValue("StudentId", targetId);

        result = (int)cmd1.ExecuteScalar();
    }

    using (SqlCommand cmd2 = new SqlCommand("UpdateStudent", connection))
    {
        cmd2.CommandType = CommandType.StoredProcedure;
        cmd2.Parameters.AddWithValue("StudentId", targetId);
        cmd2.Parameters.AddWithValue("SectionId", sectionId);
        cmd2.Parameters.AddWithValue("YearResult", result);

        cmd2.ExecuteNonQuery();
    }

    connection.Close();
}
Console.WriteLine();

Console.WriteLine("Suprimer un etudiant");
int studentIdForDelete = 2;

using (SqlConnection connection = new SqlConnection(connectionString))
{
    //using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
    using (SqlTransaction transaction = connection.BeginTransaction())
    {

        using (SqlCommand cmd = connection.CreateCommand())
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "DeleteStudent";
            cmd.Parameters.AddWithValue("StudentId", studentIdForDelete);

            cmd.Transaction = transaction; // <- Lien avec la SqlTransaction

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        transaction.Commit();  // ou  transaction.Rollback();
    }
}
Console.WriteLine();
