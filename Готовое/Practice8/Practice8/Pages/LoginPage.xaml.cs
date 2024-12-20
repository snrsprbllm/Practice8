using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class LoginPage : Page
    {
        private int failedAttempts = 0; // Счетчик неудачных попыток
        private string captchaText; // Текст капчи

        public LoginPage()
        {
            InitializeComponent();
            GenerateCaptcha();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ErrorTextBlock.Text = "Заполните все поля.";
                return;
            }

            // Хеширование пароля
            string hashedPassword = HashPassword(password);

            // Проверка в базе данных
            var user = Practice8Entities1.GetContext().Users
                .FirstOrDefault(u => u.login == login && u.password == hashedPassword);

            if (user != null)
            {
                // Устанавливаем текущего пользователя
                App.CurrentUser = user;

                // Переход на главную страницу
                NavigationService.Navigate(new CategoriesPage());

                // Сделаем кнопки меню на главном окне доступными
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.IsUserAuthorized = true;
                }
            }
            else
            {
                failedAttempts++;
                ErrorTextBlock.Text = "Неверный логин или пароль.";

                if (failedAttempts >= 3)
                {
                    // Показываем капчу
                    CaptchaTextBlock.Visibility = Visibility.Visible;
                    CaptchaTextBox.Visibility = Visibility.Visible;

                    if (!CheckCaptcha())
                    {
                        ErrorTextBlock.Text = "Неверная капча. Попробуйте снова.";
                        GenerateCaptcha();
                        return;
                    }
                }
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу регистрации
            NavigationService.Navigate(new RegisterPage());
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

        private void AdminLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем временного администратора
            var adminUser = new Users
            {
                id = 0, // Уникальный идентификатор
                login = "admin",
                role = "admin"
            };

            // Устанавливаем текущего пользователя
            App.CurrentUser = adminUser;

            // Переход на главную страницу
            NavigationService.Navigate(new CategoriesPage());

            // Сделаем кнопки меню на главном окне доступными
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.IsUserAuthorized = true;
            }
        }

        private void GenerateCaptcha()
        {
            // Генерация случайной строки для капчи
            captchaText = Guid.NewGuid().ToString().Substring(0, 6);
            CaptchaTextBlock.Text = captchaText;
        }

        private bool CheckCaptcha()
        {
            return CaptchaTextBox.Text == captchaText;
        }
    }
}