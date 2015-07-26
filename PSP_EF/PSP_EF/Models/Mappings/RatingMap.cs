using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PSP_EF.Models.Mappings
{
    public class RatingMap : EntityTypeConfiguration<Rating>
    {
        public RatingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.RatingId });

            // Properties
           // this.Property(t => t.CustomerId)
                //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RatingId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Comment)
                .HasMaxLength(256);

            // Table & Column Mappings
            //this.ToTable("Rating");
            //this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            //this.Property(t => t.ServiceId).HasColumnName("ServiceId");
            //this.Property(t => t.Rate_customer).HasColumnName("Rate_customer");
            //this.Property(t => t.Rate_service).HasColumnName("Rate_service");
            //this.Property(t => t.Type).HasColumnName("Type");
            //this.Property(t => t.Description).HasColumnName("Description");

            // Relationships
            //this.HasRequired(t => t.Customer)
              //  .WithMany(t => t.Ratings)
                //.HasForeignKey(d => d.CustomerId);
            this.HasRequired(t => t.Transaction)
                .WithMany(t => t.Rating)
                .HasForeignKey(d => d.TransactionId);
        }
    }
}
