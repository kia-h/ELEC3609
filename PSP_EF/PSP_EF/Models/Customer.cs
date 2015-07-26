using System;
using System.Collections.Generic;

namespace PSP_EF.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Please enter the city name")]
        public int CityId { get; set; }
        public string Suburb { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public DateTime? JoinedDate { get; set; }
        public DateTime? LastUpdate { get; set; }

        public virtual ICollection<PetClass> PetClasses { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<LostFound> LostFounds { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
        //public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Recommendation> Recommendations { get; set; }
    }
}