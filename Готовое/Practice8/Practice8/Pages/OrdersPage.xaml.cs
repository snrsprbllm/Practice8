using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class OrdersPage : Page
    {
        private ObservableCollection<Orders> Orders { get; set; }
        private string _lastSortColumn = "id"; // По умолчанию сортируем по ID
        private bool _isAscending = true; // По умолчанию сортировка по возрастанию

        public OrdersPage()
        {
            InitializeComponent();
            Orders = new ObservableCollection<Orders>();

            // Привязываем к ListView
            OrdersListView.ItemsSource = Orders;

            // Загружаем данные
            LoadOrders();

            // Загружаем категории для ComboBox
            LoadCategories();
        }

        // Метод для загрузки категорий
        private void LoadCategories()
        {
            try
            {
                var categories = Practice8Entities1.GetContext().Сategories.ToList();
                CategoryComboBox.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Загрузка платежей из БД
        private void LoadOrders()
        {
            try
            {
                Orders.Clear();
                var query = Practice8Entities1.GetContext().Orders.AsQueryable();

                // Если текущий пользователь не администратор, показываем только его заказы
                if (App.CurrentUser != null && App.CurrentUser.role != "admin")
                {
                    query = query.Where(o => o.user_id == App.CurrentUser.id);
                }

                var orders = query.ToList();
                foreach (var order in orders)
                {
                    Orders.Add(order);
                }

                ApplySorting();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки платежей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Добавление нового платежа
        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            var newOrder = new Orders
            {
                id = GetNextOrderId(),
                product_id = 1, // Установите значение по умолчанию
                user_id = 1, // Установите значение по умолчанию
                price = (decimal?)null, // Убедитесь, что тип совпадает с исходным
                count = (int?)null, // Убедитесь, что тип совпадает с исходным
                date = DateTime.Now
            };

            NavigationService.Navigate(new OrderEditPage(newOrder, OnOrderUpdated));
        }

        // Редактирование выбранного платежа
        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrdersListView.SelectedItem as Orders;
            if (selectedOrder != null)
            {
                NavigationService.Navigate(new OrderEditPage(selectedOrder, OnOrderUpdated));
            }
            else
            {
                MessageBox.Show("Выберите платеж для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Удаление выбранного платежа
        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrdersListView.SelectedItem as Orders;
            if (selectedOrder == null)
            {
                MessageBox.Show("Выберите платеж для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранный платеж?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var context = Practice8Entities1.GetContext();
                    context.Orders.Remove(selectedOrder);
                    context.SaveChanges();

                    LoadOrders(); // Обновляем список платежей
                    MessageBox.Show("Платеж успешно удален!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении платежа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Фильтрация по категории
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        // Сброс фильтра
        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            CategoryComboBox.SelectedIndex = 0; // Сбрасываем выбор категории
            LoadOrders(); // Загружаем все заказы
        }

        // Применение фильтра
        private void ApplyFilter()
        {
            var selectedCategory = CategoryComboBox.SelectedItem as Сategories;
            var query = Practice8Entities1.GetContext().Orders.AsQueryable();

            if (selectedCategory != null && selectedCategory.id != 0)
            {
                query = query.Where(o => o.Products.category_id == selectedCategory.id);
            }

            OrdersListView.ItemsSource = query.ToList();
        }

        // Получение следующего ID
        private int GetNextOrderId()
        {
            return Orders.Any() ? Orders.Max(o => o.id) + 1 : 1;
        }

        // Колбэк для обновления данных после редактирования
        private void OnOrderUpdated()
        {
            LoadOrders();
        }

        // Обработчик сортировки
        private void SortOrders(object sender, RoutedEventArgs e)
        {
            var columnHeader = sender as GridViewColumnHeader;
            var sortColumn = columnHeader?.Tag as string; // Получаем имя свойства из Tag

            if (string.IsNullOrEmpty(sortColumn))
            {
                MessageBox.Show("Невозможно определить столбец для сортировки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_lastSortColumn == sortColumn)
            {
                _isAscending = !_isAscending; // Инвертируем направление сортировки
            }
            else
            {
                _lastSortColumn = sortColumn;
                _isAscending = true; // Сбрасываем направление сортировки
            }

            ApplySorting();
        }

        private void ApplySorting()
        {
            var propertyInfo = typeof(Orders).GetProperty(_lastSortColumn);
            if (propertyInfo == null)
            {
                MessageBox.Show($"Свойство '{_lastSortColumn}' не найдено в типе 'Orders'.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var sortedOrders = _isAscending
                ? Orders.OrderBy(o => propertyInfo.GetValue(o, null))
                : Orders.OrderByDescending(o => propertyInfo.GetValue(o, null));

            OrdersListView.ItemsSource = sortedOrders.ToList();
        }
    }
}