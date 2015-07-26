using System;
using System.Collections.Generic;

namespace PSP_EF.Models
{
    public class Country
    {
        public int CountryId { get; set; }

        //[Required(ErrorMessage = "Please enter the country name")]
        public string Name { get; set; }
        public string Currency { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}