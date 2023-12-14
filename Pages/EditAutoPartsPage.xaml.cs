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
    /// Логика взаимодействия для EditAutoPartsPage.xaml
    /// </summary>
    public partial class EditAutoPartsPage : Page
    {
        public AutoPart AutoPart { get; set; }
        public EditAutoPartsPage(AutoPart autoPart)
        {
            InitializeComponent();
            AutoPart = autoPart;
        }
        private void ButtonGoToTableClick(object sender, RoutedEventArgs e)
        {
            Global.Global.CurrentFrame.Navigate(new TablePage("Автозапчасти"));
        }
        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            if (FieldName.Text.Trim() == "" && FieldPrice.Text.Trim() == "")
            {
                MessageBox.Show("Не все поля заполнены", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            AutoPart.Name = FieldName.Text;
            if (decimal.TryParse(FieldPrice.Text, out decimal value))
            {
                AutoPart.Price = value;
            }
            else
            {
                MessageBox.Show("Неверно указана цена", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
                
            Context.Instance.SaveChanges();
            Global.Global.CurrentFrame.Navigate(new TablePage("Автозапчасти"));
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FieldName.Text = AutoPart.Name;
            FieldPrice.Text = AutoPart.Price.ToString();
        }
    }
}
