using AutoPartsStore.DataBase;
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

namespace AutoPartsStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditUsersPage.xaml
    /// </summary>
    public partial class EditUsersPage : Page
    {
        public User User { get; set; }
        public EditUsersPage(User user)
        {
            InitializeComponent();
            User = user;
        }
        private void ButtonGoToTableClick(object sender, RoutedEventArgs e)
        {
            Global.Global.CurrentFrame.Navigate(new TablePage("Пользователи"));
        }
        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            if (FieldName.Text.Trim() == "" && FieldLastName.Text.Trim() == "" &&
                FieldLogin.Text.Trim() == "" && FieldPassword.Text.Trim() == "" && FieldRole.SelectedItem == null)
            {
                MessageBox.Show("Не все поля заполнены", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            User.Name = FieldName.Text;
            User.LastName = FieldLastName.Text;
            if (Context.Instance.User.Where(u => u.Login == FieldLogin.Text).ToList().Count() != 0)
            {
                MessageBox.Show("Логин занят", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            User.Login = FieldLogin.Text;
            User.Password = FieldPassword.Text;
            User.Role = FieldRole.SelectedItem as Role;
            Context.Instance.SaveChanges();
            Global.Global.CurrentFrame.Navigate(new TablePage("Пользователи"));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Role> roles = new List<Role>()
            {
                Context.Instance.Role.ToList()[0],
                Context.Instance.Role.ToList()[2]
            };
            FieldRole.ItemsSource = roles;
            FieldName.Text = User.Name;
            FieldLastName.Text = User.LastName;
            FieldLogin.Text = User.Login;
            FieldPassword.Text = User.Password;
            FieldRole.SelectedItem = User.Role;
        }
    }
}
