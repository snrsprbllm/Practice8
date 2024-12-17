using Practice8.Pages;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class UsersPage : Page
    {
        private bool _isSortAscending = true;
        private string _currentSortColumn = "id"; // Сортировка по умолчанию по ID

        public UsersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            var users = Practice8Entities.GetContext().Users
                .OrderBy(u => u.id) // Сортировка по умолчанию по ID
                .ToList();

            UsersListView.ItemsSource = users;
        }

        private void SortUsers(Func<Users, object> keySelector)
        {
            var users = UsersListView.ItemsSource.Cast<Users>().ToList();

            if (_isSortAscending)
            {
                users = users.OrderBy(keySelector).ToList();
            }
            else
            {
                users = users.OrderByDescending(keySelector).ToList();
            }

            UsersListView.ItemsSource = users;
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
                    SortUsers(u => u.id);
                    break;

                case "Фамилия":
                    if (_currentSortColumn == "last_name")
                    {
                        _isSortAscending = !_isSortAscending;
                    }
                    else
                    {
                        _currentSortColumn = "last_name";
                        _isSortAscending = true;
                    }
                    SortUsers(u => u.last_name);
                    break;

                case "Имя":
                    if (_currentSortColumn == "first_name")
                    {
                        _isSortAscending = !_isSortAscending;
                    }
                    else
                    {
                        _currentSortColumn = "first_name";
                        _isSortAscending = true;
                    }
                    SortUsers(u => u.first_name);
                    break;

                case "Отчество":
                    if (_currentSortColumn == "patronymic")
                    {
                        _isSortAscending = !_isSortAscending;
                    }
                    else
                    {
                        _currentSortColumn = "patronymic";
                        _isSortAscending = true;
                    }
                    SortUsers(u => u.patronymic);
                    break;

                case "Логин":
                    if (_currentSortColumn == "login")
                    {
                        _isSortAscending = !_isSortAscending;
                    }
                    else
                    {
                        _currentSortColumn = "login";
                        _isSortAscending = true;
                    }
                    SortUsers(u => u.login);
                    break;
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserEditPage());
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersListView.SelectedItem as Users;
            if (selectedUser != null)
            {
                NavigationService.Navigate(new UserEditPage(selectedUser));
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}