using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Database
{
    class DatabaseManager
    {
        public static DatabaseHelper UserDatabase { get; private set; }
        public static DatabaseHelperBook BookDatabase { get; private set; }

        public static async Task InitializeAsync()
        {


            string DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "D:\\AppDotNet.db");

            // Initialisez les helpers avec leurs chemins respectifs
            UserDatabase = new DatabaseHelper();
            BookDatabase = new DatabaseHelperBook(DbPath);


            // Chargez les données initiales (si nécessaire)
        //    string jsonFilePath = Path.Combine(Environment.CurrentDirectory, "Resources", "books.json");
        //    await BookDatabase.SeedDatabaseFromJson(jsonFilePath);
        
        }
    }
}
