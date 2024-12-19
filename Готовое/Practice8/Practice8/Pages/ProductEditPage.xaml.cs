using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class ProductEditPage : Page
    {
        private readonly Products _product;
        private readonly Action _onProductUpdated;

        public ProductEditPage(Products product, Action onProductUpdated)
        {
            InitializeComponent();
            _product = product;
            _onProductUpdated = onProductUpdated;

            // Загрузка категорий для ComboBox
            var categories = Practice8Entities.GetContext().Сategories.ToList();
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.DisplayMemberPath = "name";
            CategoryComboBox.SelectedValuePath = "id";

            // Привязка данных
            ProductNameTextBox.Text = _product.name;
            CategoryComboBox.SelectedValue = _product.category_id;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text) || CategoryComboBox.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Обновляем продукт
                _product.name = ProductNameTextBox.Text;
                _product.category_id = (int)CategoryComboBox.SelectedValue;

                var context = Practice8Entities.GetContext();
                if (!context.Products.Any(p => p.id == _product.id))
                {
                    context.Products.Add(_product); // Добавляем, если это новый продукт
                }

                context.SaveChanges();

                MessageBox.Show("Продукт сохранен.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                // Обновляем список продуктов
                _onProductUpdated?.Invoke();

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