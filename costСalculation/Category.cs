using System;
using System.Collections.Generic;
using System.Text;

namespace costСalculation
{
    class Category
    {
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
            nameCategory = name;
        }


    }
       
}
