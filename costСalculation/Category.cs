using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace costСalculation
{
    public class Category 
    {
        [Key]
        public int idCategory { get; set; }
       

        
        string nameCategory;

        public string NameCategory
        {
            get
            {
                return nameCategory;
            }
            set { nameCategory = value; }

        }

        public Category()
        {

        }
        public Category(string name)
        {
            NameCategory = name;
        }
        public Category(int index, string name)
        {
            idCategory = index;
            nameCategory = name;
        }

    }
       
}
