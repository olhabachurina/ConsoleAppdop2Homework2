// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
class Program
{
    public static string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=NotesDB;Integrated Security=True;TrustServerCertificate=True;";

    static void Main()
    {
        //User user = new User { Id = 1, Name = "Анна Иванченко" };
        //user.CreateUser();
        // Добавление заметок
       // user.AddNote("Покупки", "Хлеб, молоко, яйца");
        //user.AddNote("Встреча", "Завтра в 15:00");
        //user.AddNote("Идеи", "Новый проект");

        // Получение списка заметок
       // user.GetNotes();
        //PrintNotes(user.Notes);

        // Редактирование заметки
        //user.EditNote(2, "Важная встреча", "Завтра в 14:30");

        // Получение обновленного списка заметок
       // user.GetNotes();
       // PrintNotes(user.Notes);

        // Удаление заметки
       // user.DeleteNote(1);

        // Получение окончательного списка заметок
       // user.GetNotes();
       //PrintNotes(user.Notes);
    }

    static void PrintNotes(List<Note> notes)
    {
        Console.WriteLine($"Список заметок:");
        foreach (var note in notes)
        {
            Console.WriteLine($"{note.Id}. {note.Name} - {note.Description} (Создано: {note.CreatedDate})");
        }
    }
}

class Note
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
}

class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Note> Notes { get; set; }

    public User()
    {
        Notes = new List<Note>();
    }


    public void AddNote(string name, string description)
    {
        using (SqlConnection connection = new SqlConnection(Program.connectionString))
        {
            connection.Open();

            string insertQuery = "INSERT INTO Notes (UserId, Name, Description, CreatedDate) VALUES (@UserId, @Name, @Description, @CreatedDate);";

            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@UserId", Id);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                command.ExecuteNonQuery();
            }
        }
    }


    public void GetNotes()
    {
        using (SqlConnection connection = new SqlConnection(Program.connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT Id, Name, Description, CreatedDate FROM Notes WHERE UserId = @UserId;";

            using (SqlCommand command = new SqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@UserId", Id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Notes.Clear();

                    while (reader.Read())
                    {
                        Note note = new Note
                        {
                            Id = reader.GetInt32(0),
                            UserId = Id,
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                            CreatedDate = reader.GetDateTime(3)
                        };

                        Notes.Add(note);
                    }
                }
            }
        }
    }


    public void DeleteNote(int noteId)
    {
        using (SqlConnection connection = new SqlConnection(Program.connectionString))
        {
            connection.Open();

            string deleteQuery = "DELETE FROM Notes WHERE Id = @NoteId AND UserId = @UserId;";

            using (SqlCommand command = new SqlCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@NoteId", noteId);
                command.Parameters.AddWithValue("@UserId", Id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Заметка с Id {noteId} удалена.");
                }
                else
                {
                    Console.WriteLine($"Заметка с Id {noteId} не найдена.");
                }
            }
        }
    }


    public void EditNote(int noteId, string newName, string newDescription)
    {
        using (SqlConnection connection = new SqlConnection(Program.connectionString))
        {
            connection.Open();

            string updateQuery = "UPDATE Notes SET Name = @NewName, Description = @NewDescription WHERE Id = @NoteId AND UserId = @UserId;";

            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@NoteId", noteId);
                command.Parameters.AddWithValue("@UserId", Id);
                command.Parameters.AddWithValue("@NewName", newName);
                command.Parameters.AddWithValue("@NewDescription", newDescription);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Заметка с Id {noteId} отредактирована.");
                }
                else
                {
                    Console.WriteLine($"Заметка с Id {noteId} не найдена.");
                }
            }
        }
    }

    public void CreateUser()
    {
        using (SqlConnection connection = new SqlConnection(Program.connectionString))
        {
            connection.Open();

            string insertUserQuery = "INSERT INTO Users (Name) VALUES (@UserName);";

            using (SqlCommand command = new SqlCommand(insertUserQuery, connection))
            {
                command.Parameters.AddWithValue("@UserName", Name);

                command.ExecuteNonQuery();
            }
        }
    }
}

     
