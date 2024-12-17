using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class CategoryEditPage : Page
    {
        private Practice8Entities _context;
        private Сategories _category;

        public CategoryEditPage(Сategories category = null)
        {
            InitializeComponent();
            _context = Practice8Entities.GetContext();
            _category = category != null ? category : new Сategories();

            if (category != null)
            {
                CategoryNameTextBox.Text = category.name;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
            {
                InfoTextBlock.Text = "Заполните название категории!";
                return;
            }

            _category.name = CategoryNameTextBox.Text.Trim();

            if (_category.id == 0)
            {
                _context.Сategories.Add(_category);
            }

            _context.SaveChanges();
            MessageBox.Show("Категория успешно сохранена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}