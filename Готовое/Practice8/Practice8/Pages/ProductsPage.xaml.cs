using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.Entity;
using System;

namespace Practice8
{
    public partial class ProductsPage : Page
    {
        private bool _isSortAscending = true;
        private string _currentSortColumn = "id"; // Сортировка по умолчанию по ID

        public ProductsPage()
        {
            InitializeComponent();
            LoadCategories();
            LoadProducts();
        }

        private void LoadCategories()
        {
            var categories = Practice8Entities.GetContext().Сategories.ToList();
            categories.Insert(0, new Сategories { id = 0, name = "Все категории" });
            CategoryFilterComboBox.ItemsSource = categories;
            CategoryFilterComboBox.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            // Используем Include для загрузки связанных данных (категории)
            ProductsListView.ItemsSource = Practice8Entities.GetContext().Products
                .Include("Сategories") // Убедитесь, что это имя навигационного свойства
                .OrderBy(p => p.id) // Сортировка по умолчанию по ID
                .ToList();
        }

        private void SortProducts(Func<Products, object> keySelector)
        {
            var products = ProductsListView.ItemsSource.Cast<Products>().ToList();

            if (_isSortAscending)
            {
                products = products.OrderBy(keySelector).ToList();
            }
            else
            {
                products = products.OrderByDescending(keySelector).ToList();
            }

            ProductsListView.ItemsSource = products;
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
                    SortProducts(p => p.id);
                    break;

                case "Название":
                    if (_currentSortColumn == "name")
                    {
                        _isSortAscending = !_isSortAscending;
                    }
                    else
                    {
                        _currentSortColumn = "name";
                        _isSortAscending = true;
                    }
                    SortProducts(p => p.name);
                    break;

                case "Категория":
                    if (_currentSortColumn == "category")
                    {
                        _isSortAscending = !_isSortAscending;
                    }
                    else
                    {
                        _currentSortColumn = "category";
                        _isSortAscending = true;
                    }
                    SortProducts(p => p.Сategories.name);
                    break;
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductEditPage());
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsListView.SelectedItem as Products;
            if (selectedProduct != null)
            {
                NavigationService.Navigate(new ProductEditPage(selectedProduct));
            }
            else
            {
                MessageBox.Show("Выберите продукт для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CategoryFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var selectedCategory = CategoryFilterComboBox.SelectedItem as Сategories;
            var query = Practice8Entities.GetContext().Products.AsQueryable();

            if (selectedCategory != null && selectedCategory.id != 0)
            {
                query = query.Where(p => p.category_id == selectedCategory.id);
            }

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                query = query.Where(p => p.name.StartsWith(SearchTextBox.Text));
            }

            // Используем Include для загрузки связанных данных (категории)
            ProductsListView.ItemsSource = query
                .Include("Сategories") // Убедитесь, что это имя навигационного свойства
                .ToList();
        }
    }
}