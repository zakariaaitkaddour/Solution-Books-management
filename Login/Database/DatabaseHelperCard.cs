using SQLite;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Login.Database
{
    public class Card
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int BooksToReserve { get; set; } 
        public int UserId { get; set; }

        

        // Helper to get/set books as a list
        //public List<int>? GetBookIds()
        //{
        //    return string.IsNullOrEmpty(BooksToReserve)
        //        ? new List<int>()
        //        : JsonSerializer.Deserialize<List<int>>(BooksToReserve);
        //}

        //public void SetBookIds(List<int> bookIds)
        //{
        //    BooksToReserve = JsonSerializer.Serialize(bookIds);
        //}
    }

    public class DatabaseHelperCard
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseHelperCard(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Card>().Wait();
        }

        // Insert a new card
        public async Task<int> InsertCardAsync(Card card)
        {
            return await _database.InsertAsync(card);
        }


        // Get a card by user ID
        public async Task<Card> GetCardByUserIdAsync(int userId)
        {
            return await _database.Table<Card>().FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // Get a specific card by ID
        public async Task<Card> GetCardByIdAsync(int cardId)
        {
            return await _database.FindAsync<Card>(cardId);
        }

        // Update a card
        public async Task<int> UpdateCardAsync(Card card)
        {
            return await _database.UpdateAsync(card);
        }

        // Delete a card
        public async Task<int> DeleteCardAsync(int cardId)
        {
            var card = await GetCardByIdAsync(cardId);
            if (card != null)
            {
                return await _database.DeleteAsync(card);
            }
            return 0;
        }
    }
}
