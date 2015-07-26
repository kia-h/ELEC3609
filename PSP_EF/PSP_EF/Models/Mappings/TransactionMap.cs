using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.ModelConfiguration;

namespace PSP_EF.Models.Mappings
{
    public class TransactionMap : EntityTypeConfiguration<Transaction>
    {
        public TransactionMap()
        {
            // Primary Key
            this.HasKey(t => t.TransactionId);

            // Properties
            this.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(32);

            // Table & Column Mappings
            //this.ToTable("Transaction");
            //this.Property(t => t.TransactionId).HasColumnName("TransactionId");
            //this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            //this.Property(t => t.ServiceId).HasColumnName("ServiceId");
            //this.Property(t => t.Date).HasColumnName("Date");
            //this.Property(t => t.Status).HasColumnName("Status");

            // Relationships
            this.HasRequired(t => t.Customer)
                .WithMany(t => t.Transactions)
                .HasForeignKey(d => d.CustomerId);

        }
    }
}
