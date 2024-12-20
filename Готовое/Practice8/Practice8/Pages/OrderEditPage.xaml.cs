﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class OrderEditPage : Page
    {
        private readonly Orders _order;
        private readonly Action _onOrderUpdated;

        public OrderEditPage(Orders order, Action onOrderUpdated)
        {
            InitializeComponent();
            _order = order;
            _onOrderUpdated = onOrderUpdated;

            // Загрузка продуктов и пользователей для ComboBox
            var products = Practice8Entities1.GetContext().Products.ToList();
            var users = Practice8Entities1.GetContext().Users.ToList();

            ProductComboBox.ItemsSource = products;
            ProductComboBox.DisplayMemberPath = "name";
            ProductComboBox.SelectedValuePath = "id";

            UserComboBox.ItemsSource = users;
            UserComboBox.DisplayMemberPath = "last_name";
            UserComboBox.SelectedValuePath = "id";

            // Привязка данных
            ProductComboBox.SelectedValue = _order.product_id;
            UserComboBox.SelectedValue = _order.user_id;
            PriceTextBox.Text = _order.price.ToString(); // Преобразуем decimal в string
            CountTextBox.Text = _order.count.ToString(); // Преобразуем int в string
            DatePicker.SelectedDate = _order.date;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedValue == null ||
                UserComboBox.SelectedValue == null ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                string.IsNullOrWhiteSpace(CountTextBox.Text) ||
                DatePicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Преобразуем строки в числа
                if (!decimal.TryParse(PriceTextBox.Text, out decimal price) ||
                    !int.TryParse(CountTextBox.Text, out int count))
                {
                    MessageBox.Show("Цена и количество должны быть числами.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Обновляем платеж
                _order.product_id = (int)ProductComboBox.SelectedValue;
                _order.user_id = (int)UserComboBox.SelectedValue;
                _order.price = price; // Присваиваем decimal
                _order.count = count; // Присваиваем int
                _order.date = DatePicker.SelectedDate;

                var context = Practice8Entities1.GetContext();
                if (!context.Orders.Any(o => o.id == _order.id))
                {
                    context.Orders.Add(_order); // Добавляем, если это новый платеж
                }

                context.SaveChanges();

                MessageBox.Show("Платеж сохранен.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                // Обновляем список платежей
                _onOrderUpdated?.Invoke();

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}