using System.Data;
using Microsoft.Data.Sqlite;

namespace habit.core;

public static class Database
{
    private const string connectionString = "Data Source=habit.db";

    public static void Health()
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
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
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Conexão Falhou. Erro: {ex.Message}");
            Console.ResetColor();
        }
    }

    public static void InitDB()
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var create = connection.CreateCommand())
                {
                    create.CommandText = """
                            CREATE TABLE IF NOT EXISTS habit(
                                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    descricao TEXT NOT NULL UNIQUE,
                                    unit TEXT NOT NULL,
                                    );
                            CREATE TABLE IF NOT EXISTS HabitRecords(
                                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    habitId INTEGER NOT NULL,
                                    date TEXT NOT NULL,
                                    quantity INTEGER NOT NULL,
                                    FOREIGN KEY (habitId) REFERENCES Habit(id) ON DELETE CASCADE
                                    );
                        """;
                    create.ExecuteNonQuery();
                }
            }
            if()
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Falha na criação das tabelas: Erro: {ex.Message}");
            Console.ResetColor();
        }
    }
}
