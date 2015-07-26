using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PSP_EF.Models.Mappings
{
    public class PetClassMap : EntityTypeConfiguration<PetClass>
    {
        public PetClassMap()
        {
            // Primary Key
            this.HasKey(t => t.PetClassId);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Description)
                .HasMaxLength(255);

            this.Property(t => t.Location)
                .HasMaxLength(250);

            // Table & Column Mappings
            //this.ToTable("PetClass");
            //this.Property(t => t.PetClassId).HasColumnName("PetClassId");
            //this.Property(t => t.Name).HasColumnName("Name");
            //this.Property(t => t.Description).HasColumnName("Description");
            //this.Property(t => t.Capacity).HasColumnName("Capacity");
            //this.Property(t => t.Location).HasColumnName("Location");
            //this.Property(t => t.Schedule).HasColumnName("Schedule");
        }
    }
}
