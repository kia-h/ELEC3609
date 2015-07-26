
namespace PSP_EF.Models
{
    public class LostFound
    {
        public int LostFoundId { get; set; }
        public int CustomerId { get; set; }
        public string PetCategory { get; set; }
        public string Description { get; set; }
        public int City { get; set; }

        public virtual Customer Customer { get; set; }
    }
}