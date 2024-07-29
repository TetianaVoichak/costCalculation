
using System;
using System.Collections.Generic;
using System.Text;

namespace costСalculation
{
    class WorkingWithCategories
    {
        List<Category> categories = new List<Category>();
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
    }
}
