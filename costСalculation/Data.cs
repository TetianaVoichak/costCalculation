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
    class Data
    {

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

        //suchen, ob schon diese "Category" in der Liste gibt
        public static bool CheckCategory(Category cat)
        {
            bool temp = false;
            using (var ctx = new DataContext())
            {
                var count = ctx.Categories
                  .Where(c => c.NameCategory.ToLower() == cat.NameCategory.ToLower()).Count();

                if (count > 0) temp = true;
            }
            return temp;
        }

        public static bool CheckCategoryName(string catName)
        {
            bool temp = false;
            using (var ctx = new DataContext())
            {
                var count = ctx.Categories
                  .Where(c => c.NameCategory.ToLower() == catName.ToLower()).Count();

                if (count > 0) temp = true;
            }
            return temp;
        }

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

        private static bool CheckIfTheCategoryIsFound(Category category)
        {
            foreach (var info in GetInfoForDay())
            {
                if (info.idCategory == category.idCategory)
                    return true;
                 
            }
            return false;
        }

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

        //convert string to date from database and write to list
        public static void GetСonvertStringToDate()
        {

            using (var ctx = new DataContext())
            {

            }
        }

        public static List<InfoForDay> GetInfoForDay()
        {
            var listI = new List<InfoForDay>();

            using (var ctx = new DataContext())
            {
                // Считываем данные из базы данных в промежуточный класс /  Read data from the database into the intermediate class
                var dataFromDb = ctx.Set<InfoForDayAdditionalClass>().ToList();

                // Преобразуем строковые даты в DateTime / Convert string dates to DateTime
                var infoList = dataFromDb
                    .Select(info =>
                    {
                        DateTime parsedDate;
                        bool isDateValid = DateTime.TryParse(info.Date, out parsedDate);

                        // Преобразуем строковую дату и возвращаем объект InfoForDay / Convert the string date and return the InfoForDay object
                        return new InfoForDay
                        {
                            //idInfo = info.idInfo,
                            Date = isDateValid ? parsedDate : DateTime.MinValue,
                            Money = info.Money,
                            idCategory = info.idCategory
                        };
                    })
                    .Where(info => info.Date != DateTime.MinValue) // Исключаем записи с некорректными датами / Exclude records with incorrect dates
                    .ToList();


                foreach (var i in infoList)
                {
                    listI.Add(i);
                }


                return listI;
            }

        }


        public static void AddInfo(InfoForDay info)
        {
            InfoForDayAdditionalClass infoAdd = new InfoForDayAdditionalClass();
            // infoAdd.idInfo = info.idInfo;
            infoAdd.Date = info.Date.ToString();
            infoAdd.Money = info.Money;
            infoAdd.idCategory = info.Category1.idCategory;
            //infoAdd.idCategory = info.idCategory;

            using (var ctx = new DataContext())
            {
                ctx.InfoForDayAdditionalClassList.Add(infoAdd);
                ctx.SaveChanges();
            }

        }


        //edit date
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
                if (result != null )
                {   
                    result[0].Date = info.Date.ToString();
                    result[0].Money =  info.Money ;
                    result[0].idCategory = info.Category1.idCategory;

                    if(result.Count > 1)
                    {
                        for (int i = 1; i < result.Count; i++)
                            result[i].Money = 0;
                    }

                    ctx.SaveChanges();
                }
            }
        }

        //delete date
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
                if(element!=null)
                {
                    foreach (var item in element)
                        ctx.InfoForDayAdditionalClassList.Remove(item);
                    ctx.SaveChanges();
                }
               
            }
         }
        

        //delete remove category from current day

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

        //find year
        public static void FindYear(InfoForDay infoDays)
        {
            using(var ctx = new DataContext())
            {
                InfoForDayAdditionalClass infoFindYear = new InfoForDayAdditionalClass();
                infoFindYear.Date = infoDays.Date.ToString();
            }
        }

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



    




