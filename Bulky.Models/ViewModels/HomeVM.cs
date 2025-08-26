using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Section> Sections { get; set; } = new List<Section>();
        public IEnumerable<Product> Products { get; set; } = new List<Product>();

    }
}
