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

        [Required(ErrorMessage = "Name cannot be null or empty.")]
        [MaxLength(15,ErrorMessage = "Name length max 15 characters!")]
        public string NameCategory
        {
            get
            {
                return nameCategory;
            }
            set { nameCategory = value; }

        }

        // IDataErrorInfo implementation // часть интерфейса IDataErrorInfo
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                var validationContext = new ValidationContext(this) { MemberName = columnName };
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateProperty(GetType().GetProperty(columnName).GetValue(this), validationContext, results))
                {
                    return results[0].ErrorMessage;
                }
                return null;
            }
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
