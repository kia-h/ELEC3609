
namespace PSP_EF.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public int ProviderId { get; set; }
        public int? PetId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }

        public virtual Customer Customer { get; set; }
        //public virtual ICollection<Rating> Ratings { get; set; }
    }
}