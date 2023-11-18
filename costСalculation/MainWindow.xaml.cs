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
        public MainWindow()
        {
            InitializeComponent();
            textBox_info_current_date.Text = "Today is " + DateTime.Now.Date.ToShortDateString();
            List<Category> categList = new List<Category>();
            Category cat = new Category("makeup");
            categList.Add(cat);
            cat = new Category("ware");
            categList.Add(cat);
            cat = new Category("wohnung");
            categList.Add(cat);
            foreach (var a in categList)
                comboBox_category.Items.Add(a.NameCategory);
            datePickerMain.Text = DateTime.Now.Date.ToString();
            datePickerSetDate.Text = DateTime.Now.Date.ToString();
        }

        costsOfDay costsOf = new costsOfDay();

        private void button_add_money_to_this_category_Click(object sender, RoutedEventArgs e)
        {
            Category category = new Category(comboBox_category.SelectedItem.ToString());
            costsOf.AddInList(new InfoForDay(datePickerSetDate.SelectedDate.Value, category, decimal.Parse(textBox_cash.Text)));
            methodCheckDate(datePickerSetDate.SelectedDate.Value);
            comboBox_category_choose.SelectedIndex = 0;  
        }
        List<InfoForDay> tempList;
        private decimal totalAmountForTheDay(DateTime d, List<InfoForDay> list)
        {
           return costsOf.AmountInDay(d, list);
        }

        //method allow to know List of selected Day
        private void methodCheckDate(DateTime d)
        {
            textBox_total_amount_this_day.Text = "";
            textBox_total_amount.Text = "";
            comboBox_category_choose.Items.Clear();
            tempList = new List<InfoForDay>();
            List<Category> categ = new List<Category>();
           
            if (costsOf.CheckDate(d, out tempList) && (datePickerSetDate.SelectedDate.Value == datePickerMain.SelectedDate.Value))
            {
                textBox_total_amount_this_day.Text = costsOf.AmountInDay(d, tempList).ToString();
                var result = tempList.GroupBy(p => new { p.Category1.NameCategory });
              
                foreach (var a in result)
                    comboBox_category_choose.Items.Add(a.Key.NameCategory);
            }
          
            comboBox_category_choose.SelectedIndex = 0;
        }

     
        private void datePickerMain_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                methodCheckDate(datePickerMain.SelectedDate.Value);
            }
            catch { }    
        }


        private void methodCheckDateAndCategory(DateTime d, Category c)
        {
            tempList = new List<InfoForDay>();
            if (costsOf.CheckCategory(d, c,  out tempList))
            {
                textBox_total_amount.Text = costsOf.AmoutByCategory(tempList, c).ToString();
                //textBox_total_amount_this_day.Text = costsOf.AmountInDay(d, tempList).ToString();
            }
        }
        private void comboBox_category_choose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (comboBox_category_choose.Items.Count > 0)
                {
                    Category cat = new Category(comboBox_category_choose.SelectedItem.ToString());
                    methodCheckDateAndCategory(datePickerMain.SelectedDate.Value, cat);
                }
            }
            catch
            {

            }
        }
    }
}
