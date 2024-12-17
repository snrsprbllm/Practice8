using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class OrderEditPage : Page
    {
        private Practice8Entities _context;
        private Orders _order;

        public OrderEditPage(Orders order = null)
        {
            InitializeComponent();
            _context = Practice8Entities.GetContext();
            _order = order != null ? order : new Orders();

            LoadProducts();
            LoadUsers();

            if (order != null)
            {
                ProductComboBox.SelectedValue = order.product_id;
                UserComboBox.SelectedValue = order.user_id;
                PriceTextBox.Text = order.price;
                CountTextBox.Text = order.count;
                DatePicker.SelectedDate = order.date;
            }
        }

        private void LoadProducts()
        {
            ProductComboBox.ItemsSource = _context.Products.ToList();
        }

        private void LoadUsers()
        {
            UserComboBox.ItemsSource = _context.Users.ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedValue == null ||
                UserComboBox.SelectedValue == null ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                string.IsNullOrWhiteSpace(CountTextBox.Text) ||
                DatePicker.SelectedDate == null)
            {
                InfoTextBlock.Text = "Заполните все поля!";
                return;
            }

            _order.product_id = (int)ProductComboBox.SelectedValue;
            _order.user_id = (int)UserComboBox.SelectedValue;
            _order.price = PriceTextBox.Text.Trim();
            _order.count = CountTextBox.Text.Trim();
            _order.date = DatePicker.SelectedDate;

            if (_order.id == 0)
            {
                _context.Orders.Add(_order);
            }

            _context.SaveChanges();
            MessageBox.Show("Платеж успешно сохранен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}