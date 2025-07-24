using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts.Definitions.Charts;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization.Json;
using costСalculation.Models;
using costСalculation.Data;
using costСalculation.BusinessLogic;

namespace costСalculation.View
{
    /// <summary>
    /// Interaction logic for WindowDiagram.xaml
    /// </summary>
    public partial class WindowDiagram : Window
    {
        public List<Category> CATEGORYLIST { get; set; } = DataService.GetCategories();
        public List<InfoForDay> INFOFORDAYLIST { get; set; } = DataService.GetInfoForDay();
        public Dictionary<int, string> MonthsDictionary { get; set; }

        public List<string> Labels { get; set; }
        costsOfDay costsOf = new costsOfDay();

        public List<int> YEARS = new List<int>();
        List<CategorySum> categorySum = new List<CategorySum>();
        public SeriesCollection CollectionCategory { get; set; }


        public WindowDiagram()
        {
            InitializeComponent();

            try
            {
                MonthsDictionary = new Dictionary<int, string>
                {
                {1, "January" },
                {2, "February" },
                {3, "March" },
                {4, "April" },
                {5, "May" },
                {6, "June" },
                {7, "July" },
                {8, "August" },
                {9, "September" },
                {10, "October" },
                {11, "November" },
                {12, "December" }
            };

                List<InfoForDay> listInfoForDayCurrent = new List<InfoForDay>();

                List<InfoForDay> newList = new List<InfoForDay>();

                FillingComboboxYearsMonths();

                VisibilityDiagramAndRun();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
        //filling the combobox years and months
        void FillingComboboxYearsMonths()
        {
            YEARS = costsOf.FindYear(INFOFORDAYLIST);
            foreach (var y in YEARS)
                combobox_year.Items.Add(y);

            if (YEARS.Count > 0) combobox_year.SelectedItem = YEARS[0];
            if (MonthsDictionary.ContainsKey(DateTime.Now.Month))
            {
                comboBox_months.SelectedItem = MonthsDictionary[DateTime.Now.Month];
            }
        }


        //check if there is data for the diagram and display if there is
        void VisibilityDiagramAndRun()
        {
            if (comboBox_months.Items.Count > 0 && combobox_year.Items.Count > 0)
            {
                barChart.Visibility = Visibility.Visible;
                StartTheDiagram();
            }
            else
            {
                barChart.Visibility = Visibility.Collapsed;
                resultMoney.Content = "Unfortunately there is no data to display";
            }
        }
        void ShowDiagram(int month, int year)
        {
            categorySum = Analysis.CalculationForAnalysis(month, year, YEARS, costsOf, CATEGORYLIST);
            decimal sum = 0;
            List<CategorySum> categorySumPercent = Analysis.CalculatePercentageOfAmount(categorySum, out sum);


            // Create data for the chart
            CollectionCategory = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Diagram ( "+combobox_year.Text+", " + comboBox_months.Text+" )",
                    Values = new ChartValues<int> (categorySumPercent.Select(x=>x.percent)),
                    DataLabels = true, // Enable displaying data on columns               
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B3D4C2")),

                     LabelPoint = point =>
                     {
                            int index = (int)point.X; //Get the index of the current column
                            decimal amountInEuro = categorySum[index].TotalSum; // receive the corresponding amount
                         
                         return $"{amountInEuro:F2} Euro";
                       },
                }
            };


            barChart.Series = CollectionCategory;

            barChart.AxisY.Clear();

            // the Y axis from 0 to 100
            barChart.AxisY.Add(new Axis
            {
                Title = "Percentage (%)",
                MinValue = 0, // min
                MaxValue = 100, // max
                Separator = new LiveCharts.Wpf.Separator { Step = 10 }
            });

            barChart.AxisX.Clear();

            List<string> labelsWithPercentage = Analysis.ReturnLabelsWithPercentage(categorySum);

            if (labelsWithPercentage.Count == CollectionCategory[0].Values.Count)
            {
                barChart.AxisX.Add(new Axis
                {
                    Labels = labelsWithPercentage,
                    LabelsRotation = 45,
                    Foreground = Brushes.Black,
                    Separator = new LiveCharts.Wpf.Separator { Step = 1 }
                });
            }

            DataContext = this;
        }

        void StartTheDiagram()
        {
            try
            {
                var resultKey = MonthsDictionary.FirstOrDefault(value => value.Value == comboBox_months.SelectedItem.ToString());

                if (!resultKey.Equals(default(KeyValuePair<int, string>)))
                {
                    int monthInt = resultKey.Key;
                    ShowDiagram(monthInt, Convert.ToInt32(combobox_year.SelectedValue));
                    decimal total = Analysis.TotalSumCategory(categorySum);
                    string MonthStr = comboBox_months.SelectedItem.ToString();
                    string YearStr = combobox_year.SelectedItem.ToString();
                    resultMoney.Content = $"Total cost for {MonthStr} {YearStr}:  {total:F2} euro";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_Diagram_Click(object sender, RoutedEventArgs e)
        {
            VisibilityDiagramAndRun();
        }

        private void combobox_year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox_months.Items.Clear();
            comboBox_months.SelectedIndex = 0;
            costsOf = new costsOfDay(INFOFORDAYLIST);
            List<InfoForDay> newListThisYear = new List<InfoForDay>();
            newListThisYear = costsOf.FindDaysFromYear(Convert.ToInt32(combobox_year.SelectedItem.ToString()));
            List<int> months = costsOf.FindMonthsFromInfo(newListThisYear);

            foreach (var m in months)
            {
                if (MonthsDictionary.ContainsKey(m))
                    comboBox_months.Items.Add(MonthsDictionary[m]);
            }
        }

        private void comboBox_months_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VisibilityDiagramAndRun();
        }
    }
}
