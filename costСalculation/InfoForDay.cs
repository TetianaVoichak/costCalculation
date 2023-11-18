using System;
using System.Collections.Generic;
using System.Text;

namespace costСalculation
{
    class InfoForDay
    {
        DateTime date;
        Category category;
        decimal money;
        public DateTime Date
        {
            get
            {
                return date;
            }
            set { Date = value; }

        }

        public Category Category1
        {
            get
            {
                return category;
            }

            set
            {
                Category1 = category;
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
                Money = money;
            }
        }

        public InfoForDay()
        {

        }
        public InfoForDay(DateTime date, Category category, decimal money)
        {
            this.date = date;
            this.category = category;
            this.money = money;

        }

    }
}
