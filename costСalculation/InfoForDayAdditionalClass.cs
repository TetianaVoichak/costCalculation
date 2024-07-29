using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace costСalculation
{
    [Table("InfoForDay")]
    public class InfoForDayAdditionalClass
    {
        [Key]
        public int idInfo { get; set; }
        string date;
        decimal money;
        public int idCategory { get; set; }
        Category category;

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        public decimal Money
        {
            get { return money; }
            set { money = value; }
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
    }
}
