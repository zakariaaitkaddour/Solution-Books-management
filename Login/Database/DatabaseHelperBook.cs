using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

public class DatabaseHelperBook
{
    private readonly SQLiteAsyncConnection _database;

    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public bool IsAvailable { get; set; }
    }

    public DatabaseHelperBook(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Book>().Wait();
    }
    public Task<List<Book>> GetBooksAsync() => _database.Table<Book>().ToListAsync();

    public Task<List<Book>> GetAvailableBooks()
    {
        return _database.Table<Book>()
            .Where(book  => book.IsAvailable)
            .ToListAsync();
    }
    //public Task InsertBooksAsync(IEnumerable<Book> books) => _database.InsertAllAsync(books);
    //public async Task SeedDatabaseFromJson(string jsonPath)
    //{
    //    try
    //    {
    //        Label jsonPathT = new Label();
    //        jsonPathT.Text = "Database Path: " + jsonPath;
           

    //        // Lecture du fichier JSON
    //        var jsonData = File.ReadAllText(jsonPath);
    //        var books = JsonSerializer.Deserialize<List<Book>>(jsonData);

    //        if (books == null || books.Count == 0)
    //        {
    //            Console.WriteLine("Aucune donnée à insérer. Vérifiez le contenu du fichier JSON.");
    //            return;
    //        }

    //        Console.WriteLine($"Nombre de livres à insérer : {books.Count}");

    //        // Insertion dans la base de données
    //        await InsertBooksAsync(books);
    //        Console.WriteLine("Livres insérés avec succès !");
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Erreur lors de l'initialisation de la base de données : {ex.Message}");
    //    }
    //}


}



