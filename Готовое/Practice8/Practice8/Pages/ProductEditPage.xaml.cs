using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.Entity;
using System.Data.SqlClient;
using System;

namespace Practice8
{
    public partial class ProductEditPage : Page
    {
        private Practice8Entities _context;
        private Products _product;

        public ProductEditPage(Products product = null)
        {
            InitializeComponent();
            LoadCategories();
            _context = new Practice8Entities();
            _product = product != null ? product : new Products();

            if (product != null)
            {
                ProductNameTextBox.Text = product.name;
                CategoryComboBox.SelectedValue = product.category_id;
            }
        }

        private void LoadCategories()
        {
            var categories = Practice8Entities.GetContext().Сategories.ToList();
            categories.Insert(0, new Сategories { id = 0, name = "Все категории" });
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text) || CategoryComboBox.SelectedValue == null)
            {
                InfoTextBlock.Text = "Заполните все поля!";
                return;
            }

            // Сохраняем данные с формы в экземпляр продукта
            _product.name = ProductNameTextBox.Text.Trim();
            _product.category_id = (int)CategoryComboBox.SelectedValue;

            // Используем транзакцию для безопасности
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Если это новая запись
                    if (_product.id == 0)
                    {
                        // Включаем IDENTITY_INSERT для таблицы Products
                        _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Products ON;");

                        // Добавляем новый продукт в контекст
                        _context.Products.Add(_product);
                    }
                    else
                    {
                        // Если это редактирование существующей записи
                        var existingProduct = _context.Products.Find(_product.id);
                        if (existingProduct == null)
                        {
                            MessageBox.Show("Продукт с таким ID не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        existingProduct.name = _product.name;
                        existingProduct.category_id = _product.category_id;
                    }

                    // Сохраняем изменения в базе данных
                    _context.SaveChanges();

                    // Отключаем IDENTITY_INSERT
                    _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Products OFF;");

                    // Подтверждаем транзакцию
                    transaction.Commit();

                    MessageBox.Show("Продукт успешно сохранен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
                catch (SqlException ex)
                {
                    // Откатываем транзакцию в случае ошибки
                    transaction.Rollback();

                    // Обрабатываем ошибку
                    if (ex.Number == 2627) // Ошибка дублирования ключа
                    {
                        MessageBox.Show("Продукт с таким ID уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка при сохранении продукта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    // Откатываем транзакцию в случае ошибки
                    transaction.Rollback();

                    MessageBox.Show($"Неизвестная ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}