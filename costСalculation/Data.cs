using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Linq;
using System.Data.Entity;

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

        public static void AddCategory(Category category, out string message)
        {
            if (!CheckCategory(category))
            {
                using (var ctx = new DataContext())
                {
                    ctx.Categories.Add(category);
                    ctx.SaveChanges();
                    message = "";
                }

            }
            else message = "Element exists";

        }

        public static void DeleteCategory(Category category)
        {
            using (var ctx = new DataContext())
            {
                var element = ctx.Categories
                  .Where(c => c.NameCategory == category.NameCategory).FirstOrDefault();
                ctx.Categories.Remove(element);
                ctx.SaveChanges();
            }
        }

        //convert string to date from database and write to list
        public static void GetСonvertStringToDate()
        {

            using (var ctx = new DataContext())
            {

               // var listDate = ctx.InfoForDays.Select(x => new { x.idInfo, x.Date }).ToList();

                //return listDate;

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
        

        public static void AddInfo(InfoForDay info )
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

        }
    }


    




