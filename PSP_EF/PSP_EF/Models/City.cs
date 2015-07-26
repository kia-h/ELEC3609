using System.Collections.Generic;

namespace PSP_EF.Models
{
    public class City
    {
        public int CityId { get; set; }
        public int CountryId { get; set; }
        
        public string Name { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}