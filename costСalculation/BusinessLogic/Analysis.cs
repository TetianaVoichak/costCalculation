using costСalculation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace costСalculation.BusinessLogic
{
    //class Analysis provides methods for working with data analytics, for future diagram
    internal static class Analysis
    {

        //the method CheckYearAndMonth check if the year and month are entered correctly
        public static bool CheckYearAndMonth(int year, int month,List<int> YEARS)
        {
            if (month > 0 && month <= 12 && YEARS.Contains(year)) return true;
            else return false;
        }

        //The CalculationForAnalysis method searches for information for a diagram and returns
        //a list of CategorySum objects,each containing an id, name, total sum, and percentage for the respective category
      
        public static List<CategorySum> CalculationForAnalysis(int month, int year,List<int> YEARS, costsOfDay costsOf,List<Category> categories)
        {
            List<CategorySum> categorySum = new List<CategorySum>();
            if (CheckYearAndMonth(year, month, YEARS))
            {

                List<InfoForDay> listInfo = new List<InfoForDay>();
                listInfo = costsOf.FindDaysFromYearsMonths(month, year);
                categorySum = costsOf.ResultListSumMoneyFromMonthCategory(listInfo,categories); 

            }
            return categorySum;
        }
        //the method TotalSumCategory searches total amount by category
      
        public static decimal TotalSumCategory(List<CategorySum> categorySumList)
        {
            decimal sum = 0;
            foreach (var category in categorySumList)
                sum += category.TotalSum;

            return sum;
        }

        //the method CalculatePercentageOfAmount calculates the percentage of the total for each category
        public static List<CategorySum> CalculatePercentageOfAmount(List<CategorySum> categorySumList, out decimal sum)
        {
            sum = TotalSumCategory(categorySumList);
            foreach (var cat in categorySumList)
                cat.percent = (int)(cat.TotalSum * 100 / sum);
       
            return categorySumList;

        }

        //the method LabelsWithPercentage creates a label with the category name and its percentage of the total
        static List<string> LabelsWithPercentage(List<CategorySum> categorySumList, decimal sum)
        {
            List<string> labelsWithPercentage = categorySumList
                                        .Select(x => x.Name + "\n" + (x.TotalSum / sum * 100).ToString("0.00") + "%")
                                        .ToList();
            return labelsWithPercentage;
        }

        //the method ReturnLabelsWithPercentage returns a label with the category name and its percentage of the total
        public static List<string> ReturnLabelsWithPercentage(List<CategorySum> categorySumList)
        {
            decimal sum = 0;
            List<string> labelsWithPercentage = LabelsWithPercentage(CalculatePercentageOfAmount(categorySumList, out sum), TotalSumCategory(categorySumList));
            return labelsWithPercentage;
        }
    }
}
