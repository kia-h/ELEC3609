using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PSP_EF.Models.Mappings
{
    public class ServiceMap : EntityTypeConfiguration<Service>
    {
        public ServiceMap()
        {
            // Primary Key
            this.HasKey(t => t.ServiceId);

            // Properties
            this.Property(t => t.ServiceName)
                .HasMaxLength(50);

            this.Property(t => t.Description)
                .HasMaxLength(50);

            // Table & Column Mappings
            //this.ToTable("Service");
            //this.Property(t => t.ServiceId).HasColumnName("ServiceId");
            //this.Property(t => t.ProviderId).HasColumnName("CustomerId");
            //this.Property(t => t.ServiceName).HasColumnName("ServiceName");
            //this.Property(t => t.Description).HasColumnName("Description");
            //this.Property(t => t.Price).HasColumnName("Price");

            // Relationships
            this.HasRequired(t => t.Customer)
                .WithMany(t => t.Services)
                .HasForeignKey(d => d.ProviderId);

        }
    }
}
