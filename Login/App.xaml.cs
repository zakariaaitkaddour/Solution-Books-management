using Login.Database;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Login
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Appelez la méthode pour initialiser les données
            InitializeAppData();

            MainPage = new AppShell();
        }

        // Méthode pour initialiser les données à partir du fichier JSON
        private async void InitializeAppData()
        {
            try
            {
                // Chemin de la base de données
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "D:\\AppDotNet.db");
                
                // Chemin du fichier JSON contenant les livres
                string jsonFilePath = Path.Combine(Environment.CurrentDirectory, "Resources", "books.json");

                // Création d'une instance du gestionnaire de base de données des livres
                var dbHelper = new DatabaseHelperBook(dbPath);

                // Peupler la base de données à partir du fichier JSON
                //await dbHelper.SeedDatabaseFromJson(jsonFilePath);

                Console.WriteLine("Base de données des livres initialisée avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'initialisation de la base de données : {ex.Message}");
            }
        }

        protected override async void OnStart()
        {
            await DatabaseManager.InitializeAsync();
        }
    }
}
