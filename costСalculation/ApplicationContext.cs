using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;


namespace costСalculation
{
    class ApplicationContext :DbContext
    {
        //public DbSet<InfoForDay> InfoForDays { get; set; }
       // public DbSet<Category> Categories { get; set; }
        public ApplicationContext() : base("DefaultConnection")
        {

        }
    }
}
