using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace costСalculation
{
     class costsOfDay
    {
        List<InfoForDay> listForDay = new List<InfoForDay>();

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

       public  bool  CheckDate(DateTime d, out List<InfoForDay> tempList)
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
        
        public  decimal AmoutByCategory(List<InfoForDay> listforAmount, Category c)
        {
            decimal amount = 0;
            foreach (var a in listforAmount)
            {
                if (String.Equals(c.NameCategory.ToString(), a.Category1.NameCategory.ToString()))
                    amount += a.Money;
            }
            return amount;
        }

        //отображает категории выбранной даты / displays the categories of the selected date
        public  List<InfoForDay> InfoCurrentDayListCategory(DateTime date, List<InfoForDay> listI, List<Category> categoryList)
        {
            List<InfoForDay> listInfoCurrentDay = new List<InfoForDay>();
            var newListCategoryCurrentDay = from info in listI
                                    join category in categoryList on info.idCategory equals category.idCategory
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


        public  void InfoCurrentDay(DateTime date, List<InfoForDay> listI)
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


            /*
            List<InfoForDay> listInfoResult = new List<InfoForDay>();

            foreach(var i in newList)
            {
                listInfoResult.Add(i);
            }
            return listInfoResult;
            */
               

        }

        public  decimal AmountInDay(DateTime d, List<InfoForDay> listforAmountD)
        {
            decimal amount = 0;
            foreach (var a in listforAmountD)
            {
                if (d == a.Date) amount += a.Money;

           }
            return amount;
        }

        public  void AddIncrement(InfoForDay ifd)
        {

           
        }


    }
}
