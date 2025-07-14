using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace costСalculation.Models
{
    //CategorySum class that contains the CategoryId, Name, TotalSum and percent
    //for each category (used to plot the chart)
    internal class CategorySum
    {
        public int CategoryId { get; set; }
        public decimal TotalSum { get; set; }


        public string Name { get; set; }
        public int percent { get; set; }

    }
}
