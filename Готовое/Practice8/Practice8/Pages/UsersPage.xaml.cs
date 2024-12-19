using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice8.Pages
{
    public partial class UsersPage : Page
    {
        private ObservableCollection<Users> Users { get; set; }
        private string _lastSortColumn = "id"; // По умолчанию сортируем по ID
        private bool _isAscending = true; // По умолчанию сортировка по возрастанию

        public UsersPage()
        {
            InitializeComponent();
            Users = new ObservableCollection<Users>();

            // Привязываем к ListView
            UsersListView.ItemsSource = Users;

            // Загружаем данные
            LoadUsers();
        }

        // Загрузка пользователей из БД
        private void LoadUsers()
        {
            try
            {
                Users.Clear();
                var users = Practice8Entities.GetContext().Users.ToList();
                foreach (var user in users)
                {
                    Users.Add(user);
                }

                // Применяем сортировку по умолчанию
                ApplySorting();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Добавление нового пользователя
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var newUser = new Users
            {
                id = GetNextUserId(),
                last_name = string.Empty,
                first_name = string.Empty,
                login = string.Empty,
                password = string.Empty
            };

            NavigationService.Navigate(new UserEditPage(newUser, OnUserUpdated));
        }

        // Редактирование выбранного пользователя
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersListView.SelectedItem as Users;
            if (selectedUser != null)
            {
                NavigationService.Navigate(new UserEditPage(selectedUser, OnUserUpdated));
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Удаление выбранного пользователя
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersListView.SelectedItem as Users;
            if (selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранного пользователя?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var context = Practice8Entities.GetContext();
                    context.Users.Remove(selectedUser);
                    context.SaveChanges();

                    LoadUsers(); // Обновляем список пользователей
                    MessageBox.Show("Пользователь успешно удален!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Получение следующего ID
        private int GetNextUserId()
        {
            return Users.Any() ? Users.Max(u => u.id) + 1 : 1;
        }

        // Колбэк для обновления данных после редактирования
        private void OnUserUpdated()
        {
            LoadUsers();
        }

        // Обработчик сортировки
        private void SortUsers(object sender, RoutedEventArgs e)
        {
            var columnHeader = sender as GridViewColumnHeader;
            var sortColumn = columnHeader.Column.DisplayMemberBinding?.ToString().Replace("{Binding ", "").Replace("}", "");

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

        // Применение сортировки
        private void ApplySorting()
        {
            var sortedUsers = _isAscending
                ? Users.OrderBy(u => typeof(Users).GetProperty(_lastSortColumn).GetValue(u, null))
                : Users.OrderByDescending(u => typeof(Users).GetProperty(_lastSortColumn).GetValue(u, null));

            UsersListView.ItemsSource = sortedUsers.ToList();
        }
    }
}