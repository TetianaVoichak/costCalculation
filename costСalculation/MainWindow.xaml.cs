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
           // List<InfoForDay> infoForDayResult = new List<InfoForDay>();
           // infoForDayResult = costsOf.InfoCurrentDayListCategory(date, list, CATEGORYLIST);
           // fillShowInfotheDateinForm(date, infoForDayResult);

            costsOf.CheckDate(date, out infoList);
            costsOf.InfoCurrentDay(date, infoList);

            methodCheckDate(date);

        }


        void LoadInfoForDay()
        {
            List<InfoForDay> listInfoForDayCurrent = new List<InfoForDay>();
            INFOFORDAYLIST = Data.GetInfoForDay();
            listInfoForDayCurrent = costsOf.InfoCurrentDayListCategory(datePickerMain.SelectedDate.Value, INFOFORDAYLIST, CATEGORYLIST);
            //_initialDate = null;
            InfoCurrentDayinForm(DateTime.Today, listInfoForDayCurrent);
        }
        


        costsOfDay costsOf = new costsOfDay();


        //save info new day
        private void button_add_money_to_this_category_Click(object sender, RoutedEventArgs e)
        {
           //Category category = new Category(comboBox_category.SelectedItem.ToString());
            WorkingWithCategories workingCategory = new WorkingWithCategories(CATEGORYLIST);
            
            bool check = false;
            workingCategory.ReturnCategoryMethod(comboBox_category.SelectedItem.ToString(), out check);
            Category categForInfo;
            categForInfo = workingCategory.ReturnCategory;
            if (check == true)
            {
              
                InfoForDay info = new InfoForDay(datePickerSetDate.SelectedDate.Value, categForInfo, decimal.Parse(textBox_cash.Text));
                Data.AddInfo(info);

                LoadInfoForDay();
            }
            else
            {
                MessageBox.Show("Kann nicht zur Datenbank hinzugefügt werden");
            }

            /*
            costsOf.AddInList(new InfoForDay(datePickerSetDate.SelectedDate.Value, category, decimal.Parse(textBox_cash.Text)));

            methodCheckDate(datePickerSetDate.SelectedDate.Value);
            //costsOf.CheckDate(datePickerSetDate.SelectedDate.Value, out tempList);

            comboBox_category_choose.SelectedIndex = 0;

            */       
        }

        List<InfoForDay> tempList;
        private decimal totalAmountForTheDay(DateTime d, List<InfoForDay> list)
        {
           return costsOf.AmountInDay(d, list);
        }

        //fill in the date information in the form und show
        void fillShowInfotheDateinForm(DateTime d, List<InfoForDay> tempList)
        {
            textBox_total_amount_this_day.Text = costsOf.AmountInDay(d, tempList).ToString();
            var result = tempList.GroupBy(p => new { p.Category1.NameCategory });


            foreach (var a in result)
                comboBox_category_choose.Items.Add(a.Key.NameCategory);
            textBox_total_amount.Text = tempList[comboBox_category_choose.SelectedIndex].Money.ToString();
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
            if(costsOf.CheckDate(d, out tempList))// && (datePickerSetDate.SelectedDate.Value == datePickerMain.SelectedDate.Value))
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
                    //methodCheckDate(datePickerMain.SelectedDate.Value);
                    listInfoForDayCurrent = costsOf.InfoCurrentDayListCategory(datePickerMain.SelectedDate.Value, INFOFORDAYLIST, CATEGORYLIST);
                    //  costsOf = new costsOfDay(listInfoForDayCurrent);
                    // costsOf.CheckDate(datePickerMain.SelectedDate.Value, out newList);
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
            if (costsOf.CheckCategory(d, c,  out tempList))
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
                    //здесь ошибка
                    Category cat = new Category(comboBox_category_choose.SelectedItem.ToString());
                    methodCheckDateAndCategory(datePickerMain.SelectedDate.Value, cat);
                }
            }
            catch(Exception ex)
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
    }
}
