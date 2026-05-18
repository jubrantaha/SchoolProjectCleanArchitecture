using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Data.Entities;

namespace SchoolProject.Core.Mapping.Students
{
    public partial class StudentProfile
    {
        public void AddStudentCommadMapping()
        {
            CreateMap<AddStudentCommand, Student>()
                .ForMember(des => des.DID, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(des => des.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(des => des.NameEn, opt => opt.MapFrom(src => src.NameEn));
        }
    }
}
