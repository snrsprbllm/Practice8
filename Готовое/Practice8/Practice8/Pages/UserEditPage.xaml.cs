using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class UserEditPage : Page
    {
        private Practice8Entities _context;
        private Users _user;

        public UserEditPage(Users user = null)
        {
            InitializeComponent();
            _context = Practice8Entities.GetContext();
            _user = user != null ? user : new Users();

            if (user != null)
            {
                LastNameTextBox.Text = user.last_name;
                FirstNameTextBox.Text = user.first_name;
                PatronymicTextBox.Text = user.patronymic;
                LoginTextBox.Text = user.login;
                PasswordTextBox.Text = user.password;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                InfoTextBlock.Text = "Заполните все обязательные поля!";
                return;
            }

            _user.last_name = LastNameTextBox.Text.Trim();
            _user.first_name = FirstNameTextBox.Text.Trim();
            _user.patronymic = PatronymicTextBox.Text.Trim();
            _user.login = LoginTextBox.Text.Trim();
            _user.password = PasswordTextBox.Text.Trim();

            if (_user.id == 0)
            {
                _context.Users.Add(_user);
            }

            _context.SaveChanges();
            MessageBox.Show("Пользователь успешно сохранен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}