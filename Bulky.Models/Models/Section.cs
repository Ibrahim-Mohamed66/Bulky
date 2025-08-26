using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.Models
{
    public class Section:EntityBase
    {
        public int Id { get; set; }

        public bool IsHomePage { get; set; } = false;

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string? SectionType { get; set; }

    }

}
