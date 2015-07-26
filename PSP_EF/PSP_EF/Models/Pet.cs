
using System.ComponentModel.DataAnnotations;

namespace PSP_EF.Models
{
    public class Pet
    {
        public int PetId { get; set; }
        public int CustomerId { get; set; }
        public int? TransactionId { get; set; }
        public string PetCategory { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public bool HasClass { get; set; }
        public bool IsForAdoption { get; set; }
        public int? AdoptionId { get; set; }
        public int? PetClassId { get; set; }

        public virtual Adoption Adoption { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual PetClass PetClass { get; set; }
    }
}