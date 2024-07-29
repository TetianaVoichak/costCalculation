using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;

namespace costСalculation
{
    public class DataContext : DbContext
    {
        public DataContext() : base(new SQLiteConnection()
        {
            ConnectionString = new SQLiteConnectionStringBuilder()
            {
                DataSource = "datab.db",
                ForeignKeys = true
            }.ConnectionString
        }, true)
        { 
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Category> Categories { get; set; }

        //public DbSet<InfoForDay> InfoForDays { get; set; }
        public DbSet<InfoForDayAdditionalClass> InfoForDayAdditionalClassList { get; set; }

    }
}

