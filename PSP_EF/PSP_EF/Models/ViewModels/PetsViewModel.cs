using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSP_EF.Models.ViewModels
{
    public class PetsViewModel
    {
        public IEnumerable<Pet> Pets { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<PetClass> PetClasses { get; set; }

    }
}