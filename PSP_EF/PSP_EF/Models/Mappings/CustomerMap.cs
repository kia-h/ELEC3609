using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PSP_EF.Models.Mappings
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.CustomerId);

            // Properties
            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Suburb)
                .HasMaxLength(30);

            this.Property(t => t.Address)
                .HasMaxLength(30);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Phone)
                .HasMaxLength(15);

            // Table & Column Mappings
            //this.ToTable("Customer");
            //this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            //this.Property(t => t.FirstName).HasColumnName("FirstName");
            //this.Property(t => t.LastName).HasColumnName("LastName");
            //this.Property(t => t.CityId).HasColumnName("CityId");
            //this.Property(t => t.Suburb).HasColumnName("Suburb");
            //this.Property(t => t.Address).HasColumnName("Address");
            //this.Property(t => t.Password).HasColumnName("Password");
            //this.Property(t => t.Email).HasColumnName("Email");
            //this.Property(t => t.DateOfBirth).HasColumnName("DateOfBirth");
            //this.Property(t => t.Description).HasColumnName("Description");
            //this.Property(t => t.Phone).HasColumnName("Phone");
            //this.Property(t => t.JoinedDate).HasColumnName("JoinedDate");
            //this.Property(t => t.LastUpdate).HasColumnName("LastUpdate");

            // Relationships
            this.HasRequired(t => t.City)
                .WithMany(t => t.Customers)
                .HasForeignKey(d => d.CityId);

        }
    }
}
