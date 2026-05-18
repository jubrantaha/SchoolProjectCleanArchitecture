using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Data.Entities;

namespace SchoolProject.Infrustructure.Configurations
{
    public class DepartmentSubjectConfigurations : IEntityTypeConfiguration<DepartmentSubject>
    {
        public void Configure(EntityTypeBuilder<DepartmentSubject> builder)
        {

            builder.HasKey(x => new { x.SubID, x.DID });

            builder.HasOne(ds => ds.Department)
                 .WithMany(s => s.DepartmentSubjects)
                 .HasForeignKey(ds => ds.DID);

            builder.HasOne(ds => ds.Subject)
                  .WithMany(s => s.DepartmentsSubjects)
                  .HasForeignKey(x => x.SubID);
        }
    }
}
