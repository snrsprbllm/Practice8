using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class CategoryEditPage : Page
    {
        private readonly Сategories _category;
        private readonly Action _onCategoryUpdated;

        public CategoryEditPage(Сategories category, Action onCategoryUpdated)
        {
            InitializeComponent();
            _category = category;
            _onCategoryUpdated = onCategoryUpdated;

            // Привязка данных
            CategoryNameTextBox.Text = _category.name;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
            {
                MessageBox.Show("Название категории не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Обновляем категорию
                _category.name = CategoryNameTextBox.Text;

                var context = Practice8Entities1.GetContext();
                if (!context.Сategories.Any(c => c.id == _category.id))
                {
                    context.Сategories.Add(_category); // Добавляем, если это новая категория
                }

                context.SaveChanges();

                MessageBox.Show("Категория сохранена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                // Обновляем список категорий
                _onCategoryUpdated?.Invoke();

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