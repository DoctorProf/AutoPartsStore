using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using OfficeOpenXml;


namespace AutoPartsStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для TablePage.xaml
    /// </summary>
    public partial class TablePage : Page
    {
        public void Search(string table, string text)
        {
            switch (table)
            {
                case "Пользователи":
                    if (text.Trim() == "" || text == null)
                    {
                        MainTable.ItemsSource = Context.Instance.User.Include("Role").ToList();
                    }
                    else
                    {
                        MainTable.ItemsSource = Context.Instance.User
                        .Where(u => u.Name.ToLower().Contains(text.ToLower()) ||
                                    u.LastName.ToLower().Contains(text.ToLower()) ||
                                    u.Login.ToLower().Contains(text.ToLower()) ||
                                    u.Role.Name.ToLower().Contains(text.ToLower()))
                        .ToList();
                    }
                    break;
                case "Заказы":
                    if (text.Trim() == "" || text == null)
                    {
                        MainTable.ItemsSource = Context.Instance.Order.Include("User").Include("AutoPart").ToList();
                    }
                    else
                    {
                        MainTable.ItemsSource = Context.Instance.Order
                        .Where(o => o.User.LastName.ToLower().Contains(text.ToLower()) ||
                                    o.AutoPart.Name.ToLower().Contains(text.ToLower()) ||
                                    o.AutoPart.Price.ToString().ToLower().Contains(text.ToLower()) ||
                                    o.Date.ToLower().Contains(text.ToLower()) ||
                                    o.Address.ToLower().Contains(text.ToLower()))
                        .ToList();
                    }
                    break;
                case "Автозапчасти":
                    if (text.Trim() == "" || text == null)
                    {
                        MainTable.ItemsSource = Context.Instance.AutoPart.ToList();
                    }
                    else
                    {
                        MainTable.ItemsSource = Context.Instance.AutoPart
                        .Where(a => a.Name.ToLower().Contains(text.ToLower()) ||
                                    a.Price.ToString().ToLower().Contains(text.ToLower()))
                        .ToList();
                    }
                    break;
            }
            
        }
        private void ExportToExcel(string table)
        {
            LicenseContext license = LicenseContext.NonCommercial;
            ExcelPackage.LicenseContext = license;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(table);

                for (int i = 0; i < MainTable.Columns.Count(); i++)
                    excelWorksheet.Cells[1, i + 1].Value = MainTable.Columns[i].Header;

                List<User> users = MainTable.ItemsSource as List<User>;
                List<Order> orders = MainTable.ItemsSource as List<Order>;
                List<AutoPart> autoParts = MainTable.ItemsSource as List<AutoPart>;

                for (int row = 0; row < MainTable.Items.Count - 1; row++)
                {
                    for (int col = 0; col < MainTable.Columns.Count; col++)
                    {
                        switch (table)
                        {
                            case "Пользователи":
                                excelWorksheet.Cells[row + 2, 1].Value = users[row].Name;
                                excelWorksheet.Cells[row + 2, 2].Value = users[row].LastName;
                                excelWorksheet.Cells[row + 2, 3].Value = users[row].Login;
                                excelWorksheet.Cells[row + 2, 4].Value = users[row].Role.Name;
                                break;

                            case "Автозапчасти":
                                excelWorksheet.Cells[row + 2, 1].Value = autoParts[row].Name;
                                excelWorksheet.Cells[row + 2, 2].Value = autoParts[row].Price;
                                break;
                            case "Заказы":
                                excelWorksheet.Cells[row + 2, 1].Value = orders[row].User.LastName;
                                excelWorksheet.Cells[row + 2, 2].Value = orders[row].AutoPart.Name;
                                excelWorksheet.Cells[row + 2, 3].Value = orders[row].AutoPart.Price;
                                excelWorksheet.Cells[row + 2, 4].Value = orders[row].Date;
                                excelWorksheet.Cells[row + 2, 5].Value = orders[row].Address;
                                break;
                        }
                    }
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog() { FileName = table, DefaultExt = ".xlsx" };
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        excelPackage.SaveAs(new FileInfo(saveFileDialog.FileName));
                        MessageBox.Show("Успешно сохранено", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось сохранить файл.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        public DataGridTextColumn CreateColumn(string header, string binding)
        {
            var column = new DataGridTextColumn()
            {
                Binding = new Binding(binding),
                Header = header,
                IsReadOnly = true,
            };
            return column;
        }
        public string Table { get; set; }
        public TablePage(string table)
        {
            InitializeComponent();
            Table = table;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Global.Global.CurrentUser.Role == Context.Instance.Role.ToList()[1])
            {
                ButtonAdd.IsEnabled = false;
                ButtonDelete.IsEnabled = false;
            }
            switch (Table)
            {
                case "Пользователи":
                    MainTable.Columns.Clear();
                    MainTable.Columns.Add(CreateColumn("Имя", "Name"));
                    MainTable.Columns.Add(CreateColumn("Фамилия", "LastName"));
                    MainTable.Columns.Add(CreateColumn("Логин", "Login"));
                    MainTable.Columns.Add(CreateColumn("Роль", "Role.Name"));
                    MainTable.ItemsSource = Context.Instance.User.Include("Role").ToList();
                    break;
                case "Заказы":
                    MainTable.Columns.Clear();
                    MainTable.Columns.Add(CreateColumn("Пользователь", "User.LastName"));
                    MainTable.Columns.Add(CreateColumn("Автозапчасть", "AutoPart.Name"));
                    MainTable.Columns.Add(CreateColumn("Цена", "AutoPart.Price"));
                    MainTable.Columns.Add(CreateColumn("Дата", "Date"));
                    MainTable.Columns.Add(CreateColumn("Адресс", "Address"));
                    MainTable.ItemsSource = Context.Instance.Order.Include("User").Include("AutoPart").ToList();
                    break;
                case "Автозапчасти":
                    MainTable.Columns.Clear();
                    MainTable.Columns.Add(CreateColumn("Название", "Name"));
                    MainTable.Columns.Add(CreateColumn("Цена", "Price"));
                    MainTable.ItemsSource = Context.Instance.AutoPart.ToList();
                    if (Global.Global.CurrentUser.Role == Context.Instance.Role.ToList()[2])
                    {
                        ButtonAdd.IsEnabled = false;
                        ButtonDelete.IsEnabled = false;
                    }
                    break;
            }
        }
        private void ButtonGoToMenuClick(object sender, RoutedEventArgs e)
        {
            Global.Global.CurrentFrame.Navigate(new MainPage());
        }

        private void ButtonAddClick(object sender, RoutedEventArgs e)
        {
            switch (Table)
            {
                case "Пользователи":
                    User user = new User();
                    Context.Instance.User.Add(user);
                    Global.Global.CurrentFrame.Navigate(new EditUsersPage(user));
                    break;
                case "Заказы":
                    Order order = new Order();
                    Context.Instance.Order.Add(order);
                    Global.Global.CurrentFrame.Navigate(new EditOrdersPage(order));
                    break;
                case "Автозапчасти":
                    AutoPart autoPart = new AutoPart();
                    Context.Instance.AutoPart.Add(autoPart);
                    Global.Global.CurrentFrame.Navigate(new EditAutoPartsPage(autoPart));
                    break;
            }
        }

        private void ButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            if (MainTable.SelectedItem == null)
            {
                return;
            }
            MessageBoxResult result = MessageBox.Show("Действительно хотите удалить?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;
            switch (Table)
            {
                case "Пользователи":
                    User selectedUser = MainTable.SelectedItem as User;
                    if (Global.Global.CurrentUser == MainTable.SelectedItem)
                    {
                        MessageBox.Show("Нельзя удалить самого себя", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    List<Order> ordersUser = Context.Instance.Order.Where(o => o.User.ID == selectedUser.ID).ToList();
                    if (ordersUser.Count() != 0)
                    {
                        MessageBox.Show("У этого пользователя есть заказы", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    Context.Instance.User.Remove(MainTable.SelectedItem as User);
                    Context.Instance.SaveChanges();
                    MainTable.ItemsSource = Context.Instance.User.ToList();
                    break;
                case "Заказы":
                    if (Global.Global.CurrentUser != (MainTable.SelectedItem as Order).User)
                    {
                        MessageBox.Show("Нельзя удалить не свой заказ", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    Context.Instance.Order.Remove(MainTable.SelectedItem as Order);
                    Context.Instance.SaveChanges();

                    MainTable.ItemsSource = Context.Instance.Order.ToList();
                    break;
                case "Автозапчасти":
                    AutoPart selectedAutoPart = MainTable.SelectedItem as AutoPart;
                    List<Order> ordersAutoPart = Context.Instance.Order.Where(o => o.AutoPart.ID == selectedAutoPart.ID).ToList();
                    if(ordersAutoPart.Count() != 0)
                    {
                        MessageBox.Show("Существуют заказы на эту деталь", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    Context.Instance.AutoPart.Remove(MainTable.SelectedItem as AutoPart);
                    Context.Instance.SaveChanges();
                    MainTable.ItemsSource = Context.Instance.AutoPart.ToList();
                    break;
            }
        }

        private void ButtonExcelClick(object sender, RoutedEventArgs e)
        {
            ExportToExcel(Table);
        }

        private void MainTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MainTable.SelectedItem == null)
                return;
            switch (Table)
            {
                case "Пользователи":
                    Global.Global.CurrentFrame.Navigate(new EditUsersPage(MainTable.SelectedItem as User));
                    break;
                case "Заказы":
                    Global.Global.CurrentFrame.Navigate(new EditOrdersPage(MainTable.SelectedItem as Order));
                    break;
                case "Автозапчасти":
                    Global.Global.CurrentFrame.Navigate(new EditAutoPartsPage(MainTable.SelectedItem as AutoPart));
                    break;
            }
        }

        private void FieldSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(Table, FieldSearch.Text);
        }
    }
}
