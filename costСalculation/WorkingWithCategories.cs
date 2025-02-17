﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            /*
            bool check = false;
            foreach (var a in categories)
                if (categoryName == a.NameCategory)
                {
                    check = true;
                    return check;
                }
            return check;
            */
            return categories.Any(c => c.NameCategory == categoryName);
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
        //check for correctness of entered data, the line should not be more than 20, for the property in the Category class this is an attribute
        //проверка на корректность введенных данных, строка не должна быть больше 20 , у свойства в классе Category это атрибут
        public bool ValidateTextBoxCategory(string categoryName, out string error)
        {
            error = null;
            var context = new ValidationContext(categoryName);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(categoryName, context, results, true))
            {
                foreach (var validationResult in results)
                {
                    error = validationResult.ErrorMessage;
                }
                return false;
            }
            return true;
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

        
        public void EditCategory(Category categ, string newNameCateg)
        {
            if(CheckCategory(categ.NameCategory))
            {
                var category = categories.FirstOrDefault(c => c.idCategory == categ.idCategory);
                if (category != null)
                {
                    category.NameCategory = newNameCateg; 
                }
            }    
        }
        public void SearchCategory(string nameCateg)
        {

        }
    }
}
