using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Data.Entities;

namespace SchoolProject.Core.Mapping.Students
{
    public partial class StudentProfile
    {
        public void GetStudentListMapping()
        {
            CreateMap<Student, GetStudentListRespons>()
                .ForMember(des => des.DepartmentName, opt => opt.MapFrom(src => src.GetLocalized(src.Department.DNameAr, src.Department.DNameEn)))
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.GetLocalized(src.NameAr, src.NameEn)));
        }
    }
}
