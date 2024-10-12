using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Globalization;

namespace costСalculation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //ApplicationContext db;
        public List<Category> CATEGORYLIST { get; set; } = Data.GetCategories();
        public List<InfoForDay> INFOFORDAYLIST { get; set; } = Data.GetInfoForDay();

        private DateTime? _initialDate;
        public MainWindow()
        {
            InitializeComponent();
            textBox_info_current_date.Text = "Today is " + DateTime.Now.Date.ToShortDateString();

            foreach (var a in CATEGORYLIST)
                comboBox_category.Items.Add(a.NameCategory);


            datePickerSetDate.SelectedDate = DateTime.Today;

            datePickerMain.SelectedDate = DateTime.Today;

            _initialDate = datePickerMain.SelectedDate;


        }

        //загружаем информацию данной даты в формы / load information of this date into forms
        void InfoCurrentDayinForm(DateTime date, List<InfoForDay> list)
        {
            costsOf = new costsOfDay(list);
            List<InfoForDay> infoList = new List<InfoForDay>();

            costsOf.InfoCurrentDay(date, list);//???

            costsOf.CheckDate(date, out infoList);
            costsOf.InfoCurrentDay(date, infoList);

            methodCheckDate(date);

        }


        void LoadInfoForDay(DateTime date)
        {
            List<InfoForDay> listInfoForDayCurrent = new List<InfoForDay>();
            INFOFORDAYLIST = Data.GetInfoForDay();
            listInfoForDayCurrent = costsOf.InfoCurrentDayListCategory(datePickerMain.SelectedDate.Value, INFOFORDAYLIST, CATEGORYLIST);
            InfoCurrentDayinForm(date, listInfoForDayCurrent);
        }

        costsOfDay costsOf = new costsOfDay();
        decimal money;//a variable that stores money

        //check and convert text to currency format
        decimal workWithMoneyFromTxt(string str_money)
        {
            string strMoneyTrim = str_money.Trim();
            // Заменяем точку на запятую, если точка используется в качестве десятичного разделителя
            // Replace the period with a comma if the period is used as a decimal separator

            strMoneyTrim = strMoneyTrim.Replace('.', ',');

            // Пробуем преобразовать строку в decimal / change to decimal from string
            if (decimal.TryParse(strMoneyTrim, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal result))
            {
                money = result;
            }
            else
            {
                // Сообщаем об ошибке
                MessageBox.Show("Invalid number format");
                money = 0;
            }
            return money;

        }

        //save info new day
        private void button_add_money_to_this_category_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Category categForInfo;
                if (workingWithCategoryMethod(out categForInfo, comboBox_category.SelectedItem.ToString()) == true)
                {

                    workWithMoneyFromTxt(textBox_cash.Text);
                    if (money > 0)
                    {
                        InfoForDay info = new InfoForDay(datePickerSetDate.SelectedDate.Value, categForInfo, money);
                        Data.AddInfo(info);

                        LoadInfoForDay(datePickerMain.SelectedDate.Value);
                        money = 0;
                        MessageBox.Show("data saved successfully");
                    }
                    else
                    {
                        MessageBox.Show("Invalid number format");
                        money = 0;
                    }

                }
                else
                {
                    MessageBox.Show("Cannot be added to the database");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        List<InfoForDay> tempList;
        private decimal totalAmountForTheDay(DateTime d, List<InfoForDay> list)
        {
            return costsOf.AmountInDay(d, list);
        }

        //fill in the date information in the form und show
        void fillShowInfotheDateinForm(DateTime d, List<InfoForDay> tempList)
        {
            //???
            textBox_total_amount_this_day.Text = costsOf.AmountInDay(d, tempList).ToString();
            var result = tempList.GroupBy(p => new { p.Category1.NameCategory });


            foreach (var a in result)
                comboBox_category_choose.Items.Add(a.Key.NameCategory);
            //   textBox_total_amount.Text = tempList[comboBox_category_choose.SelectedIndex].Money.ToString();
            Category cat = new Category(comboBox_category_choose.SelectedItem.ToString());
            methodCheckDateAndCategory(d, cat);
        }



        //method allow to know List of selected Day
        private void methodCheckDate(DateTime d)
        {
            textBox_total_amount_this_day.Text = "";
            textBox_total_amount.Text = "";
            comboBox_category_choose.Items.Clear();
            tempList = new List<InfoForDay>();

            List<Category> categ = new List<Category>();
            comboBox_category_choose.SelectedIndex = 0;
            if (costsOf.CheckDate(d, out tempList))
            {
                fillShowInfotheDateinForm(d, tempList);
                datePickerMain.Text = d.ToString();
            }
            comboBox_category_choose.SelectedIndex = 0;
        }


        private void datePickerMain_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DateTime? currentDay = datePickerMain.SelectedDate;
                if (_initialDate != currentDay)
                {
                    _initialDate = currentDay;
                    List<InfoForDay> listInfoForDayCurrent = new List<InfoForDay>();
                    List<InfoForDay> newList = new List<InfoForDay>();
                    listInfoForDayCurrent = costsOf.InfoCurrentDayListCategory(datePickerMain.SelectedDate.Value, INFOFORDAYLIST, CATEGORYLIST);
                    InfoCurrentDayinForm(datePickerMain.SelectedDate.Value, listInfoForDayCurrent);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void methodCheckDateAndCategory(DateTime d, Category c)
        {
            tempList = new List<InfoForDay>();
            if (costsOf.CheckCategory(d, c, out tempList))
            {
                textBox_total_amount.Text = costsOf.AmoutByCategory(tempList, c).ToString();
            }
        }
        private void comboBox_category_choose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (comboBox_category_choose.Items.Count > 0)
                {
                    //здесь ошибка??
                    Category cat = new Category(comboBox_category_choose.SelectedItem.ToString());
                    methodCheckDateAndCategory(datePickerMain.SelectedDate.Value, cat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void UpdateInfoInComboboxCategory()
        {
            CATEGORYLIST = Data.GetCategories();
            comboBox_category.Items.Clear();
            foreach (var a in CATEGORYLIST)
                comboBox_category.Items.Add(a.NameCategory);
            comboBox_category.SelectedIndex = 0;
        }

        private void button_add_new_category_Click(object sender, RoutedEventArgs e)
        {
            Window_add_category window_Add_Category = new Window_add_category();
            window_Add_Category.Owner = this;
            window_Add_Category.ShowDialog();
        }


        private void comboBox_category_choose_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        //edit current entry
        private void button_edit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to edit the selected date?",
                     "Edit",
                     MessageBoxButton.YesNo,
                     MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                textBox_total_amount.IsEnabled = true;
                button_delete_category.IsEnabled = true;
                button_save_edit.IsEnabled = true;

            }
            else
            {
                button_delete_category.IsEnabled = false;
                button_save_edit.IsEnabled = false;
                textBox_total_amount.IsEnabled = false;
            }
        }

        //determining which category is selected in order to change the data in the future / определение какая категория выбрана чтобы в будущем менять данные
        bool workingWithCategoryMethod(out Category categForInfo, string categoryFromCombobox)
        {
            WorkingWithCategories workingCategory = new WorkingWithCategories(CATEGORYLIST);
            bool check = false;
            workingCategory.ReturnCategoryMethod(categoryFromCombobox, out check);
            bool categoryIsAvailable = check;
            categForInfo = workingCategory.ReturnCategory;
            return categoryIsAvailable;
        }

        //save edited date
        private void button_save_edit_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (comboBox_category_choose.Items.Count == 0)
                {
                    MessageBox.Show("Editing is not possible, first add information about the day in the left part of the app.");
                }

                else
                {
                    Category categForInfo;
                    if (workingWithCategoryMethod(out categForInfo, comboBox_category_choose.SelectedItem.ToString()) == true)
                    {
                        //Category category = new Category();
                        //category = categForInfo;
                        money = workWithMoneyFromTxt(textBox_total_amount.Text);
                        if (money > 0)
                        {
                            InfoForDay info = new InfoForDay(datePickerMain.SelectedDate.Value, categForInfo, money);
                            Data.EditInfo(info);
                            LoadInfoForDay(datePickerMain.SelectedDate.Value);
                            money = 0;
                            textBox_total_amount.IsEnabled = false;
                            button_delete_category.IsEnabled = false;
                            button_save_edit.IsEnabled = false; 

                            MessageBox.Show("changes have been saved");
                        }
                        else
                        {
                            MessageBox.Show("Invalid number format");
                            money = 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cannot be added to the database");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to delete the selected date?",
                      "Delete",
                      MessageBoxButton.YesNo,
                      MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (comboBox_category_choose.Items.Count > 0)
                    {
                        Category categForInfo;
                        if (workingWithCategoryMethod(out categForInfo, comboBox_category_choose.SelectedItem.ToString()) == true)
                        {
                            InfoForDay info = new InfoForDay(datePickerMain.SelectedDate.Value, categForInfo, money);
                           
                            Data.DeleteDate(info);
                            LoadInfoForDay(datePickerMain.SelectedDate.Value);
                            money = 0;
                            textBox_total_amount.IsEnabled = false;
                            MessageBox.Show("changes have been saved");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Date does not exist!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void button_delete_category_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to remove the current category from the list?",
                     "Delete  category",
                     MessageBoxButton.YesNo,
                     MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (comboBox_category_choose.Items.Count > 0)
                    {
                        Category categForInfo;
                        if (workingWithCategoryMethod(out categForInfo, comboBox_category_choose.SelectedItem.ToString()) == true)
                        {
                            money = workWithMoneyFromTxt(textBox_total_amount.Text);
                            if (money > 0)
                            {
                                InfoForDay info = new InfoForDay(datePickerMain.SelectedDate.Value, categForInfo, money);
                                Data.DeleteCategoryFromDate(info, categForInfo);
                                LoadInfoForDay(datePickerMain.SelectedDate.Value);
                                money = 0;
                                textBox_total_amount.IsEnabled = false;
                                button_delete_category.IsEnabled = false;
                                button_save_edit.IsEnabled = false;
                                MessageBox.Show("changes have been saved");
                            }
                            else
                            {
                                MessageBox.Show("Invalid number format");
                                money = 0;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Category does not exist!");
                        textBox_total_amount.IsEnabled = false;
                        button_delete_category.IsEnabled = false;
                        button_save_edit.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }

}