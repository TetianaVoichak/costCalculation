
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Controls.Primitives;

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
       public WorkingWithCategories(Category c) { returnCategory = c; }

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
        //проверка на корректность введенных данных, строка не должна быть больше 20 , у свойства в классе Category это атрибут
        public bool ValidateCategory(Category category, out string error)
        {
            error = null;
            var context = new ValidationContext(category); 
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(category, context, results, true))
            {
                foreach (var validationResult in results)
                {
                    error = validationResult.ErrorMessage;
                }
                return false;
            }
            return true;
        }
        public void DefinitionOfTheSelectedCategory( Category categoryOut, string categFromUser)
        {

        }
    }
}
