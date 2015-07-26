using System;
using System.Collections.Generic;

namespace PSP_EF.Models
{
    public class PetClass
    {
        public int PetClassId { get; set; }

        //[Required(ErrorMessage = "Please enter the class name.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Capacity { get; set; }
        public string Location { get; set; }
        public DateTime? Schedule { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
        public virtual ICollection<Customer> BookedCustomers { get; set; }
    }
}