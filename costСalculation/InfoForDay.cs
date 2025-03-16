using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace costСalculation
{
    //class InfoForDay describing information about expenses for the day, 
    //including a list of categories and a date in the DateTime type
    public class InfoForDay
    {
        [Key]
        public int idInfo { get; set; }
        public int idCategory { get; set; }
        DateTime date;
        Category category ;
        decimal money;


     
        public DateTime Date
        {
            get
            {
                return date;
            }
            set { date = value; }
        }

        public Category Category1
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
            }
        }

        public decimal Money
        {
            get
            {
                return money;
            }
            set
            {
                money = value;
            }
        }

        public InfoForDay()
        {
            category = new Category();
        }
        public InfoForDay(DateTime _date, Category _category, decimal _money)
        {
            date = _date;
            category = _category;
            money = _money;
        }
        public InfoForDay(DateTime _date, decimal _money, int _idcategory)
        {
              date = _date;
              idCategory = _idcategory;
              money = _money;
        }

    }
}
