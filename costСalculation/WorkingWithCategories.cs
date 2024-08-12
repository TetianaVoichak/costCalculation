﻿
using System;
using System.Collections.Generic;
using System.Text;

namespace costСalculation
{
    class WorkingWithCategories
    {
        List<Category> categories = new List<Category>();

        Category returnCategory;
        public Category ReturnCategory
        {
            get { return returnCategory; }
            set { returnCategory = value; }
        }
           public WorkingWithCategories(List<Category> list)
            {
                    categories = list;
            }
       

        public void AddInList(Category item)
        {
            if(!CheckCategory(item.NameCategory))
                categories.Add(item);
        }

        public void RemoveFromList(Category item)
        {
            categories.Remove(item);
        }
        public bool CheckCategory(string categoryName)
        {
            bool check = false;
            foreach (var a in categories)
                if (categoryName == a.NameCategory)
                {
                    check = true;
                    return check;
                }
            return check;
        }

        public void ReturnCategoryMethod(string categoryName, out bool check)
        {
            check = false;
            foreach (var a in categories)
                if (categoryName == a.NameCategory)
                {
                    check = true;
                    ReturnCategory = a;
                    return;
                }
        }
    }
}
