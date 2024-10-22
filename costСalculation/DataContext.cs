using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace costСalculation
{
    public class DataContext : DbContext
    {
        public string ExceptionSqlDbContext {  get; set; }
       
        public DataContext() : base(new SQLiteConnection()
        {
            
            ConnectionString = new SQLiteConnectionStringBuilder()
            {

                //DataSource = "datab.db",
                //DataSource = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CostCalculation", "datab.db"),
                DataSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "datab.db"),
                ForeignKeys = true
            }.ConnectionString
        }, true)
        {
            try
            {
                // Инициализируем базу данных при первом запуске
                //Database.SetInitializer(new CreateDatabaseIfNotExists<DataContext>());

                // Пример вызова для проверки подключения
                this.Database.Connection.Open();
            }
            catch (Exception ex)
            {
                ExceptionSqlDbContext = ex.ToString();
            }
            finally
            {
                if (this.Database.Connection.State == ConnectionState.Open)
                {
                    this.Database.Connection.Close();
                }
            }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<Category> Categories { get; set; }

        public DbSet<InfoForDayAdditionalClass> InfoForDayAdditionalClassList { get; set; }

    }
}

