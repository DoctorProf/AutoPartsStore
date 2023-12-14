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
    /// Логика взаимодействия для EditOrdersPage.xaml
    /// </summary>
    public partial class EditOrdersPage : Page
    {
        public Order Order { get; set; }
        public EditOrdersPage(Order order)
        {
            InitializeComponent();
            Order = order;
        }
        private void ButtonGoToTableClick(object sender, RoutedEventArgs e)
        {
            Global.Global.CurrentFrame.Navigate(new TablePage("Заказы"));
        }
        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            if (FieldAdress.Text.Trim() == "" && FieldAutoPart.SelectedItem == null)
            {
                MessageBox.Show("Не все поля заполнены", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Order.Date= DateTime.Now.ToString("d");
            Order.User = Global.Global.CurrentUser;
            Order.AutoPart = FieldAutoPart.SelectedItem as AutoPart;
            Order.Address = FieldAdress.Text;
            Context.Instance.SaveChanges();
            Global.Global.CurrentFrame.Navigate(new TablePage("Заказы"));
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FieldAutoPart.ItemsSource = Context.Instance.AutoPart.ToList();
            FieldAdress.Text = Order.Address;
            FieldAutoPart.SelectedItem = Order.AutoPart;
        }
        private void FieldAutoPart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FieldAutoPart.SelectedItem == null)
                return;
            FieldPrice.Text = (FieldAutoPart.SelectedItem as AutoPart).Price.ToString();
        }
    }
}
