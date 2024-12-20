using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel;
using Microsoft.Win32;

namespace Practice8.Pages
{
    public partial class ReportPage : Page
    {
        public ReportPage()
        {
            InitializeComponent();
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
{
    try
    {
        var startDate = StartDatePicker.SelectedDate;
        var endDate = EndDatePicker.SelectedDate;

        if (startDate == null || endDate == null)
        {
            MessageBox.Show("Выберите даты для формирования отчета.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (startDate > endDate)
        {
            MessageBox.Show("Дата начала не может быть позже даты окончания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var context = Practice8Entities1.GetContext();
        var reportData = context.Orders
            .Where(o => o.date != null && o.date >= startDate && o.date <= endDate)
            .Select(o => new
            {
                CategoryName = o.Products.Сategories.name,
                ProductName = o.Products.name,
                UserName = o.Users.last_name + " " + o.Users.first_name, // Используем конкатенацию строк
                Price = o.price, // Теперь decimal
                Count = o.count, // Теперь int
                Sum = o.sum, // Теперь decimal
                Date = o.date // Убедитесь, что o.date имеет тип DateTime?
            })
            .OrderBy(o => o.CategoryName)
            .ThenBy(o => o.Date)
            .ToList();

        // Добавляем итоговую строку
        var totalSum = reportData.Sum(o => o.Sum);
        reportData.Add(new
        {
            CategoryName = "Итого",
            ProductName = string.Empty,
            UserName = string.Empty,
            Price = (decimal?)null, // Убедитесь, что тип совпадает с исходным
            Count = (int?)null, // Убедитесь, что тип совпадает с исходным
            Sum = totalSum,
            Date = (DateTime?)null // Убедитесь, что тип совпадает с исходным
        });

        ReportListView.ItemsSource = reportData;
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Ошибка формирования отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var startDate = StartDatePicker.SelectedDate;
                var endDate = EndDatePicker.SelectedDate;

                if (startDate == null || endDate == null)
                {
                    MessageBox.Show("Выберите даты для формирования отчета.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (startDate > endDate)
                {
                    MessageBox.Show("Дата начала не может быть позже даты окончания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var context = Practice8Entities1.GetContext();
                var reportData = context.Orders
                    .Where(o => o.date != null && o.date >= startDate && o.date <= endDate)
                    .Select(o => new
                    {
                        CategoryName = o.Products.Сategories.name,
                        ProductName = o.Products.name,
                        UserName = o.Users.last_name + " " + o.Users.first_name, // Используем конкатенацию строк
                        Price = o.price, // Теперь decimal
                        Count = o.count, // Теперь int
                        Sum = o.sum, // Теперь decimal
                        Date = o.date // Убедитесь, что o.date имеет тип DateTime?
                    })
                    .OrderBy(o => o.CategoryName)
                    .ThenBy(o => o.Date)
                    .ToList();

                // Добавляем итоговую строку
                var totalSum = reportData.Sum(o => o.Sum);
                reportData.Add(new
                {
                    CategoryName = "Итого",
                    ProductName = string.Empty,
                    UserName = string.Empty,
                    Price = (decimal?)null, // Убедитесь, что тип совпадает с исходным
                    Count = (int?)null, // Убедитесь, что тип совпадает с исходным
                    Sum = totalSum,
                    Date = (DateTime?)null // Убедитесь, что тип совпадает с исходным
                });

                // Создаем Excel-файл
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Отчет");

                    // Заголовки столбцов
                    worksheet.Cell(1, 1).Value = "Категория";
                    worksheet.Cell(1, 2).Value = "Продукт";
                    worksheet.Cell(1, 3).Value = "Пользователь";
                    worksheet.Cell(1, 4).Value = "Цена";
                    worksheet.Cell(1, 5).Value = "Количество";
                    worksheet.Cell(1, 6).Value = "Сумма";
                    worksheet.Cell(1, 7).Value = "Дата";

                    // Заполняем данные
                    for (int i = 0; i < reportData.Count; i++)
                    {
                        var row = reportData[i];
                        worksheet.Cell(i + 2, 1).Value = row.CategoryName;
                        worksheet.Cell(i + 2, 2).Value = row.ProductName;
                        worksheet.Cell(i + 2, 3).Value = row.UserName;
                        worksheet.Cell(i + 2, 4).Value = row.Price;
                        worksheet.Cell(i + 2, 5).Value = row.Count;
                        worksheet.Cell(i + 2, 6).Value = row.Sum;
                        worksheet.Cell(i + 2, 7).Value = row.Date?.ToString("dd.MM.yyyy");
                    }

                    // Сохраняем файл
                    var saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel files (*.xlsx)|*.xlsx",
                        FileName = "Отчет.xlsx"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Отчет успешно экспортирован в Excel.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}