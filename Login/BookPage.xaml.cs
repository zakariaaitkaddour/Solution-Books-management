using Login.Database;
using System.Collections.ObjectModel;

namespace Login.Views
{
    public partial class BooksPage : ContentPage
    {
        private string _firstName;
        private int _userId;


        private DatabaseHelperCard _cardHelper;

        public ObservableCollection<Book> Books { get; set; }

        public BooksPage(string firstname, int userId)
        {


            InitializeComponent();
            //_firstName = firstname;
            _userId = userId;
            _cardHelper = new DatabaseHelperCard("D:\\AppDotNet.db");

            //BonjourLabel.Text = $"Bonjour {_firstName}";
            LoadBooks();

            Books = new ObservableCollection<Book>
        {
            new Book { Id = 21, Title = "Book 1", Author = "Author 1", Genre = "Fiction", IsAvailable = true },
            new Book { Id = 22, Title = "Book 2", Author = "Author 2", Genre = "Non-fiction", IsAvailable = true },
        };

            BooksCollectionView.ItemsSource = Books;
        }



        private async void LoadBooks()
        {
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "D:\\AppDotNet.db");
                var dbHelper = new DatabaseHelperBook(dbPath);
                var availableBooks = await dbHelper.GetAvailableBooks();

                if (availableBooks != null && availableBooks.Count > 0)
                {
                    BooksCollectionView.ItemsSource = availableBooks;
                }
                else
                {
                    await DisplayAlert("Info", "Aucun livre disponible.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", $"Erreur lors du chargement des livres : {ex.Message}", "OK");
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Clear session 
            Preferences.Set("IsLoggedIn", false);

            await Navigation.PushAsync(new MainPage());

            Navigation.RemovePage(this);
        }

        //private async void OnReserverClicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var button = sender as Button;
        //        if (button == null)
        //        {
        //            await DisplayAlert("Error", "Button is null.", "OK");
        //            return;
        //        }

        //        var book = button.BindingContext as Book;
        //        if (book == null)
        //        {
        //            await DisplayAlert("Error", "Book data is null.", "OK");
        //            return;
        //        }

        //        // Simulate a database operation or actual database code here
        //        await ReserveBookAsync(book);  // Assume this method interacts with the database

        //        await DisplayAlert("Success", $"You reserved: {book.Title}", "OK");
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
        //        Console.WriteLine($"Exception: {ex.Message}");  // Log the exception
        //    }
        //}

        //private async Task ReserveBookAsync(Book book)
        //{
        //    // Example database interaction
        //    try
        //    {
        //        // Your database logic to reserve the book goes here
        //        // For example, saving the reservation to a database

        //        var dbHelper = new  DatabaseHelperCard("D:\\AppDotnet.db");

        //        var card = new Card
        //        {
        //            Date = DateTime.Now.Date,
        //            BooksToReserve = book.Id,
        //            UserId = _userId
        //        };  

        //        await dbHelper.InsertCardAsync(card);
        //    }
        //    catch (Exception dbEx)
        //    {
        //        Console.WriteLine($"Database error: {dbEx.Message}");
        //        throw new Exception("Database operation failed", dbEx);  // Rethrow or handle as needed
        //    }
        //}

        //private async void OnReserverClicked(object sender, EventArgs e)
        //{
        //    await DisplayAlert("Info", "Button clicked. Logic executed successfully.", "OK");
        //}


        private async void OnReserverClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var book = button?.BindingContext as Book;

                if (book == null)
                {
                    await DisplayAlert("Error", "Book data is missing.", "OK");
                    return;
                }

                int userId = Preferences.Get("UserId", -1);
                if (userId == -1)
                {
                    await DisplayAlert("Error", "User session not found.", "OK");
                    return;
                }

                var dbHelperCard = new DatabaseHelperCard("D:\\AppDotNet.db");

                // Check if the user has an existing card
                var existingCard = await dbHelperCard.GetCardByUserIdAsync(userId);

                if (existingCard == null)
                {
                    // Create a new card

                    existingCard = new Card
                    {
                        Date = DateTime.Now,
                        BooksToReserve = book.Id,
                        UserId = userId
                    };
                    await dbHelperCard.InsertCardAsync(existingCard);
                }
                else
                {
                    // Check if the user already has 2 books reserved
                    //if (existingCard.BooksToReserve.Count >= 2)
                    //{
                    //    await DisplayAlert("Error", "You can only reserve up to 2 books per week.", "OK");
                    //    return;
                    //}

                    //else { 

                    //}

                    // Add the book to the existing card
                    //existingCard.BooksToReserve.Add(book.Id);
                    //await dbHelperCard.UpdateCardAsync(existingCard);
                }

                await DisplayAlert("Success", "Book reserved successfully!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                Console.WriteLine(ex); // Log the exception for debugging
            }
        }

    }

}
