using Login.Database;
using SQLite;
using static Login.Database.DatabaseHelper;


namespace Login;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();
        InitializeDatabase();
    }

    private async void InitializeDatabase()
    {
        await DatabaseHelper.InitializeAsync();
    }

    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) ||
            string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
            string.IsNullOrWhiteSpace(UsernameEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            MessageLabel.TextColor = Colors.Orange;
            MessageLabel.Text = "All fields are required!";
            return;
        }

        try
        {
            var userCheck = await DatabaseHelper.GetUserAsyncNewOne(UsernameEntry.Text);

            if (userCheck == null)
            {
                string rawPassword = PasswordEntry.Text;
                string hashedPassword = PasswordHasher.hashPassword(rawPassword);

                var user = new Users
                {
                    firstname = FirstNameEntry.Text,
                    lastname = LastNameEntry.Text,
                    username = UsernameEntry.Text,
                    password = hashedPassword,
                };

                await DatabaseHelper.AddUserAsync(user);

                FirstNameEntry.Text = string.Empty;
                LastNameEntry.Text = string.Empty;
                UsernameEntry.Text = string.Empty;
                PasswordEntry.Text = string.Empty;

                MessageLabel.TextColor = Colors.Green;
                MessageLabel.Text = "Account created successfully!";

            }
            else
            {
                MessageLabel.TextColor = Colors.Red;
                MessageLabel.Text = "Username already exists, please choose another one.";
            }
        }
        catch (SQLiteException sqlEx)
        {
            Console.WriteLine($"SQLite Error: {sqlEx.Message}");
            MessageLabel.TextColor = Colors.Red;
            MessageLabel.Text = "Database error: Please try again.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            MessageLabel.TextColor = Colors.Red;
            MessageLabel.Text = $"An error occurred: {ex.Message}";
        }
    }
}
