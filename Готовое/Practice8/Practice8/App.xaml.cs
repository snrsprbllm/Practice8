using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Practice8
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Users CurrentUser { get; set; }

        public App()
        {
            CurrentUser = null; // Изначально пользователь не авторизован
        }
    }
}
