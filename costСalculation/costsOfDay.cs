using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Shell;

namespace costСalculation
{
     class costsOfDay
    {
        List<InfoForDay> listForDay = new List<InfoForDay>();
        public costsOfDay()
        {

        }

        public costsOfDay(List<InfoForDay> list)
        {
            listForDay = list;
        }

        //method that returns the amount per day
         decimal Sum(InfoForDay forDay)
        {
            decimal s = 0;
            return s;
        }
        
        public  void AddInList(InfoForDay item)
        {
            listForDay.Add(item);
        }

        public  void RemoveFromList(InfoForDay item)
        {
            listForDay.Remove(item);
        }

       public bool CheckDate(DateTime d, out List<InfoForDay> tempList)
       {
            bool check = false;
            tempList = new List<InfoForDay>();
            foreach (var a in listForDay)
                if (d == a.Date)
                {
                    tempList.Add(a);
                    check = true;
                }

            return check;
        }
        public  bool CheckCategory(DateTime d,Category c, out List<InfoForDay> tempList)
        {
            bool check = false;
            tempList = new List<InfoForDay>();
            foreach (var a in listForDay)
                if (c.NameCategory.ToString() == a.Category1.NameCategory.ToString() 
                    && d ==a.Date)
                {
                    tempList.Add(a);
                    check = true;
                }

            return check;
        }

        //сумма по конкретной категории / amount for a specific category
        public decimal AmoutByCategory(List<InfoForDay> listforAmount, Category c)
        {
            decimal amount = 0;
            foreach (var a in listforAmount)
            {
                if (String.Equals(c.NameCategory.ToString(), a.Category1.NameCategory.ToString()))
                    amount += a.Money;
            }
            return amount;
        }

        //возвращает данные по дате сделав выборку по категории выбранной даты / returns data by date by selecting by the category of the selected date
        public List<InfoForDay> InfoCurrentDayListCategory(DateTime date, List<InfoForDay> listI, List<Category> categoryList)
        {
            List<InfoForDay> listInfoCurrentDay = new List<InfoForDay>();
            var newListCategoryCurrentDay = from info in listI
                                    join category in categoryList on info.idCategory equals category.idCategory where info.Date == date
                                    select new InfoForDay
                                    {
                                        idInfo = info.idInfo,
                                        Date = info.Date,
                                        Money = info.Money,
                                        Category1 = new Category(category.idCategory, category.NameCategory)
                                    };
            foreach(var info in newListCategoryCurrentDay)
            {
                listInfoCurrentDay.Add(info);
            }
            return listInfoCurrentDay;
        }


           void InfoCurrentDay(DateTime date, List<InfoForDay> listI)
        {

            var newList = from info in listI
                          join InfoForDay in listI
                          on info.idCategory equals InfoForDay.idCategory
                          group info by new InfoForDay
                          {
                              idInfo = info.idInfo,
                              Date = info.Date,
                              Money = info.Money,
                              idCategory = info.idCategory
                          };

        }
        //сумма денег за день общая / total amount of money per day
        public decimal AmountInDay(DateTime d, List<InfoForDay> listforAmountD)
        {
            decimal amount = 0;
            foreach (var a in listforAmountD)
            {
                if (d == a.Date) amount += a.Money;

           }
            return amount;
        }

        //находим года какие существуют в базе / to find the years that exist in the list
        public List<int> FindYear(List<InfoForDay> listI)
        { 
            List<int> newListYear = (from info in listI
                              group info by info.Date.Year into yearGroup
                              select yearGroup.Key).ToList();
            return newListYear;
        }

        //поиск дней в опредленном году / search for days in a given year
        public List<InfoForDay> FindDaysFromYear(int year)
        {

            /** List<InfoForDay> newList = (from info in listForDay
                                        where info.Date.Year == year
                                        select info).ToList();.*/


            List<InfoForDay> newList = (from info in listForDay
                                        where info.Date.Year == year
                                        select info).ToList();

            

            return newList;
        }


        //отдает список дат определенного года и месяца / gives a list of dates for a specific year and month
        public List<InfoForDay> FindDaysFromYearsMonths(int month, int year)
        {
            List<InfoForDay> newList = (from info in listForDay
                                        where info.Date.Month == month && info.Date.Year == year
                                        select info).ToList();
            return newList;
        }


        //поиск месяцев / search months
        public List<int> FindMonthsFromInfo(List<InfoForDay> informationdate)
        {
            List<int> newListMonths = (from info in informationdate
                                       group info by info.Date.Month into monthGroup
                            select monthGroup.Key).ToList();
            return newListMonths;
        }

        //the method returns the amount for each category of a specific month
        //метод возврщает сумму по каждой категории определенного месяца
        public List<CategorySum> ResultListSumMoneyFromMonthCategory(List<InfoForDay> list, List<Category> categoryList)
        {

            List<CategorySum> categorySum = (from info in list
                                                 group info by info.idCategory into categoryGroup
                                                 join category in categoryList on categoryGroup.Key equals category.idCategory
                                                 select new CategorySum
                                                 {
                                                     CategoryId = categoryGroup.Key,
                                                     TotalSum = categoryGroup.Sum(info => info.Money),
                                                     Name = category.NameCategory 
                                                 }).ToList();

            return categorySum;
            /*
             List<CategorySum> categorySum = (from info in list
                                      group info by info.idCategory into categoryGroup
                                      select new CategorySum
                                      {
                                          CategoryId = categoryGroup.Key,
                                          TotalSum = categoryGroup.Sum(info => info.Money)
                                      }).ToList();
            return categorySum;
             */
        }




        /*
        public decimal SumMonth(List<InfoForDay> listInfoMonth,)
        {
            foreach (var info in listInfoMonth) {
            AmountInDay(d, tempList).ToString();
            var result = tempList.GroupBy(p => new { p.Category1.NameCategory });
        }
        */



    }
    }
