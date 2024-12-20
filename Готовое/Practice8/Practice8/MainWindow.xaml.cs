using Practice8.Pages;
using System.ComponentModel;
using System.Windows;

namespace Practice8
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _isUserAuthorized;

        public bool IsUserAuthorized
        {
            get => _isUserAuthorized;
            set
            {
                _isUserAuthorized = value;
                OnPropertyChanged(nameof(IsUserAuthorized));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this; // Устанавливаем DataContext для привязки
            CheckAuthorization();
            MainFrame.Navigate(new LoginPage()); // Начальная страница - LoginPage
        }

        private void CheckAuthorization()
        {
            // Проверяем, авторизован ли пользователь
            IsUserAuthorized = App.CurrentUser != null;
        }

        private void Categories_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CategoriesPage());
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductsPage());
        }

        private void Payments_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrdersPage());
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsersPage());
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReportPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}