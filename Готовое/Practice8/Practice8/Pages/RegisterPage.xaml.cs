using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            string confirmPassword = ConfirmPasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ErrorTextBlock.Text = "Заполните все поля.";
                return;
            }

            if (password != confirmPassword)
            {
                ErrorTextBlock.Text = "Пароли не совпадают.";
                return;
            }

            // Проверка, существует ли пользователь с таким логином
            var existingUser = Practice8Entities1.GetContext().Users.FirstOrDefault(u => u.login == login);
            if (existingUser != null)
            {
                ErrorTextBlock.Text = "Пользователь с таким логином уже существует.";
                return;
            }

            // Хеширование пароля
            string hashedPassword = HashPassword(password);

            // Создание нового пользователя
            var newUser = new Users
            {
                login = login,
                password = hashedPassword,
                last_name = "Новый",
                first_name = "Пользователь",
                patronymic = ""
            };

            try
            {
                Practice8Entities1.GetContext().Users.Add(newUser);
                Practice8Entities1.GetContext().SaveChanges();

                MessageBox.Show("Регистрация прошла успешно!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                // Перенаправление на страницу входа
                NavigationService.Navigate(new LoginPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}