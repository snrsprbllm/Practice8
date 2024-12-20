using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class ProductsPage : Page
    {
        private ObservableCollection<Products> Products { get; set; }
        private string _lastSortColumn = "id"; // По умолчанию сортируем по ID
        private bool _isAscending = true; // По умолчанию сортировка по возрастанию

        public ProductsPage()
        {
            InitializeComponent();
            Products = new ObservableCollection<Products>();

            // Привязываем к ListView
            ProductsListView.ItemsSource = Products;

            // Загружаем данные
            LoadProducts();

            // Загружаем данные категорий в ComboBox
            LoadCategories();
        }

        // Метод для загрузки категорий
        private void LoadCategories()
        {
            try
            {
                var categories = Practice8Entities1.GetContext().Сategories.ToList();
                CategoryFilterComboBox.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Загрузка продуктов из БД
        private void LoadProducts()
        {
            try
            {
                Products.Clear();
                var products = Practice8Entities1.GetContext().Products.ToList();
                foreach (var product in products)
                {
                    Products.Add(product);
                }

                // Применяем сортировку по умолчанию
                ApplySorting();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продуктов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Добавление нового продукта
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new Products
            {
                id = GetNextProductId(),
                name = string.Empty,
                category_id = 1 // Установите значение по умолчанию
            };

            NavigationService.Navigate(new ProductEditPage(newProduct, OnProductUpdated));
        }

        // Редактирование выбранного продукта
        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsListView.SelectedItem as Products;
            if (selectedProduct != null)
            {
                NavigationService.Navigate(new ProductEditPage(selectedProduct, OnProductUpdated));
            }
            else
            {
                MessageBox.Show("Выберите продукт для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Удаление выбранного продукта
        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsListView.SelectedItem as Products;
            if (selectedProduct == null)
            {
                MessageBox.Show("Выберите продукт для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранный продукт?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var context = Practice8Entities1.GetContext();
                    context.Products.Remove(selectedProduct);
                    context.SaveChanges();

                    LoadProducts(); // Обновляем список продуктов
                    MessageBox.Show("Продукт успешно удален!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении продукта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Получение следующего ID
        private int GetNextProductId()
        {
            return Products.Any() ? Products.Max(p => p.id) + 1 : 1;
        }

        // Колбэк для обновления данных после редактирования
        private void OnProductUpdated()
        {
            LoadProducts();
        }

        // Обработчик события для ComboBox
        private void CategoryFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        // Обработчик события для TextBox
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        // Применение фильтра
        private void ApplyFilter()
        {
            var selectedCategory = CategoryFilterComboBox.SelectedItem as Сategories;
            var searchText = SearchTextBox.Text.ToLower();

            var query = Practice8Entities1 .GetContext().Products.AsQueryable();

            if (selectedCategory != null && selectedCategory.id != 0)
            {
                query = query.Where(p => p.category_id == selectedCategory.id);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(p => p.name.ToLower().Contains(searchText));
            }

            ProductsListView.ItemsSource = query.ToList();
        }

        // Обработчик сортировки
        private void SortProducts(object sender, RoutedEventArgs e)
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
            var propertyInfo = typeof(Products).GetProperty(_lastSortColumn);
            if (propertyInfo == null)
            {
                MessageBox.Show($"Свойство '{_lastSortColumn}' не найдено в типе 'Products'.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var sortedProducts = _isAscending
                ? Products.OrderBy(p => propertyInfo.GetValue(p, null))
                : Products.OrderByDescending(p => propertyInfo.GetValue(p, null));

            ProductsListView.ItemsSource = sortedProducts.ToList();
        }
    }
}