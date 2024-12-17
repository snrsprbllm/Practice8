using Practice8.Pages;
using System.Windows;

namespace Practice8
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            //MainFrame.Navigate(new ReportPage());
        }
    }
}
