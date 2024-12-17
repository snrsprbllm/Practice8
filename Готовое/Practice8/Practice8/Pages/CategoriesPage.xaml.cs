using Practice8.Pages;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class CategoriesPage : Page
    {
        private bool _isSortAscending = true;
        private string _currentSortColumn = "id"; // Сортировка по умолчанию по ID

        public CategoriesPage()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = Practice8Entities.GetContext().Сategories
                .OrderBy(c => c.id) // Сортировка по умолчанию по ID
                .ToList();

            CategoriesListView.ItemsSource = categories;
        }

        private void SortCategories(Func<Сategories, object> keySelector)
        {
            var categories = CategoriesListView.ItemsSource.Cast<Сategories>().ToList();

            if (_isSortAscending)
            {
                categories = categories.OrderBy(keySelector).ToList();
            }
            else
            {
                categories = categories.OrderByDescending(keySelector).ToList();
            }

            CategoriesListView.ItemsSource = categories;
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
                    SortCategories(c => c.id);
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
                    SortCategories(c => c.name);
                    break;
            }
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoryEditPage());
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = CategoriesListView.SelectedItem as Сategories;
            if (selectedCategory != null)
            {
                NavigationService.Navigate(new CategoryEditPage(selectedCategory));
            }
            else
            {
                MessageBox.Show("Выберите категорию для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}