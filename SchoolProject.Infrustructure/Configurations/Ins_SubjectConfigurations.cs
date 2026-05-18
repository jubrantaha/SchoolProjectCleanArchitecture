using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Data.Entities;

namespace SchoolProject.Infrustructure.Configurations
{
    public class Ins_SubjectConfigurations : IEntityTypeConfiguration<Ins_Subject>
    {
        public void Configure(EntityTypeBuilder<Ins_Subject> builder)
        {
            builder.HasKey(x => new { x.SubID, x.InsId });

            builder.HasOne(i => i.instructor)
                .WithMany(i => i.Ins_Subjects)
                .HasForeignKey(i => i.InsId);

            builder.HasOne(s => s.Subject)
                .WithMany(s => s.Ins_Subjects)
                .HasForeignKey(x => x.SubID);


        }
    }
}
