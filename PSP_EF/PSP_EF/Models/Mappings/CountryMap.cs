using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PSP_EF.Models.Mappings
{
    public class CountryMap : EntityTypeConfiguration<Country>
    {
        public CountryMap()
        {
            // Primary Key
            this.HasKey(t => t.CountryId);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.Currency)
                .HasMaxLength(5);

            // Table & Column Mappings
            //this.ToTable("Country");
            //this.Property(t => t.CountryId).HasColumnName("CountryId");
            //this.Property(t => t.Name).HasColumnName("Name");
            //this.Property(t => t.Currency).HasColumnName("Currency");
        }
    }
}
