using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.ModelConfiguration;

namespace PSP_EF.Models.Mappings
{
    public class RecommendationMap : EntityTypeConfiguration<Recommendation>
    {
        public RecommendationMap()
        {
            // Primary Key
            this.HasKey(t => t.RecommendationId);

            // Properties
            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            //this.ToTable("Transaction");
            //this.Property(t => t.TransactionId).HasColumnName("TransactionId");
            //this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            //this.Property(t => t.ServiceId).HasColumnName("ServiceId");
            //this.Property(t => t.Date).HasColumnName("Date");
            //this.Property(t => t.Status).HasColumnName("Status");

            // Relationships
            this.HasRequired(t => t.Customer)
                .WithMany(t => t.Recommendations)
                .HasForeignKey(d => d.CustomerAid);


            this.HasRequired(t => t.Customer)
                .WithMany(t => t.Recommendations)
                .HasForeignKey(d => d.CustomerBid);

            this.HasRequired(t => t.Customer)
                .WithMany(t => t.Recommendations)
                .HasForeignKey(d => d.CustomerCid);
        }
    }
}
