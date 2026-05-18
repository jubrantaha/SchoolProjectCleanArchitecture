using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Data.Entities;

namespace SchoolProject.Infrustructure.Configurations
{
    public class StudentSubjectConfigurations : IEntityTypeConfiguration<StudentSubject>
    {
        public void Configure(EntityTypeBuilder<StudentSubject> builder)
        {
            builder.HasKey(x => new { x.SubID, x.StudID });


            builder.HasOne(s => s.Student)
                .WithMany(i => i.StudentSubject)
                .HasForeignKey(i => i.StudID);

            builder.HasOne(s => s.Subject)
            .WithMany(s => s.StudentsSubjects)
            .HasForeignKey(x => x.SubID);

        }
    }
}
