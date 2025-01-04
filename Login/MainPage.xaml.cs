using Login.Database;
using static Login.Database.DatabaseHelper;
using Login.Views;
using Microsoft.Maui.Storage;


namespace Login
{
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
            Task.Run(DatabaseHelper.InitializeAsync).Wait();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string enteredPass = PasswordEntry.Text;
            string passwordHashed = await DatabaseHelper.GetSotoredPassword(UsernameEntry.Text);

            if (passwordHashed != null && DatabaseHelper.PasswordHasher.verifyPassword(enteredPass, passwordHashed))
            {
                // Assuming you have a user object from your database
                var user = await DatabaseHelper.GetUserAsyncNewOne(UsernameEntry.Text); // Implement this function

                if (user != null)
                {
                    Preferences.Set("IsLoggedIn", true);
                    Preferences.Set("Username", UsernameEntry.Text);
                    Preferences.Set("UserId", user.Id);
                    

                    // Navigate to BooksPage
                    await Navigation.PushAsync(new BooksPage(user.firstname, user.Id));
                    UsernameEntry.Text = string.Empty;
                    PasswordEntry.Text = string.Empty;

                }
                else
                {
                    MessageLabel.TextColor = Colors.Red;
                    MessageLabel.Text = "User not found.";
                }
            }
            else
            {
                MessageLabel.TextColor = Colors.Red;
                MessageLabel.Text = "Username or Password incorrect";
            }
        }


        private async void OnSignUpClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }



    }

}
