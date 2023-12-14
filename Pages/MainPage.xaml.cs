using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoPartsStore.DataBase;

namespace AutoPartsStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ButtonGoOutClick(object sender, RoutedEventArgs e)
        {
            Global.Global.CurrentUser = null;
            Global.Global.CurrentFrame.Navigate(new LoginPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NameUser.Text = "Пользователь: " + Global.Global.CurrentUser.Name + " " + Global.Global.CurrentUser.LastName;
            RoleUser.Text = "Роль: " + Global.Global.CurrentUser.Role.Name;
            if(Global.Global.CurrentUser.Role == Context.Instance.Role.ToList()[1])
            {
                ButtonUsers.IsEnabled = false;
                ButtonOrders.IsEnabled = false;
            }
            if (Global.Global.CurrentUser.Role == Context.Instance.Role.ToList()[2])
            {
                ButtonUsers.IsEnabled = false;
            }
        }

        private void ButtonUserClick(object sender, RoutedEventArgs e)
        {
            Global.Global.CurrentFrame.Navigate(new TablePage((sender as Button).Content as string));
        }

        private void ButtonOrdersClick(object sender, RoutedEventArgs e)
        {
            Global.Global.CurrentFrame.Navigate(new TablePage((sender as Button).Content as string));
        }

        private void ButtonAutoPartsClick(object sender, RoutedEventArgs e)
        {
            Global.Global.CurrentFrame.Navigate(new TablePage((sender as Button).Content as string));
        }
    }
}
