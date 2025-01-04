using System;
using SQLite;
using System.IO;
using System.Threading.Tasks;
using BCrypt;

namespace Login.Database
{
    internal class DatabaseHelper
    {
        private static SQLiteAsyncConnection _database;

        public class Users
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }
            public String firstname { get; set; }
            public string lastname { get; set; }
            public String username { get; set; }
            public string password { get; set; }
        }

        public static async Task InitializeAsync ()
        {
            if (_database != null) return;

            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "D:\\AppDotNet.db");
            _database = new SQLiteAsyncConnection(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            await _database.CreateTableAsync<Users>();
        }

        public static SQLiteAsyncConnection GetDatabaseConnection()
        {
            return _database;
        }

        // Hasher un mot de passe
        public static class PasswordHasher
        {
            public static string hashPassword(string password)
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            }

            public static bool verifyPassword(string password, string hashedPassword)
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
        }

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public static async Task<T> ExecuteWithLockAsync<T>(Func<Task<T>> action)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await action();
            }
            finally
            {
                _semaphore.Release();
            }
        }


        public static async Task AddUserAsync(Users user)
        {
            await _database.InsertAsync(user);
        }

        //public static Task<Users> GetUserAsync(string username, string password)
        //{
        //    return _database.Table<Users>()
        //        .Where(u => u.username == username && u.password == password)
        //        .FirstOrDefaultAsync();
        //}

        public static async Task<string> GetSotoredPassword(string username)
        {
            var user = await _database.Table<Users>()
                                        .Where(u => u.username == username)
                                        .FirstOrDefaultAsync();
            return user?.password;
        }

        public static async Task<Users> GetUserAsyncNewOne(string username) {
            return await _database.Table<Users>()
                .Where(u => u.username == username)
                .FirstOrDefaultAsync();
        }





    }
}
