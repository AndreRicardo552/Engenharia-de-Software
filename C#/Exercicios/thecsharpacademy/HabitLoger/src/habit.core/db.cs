using System.Data;
using Microsoft.Data.Sqlite;

namespace habit.core;

public static class Database
{
    private const string connectionString =
        "Server=localhost;Database=LojaDb;Trusted_connection=True;TrustServerCertificate=True;";

    public static void Connect()
    {
        try
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            if (connection.State == ConnectionState.Open)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(
                    $"Conexão estabeleciade. Status: {connection.State}. Versão do SQL: {connection.ServerVersion}"
                );
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Conexão Falhou. Erro: {ex.Data}");
            Console.ResetColor();
        }
    }
}
