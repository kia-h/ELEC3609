using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PSP_EF.Models.Mappings
{
    public class PetMap : EntityTypeConfiguration<Pet>
    {
        public PetMap()
        {
            // Primary Key
            this.HasKey(t => t.PetId);

            // Properties
            this.Property(t => t.PetCategory)
                .IsRequired()
                .HasMaxLength(8);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            //this.ToTable("Pet");
            //this.Property(t => t.PetId).HasColumnName("PetId");
            //this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            //this.Property(t => t.TransactionId).HasColumnName("TransactionId");
            //this.Property(t => t.PetCategory).HasColumnName("PetCategory");
            //this.Property(t => t.Price).HasColumnName("Price");
            //this.Property(t => t.Description).HasColumnName("Description");

            // Relationships
            this.HasRequired(t => t.Customer)
                .WithMany(t => t.Pets)
                .HasForeignKey(d => d.CustomerId);
            this.HasOptional(t => t.PetClass).WithMany(t => t.Pets).HasForeignKey(d=>d.PetClassId);

        }
    }
}
