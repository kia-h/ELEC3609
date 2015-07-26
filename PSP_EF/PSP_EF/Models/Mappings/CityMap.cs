using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSP_EF.Models.Mappings
{
    public class CityMap : EntityTypeConfiguration<City>
    {
        public CityMap()
        {
            // Primary Key
            this.HasKey(t => t.CityId);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            //this.ToTable("City");
            //this.Property(t => t.CityId).HasColumnName("CityId");
            //this.Property(t => t.CountryId).HasColumnName("CountryId");
            //this.Property(t => t.Name).HasColumnName("Name");

            // Relationships
            this.HasRequired(t => t.Country)
                .WithMany(t => t.Cities)
                .HasForeignKey(d => d.CountryId);

        }
    }
}
