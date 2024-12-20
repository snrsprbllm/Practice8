using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class CategoriesPage : Page
    {
        private ObservableCollection<Сategories> Categories { get; set; }
        private string _lastSortColumn = "id"; // По умолчанию сортируем по ID
        private bool _isAscending = true; // По умолчанию сортировка по возрастанию

        public CategoriesPage()
        {
            InitializeComponent();
            Categories = new ObservableCollection<Сategories>();

            // Привязываем к ListView
            CategoriesListView.ItemsSource = Categories;

            // Загружаем данные
            LoadCategories();
        }

        // Загрузка категорий из БД
        private void LoadCategories()
        {
            try
            {
                Categories.Clear();
                var categories = Practice8Entities1.GetContext().Сategories.ToList();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                // Применяем сортировку по умолчанию
                ApplySorting();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Добавление новой категории
        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var newCategory = new Сategories
            {
                id = GetNextCategoryId(),
                name = string.Empty
            };

            NavigationService.Navigate(new CategoryEditPage(newCategory, OnCategoryUpdated));
        }

        // Редактирование выбранной категории
        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = CategoriesListView.SelectedItem as Сategories;
            if (selectedCategory != null)
            {
                NavigationService.Navigate(new CategoryEditPage(selectedCategory, OnCategoryUpdated));
            }
            else
            {
                MessageBox.Show("Выберите категорию для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Удаление выбранной категории
        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = CategoriesListView.SelectedItem as Сategories;
            if (selectedCategory == null)
            {
                MessageBox.Show("Выберите категорию для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранную категорию?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var context = Practice8Entities1.GetContext();
                    context.Сategories.Remove(selectedCategory);
                    context.SaveChanges();

                    LoadCategories(); // Обновляем список категорий
                    MessageBox.Show("Категория успешно удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении категории: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Получение следующего ID
        private int GetNextCategoryId()
        {
            return Categories.Any() ? Categories.Max(c => c.id) + 1 : 1;
        }

        // Колбэк для обновления данных после редактирования
        private void OnCategoryUpdated()
        {
            LoadCategories();
        }

        // Обработчик сортировки
        private void SortCategories(object sender, RoutedEventArgs e)
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
            var propertyInfo = typeof(Сategories).GetProperty(_lastSortColumn);
            if (propertyInfo == null)
            {
                MessageBox.Show($"Свойство '{_lastSortColumn}' не найдено в типе 'Сategories'.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var sortedCategories = _isAscending
                ? Categories.OrderBy(c => propertyInfo.GetValue(c, null))
                : Categories.OrderByDescending(c => propertyInfo.GetValue(c, null));

            CategoriesListView.ItemsSource = sortedCategories.ToList();
        }
    }
}