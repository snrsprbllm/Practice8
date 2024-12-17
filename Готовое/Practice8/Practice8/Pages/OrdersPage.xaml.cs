using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.Entity;
using Practice8.Pages;

namespace Practice8.Pages
{
    public partial class OrdersPage : Page
    {
        private bool _isSortAscending = true;
        private string _currentSortColumn = "id"; // Сортировка по умолчанию по ID

        public OrdersPage()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            var orders = Practice8Entities.GetContext().Orders
                .Include("Products")
                .Include("Users")
                .OrderBy(o => o.id) // Сортировка по умолчанию по ID
                .ToList();

            OrdersListView.ItemsSource = orders;
        }

        private void SortOrders(Func<Orders, object> keySelector)
        {
            var orders = OrdersListView.ItemsSource.Cast<Orders>().ToList();

            if (_isSortAscending)
            {
                orders = orders.OrderBy(keySelector).ToList();
            }
            else
            {
                orders = orders.OrderByDescending(keySelector).ToList();
            }

            OrdersListView.ItemsSource = orders;
            _isSortAscending = !_isSortAscending; // Переключаем направление сортировки
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var header = sender as GridViewColumnHeader;
            if (header == null) return;

            string column = header.Content.ToString();

            switch (column)
            {
                case "ID":
                    if (_currentSortColumn == "id")
                    {
                        _isSortAscending = !_isSortAscending;
                    }
                    else
                    {
                        _currentSortColumn = "id";
                        _isSortAscending = true;
                    }
                    SortOrders(o => o.id);
                    break;

                case "Продукт":
                    if (_currentSortColumn == "product")
                    {
                        _isSortAscending = !_isSortAscending;
                    }
                    else
                    {
                        _currentSortColumn = "product";
                        _isSortAscending = true;
                    }
                    SortOrders(o => o.Products.name);
                    break;

                case "Пользователь":
                    if (_currentSortColumn == "user")
                    {
                        _isSortAscending = !_isSortAscending;
                    }
                    else
                    {
                        _currentSortColumn = "user";
                        _isSortAscending = true;
                    }
                    SortOrders(o => o.Users.last_name);
                    break;

                case "Цена":
                    if (_currentSortColumn == "price")
                    {
                        _isSortAscending = !_isSortAscending;
                    }
                    else
                    {
                        _currentSortColumn = "price";
                        _isSortAscending = true;
                    }
                    SortOrders(o => o.price);
                    break;

                case "Количество":
                    if (_currentSortColumn == "count")
                    {
                        _isSortAscending = !_isSortAscending;
                    }
                    else
                    {
                        _currentSortColumn = "count";
                        _isSortAscending = true;
                    }
                    SortOrders(o => o.count);
                    break;

                case "Сумма":
                    if (_currentSortColumn == "sum")
                    {
                        _isSortAscending = !_isSortAscending;
                    }
                    else
                    {
                        _currentSortColumn = "sum";
                        _isSortAscending = true;
                    }
                    SortOrders(o => o.sum);
                    break;

                case "Дата":
                    if (_currentSortColumn == "date")
                    {
                        _isSortAscending = !_isSortAscending;
                    }
                    else
                    {
                        _currentSortColumn = "date";
                        _isSortAscending = true;
                    }
                    SortOrders(o => o.date);
                    break;
            }
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderEditPage());
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrdersListView.SelectedItem as Orders;
            if (selectedOrder != null)
            {
                NavigationService.Navigate(new OrderEditPage(selectedOrder));
            }
            else
            {
                MessageBox.Show("Выберите платеж для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}