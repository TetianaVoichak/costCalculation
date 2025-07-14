using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;


namespace costСalculation.Data
{
    class ApplicationContext :DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {

        }
    }
}
