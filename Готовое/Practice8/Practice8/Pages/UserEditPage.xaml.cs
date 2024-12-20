using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class UserEditPage : Page
    {
        private readonly Users _user;
        private readonly Action _onUserUpdated;

        public UserEditPage(Users user, Action onUserUpdated)
        {
            InitializeComponent();
            _user = user;
            _onUserUpdated = onUserUpdated;

            // Привязка данных
            LastNameTextBox.Text = _user.last_name;
            FirstNameTextBox.Text = _user.first_name;
            PatronymicTextBox.Text = _user.patronymic;
            LoginTextBox.Text = _user.login;
            PasswordTextBox.Text = _user.password;
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

            try
            {
                var context = Practice8Entities1.GetContext();
                if (_user.id == 0)
                {
                    context.Users.Add(_user);
                }

                context.SaveChanges();
                MessageBox.Show("Пользователь успешно сохранен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                _onUserUpdated?.Invoke();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}