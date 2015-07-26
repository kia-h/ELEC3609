using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSP_EF.Models.Mappings
{
    public class LF_CommentMap : EntityTypeConfiguration<LF_Comment>
    {
        public LF_CommentMap()
        {
            // Primary Key
            this.HasKey(t => t.CommentId);

            this.Property(t => t.Comment)
                .HasMaxLength(300);
        }
    }
}