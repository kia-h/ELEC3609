using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PSP_EF.Models.Mappings
{
    public class AdoptionMap : EntityTypeConfiguration<Adoption>
    {
        public AdoptionMap()
        {
            // Primary Key
            this.HasKey(t => t.AdoptionId);


            // Relationships
            //this.HasRequired(t => t.Pet)
                //.WithOptional
                //.HasForeignKey(d => d.ProviderId);

        }
    }
}
