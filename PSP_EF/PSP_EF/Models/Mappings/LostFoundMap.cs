using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PSP_EF.Models.Mappings
{
    public class LostFoundMap : EntityTypeConfiguration<LostFound>
    {
        public LostFoundMap()
        {
            // Primary Key
            this.HasKey(t => t.LostFoundId);

            // Properties
            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(250);

            // Properties
            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            //this.ToTable("LostFound");
            //this.Property(t => t.Id).HasColumnName("Id");
            //this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            //this.Property(t => t.Description).HasColumnName("Description");
            //this.Property(t => t.Suburb).HasColumnName("Suburb");

            // Relationships
            this.HasRequired(t => t.Customer)
                .WithMany(t => t.LostFounds)
                .HasForeignKey(d => d.CustomerId);

        }
    }
}
