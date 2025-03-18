using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Linq;
using System.Data.Entity;
using System.Resources;
using System.Windows.Interop;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace costСalculation
{
    //class Data is designed to work with the database
    class Data
    {
        //the method GetCategories gets a list of categories from the database
        public static List<Category> GetCategories()
        {
            var list = new List<Category>();
            using (var ctx = new DataContext())
            {
                foreach (var c in ctx.Categories)
                {
                    list.Add(c);
                }
                return list;

            }
        }

        //General method to check if a category with a given name exists
        private static bool CategoryExists(string categoryName)
        {
            using (var ctx = new DataContext())
            {
                // Checks if a category with the specified name exists (ignoring case).
                // The method returns true if such a category is found, and false if not.
                return ctx.Categories.Any(c => c.NameCategory.ToLower() == categoryName.ToLower());
            }
        }


        //the method CheckCategory that calls a method to check if a category with the specified name exists
        //input parameter type Category
        public static bool CheckCategory(Category cat)
        {
            return CategoryExists(cat.NameCategory);
        }
        //the method CheckCategoryName that calls a method to check if a category with the specified name exists
        //input parameter type string
        public static bool CheckCategoryName(string catName)
        {
            return CategoryExists(catName);
        }

        //the method AddCategory adds a new category to the database,
        //first checking whether the category is already in the database
        //and using the out argument we return a message that the element already exists or null
        public static void AddCategory(Category category, out string message)
        {
            if (!CheckCategory(category))
            {
                using (var ctx = new DataContext())
                {
                    ctx.Categories.Add(category);
                    ctx.SaveChanges();
                    message = null;
                }

            }
            else message = "Element exists";
        }

        //the method CheckIfTheCategoryIsFound searches for a category in the general list
   
        private static bool CheckIfTheCategoryIsFound(Category category)
        {
            // Checks if there is at least one element where idCategory matches category.idCategory
            return GetInfoForDay().Any(info => info.idCategory == category.idCategory);

        }

        //the method DeleteCategory deletes the category or, using the parameter with the out argument,
        //passes information about the impossibility of deleting due to links in the external table
        public static void DeleteCategory(Category category, out string msg)
        {
            if (!CheckIfTheCategoryIsFound(category))
            {
                using (var ctx = new DataContext())
                {
                    var element = ctx.Categories
                      .Where(c => c.NameCategory == category.NameCategory).First();
                    ctx.Categories.Remove(element);
                    ctx.SaveChanges();
                    msg = "";
                }
            }
            else
            {
                msg = "Unable to delete. This category is in use.";
            }

        }

        //the method GetInfoForDay reads information about each day from the database
        public static List<InfoForDay> GetInfoForDay()
        {
            var listI = new List<InfoForDay>();

            using (var ctx = new DataContext())
            {
                // Read data from the database into the intermediate class
                var dataFromDb = ctx.Set<InfoForDayAdditionalClass>().ToList();

                // Convert string dates to DateTime
                var infoList = dataFromDb
                    .Select(info =>
                    {
                        DateTime parsedDate;
                        bool isDateValid = DateTime.TryParse(info.Date, out parsedDate);

                        // Convert the string date and return the InfoForDay object
                        return new InfoForDay
                        {
                            Date = isDateValid ? parsedDate : DateTime.MinValue,
                            Money = info.Money,
                            idCategory = info.idCategory
                        };
                    })
                    .Where(info => info.Date != DateTime.MinValue) // Exclude records with incorrect dates
                    .ToList();


                foreach (var i in infoList)
                {
                    listI.Add(i);
                }

                return listI;
            }

        }

        //the method AddInfo adds information about a new day to the database
        public static void AddInfo(InfoForDay info)
        {
            InfoForDayAdditionalClass infoAdd = new InfoForDayAdditionalClass();
            infoAdd.Date = info.Date.ToString();
            infoAdd.Money = info.Money;
            infoAdd.idCategory = info.Category1.idCategory;

            using (var ctx = new DataContext())
            {
                ctx.InfoForDayAdditionalClassList.Add(infoAdd);
                ctx.SaveChanges();
            }

        }


        //the method EditInfo edits info from a date in the DB
        public static void EditInfo(InfoForDay info)
        {
            InfoForDayAdditionalClass infoEdit = new InfoForDayAdditionalClass();
            infoEdit.Date = info.Date.ToString();
            infoEdit.Money = info.Money;
            infoEdit.idCategory = info.Category1.idCategory;

            using (var ctx = new DataContext())
            {
                string date = info.Date.ToString();
                var result = ctx.InfoForDayAdditionalClassList
                    .Where(b => b.Date == date && b.idCategory == infoEdit.idCategory)
                    .ToList();
                if (result.Any()) //Checking if there are records
                {
                    //in the database we edit in the first found record with the selected date
                    result[0].Date = info.Date.ToString();
                    result[0].Money = info.Money;
                    result[0].idCategory = info.Category1.idCategory;
                    //if there were more lines with this date and this category, then in the remaining lines in money we write 0
                    if (result.Count > 1)
                    {
                        for (int i = 1; i < result.Count; i++)
                            result[i].Money = 0;
                    }

                    ctx.SaveChanges();
                }
            }
        }

        //the method DeleteDate allows you to delete a date from the database
        public static void DeleteDate(InfoForDay info)
        {
            string dateDeleteStr = info.Date.ToString();
            using (var ctx = new DataContext())
            {

                InfoForDayAdditionalClass infoEdit = new InfoForDayAdditionalClass();
                infoEdit.Date = info.Date.ToString();
                infoEdit.Money = info.Money;
                infoEdit.idCategory = info.Category1.idCategory; ;


                var element = ctx.InfoForDayAdditionalClassList
                .Where(e => e.Date == dateDeleteStr).ToList();
                if (element != null)
                {
                    foreach (var item in element)
                        ctx.InfoForDayAdditionalClassList.Remove(item);
                    ctx.SaveChanges();
                }

            }
        }


        //the method DeleteCategoryFromDate allows you to delete a category from the database from current day

        public static void DeleteCategoryFromDate(InfoForDay info, Category category)
        {

            using (var ctx = new DataContext())
            {

                InfoForDayAdditionalClass infoEdit = new InfoForDayAdditionalClass();
                string strDate = "";
                infoEdit.Date = info.Date.ToString();
                strDate = infoEdit.Date;
                infoEdit.Money = info.Money;
                infoEdit.idCategory = info.Category1.idCategory; ;
                string msg = "";

                var element = ctx.InfoForDayAdditionalClassList
                .Where(e => e.Category1.idCategory == category.idCategory && e.Date == strDate).ToList();
                if (element != null)
                {
                    foreach (var item in element)
                        ctx.InfoForDayAdditionalClassList.Remove(item);
                    ctx.SaveChanges();
                }
                else
                {
                    msg = "Category not found in database";
                }
            }
        }

        //the method FindYear find info about the year
        public static void FindYear(InfoForDay infoDays)
        {
            using (var ctx = new DataContext())
            {
                InfoForDayAdditionalClass infoFindYear = new InfoForDayAdditionalClass();
                infoFindYear.Date = infoDays.Date.ToString();
            }
        }

        //the  method Edit Category edits a category or returns information that such a category already exists
        public static void EditCategory(Category category, string newNameCateg, out string message)
        {
            message = null;
            if (!CheckCategoryName(newNameCateg))
            {

                using (var ctx = new DataContext())
                {

                    var categoryToUpdate = ctx.Categories.FirstOrDefault(c => c.idCategory == category.idCategory);
                    if (categoryToUpdate != null)
                    {
                        categoryToUpdate.NameCategory = newNameCateg;
                        ctx.SaveChanges();
                        message = null;
                    }

                }
            }
            else message = "Element exists";

        }

    }
}








